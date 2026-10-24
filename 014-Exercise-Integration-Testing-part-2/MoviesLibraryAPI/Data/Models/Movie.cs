using System.ComponentModel.DataAnnotations;

namespace MoviesLibraryAPI.Data.Models;

public class Movie : IValidatableObject
{
    public const int YearReleasedMin = 1900;

    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(100)]
    public string Director { get; set; } = null!;

    public int YearReleased { get; set; }

    [Required]
    [StringLength(100)]
    public string Genre { get; set; } = null!;

    [Range(1, 500)]
    public int Duration { get; set; }

    [Range(0.0, 10.0)]
    public double Rating { get; set; }

    // Allow announced movies a few years ahead instead of a hard-coded max year.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (YearReleased < YearReleasedMin || YearReleased > DateTime.UtcNow.Year + 5)
        {
            yield return new ValidationResult("Year released is out of range.", [nameof(YearReleased)]);
        }
    }
}
