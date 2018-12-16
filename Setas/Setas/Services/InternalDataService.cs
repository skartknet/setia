using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;
using Setas.Common;
using Setas.Common.Models;
using Setas.Models.Data;
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
            if (_database == null)
            {
                OpenDatabase();
            }

#if DEBUG
            //File.Delete(DBPATH);      
            
#endif

            CreateDatabaseStructure();
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


        private void CreateDatabaseStructure()
        {
            try
            {
                _database.CreateTableAsync<MushroomData>();
                _database.CreateTableAsync<ConfigurationData>();

                var configElements = new List<ConfigurationData>()
                {
                    new ConfigurationData
                    {
                        Alias = Constants.LatestContentUpdatePropertyAlias,
                        Value = null
                    }
                };

                _database.InsertAllAsync(configElements);
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

            if (options.Edibles != null && options.Edibles.Any())
            {
                result = result.Where(m => options.Edibles.Contains(m.CookingInterest));
            }



            return await result.OrderBy(n=>n.Name).ToListAsync();
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

        public async Task<Configuration> GetConfigurationAsync()
        {
            var config = new Configuration();
            try
            {
                var data = await _database.Table<ConfigurationData>().ToListAsync();
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


        private async Task<IEnumerable<ConfigurationData>> GetConfigurationDataAsync()
        {
            var data = await _database.Table<ConfigurationData>().ToListAsync();
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
                ConfigurationData d = new ConfigurationData
                {
                    Alias = Constants.LatestContentUpdatePropertyAlias,
                    Value = DateTime.UtcNow.ToString()
                };
                await _database.InsertOrReplaceAsync(d);

            }

        }
    }
}
