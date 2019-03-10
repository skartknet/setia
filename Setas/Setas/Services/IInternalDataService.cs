﻿using Setas.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Setas.Models.Data;
using System;
using Setas.Data;

namespace Setas.Services
{
    public interface IInternalDataService
    {
        Task<IEnumerable<Mushroom>> GetMushroomsAsync(SearchOptions options, params int[] ids);
        Task<Mushroom> GetMushroomAsync(int id);
        Task<Configuration> GetConfigurationAsync();
        Task InsertMushroomsAsync(IEnumerable<Mushroom> sourceItems);

        Task<IEnumerable<HistoryItem>> GetHistoryAsync();

        /// <summary>
        /// Sets the content updated date to Now
        /// </summary>
        /// <returns></returns>
        Task SetContentUpdatedAsync();

        Task SaveHistoryItemAsync(HistoryItem item);
    }
}
