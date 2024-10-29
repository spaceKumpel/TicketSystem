using Microsoft.EntityFrameworkCore;
namespace TicketStystemModules.Modules
{
    public class TicketContext : DbContext
    {
        public TicketContext(DbContextOptions options):base(options){ }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
