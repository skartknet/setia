using Microsoft.AppCenter.Crashes;
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

        readonly string DBPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME);

        private static SQLiteAsyncConnection _database;

        public InternalDataService()
        {
            OpenDatabase(DBPATH);
            var dataTable = _database.GetTableInfoAsync("Mushroom").Result;
            if (dataTable.Count == 0)
            {
                try
                {
                    this.CreateDatabaseStructure(DBPATH);
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex);
                }
            }
        }

        private void OpenDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        private void CreateDatabaseStructure(string dbPath)
        {
            _database.CreateTableAsync<Mushroom>().Wait();
            _database.CreateTableAsync<Configuration>().Wait();

        }


        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options, params int[] ids)
        {

            if (ids != null && ids.Any())
            {
                return await _database.Table<Mushroom>().Where(m => ids.Contains(m.Id)).ToListAsync();
            }
            else
            {
                return await _database.Table<Mushroom>().ToListAsync();
            }

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


    }
}
