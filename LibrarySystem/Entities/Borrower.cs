using System;
using System.Collections.Generic;
using System.Text;

namespace LibrarySystem.Entities
{

    public class Borrower
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime MembershipDate { get; set; }

        // Many-to-Many: Borrower <-> Book (through Loan)
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public ICollection<Book> Books { get; set; } = new List<Book>();   // skip navigation
    }
}
