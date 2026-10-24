using System.ComponentModel.DataAnnotations;
using Homies.Models;

namespace Homies.Tests;

public class EventFormModelTests
{
    private static List<ValidationResult> Validate(EventFormModel model)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(model, new ValidationContext(model), results, validateAllProperties: true);
        return results;
    }

    private static EventFormModel Valid() => new()
    {
        Name = "Board games",
        Description = "Bring your favourite game",
        Start = new DateTime(2026, 6, 1, 18, 0, 0),
        End = new DateTime(2026, 6, 1, 20, 0, 0),
        TypeId = 1,
    };

    [Test]
    public void ValidModel_HasNoErrors() => Assert.That(Validate(Valid()), Is.Empty);

    [TestCase("Four")]
    [TestCase("This name is way too long")]
    public void Name_OutsideLengthRange_IsInvalid(string name)
    {
        var model = Valid();
        model.Name = name;
        Assert.That(Validate(model).Single().MemberNames, Is.EqualTo(new[] { nameof(EventFormModel.Name) }));
    }

    [Test]
    public void Description_TooShort_IsInvalid()
    {
        var model = Valid();
        model.Description = "Too short";
        Assert.That(Validate(model).Single().MemberNames, Is.EqualTo(new[] { nameof(EventFormModel.Description) }));
    }

    [Test]
    public void End_NotAfterStart_IsInvalid()
    {
        var model = Valid();
        model.End = model.Start;
        Assert.That(Validate(model).Single().MemberNames, Is.EqualTo(new[] { nameof(EventFormModel.End) }));
    }
}
