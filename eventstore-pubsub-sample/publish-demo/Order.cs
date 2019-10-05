using System;
using System.Collections.Generic;
using System.Linq;

namespace console_app
{
    public class Order: Aggregate
    {
        protected override void When(object e)
        {
            switch (e)
            {
                case V1.OrderCreated x:
                    Id = x.OrderId;
                    Status = OrderStatus.Init;
                    CustomerId = x.CustomerId;
                    Details = new List<OrderDetail>();
                    break;
                case V1.ItemAdded x:
                    Details.Add(new OrderDetail
                    {
                        Id = x.ProductId,
                        Price = x.Price,
                        Quantity = x.Quantity
                    });
                    break;
                case V1.ItemRemoved x:
                     
                    var detail = Details.SingleOrDefault(q => q.Id.Id == x.ProductId.Id);
                    Details.Remove(detail);
                    break;
                     
                case V1.Shipping x:
                    ShipDateTime = x.ShipDate;
                    Status = OrderStatus.Shiping;
                    break;
                     
                case V1.Delivered x:
                    DeliverTime = x.DeliverTime;
                    Status = OrderStatus.Delivered;
                    break;
                
                case V1.Cancelled x:
                    CancelationTime = x.CancelTime;
                    Status = OrderStatus.Cancelled;
                    break;
            }
        }

        

        #region Properties
        public DateTime CancelationTime { get; private set; }
        public Order()
        {
            Details= new List<OrderDetail>();
        }
        

        private List<OrderDetail> Details { get; set; }
        
        public CustomerId CustomerId { get; set; }

       
        
        public OrderStatus Status { get; private set; }

        public List<OrderDetail> GetItems => this.Details;

        public DateTime ShipDateTime { get; private set; }
        public DateTime DeliverTime { get; set; }
        #endregion
        
        #region Methods
        public static Order Create(Guid orderId,CustomerId customerId)
        {
            var order = new Order();

            order.Apply(new V1.OrderCreated
            {
                OrderId=orderId,
                CustomerId = customerId
            });
            return order;

        }
        public void AddItem(ProductId productId, int q, float price)
        {
            Apply(new V1.ItemAdded
            {
                ProductId=productId,
                Quantity=q,
                Price=price
            });
            
            
        }
        public void RemoveItem(ProductId productId)
        {
            Apply(new V1.ItemRemoved
            {
                ProductId=productId
            });
        }

        public void Ship()
        {
            Apply(new V1.Shipping
            {
                ShipDate=DateTime.UtcNow
            });
        }

        public void Complate()
        {
            Apply(new V1.Delivered
            {
                DeliverTime = DateTime.UtcNow
            });
        }

        public void Cancel()
        {
            Apply(new V1.Cancelled
            {
                CancelTime = DateTime.UtcNow
            });
        }
        
        public override string ToString()
        {
            return $"OrderId:{Id} \n Status:{Status}\n Version:{Version}";
        }
        #endregion
    }
    
    public class OrderDetail
    {
        public ProductId Id { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }

    public class ProductId
    {
        public ProductId(string id)
        {
            Id = id;
        }
        public string Id { get; set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public class CustomerId
    {
        public string Id { get; set; }

        public CustomerId(string id)
        {
            Id = id;
        }
            
    }
    
    public enum OrderStatus
    {
        Init,
        Shiping,
        Delivered,
        Cancelled
    }
}