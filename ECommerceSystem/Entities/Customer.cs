using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        // One-to-Many: Customer -> Orders
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
