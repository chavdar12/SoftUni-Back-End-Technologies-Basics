using Homies.Models;

namespace Homies.Services;

public interface IEventService
{
    Task<int> AddEventAsync(EventFormModel model, string userId);
    Task<IEnumerable<EventSummary>> GetAllEventsAsync();
    Task<EventDetails?> GetEventDetailsAsync(int eventId);
    Task<string?> GetEventOrganiserIdAsync(int eventId);
    Task<bool> UpdateEventAsync(int eventId, EventFormModel model, string userId);
    Task<bool> JoinEventAsync(int eventId, string userId);
    Task<bool> LeaveEventAsync(int eventId, string userId);
    Task<IEnumerable<EventSummary>> GetUserJoinedEventsAsync(string userId);
    Task<IEnumerable<TypeViewModel>> GetAllTypesAsync();
}
