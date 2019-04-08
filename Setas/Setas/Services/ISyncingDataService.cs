using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Setas.Services
{
    public interface ISyncingDataService
    {
        bool IsSynced { get; }
        Task SyncDataAsync();
    }
}
