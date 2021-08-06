using Microsoft.AspNetCore.Identity;
using System;

namespace StarCleaningService_Identity.Model
{
    public class WebUser:IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }

        [PersonalData]
        public DateTime DOB { get; set; }

        [PersonalData]
        public string Gender { get; set; }

        [PersonalData]
        public string City { get; set; }

        [PersonalData]
        public byte[] Picture { get; set; }
    }
}
