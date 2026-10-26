using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // One-to-Many: Category -> Products
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
