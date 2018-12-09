using Setas.Common.Models;
using Setas.Models;
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

        readonly string DBPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME);

        private static SQLiteAsyncConnection _database;


        public InternalDataService()
        {

            OpenDatabase();

#if DEBUG
            //Task.Run(async () => await _database.DropTableAsync<MushroomData>()).Wait();
            //Task.Run(async ()=>await this.SetContentUpdatedAsync()).Wait();
#endif



            var dataTable = _database.GetTableInfoAsync("Mushroom").Result;
            if (dataTable.Count == 0)
            {
                CreateDatabaseStructure();
            }
        }

        private void OpenDatabase()
        {
            try
            {
                _database = new SQLiteAsyncConnection(DBPATH);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Database", ex);
            }
        }


        private void CreateDatabaseStructure()
        {
            try
            {
                _database.CreateTableAsync<MushroomData>().Wait();
                _database.CreateTableAsync<Configuration>().Wait();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Database", ex);
            }

        }




        public async Task<IEnumerable<MushroomData>> GetMushroomsAsync(SearchOptions options, params int[] ids)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            AsyncTableQuery<MushroomData> result = _database.Table<MushroomData>();

            if (ids != null && ids.Any())
            {
                result = result.Where(m => ids.Contains(m.Id));
            }

            if (options.Edible.HasValue)
            {
                result = result.Where(m => m.CookingInterest == options.Edible);
            }



            return await result.ToListAsync();
        }


        public Task<MushroomData> GetMushroomAsync(int id)
        {
            return _database.Table<MushroomData>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertMushroomsAsync(IEnumerable<MushroomData> items)
        {
            foreach (var item in items)
            {
                await this.InsertMushroomAsync(item);
            }
        }

        public Task InsertMushroomAsync(MushroomData item)
        {
            return _database.InsertOrReplaceAsync(item);
        }

        public Task<Configuration> GetConfigurationAsync()
        {
            return _database.Table<ConfigurationData>().FirstOrDefaultAsync();
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
