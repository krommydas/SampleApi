using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleApi.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    [ApiController]
    public class AlertController : ControllerBase
    {
        public AlertController(BusinessLogic.AlertSystem alertSystem)
        {
            Alerts = alertSystem;
        }

        private BusinessLogic.AlertSystem Alerts;

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IAsyncResult GetAll()
        {
            return Alerts.GetAlerts();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public IAsyncResult Get([FromUri] int id)
        {
            return Alerts.GetSingleAlert(id);
        }

        [System.Web.Http.HttpPost]
        public IAsyncResult Post([System.Web.Http.FromBody] BusinessLogic.Alert value)
        {
            return Alerts.InsertAlert(value);
        }

        [System.Web.Http.HttpPut]
        public IAsyncResult Put(int id, [System.Web.Http.FromBody] BusinessLogic.Alert value)
        {
            return Alerts.UpdateAlert(id, value);
        }

        [System.Web.Http.HttpDelete]
        public IAsyncResult Delete([FromUri] int id)
        {
            return Alerts.DeleteAlert(id);
        }
    }
}
