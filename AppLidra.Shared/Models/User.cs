using System.ComponentModel.DataAnnotations;

namespace AppLidra.Shared.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    [MinLength(6)]
    public string? Password { get; set; }
}
