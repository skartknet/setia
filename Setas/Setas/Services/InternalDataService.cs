using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Setas.Common;
using Setas.Common.Models;
using Setas.Data;
using Setas.Models.Data;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
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
            if (_database == null)
            {
                OpenDatabase();
            }

#if DEBUG
            File.Delete(DBPATH);      
#endif

           Task.Run(async () =>await  CreateDatabaseStructure()).Wait();
        }


        private void OpenDatabase()
        {
            try
            {
                _database = new SQLiteAsyncConnection(DBPATH);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                UserDialogs.Instance.Alert("Error creating database");
                throw new Exception("Error creating Database", ex);

            }
        }


        private async Task CreateDatabaseStructure()
        {
            try
            {
                await _database.CreateTableAsync<Mushroom>();
                await _database.CreateTableAsync<Models.Data.ConfigurationItem>();
                await _database.CreateTableAsync<HistoryItem>();


                var configElements = new List<Models.Data.ConfigurationItem>()
                {
                    new Models.Data.ConfigurationItem
                    {
                        Alias = Constants.LatestContentUpdatePropertyAlias,
                        Value = null
                    }
                };

                await _database.InsertAllAsync(configElements);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating Database", ex);
            }

        }


        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(IEnumerable<int> ids)
        {
            AsyncTableQuery<Mushroom> result = _database.Table<Mushroom>();

            if (ids != null && ids.Any())
            {
                result = result.Where(m => ids.Contains(m.Id));
            }

            return await result.ToListAsync();
        }


        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options)
        {            

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            AsyncTableQuery<Mushroom> result = _database.Table<Mushroom>();       

            if (options.Edibles != null && options.Edibles.Any())
            {
                result = result.Where(m => options.Edibles.Contains(m.CookingInterest));
            }

            return await result.OrderBy(n=>n.Name)
                    .Skip((options.Page - 1) * options.ItemsPerPage)
                    .Take(options.ItemsPerPage)
                    .ToListAsync();
        }


        public async Task<Mushroom> GetMushroomAsync(int id)
        {
            return await _database.Table<Mushroom>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertMushroomsAsync(IEnumerable<Mushroom> items)
        {
            foreach (var item in items)
            {
                await this.InsertMushroomAsync(item);
            }
        }

        public async Task<IEnumerable<HistoryItem>> GetHistoryAsync()
        {
            return await _database.GetAllWithChildrenAsync<HistoryItem>();
        }

        public async Task SaveHistoryItemAsync(HistoryItem item)
        {
            await _database.InsertAsync(item);
        }

        public async Task InsertMushroomAsync(Mushroom item)
        {
            await _database.InsertOrReplaceAsync(item);
        }

        public async Task<Common.Models.Configuration> GetConfigurationAsync()
        {
            var config = new Common.Models.Configuration();
            try
            {
                var data = await _database.Table<Models.Data.ConfigurationItem>().ToListAsync();
                if (DateTime.TryParse(data.FirstOrDefault(r => r.Alias == Constants.LatestContentUpdatePropertyAlias).Value.ToString(), out DateTime _lastUpdate))
                {
                    config.LatestContentUpdate = _lastUpdate;
                }
            }
            catch
            {
                //table empty
            }

            return config;
        }


        private async Task<IEnumerable<Models.Data.ConfigurationItem>> GetConfigurationDataAsync()
        {
            var data = await _database.Table<Models.Data.ConfigurationItem>().ToListAsync();
            return data;
        }




        /// <summary>
        /// Sets the Lastest Updated Content Datetime to now
        /// </summary>
        /// <returns></returns>
        public async Task SetContentUpdatedAsync()
        {
            var config = await GetConfigurationDataAsync();
            var lstUpdateData = config.FirstOrDefault(n => n.Alias == Constants.LatestContentUpdatePropertyAlias);


            if (lstUpdateData != null)
            {
                lstUpdateData.Value = DateTime.UtcNow.ToString();
                await _database.InsertOrReplaceAsync(lstUpdateData);

            }
            else
            {
                Models.Data.ConfigurationItem d = new Models.Data.ConfigurationItem
                {
                    Alias = Constants.LatestContentUpdatePropertyAlias,
                    Value = DateTime.UtcNow.ToString()
                };
                await _database.InsertOrReplaceAsync(d);

            }

        }
    }
}
