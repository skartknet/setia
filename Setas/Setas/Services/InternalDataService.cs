using Newtonsoft.Json;
using Setas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Setas.Services
{

    public class InternalDataService : IDataService
    {

        readonly Uri baseUrl = new Uri("http://172.17.198.145:5000/umbraco/api/");
        private static readonly object padlock = new object();

        private static InternalDataService _instance;

        public static InternalDataService Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new InternalDataService();
                    }
                    return _instance;
                }
            }
        }


        private InternalDataService()
        {

        }
      


        public async Task<IEnumerable<Mushroom>> GetMushroomsAsync(params int[] ids)
        {
            throw new NotImplementedException();
        }


        public async Task<Mushroom> GetMushroomAsync(int id)
        {
            throw new NotImplementedException();

        }
    }
}
