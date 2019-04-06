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
using System.Text;
using System.Threading.Tasks;

namespace Setas.Services
{
    //Instantiated as Single Instance by Autofac.
    public class InternalDataService : IInternalDataService
    {
        private const string DBNAME = "MushroomsDb.db3";
        private readonly string DBPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME);

        private static SQLiteAsyncConnection _database;

        public bool DatabaseInitialized { get; private set; }

        public InternalDataService()
        {

#if DEBUG
            File.Delete(DBPATH);
#endif
        }

        public async Task Initialise()
        {
            if (!File.Exists(DBPATH))
            {
                OpenDatabase();
                await CreateDatabaseStructure();
            }
            else
            {
                OpenDatabase();
            }


            DatabaseInitialized = true;

        }


        private void OpenDatabase()
        {
            try
            {
                _database = new SQLiteAsyncConnection(DBPATH);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error opening Database. {ex.Message}", ex);
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

                DatabaseInitialized = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating Database. {ex.Message}", ex);
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



        public async Task<int> GetTotalCountAsync(SearchOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var sql = new StringBuilder("SELECT COUNT(*) FROM Mushrooms");

            List<string> whereArgs = new List<string>();

            CreateWhereStatement(options, sql, whereArgs);

            var totalItems = await _database.ExecuteScalarAsync<int>(sql.ToString(), whereArgs.ToArray());

            return totalItems;
        }



        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options)
        {

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var sql = new StringBuilder("SELECT * FROM Mushrooms");

            List<string> whereArgs = new List<string>();

            CreateWhereStatement(options, sql, whereArgs);

            //add pagination statement
            sql.Append($" ORDER BY Name LIMIT {options.PageSize} OFFSET {(options.Page - 1) * options.PageSize}");


            List<Mushroom> result = await _database.QueryAsync<Mushroom>(sql.ToString(), whereArgs.ToArray());


            return result;
        }


        private static void CreateWhereStatement(SearchOptions options, StringBuilder sql, List<string> whereArgs)
        {
            var whereSql = new StringBuilder(" WHERE ");

            //we don't filter by Edibles filter is there is a Query term.
            if (string.IsNullOrEmpty(options.QueryTerm) && options.Edibles != null && options.Edibles.Any())
            {
                whereSql.Append("CookingInterest IN (?)");
                var ediblesFilter = string.Join(",", options.Edibles.Select(s => ((int)s).ToString()).ToArray());
                whereArgs.Add(ediblesFilter);
            }

            if (!string.IsNullOrEmpty(options.QueryTerm))
            {
                if (whereArgs.Count > 0) whereSql.Append(" AND ");

                whereSql.Append("Name LIKE ? COLLATE NOCASE");
                whereArgs.Add($"%{options.QueryTerm}%");

            }

            if (whereArgs.Any())
            {
                sql.Append(whereSql);
            }
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
