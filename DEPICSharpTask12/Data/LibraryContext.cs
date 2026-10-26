using LibrarySystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Data;

public class LibraryContext : DbContext
{
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Borrower> Borrowers => Set<Borrower>();
    public DbSet<Loan> Loans => Set<Loan>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=Library;Trusted_Connection=true;TrustServerCertificate=true;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(e =>
        {
            e.Property(a => a.Name).IsRequired().HasMaxLength(100);
            e.Property(a => a.BirthDate).HasColumnType("date");
        });

        modelBuilder.Entity<Borrower>(e =>
        {
            e.Property(b => b.Name).IsRequired().HasMaxLength(100);
            e.Property(b => b.MembershipDate).HasColumnType("date");
        });

        modelBuilder.Entity<Book>(e =>
        {
            e.ToTable("Book");
            e.Property(b => b.Title).IsRequired().HasMaxLength(200);
            e.Property(b => b.ISBN).IsRequired().HasMaxLength(20);
            e.HasIndex(b => b.ISBN).IsUnique();

            // One-to-Many: Author -> Book
            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Many-to-Many: Book <-> Borrower through Loan
        modelBuilder.Entity<Book>()
            .HasMany(b => b.Borrowers)
            .WithMany(br => br.Books)
            .UsingEntity<Loan>(
                right => right.HasOne(l => l.Borrower)
                              .WithMany(br => br.Loans)
                              .HasForeignKey(l => l.BorrowerId),
                left => left.HasOne(l => l.Book)
                            .WithMany(b => b.Loans)
                            .HasForeignKey(l => l.BookId),
                join =>
                {
                    // LoanDate is part of the key so the same borrower can borrow the same book again later
                    join.HasKey(l => new { l.BookId, l.BorrowerId, l.LoanDate });
                    join.Property(l => l.LoanDate).HasColumnType("date");
                    join.Property(l => l.ReturnDate).HasColumnType("date");
                    join.ToTable("Loans");
                });
    }
}
