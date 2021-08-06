namespace StarCleaningService_Identity.Model
{
    public class UserRating
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string userId { get; set; }
        public string whoRated { get; set; }
    }
}
