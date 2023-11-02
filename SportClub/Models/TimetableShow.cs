namespace SportClub.Models
{
    public class TimetableShow
    {
        public int Id { get; set; }
        public List<string> Times { get; set; } = new();
    }
}
