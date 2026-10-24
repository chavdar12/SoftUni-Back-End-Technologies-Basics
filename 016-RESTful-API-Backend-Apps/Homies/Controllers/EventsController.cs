using Homies.Models;
using Homies.Services;
using Microsoft.AspNetCore.Mvc;

namespace Homies.Controllers;

// shortcut: the caller identifies itself with an X-User-Id header instead of real authentication; add ASP.NET Identity/JWT before exposing this publicly.
[ApiController]
[Route("api/events")]
public class EventsController(IEventService eventService) : ControllerBase
{
    private const string UserHeader = "X-User-Id";

    [HttpGet]
    public async Task<IEnumerable<EventSummary>> All() => await eventService.GetAllEventsAsync();

    [HttpGet("types")]
    public async Task<IEnumerable<TypeViewModel>> Types() => await eventService.GetAllTypesAsync();

    [HttpGet("joined")]
    public async Task<IEnumerable<EventSummary>> Joined([FromHeader(Name = UserHeader)] string userId) =>
        await eventService.GetUserJoinedEventsAsync(userId);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EventDetails>> Details(int id) =>
        await eventService.GetEventDetailsAsync(id) is { } details ? details : NotFound();

    [HttpPost]
    public async Task<IActionResult> Add(EventFormModel model, [FromHeader(Name = UserHeader)] string userId)
    {
        try
        {
            var id = await eventService.AddEventAsync(model, userId);
            return CreatedAtAction(nameof(Details), new { id }, new { id });
        }
        catch (ArgumentException ex)
        {
            return ValidationError(nameof(model.TypeId), ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Edit(int id, EventFormModel model, [FromHeader(Name = UserHeader)] string userId)
    {
        var organiserId = await eventService.GetEventOrganiserIdAsync(id);
        if (organiserId == null)
        {
            return NotFound();
        }
        if (organiserId != userId)
        {
            return StatusCode(StatusCodes.Status403Forbidden);
        }

        try
        {
            await eventService.UpdateEventAsync(id, model, userId);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return ValidationError(nameof(model.TypeId), ex.Message);
        }
    }

    [HttpPost("{id:int}/join")]
    public async Task<IActionResult> Join(int id, [FromHeader(Name = UserHeader)] string userId)
    {
        if (await eventService.GetEventOrganiserIdAsync(id) == null)
        {
            return NotFound();
        }
        return await eventService.JoinEventAsync(id, userId) ? NoContent() : Conflict("Already joined.");
    }

    [HttpPost("{id:int}/leave")]
    public async Task<IActionResult> Leave(int id, [FromHeader(Name = UserHeader)] string userId) =>
        await eventService.LeaveEventAsync(id, userId) ? NoContent() : NotFound();

    private IActionResult ValidationError(string field, string message)
    {
        ModelState.AddModelError(field, message);
        return ValidationProblem(ModelState);
    }
}
