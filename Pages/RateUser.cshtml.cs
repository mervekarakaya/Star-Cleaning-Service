using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StarCleaningService_Identity.Data;
using StarCleaningService_Identity.Model;

namespace StarCleaningService_Identity.Pages
{
    [Authorize]
    public class RateUserModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebUser> _userManager;

        public RateUserModel(ApplicationDbContext context, UserManager<WebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string RatedUser { get; set; }


        public List<RatedUser> UserRatingsList { get; set; }

        public async Task ListRatings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the logged-in user's userId

            //Get all users without the logged-in user
            var AllUsers = await (from u in _context.Users
                                  where u.Id != userId
                                  select u).ToListAsync();
            //Get all ratings
            var AllRatings = await (from r in _context.UserRatings
                                    select r).ToListAsync();

            //Intialize and fill [BindPropoerty] UserRatingList containing user information and average ratings 
            UserRatingsList = new();

            foreach (var item in AllUsers)
            {
                //if user has roles (note that in this application only admins have a role), do not add this user to the list
                var user = await _userManager.FindByIdAsync(item.Id);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count >= 1)//if logged user is admin
                {
                    continue;
                }
                //find all ratings of the current user in foreach loop
                List<int> userrate = await (from x in _context.UserRatings where x.userId == item.Id select x.Rating).ToListAsync();
                var userating = new RatedUser();
                if (userrate.Count >= 1)//if this user has at least one rating
                {
                    //calculate current user's average rating
                    double avg = CalculateRate.GetRating(userrate);
                   
                    //set values
                    userating.AvgRating = avg.ToString();
                    userating.UserId = item.Id;
                    userating.Name = item.Name;
                    userating.PhoneNumber = item.PhoneNumber;
                    userating.Picture = item.Picture;
                    userating.Email = item.Email;
                    UserRatingsList.Add(userating);
                }
                else  //if user has no ratings do not calculate average
                {
                    //set values
                    userating.AvgRating = "0";
                    userating.UserId = item.Id;
                    userating.Name = item.Name;
                    userating.PhoneNumber = item.PhoneNumber;
                    userating.Picture = item.Picture;
                    userating.Email = item.Email;
                    UserRatingsList.Add(userating);
                }
            }
        }

        public async Task OnGetAsync()
        {
            //List users and their average ratings
            await ListRatings();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            //List users and their average ratings in each post
            await ListRatings();

            return Page();
        }

    }
}
