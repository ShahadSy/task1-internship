using System.ComponentModel.DataAnnotations;

namespace internship.ViewModels;

public class UserFormViewModel
{
    public string? Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required]
    public string Role { get; set; } = string.Empty;
}
//This ViewModel is used when creating or editing a user.
