using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;

        // Many-to-One: Book -> Author
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

        // Many-to-Many: Book <-> Borrower (through Loan)
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Borrower> Borrowers { get; set; } = new List<Borrower>(); // skip navigation
    }
}
