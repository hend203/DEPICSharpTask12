using Microsoft.EntityFrameworkCore;
using ECommerceSystem;
using ECommerceSystem.Entities;
using ECommerceSystem.Data;

using var db = new ECommerceContext();

db.Database.Migrate();   // applies migrations (dotnet ef migrations add InitialCreate first)

if (!db.Categories.Any())
{
    var electronics = new Category { Name = "Electronics" };
    var books = new Category { Name = "Books" };

    var laptop = new Product { Name = "Laptop", Price = 25000m, Category = electronics };
    var phone  = new Product { Name = "Phone",  Price = 12000m, Category = electronics };
    var novel  = new Product { Name = "Novel",  Price = 150m,   Category = books };

    var hend = new Customer { Name = "Hend Mohamed", Email = "hend@example.com" };

    var order = new Order { OrderDate = DateTime.Now, Customer = hend };
    order.OrderDetails.Add(new OrderDetail { Product = laptop, Quantity = 1 });
    order.OrderDetails.Add(new OrderDetail { Product = novel,  Quantity = 3 });

    db.AddRange(electronics, books, laptop, phone, novel, hend, order);
    db.SaveChanges();
}

// Navigation properties in action: Customer -> Orders -> OrderDetails -> Product -> Category
var customers = db.Customers
    .Include(c => c.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.Product).ThenInclude(p => p.Category)
    .ToList();

foreach (var c in customers)
{
    Console.WriteLine($"Customer: {c.Name} ({c.Email})");
    foreach (var o in c.Orders)
    {
        Console.WriteLine($"  Order #{o.Id} on {o.OrderDate:d}");
        foreach (var od in o.OrderDetails)
            Console.WriteLine($"    {od.Quantity} x {od.Product.Name} [{od.Product.Category.Name}] @ {od.Product.Price:C}");
    }
}

// One-to-Many: products of a category
var cat = db.Categories.Include(c => c.Products).First(c => c.Name == "Electronics");
Console.WriteLine($"\nCategory {cat.Name}: {string.Join(", ", cat.Products.Select(p => p.Name))}");

// Skip navigation: products in order #1 without touching OrderDetail
var o1 = db.Orders.Include(o => o.Products).First();
Console.WriteLine($"Order #{o1.Id} products: {string.Join(", ", o1.Products.Select(p => p.Name))}");
