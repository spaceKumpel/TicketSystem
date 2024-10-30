using Microsoft.AspNetCore.Mvc;
using TicketStystemModules.Modules;
using TicketStystemModules.Services;

[ApiController]
[Route("api/[controller]")]
public class TicketController : ControllerBase
{
    private readonly TicketService _ticketService;

    public TicketController(TicketService ticketService)
    {
        _ticketService = ticketService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTickets([FromQuery] string? title, [FromQuery] string titleFilter = "equals",
                                                [FromQuery] DateTime? dateFrom = null, [FromQuery] DateTime? dateTo = null,
                                                [FromQuery] string sortBy = "CreatedAt", [FromQuery] string sortOrder = "asc",
                                                [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _ticketService.GetTicketsAsync(title, titleFilter, dateFrom, dateTo, sortBy, sortOrder, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(Guid id)
    {
        var ticket = await _ticketService.GetTicketByIdAsync(id);
        if (ticket is null) return NotFound();
        return Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> AddTicket(AddTicketDTO data)
    {
        var newTicket = await _ticketService.AddTicketAsync(data);
        return CreatedAtAction(nameof(GetTicketById), new { id = newTicket.Id }, newTicket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicket(Guid id, EditTicketDTO updateTicket)
    {
        var result = await _ticketService.UpdateTicketAsync(id, updateTicket);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(Guid id)
    {
        var result = await _ticketService.DeleteTicketAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}