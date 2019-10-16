using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<BusinessLogic.Alert>>> GetAll()
        {
            return await Alerts.GetAlerts();
        }

        [Microsoft.AspNetCore.Mvc.HttpGet("{id}")]
        public async Task<ActionResult<BusinessLogic.Alert>> GetById([FromUri] int id)
        {
            return await Alerts.GetSingleAlert(id);
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<BusinessLogic.Alert>> Create([System.Web.Http.FromBody] BusinessLogic.Alert value)
        {
            await Alerts.InsertAlert(value);

            return CreatedAtAction(nameof(GetById), new { id = value.ID }, value);
        }

        [Microsoft.AspNetCore.Mvc.HttpPut("{id}")]
        [Consumes(System.Net.Mime.MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(int id, BusinessLogic.Alert value)
        {
            if (id != value.ID)
            {
                return BadRequest();
            }

            await Alerts.UpdateAlert(id, value);

            return NoContent();
        }

        [Microsoft.AspNetCore.Mvc.HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await Alerts.DeleteAlert(id);

            return NoContent();
        }
    }
}
