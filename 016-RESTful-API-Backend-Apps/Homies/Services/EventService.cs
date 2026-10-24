using Homies.Data;
using Homies.Data.Models;
using Homies.Models;
using Microsoft.EntityFrameworkCore;

namespace Homies.Services;

public class EventService(HomiesDbContext context, TimeProvider timeProvider) : IEventService
{
    public async Task<int> AddEventAsync(EventFormModel model, string userId)
    {
        await EnsureTypeExistsAsync(model.TypeId);
        var entity = new Event
        {
            Name = model.Name,
            Description = model.Description,
            OrganiserId = userId,
            CreatedOn = timeProvider.GetUtcNow().UtcDateTime,
            Start = model.Start,
            End = model.End,
            TypeId = model.TypeId,
        };
        context.Events.Add(entity);
        await context.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<EventSummary>> GetAllEventsAsync() =>
        await context.Events
            .OrderBy(e => e.Start)
            .Select(e => new EventSummary(e.Id, e.Name, e.Start, e.Type.Name, e.OrganiserId))
            .ToListAsync();

    public Task<EventDetails?> GetEventDetailsAsync(int eventId) =>
        context.Events
            .Where(e => e.Id == eventId)
            .Select(e => new EventDetails(e.Id, e.Name, e.Description, e.Start, e.End, e.CreatedOn, e.Type.Name, e.OrganiserId, e.Participants.Count))
            .FirstOrDefaultAsync();

    public Task<string?> GetEventOrganiserIdAsync(int eventId) =>
        context.Events.Where(e => e.Id == eventId).Select(e => e.OrganiserId).FirstOrDefaultAsync();

    public async Task<bool> UpdateEventAsync(int eventId, EventFormModel model, string userId)
    {
        var entity = await context.Events.FindAsync(eventId);
        if (entity == null || entity.OrganiserId != userId)
        {
            return false;
        }

        await EnsureTypeExistsAsync(model.TypeId);
        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Start = model.Start;
        entity.End = model.End;
        entity.TypeId = model.TypeId;
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> JoinEventAsync(int eventId, string userId)
    {
        if (!await context.Events.AnyAsync(e => e.Id == eventId) ||
            await context.EventsParticipants.AnyAsync(ep => ep.EventId == eventId && ep.HelperId == userId))
        {
            return false;
        }

        context.EventsParticipants.Add(new EventParticipant { EventId = eventId, HelperId = userId });
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> LeaveEventAsync(int eventId, string userId) =>
        await context.EventsParticipants.Where(ep => ep.EventId == eventId && ep.HelperId == userId).ExecuteDeleteAsync() > 0;

    public async Task<IEnumerable<EventSummary>> GetUserJoinedEventsAsync(string userId) =>
        await context.EventsParticipants
            .Where(ep => ep.HelperId == userId)
            .OrderBy(ep => ep.Event.Start)
            .Select(ep => new EventSummary(ep.Event.Id, ep.Event.Name, ep.Event.Start, ep.Event.Type.Name, ep.Event.OrganiserId))
            .ToListAsync();

    public async Task<IEnumerable<TypeViewModel>> GetAllTypesAsync() =>
        await context.Types.OrderBy(t => t.Id).Select(t => new TypeViewModel(t.Id, t.Name)).ToListAsync();

    private async Task EnsureTypeExistsAsync(int typeId)
    {
        if (!await context.Types.AnyAsync(t => t.Id == typeId))
        {
            throw new ArgumentException($"Event type {typeId} does not exist.", nameof(typeId));
        }
    }
}
