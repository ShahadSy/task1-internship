using System.ComponentModel.DataAnnotations;

namespace internship.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 999999.99)]
    public decimal Price { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }
}

//This represents one Product in C# and in the database.