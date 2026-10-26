using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }

        // Many-to-One: Product -> Category
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        // Many-to-Many: Product <-> Order (through OrderDetail)
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();   // skip navigation
    }
}
