using AppEventGrid.Model;
using EventWebHookConsumer1.Controllers;
using EventWebHookConsumer1.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Xunit;

namespace EventGrid.Tests
{
    public class UnitTest1
    {

        [Fact]
        public void PostClienteApp1()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var repo = new RepositorioCliente(config);
            var controler = new UpdatesController(repo);
            var eventos = GetEventsList();
            var result = controler.PostCliente(eventos);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public void PostClienteApp2()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var repo = new EventWebHookConsumer2.Repositories.RepositorioCliente(config);
            var controler = new EventWebHookConsumer2.Controllers.UpdatesController(repo);
            var eventos = GetEventsList();
            var result = controler.PostCliente(eventos);
            Assert.IsType<OkResult>(result);

        }


        static EventGridEvent[] GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();
            for (int i = 1; i <= 10; i++)
            {
                var cliente = new Cliente()
                {
                    Codigo = i,
                    Nome = $"Joao da Silva {i} - Atualizado em {DateTime.Now.ToString()}",
                    Email = $"joao{i}@gmail.com"
                };
                eventsList.Add(new EventGridEvent()
                {

                    Id = Guid.NewGuid().ToString(),
                    EventType = "MRV.ClienteAtualizado",
                    Data = JObject.FromObject(cliente),
                    EventTime = DateTime.Now,
                    Subject = "Cliente Atualizado",
                    DataVersion = "1.0",
                });

            }
            return eventsList.ToArray();
        }
    }
}
