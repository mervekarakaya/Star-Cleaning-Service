using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarCleaningService_Identity.Model
{
    public class RatedUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] Picture { get; set; }
        public string AvgRating { get; set; }
        public string UserId { get; set; }
        public bool isRated { get; set; }
    }
}
