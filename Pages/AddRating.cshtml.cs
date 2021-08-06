using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StarCleaningService_Identity.Data;
using StarCleaningService_Identity.Model;

namespace StarCleaningService_Identity.Pages
{
    public class AddRatingModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebUser> _userManager;

        [BindProperty]
        public string Rate { get; set; }

        [BindProperty]
        public bool isAlreadyRated { get; set; }


        [BindProperty]
        public string RatedUSERID { get; set; }//the id of which user will be rated

        public AddRatingModel(ApplicationDbContext context, UserManager<WebUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet(string ratedUserID)
        {
            RatedUSERID = ratedUserID;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            //the id of whoever is currently logged in to the system
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

            var userRating = new UserRating();

            userRating.Rating = Convert.ToInt32(Rate);
            userRating.userId = RatedUSERID;
            userRating.whoRated = userId;

            await _context.UserRatings.AddAsync(userRating);
            await _context.SaveChangesAsync();

            return RedirectToPage("./RateUser");
        }
    }
}
