using internship.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace internship.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext(options)
{
    //This line creates the Products table:
    public DbSet<Product> Products { get; set; }
}

//This is the bridge between C# models and database tables.