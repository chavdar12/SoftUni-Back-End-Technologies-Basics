namespace Homies.Data.Models;

public class Event
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OrganiserId { get; set; } = null!;
    public DateTime CreatedOn { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public int TypeId { get; set; }
    public EventType Type { get; set; } = null!;
    public List<EventParticipant> Participants { get; set; } = [];
}

public class EventType
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
}

public class EventParticipant
{
    public string HelperId { get; set; } = null!;
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
}
