namespace TicketStystemModules.Modules
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class AddTicketDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class EditTicketDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}