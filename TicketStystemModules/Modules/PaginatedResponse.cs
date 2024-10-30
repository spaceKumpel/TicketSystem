namespace TicketStystemModules.Modules
{
    public class PaginatedResponse<T>
    {
        public int CurrentPage { get; set; }
        public int ResultsPerPage { get; set; }
        public int TotalPages { get; set; }
        public long TotalResults { get; set; }
        public List<T> Data { get; set; }
    }
}
