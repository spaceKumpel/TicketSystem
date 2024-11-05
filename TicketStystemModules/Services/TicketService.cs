using Microsoft.EntityFrameworkCore;
using TicketStystemModules.Modules;
namespace TicketStystemModules.Services
{
    public class TicketService
    {
        private readonly TicketContext _context;

        public TicketService(TicketContext context)
        {
            _context = context;
        }
     
        public async Task<PaginatedResponse<Ticket>> GetTicketsAsync(string? title, string titleFilter, DateTime? dateFrom, DateTime? dateTo, string sortBy, string sortOrder, int page, int pageSize)
        {
            var query = _context.Tickets.AsQueryable();

            if (!string.IsNullOrEmpty(title))
            {
                var titleList = title.Split(',');
                query = titleFilter switch
                {
                    "equals" => query.Where(t => t.Title == title),
                    "in" => query.Where(t => titleList.Contains(t.Title)),
                    "not in" => query.Where(t => !titleList.Contains(t.Title)),
                    "not equals" => query.Where(t => t.Title != title),
                    _ => query
                };
            }

            if (dateFrom.HasValue)
                query = query.Where(t => t.CreatedAt >= dateFrom.Value);
            if (dateTo.HasValue)
                query = query.Where(t => t.CreatedAt <= dateTo.Value);

            query = (sortBy.ToLower(), sortOrder.ToLower()) switch
            {
                ("title", "asc") => query.OrderBy(t => t.Title),
                ("title", "desc") => query.OrderByDescending(t => t.Title),
                ("createdat", "asc") => query.OrderBy(t => t.CreatedAt),
                ("createdat", "desc") => query.OrderByDescending(t => t.CreatedAt),
                _ => query.OrderBy(t => t.CreatedAt)
            };

            var totalResults = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);
            var tickets = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedResponse<Ticket>
            {
                Data = tickets,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalResults = totalResults
            };
        }

        public async Task<Ticket?> GetTicketByIdAsync(Guid id) => await _context.Tickets.FindAsync(id);

        public async Task<Ticket> AddTicketAsync(AddTicketDTO data)
        {
            var newTicket = new Ticket
            {
                Id = Guid.NewGuid(),
                Title = data.Title,
                Description = data.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();
            return newTicket;
        }

        public async Task<bool> UpdateTicketAsync(Guid id, EditTicketDTO updateTicket)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is null) return false;

            ticket.Title = updateTicket.Title;
            ticket.Description = updateTicket.Description;

            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTicketAsync(Guid id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket is null) return false;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
