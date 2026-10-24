using System.ComponentModel.DataAnnotations;

namespace Homies.Models;

public class EventFormModel : IValidatableObject
{
    [Required]
    [StringLength(20, MinimumLength = 5)]
    public string Name { get; set; } = "";

    [Required]
    [StringLength(500, MinimumLength = 15)]
    public string Description { get; set; } = "";

    public DateTime Start { get; set; }

    public DateTime End { get; set; }

    public int TypeId { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (End <= Start)
        {
            yield return new ValidationResult("End must be after Start.", [nameof(End)]);
        }
    }
}

public record EventSummary(int Id, string Name, DateTime Start, string Type, string OrganiserId);

public record EventDetails(int Id, string Name, string Description, DateTime Start, DateTime End, DateTime CreatedOn, string Type, string OrganiserId, int ParticipantsCount);

public record TypeViewModel(int Id, string Name);
