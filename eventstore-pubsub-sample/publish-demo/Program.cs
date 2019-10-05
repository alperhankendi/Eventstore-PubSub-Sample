using System;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
            
            var aggregateStore = new AggregateStore(gesConnection);
            
            

            var orderId = Guid.NewGuid();
            var order = Order.Create(orderId,new CustomerId("alper"));
            
            order.AddItem(new ProductId("ekmek"),1,5 );
            order.AddItem(new ProductId("yumurta"),5,2 );
            order.AddItem(new ProductId("sucuk"),5,2 );

            order.RemoveItem(new ProductId("sucuk"));
            order.Ship();


            await aggregateStore.Save(order);

            Console.WriteLine($"Saved Order=>{order.ToString()}");



            var orderRead = await aggregateStore.Load<Order>(orderId.ToString());
            
            orderRead.Complate();

            await aggregateStore.Save(orderRead);
            
            Console.WriteLine(orderRead);
            
            
            Console.ReadKey();
        }
    }

    public class AggregateStore
    {
        private readonly IEventStoreConnection eventStoreConnection;

        public AggregateStore(IEventStoreConnection eventStoreConnection)
        {
            this.eventStoreConnection = eventStoreConnection;
        }
        public async Task<T> Load<T>(string aggregateId) where T:Aggregate
        {

            var streamName = GetStreamName<T>(aggregateId);
            var events =  await eventStoreConnection.ReadStreamEventsForwardAsync(streamName, 0, 100, false);
            
            var history = events.Events.Select(e =>
            {
                var t = Type.GetType(e.Event.EventType);
                return JsonConvert.DeserializeObject(Encoding.Default.GetString(e.Event.Data), t);

            }).ToArray();

            var aggregate = Activator.CreateInstance<T>();
            
            aggregate.Load(history);

            return aggregate;
        }

        public async Task Save(Aggregate aggregate)
        {
            var changes = aggregate.GetChanges().Select(e =>
            {

                var serializedObject = JsonConvert.SerializeObject(e);

                var data = System.Text.Encoding.Default.GetBytes(serializedObject);

                return new EventData(Guid.NewGuid(), e.GetType().FullName, true, data,null);
            });

            var streamName = GetStreamName(aggregate);
            await eventStoreConnection.AppendToStreamAsync(streamName, aggregate.Version,changes);
           
        }
        
        string GetStreamName(Aggregate aggregate)=>$"{aggregate.GetType().FullName}-{aggregate.Id}";
        
        string GetStreamName<T>(string aggregateId)=>$"{typeof(T).FullName}-{aggregateId}";
    }


    
   
}
