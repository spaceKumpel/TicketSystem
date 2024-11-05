using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.Extensions.Configuration;
namespace TicketStystemModules.Modules
{
    public class TicketContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public TicketContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .Property(t => t.Id)
                .HasColumnType("uuid");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("TicketDatabase"));
        }

        public DbSet<Ticket> Tickets { get; set; }
    }

}
