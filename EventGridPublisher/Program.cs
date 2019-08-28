using AppEventGrid.Model;
using EventGrid.Repositories;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace EventGrid
{
    class Program
    {
        private static IRepositorioCliente repo;
        private static IConfigurationRoot _config;


        private static IConfigurationRoot Config {
            get {
                if (_config == null)
                {
                    var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    _config = builder.Build();
                }
                return _config;
            }
        }

        private static string TopicHostname
        {
            get {
                var endpoint = Config["EventGrid:EndPoint"];
                return new Uri(endpoint).Host;
            }
        }

        private static EventGridClient EventClient
        {
            get
            {               
                TopicCredentials topicCredentials = new TopicCredentials(Config["EventGrid:AutorizationKey"]);
                return new EventGridClient(topicCredentials);
            }
        }


        static void Main(string[] args)
        {                    
            repo = new RepositorioCliente(Config);
            var lista = repo.Listar();
            //Insere registros
            if (lista.Count == 0)
            {  
                var eventos = GetEventsList();
                EventClient.PublishEventsAsync(TopicHostname, eventos).GetAwaiter().GetResult();
            }
            else
            {
                //Atualiza registros
                AtualizaRegistros(lista);
             
            }
            Console.Write("Published events to Event Grid topic. Pressione enter");
            Console.ReadLine();
        }

        private static void AtualizaRegistros(List<Cliente> lista)
        {
            var eventsList = new List<EventGridEvent>();
            foreach (Cliente c in lista)
            {
                c.DataModificacao = DateTime.Now;
                c.Nome = $"Cliente {c.Codigo}";
                c.Email = $"cliente{c.Codigo}@gmail.com";
                eventsList.Add(new EventGridEvent()
                {

                    Id = Guid.NewGuid().ToString(),
                    EventType = "MRV.ClienteAtualizado",                    
                    Data = c,
                    EventTime = DateTime.Now,
                    Subject = "Clientes Criados",
                    DataVersion = "1.0",
                });
            }
            EventClient.PublishEventsAsync(TopicHostname, eventsList.ToArray()).GetAwaiter().GetResult();
        }


        static EventGridEvent[] GetEventsList()
        {
            List<EventGridEvent> eventsList = new List<EventGridEvent>();
            for (int i = 1; i <= 1000; i++)
            {
                eventsList.Add(new EventGridEvent()                {

                    Id = Guid.NewGuid().ToString(),
                    EventType = "MRV.ClienteAtualizado",
                    Data = new Cliente()
                    {
                       Codigo = 0,
                       Nome = $"Joao da Silva {i} - Atualizado em {DateTime.Now.ToString()}",
                       Email = $"joao{i}@gmail.com",
                       DataModificacao = DateTime.Now
                    },
                    EventTime = DateTime.Now,
                    Subject = "Clientes Criados",
                    DataVersion = "1.0",
                });

            }
            return eventsList.ToArray();

        }
    }
}
