using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;


namespace SportsStore.Models

{
    //For providing access to EFC's underlying functionality
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        //For accessing the 'Products' object in the DB.
        public DbSet<Product> Products { get; set; }
    }
}