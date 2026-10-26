using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerceSystem.Entities
{

    // Join entity with payload (Quantity)
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
