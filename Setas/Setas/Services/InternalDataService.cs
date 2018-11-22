using Newtonsoft.Json;
using Setas.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Setas.Services
{
    //Instantiated as Single Instance by Autofac.
    public class InternalDataService : IDataService
    {
        const string DBNAME = "MushroomsDb.db3";

        readonly string DBPATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DBNAME);

        static SQLiteAsyncConnection _database;
        private SQLiteAsyncConnection Database {
            get
            {
                if(_database == null)
                {
                    if (!File.Exists(DBPATH))
                    {
                        this.CreateDatabase(DBPATH);
                    }
                    else
                    {
                        this.OpenDatabase(DBPATH);
                    }
                }

                return _database;
            }
        }

        private void OpenDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
        }

        private void CreateDatabase(string dbPath)
        {            
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Mushroom>().Wait();
        }

        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids)
        {
            IEnumerable<Mushroom> items = null;

            if (ids.Any())
            {
                items = await _database.Table<Mushroom>().Where(m=>ids.Contains(m.Id)).ToListAsync();
            }
            else
            {
                items = await _database.Table<Mushroom>().ToListAsync();
            }


            return items;
        }


        public async Task<Mushroom> GetMushroomAsync(int id)
        {
            return await _database.Table<Mushroom>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task InsertMushroomsAsync(IEnumerable<Mushroom> items)
        {
            await _database.InsertOrReplaceAsync(items);
        }
    }
}
