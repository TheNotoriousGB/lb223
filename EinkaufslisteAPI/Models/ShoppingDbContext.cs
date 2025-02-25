using Microsoft.EntityFrameworkCore;


namespace EinkaufslisteAPI.Models
{
    public class ShoppingDbContext(DbContextOptions<ShoppingDbContext> options) : DbContext(options)
    {
        public DbSet<ShoppingItem> ShoppingItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingItem>()
                .HasKey(i => i.Id);

            modelBuilder.Entity<ShoppingItem>()
                .Property(i => i.Name)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<ShoppingItem>()
                .Property(i => i.UserId)
                .IsRequired();

            // Füge die Konfiguration für Quantity hinzu
            modelBuilder.Entity<ShoppingItem>()
                .Property(i => i.Quantity)
                .IsRequired(); // Oder lasse es weg, wenn Quantity optional sein soll
        }

    }
}
