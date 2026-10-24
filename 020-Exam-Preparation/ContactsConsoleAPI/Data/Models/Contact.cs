using System.ComponentModel.DataAnnotations;

namespace ContactsConsoleAPI.Data.Models;

public class Contact
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = null!;

    [Required]
    [MaxLength(15)]
    [RegularExpression(@"^\+?[0-9 ]+$", ErrorMessage = "Phone may contain only digits, spaces and a leading +.")]
    public string Phone { get; set; } = null!;

    [Required]
    [EmailAddress]
    [MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required]
    [MaxLength(10)]
    public string Gender { get; set; } = null!;

    [Required]
    [RegularExpression("^[0-9A-Z]{10,30}$", ErrorMessage = "ULID must be 10 to 30 uppercase letters or digits.")]
    public string ULID { get; set; } = null!;
}
