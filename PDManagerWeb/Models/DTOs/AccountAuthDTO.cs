namespace PDManagerWeb.Models.DTOs
{
    public record AccountAuthDTO
    {
        public string Login { get; set; }

        public string Password { get; set; }
    }
}
