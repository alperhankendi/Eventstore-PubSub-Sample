using System;

namespace console_app
{
    public static class V1
    {
        public class Delivered
        {
            public DateTime DeliverTime { get; set; }
        }
        public class Shipping
        {
            public DateTime ShipDate { get; set; }
        }

        public class ItemRemoved
        {
            public ProductId ProductId { get; set; }
        }

        public class ItemAdded
        {
            public ProductId ProductId { get; set; }
            public int Quantity { get; set; }
            public float Price { get; set; }
        }

        public class OrderCreated
        {
            public CustomerId CustomerId { get; set; }
            public Guid OrderId { get; set; }
        }

        public class Cancelled
        {
            public DateTime CancelTime { get; set; }
        }
    }
    
}