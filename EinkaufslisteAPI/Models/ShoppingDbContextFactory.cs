
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;
    using System.IO;

    namespace EinkaufslisteAPI.Models
    {
        public class ShoppingDbContextFactory : IDesignTimeDbContextFactory<ShoppingDbContext>
        {
            public ShoppingDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                var optionsBuilder = new DbContextOptionsBuilder<ShoppingDbContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);

                return new ShoppingDbContext(optionsBuilder.Options);
            }
        }
    }

