using Microsoft.EntityFrameworkCore;
using LibrarySystem.Entities;
using LibrarySystem.Data;

using var db = new LibraryContext();
db.Database.Migrate();

if (!db.Authors.Any())
{
    var author = new Author { Name = "Naguib Mahfouz", BirthDate = new DateTime(1911, 12, 11) };
    var b1 = new Book { Title = "Palace Walk",     ISBN = "978-0385264662", Author = author };
    var b2 = new Book { Title = "Palace of Desire", ISBN = "978-0385264686", Author = author };

    var ali = new Borrower { Name = "Ali Hassan", MembershipDate = new DateTime(2024, 1, 15) };
    var sara = new Borrower { Name = "Sara Ahmed", MembershipDate = new DateTime(2025, 3, 2) };

    var loans = new[]
    {
        new Loan { Book = b1, Borrower = ali,  LoanDate = new DateTime(2026, 9, 1), ReturnDate = new DateTime(2026, 9, 15) },
        new Loan { Book = b2, Borrower = ali,  LoanDate = new DateTime(2026, 10, 1) },
        new Loan { Book = b1, Borrower = sara, LoanDate = new DateTime(2026, 10, 10) }
    };

    db.AddRange(author, b1, b2, ali, sara);
    db.AddRange(loans);
    db.SaveChanges();
}

// Author -> Books
foreach (var a in db.Authors.Include(a => a.Books))
    Console.WriteLine($"{a.Name}: {string.Join(", ", a.Books.Select(b => b.Title))}");

// Book -> Loans -> Borrower
Console.WriteLine();
foreach (var b in db.Books.Include(b => b.Loans).ThenInclude(l => l.Borrower))
    foreach (var l in b.Loans)
        Console.WriteLine($"'{b.Title}' borrowed by {l.Borrower.Name} on {l.LoanDate:d} " +
                          (l.ReturnDate is null ? "(not returned)" : $"returned {l.ReturnDate:d}"));

// Skip navigation: all books a borrower has ever taken
var ali2 = db.Borrowers.Include(b => b.Books).First(b => b.Name == "Ali Hassan");
Console.WriteLine($"\n{ali2.Name} read: {string.Join(", ", ali2.Books.Select(b => b.Title))}");
