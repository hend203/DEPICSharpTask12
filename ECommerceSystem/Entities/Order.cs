using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        // Many-to-One: Order -> Customer
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        // Many-to-Many: Order <-> Product (through OrderDetail)
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Product> Products { get; set; } = new List<Product>(); // skip navigation
    }
}
