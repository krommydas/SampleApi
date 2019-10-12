using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        public AlertController(BusinessLogic.AlertSystem alertSystem)
        {
            Alerts = alertSystem;
        }

        private BusinessLogic.AlertSystem Alerts;

        [HttpGet]
        public IAsyncResult Get()
        {
            return Alerts.GetAlerts();
        }

        [HttpGet]
        public IAsyncResult Get(int id)
        {
            return Alerts.GetSingleAlert(id);
        }

        [HttpPost]
        public IAsyncResult Post([FromBody] BusinessLogic.Alert value)
        {
            return Alerts.InsertAlert(value);
        }

        [HttpPut]
        public IAsyncResult Put(int id, [FromBody] BusinessLogic.Alert value)
        {
            return Alerts.UpdateAlert(id, value);
        }

        [HttpDelete]
        public IAsyncResult Delete(int id)
        {
            return Alerts.DeleteAlert(id);
        }
    }
}
