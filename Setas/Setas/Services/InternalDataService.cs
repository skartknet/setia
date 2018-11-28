using Setas.Common.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Setas.Services
{
    //Instantiated as Single Instance by Autofac.
    public class InternalDataService : IInternalDataService
    {
        const string DBNAME = "MushroomsDb.db3";

        readonly string DBPATH = Path.Combine("/data/user/0/com.companyname.Setas/databases", DBNAME);

        private static SQLiteAsyncConnection _database;        


        public bool IsContextReady { get; set; }

        public InternalDataService()
        {            

            OpenDatabase();
            var dataTable = _database.GetTableInfoAsync("Mushroom").Result;
            if (dataTable.Count == 0)
            {
                CreateDatabaseStructure();
            }            
        }

        private void OpenDatabase()
        {
            _database = new SQLiteAsyncConnection(DBPATH);
        }

     

        private void CreateDatabaseStructure()
        {
            try
            {
                _database.CreateTableAsync<Mushroom>().Wait();
                _database.CreateTableAsync<Configuration>().Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Database", ex);
            }

        }




        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options, params int[] ids)
        {
            AsyncTableQuery<Mushroom> result = _database.Table<Mushroom>();

            if (ids != null && ids.Any())
            {
                result = result.Where(m => ids.Contains(m.Id));
            }

            if (options.Edible.HasValue)
            {
                result = result.Where(m => m.CookingInterest == options.Edible);
            }

            return await result.ToListAsync(); ;
        }


        public Task<Mushroom> GetMushroomAsync(int id)
        {
            return _database.Table<Mushroom>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertMushroomsAsync(IEnumerable<Mushroom> items)
        {
            foreach (var item in items)
            {
                await this.InsertMushroomAsync(item);
            }
        }

        public Task InsertMushroomAsync(Mushroom item)
        {
            return _database.InsertOrReplaceAsync(item);
        }

        public Task<Configuration> GetConfigurationAsync()
        {
            return _database.Table<Configuration>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Sets the Lastest Updated Content Datetime to now
        /// </summary>
        /// <returns></returns>
        public async Task SetContentUpdatedAsync()
        {
            Configuration config = await GetConfigurationAsync();
            if (config != null)
            {
                config.LatestContentUpdate = DateTime.UtcNow;
            }
            else
            {
                config = new Configuration()
                {
                    LatestContentUpdate = DateTime.UtcNow
                };
            }

            await _database.InsertOrReplaceAsync(config);
        }
    }
}
