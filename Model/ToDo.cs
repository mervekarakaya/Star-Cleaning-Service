using System;

namespace StarCleaningService_Identity.Model
{
    public class ToDo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Task { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string Category { get; set; }
        public string Priority { get; set; }

    }
}
