using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleApi.BusinessLogic
{
    public class AlertSystem
    {
        public AlertSystem(Storage.DataService dataService)
        {
            data = dataService;
        }

        Storage.DataService data;

        public Task<List<Alert>> GetAlerts()
        {
            return data.GetAllAlerts<Alert>();
        }

        public Task<Alert> GetSingleAlert(int id)
        {
            return data.GetAlert<Alert>(id);
        }

        public Task InsertAlert(Alert item)
        {
            return data.InsertAlert(item.ToStorageItem());
        }

        public Task DeleteAlert(int id)
        {
            return data.DeleteAlert(id);
        }

        public Task UpdateAlert(int itemID, Alert update)
        {
            return data.UpdateAlert(itemID, update.ToStorageItem());
        }
    }
}
