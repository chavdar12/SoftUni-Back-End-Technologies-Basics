using System.ComponentModel.DataAnnotations;

namespace LibroConsoleAPI.Data.Models;

public class Book : IValidatableObject
{
    public const int TitleMaxLength = 255;
    public const int AuthorMaxLength = 100;
    public const int GenreMaxLength = 50;
    public const int YearPublishedMin = 1700;

    public int Id { get; set; }

    [Required]
    [StringLength(TitleMaxLength)]
    public string Title { get; set; } = null!;

    [Required]
    [StringLength(AuthorMaxLength)]
    public string Author { get; set; } = null!;

    [Required]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "ISBN must be exactly 13 digits.")]
    public string ISBN { get; set; } = null!;

    public int YearPublished { get; set; }

    [Required]
    [StringLength(GenreMaxLength)]
    public string Genre { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Pages must be greater than 0.")]
    public int Pages { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public double Price { get; set; }

    // The course skeleton hard-coded 2024 as the max year; use the current year so the check doesn't rot.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (YearPublished < YearPublishedMin || YearPublished > DateTime.UtcNow.Year)
        {
            yield return new ValidationResult(
                $"Year published must be between {YearPublishedMin} and the current year.", [nameof(YearPublished)]);
        }
    }
}
