using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppEventGrid.Model;
using Dapper;
using EventWebHookConsumer1.Repositories;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//https://github.com/Azure-Samples/azure-event-grid-viewer/blob/master/viewer/Models/CloudEvent.cs

namespace EventWebHookConsumer1.Controllers
{
    [Route("api/[controller]")]
    public class UpdatesController : Controller
    {
        private TelemetryClient telemetry = new TelemetryClient();
        IRepositorioCliente Repositorio;
        public UpdatesController(IRepositorioCliente repositorio)
        {
            Repositorio = repositorio;
        }

        [HttpPost("cliente")]
        public IActionResult PostCliente([FromBody]EventGridEvent[] ev)
        {
            if (ev == null && ev.Length == 0)
            {
                telemetry.TrackEvent("BadRequest");
                return BadRequest();
            }
            try
            {
                foreach (EventGridEvent item in ev)
                {
                    if (item.EventType == EventTypes.EventGridSubscriptionValidationEvent)
                    {
                        var data = (item.Data as JObject).ToObject<SubscriptionValidationEventData>();
                        var response = new Microsoft.Azure.EventGrid.Models.SubscriptionValidationResponse(data.ValidationCode);                        
                        return Ok(response);
                    }
                    else
                    {
                       if (item.EventType == "MRV.ClienteAtualizado")
                       {
                          var data = (item.Data as JObject).ToObject<Cliente>();
                          Repositorio.Salvar(data);
                        }                     
                     
                    }
                }
                return Ok();
            }
            catch(Exception ex) {
                telemetry.TrackException(ex);
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("insere/{codigo}")]
        public IActionResult InsereCliente(int codigo)
        {
            var cliente = new Cliente() { Codigo = codigo, Nome = "teste", Email = "teste", DataModificacao = DateTime.Now };
            Repositorio.Salvar(cliente);
            return Ok();
        }
    }
}
