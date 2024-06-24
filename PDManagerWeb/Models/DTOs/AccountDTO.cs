namespace PDManagerWeb.Models.DTOs
{
    public record AccountDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public bool IsDeleted { get; set; }
    }
}
