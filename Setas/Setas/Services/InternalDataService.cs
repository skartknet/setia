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
        static SQLiteAsyncConnection _database;
        private SQLiteAsyncConnection Database {
            get
            {
                if(_database == null)
                {
                    this.CreateDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MushroomsDb.db3"));
                }

                return _database;
            }
        };

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
    }
}
