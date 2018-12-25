using System;
using System.Collections.Generic;
using EventStore.ClientAPI;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace console_app
{

    public class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            
            var gesConnection = EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113"),
                 "InputFromFileConsoleApp");
            
            
            await gesConnection.ConnectAsync();
            var streamName = "abc";


            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new User()
            {
                Name = "Alper",
                Surname = "Hankendi"
            }));

            var g = Guid.NewGuid();
            //streamName += "_" + g.ToString();
           
               
                
                await gesConnection.AppendToStreamAsync(streamName, 3, new List<EventData>()
                {
                    new EventData(g, "addUser", true, msg, null)
                });

            Console.ReadKey();
            
           
        }
    }
}
