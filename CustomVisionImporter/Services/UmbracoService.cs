﻿using Setas.Common.Models.Api;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CustomVisionImporter.Services
{
    class UmbracoService
    {
        private HttpClient _client;

        public Uri ApiBase { get; set; }

        public UmbracoService()
        {
            _client = new HttpClient();
        }

        /// <summary>
        /// Creates a node in Umbraco and returns the id of the new node. It returns 0 if unsuccessful.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<int> CreateNode(ImportNodeContent content)
        {
            string endpoint = "content/createnode";

            var queryContent = new StringContent(content.ToString(), Encoding.UTF8, "application/json");
            var result = _client.PostAsync(new Uri(ApiBase, endpoint), queryContent).Result;

            if (result.IsSuccessStatusCode)
            {
                var rContent = await result.Content.ReadAsStringAsync();
                if(int.TryParse(rContent, out int nodeId))
                {
                    return nodeId;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                throw new Exception(result.ReasonPhrase);
            }
        }

    }
}