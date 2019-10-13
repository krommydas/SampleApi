using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleApi.BusinessLogic
{
    public class AlertSystem
    {
        public AlertSystem(Storage.DataService dataService)
        {
            Data = dataService;
        }

        Storage.DataService Data;
        //HttpClient HttpClient;
        //ExternalProviderConfiguration ExternalProviderConfiguration;

        #region CRUD

        public Task<List<Alert>> GetAlerts()
        {
            return Data.GetAllAlerts<Alert>();
        }

        public Task<Alert> GetSingleAlert(int id)
        {
            return Data.GetAlert<Alert>(id);
        }

        public Task InsertAlert(Alert item)
        {
            return Data.InsertAlert(item.ToStorageItem());
        }

        public async Task InsertAlerts(IEnumerable<Alert> items)
        {
            await Data.InsertAlerts(items.Distinct().Select(x => x.ToStorageItem()));
        }

        public Task DeleteAlert(int id)
        {
            return Data.DeleteAlert(id);
        }

        public Task UpdateAlert(int itemID, Alert update)
        {
            return Data.UpdateAlert(itemID, update.ToStorageItem());
        }

        #endregion
    }
}
