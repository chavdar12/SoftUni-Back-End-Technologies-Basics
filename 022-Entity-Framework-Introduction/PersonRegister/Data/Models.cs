using System.ComponentModel.DataAnnotations;

namespace PersonRegister.Data;

public class Town
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    public List<Person> People { get; set; } = [];
}

public class Person
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Range(0, 150)]
    public int Age { get; set; }

    [MaxLength(100)]
    [EmailAddress]
    public string? Email { get; set; }

    public int TownId { get; set; }
    public Town Town { get; set; } = null!;
}
