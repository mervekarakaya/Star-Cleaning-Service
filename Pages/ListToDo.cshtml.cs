using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using StarCleaningService_Identity.Data;
using StarCleaningService_Identity.Model;

namespace StarCleaningService_Identity.Pages
{
    [Authorize]
    public class ListToDoModel : PageModel
    {
        private readonly ApplicationDbContext _context;// Database connection
        private readonly UserManager<WebUser> _userManager; // To access Microsoft Identity Class

        //Constructor
        public ListToDoModel(UserManager<WebUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IList<ToDo> Todos { get; set; } // To store list of todo in database

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// Getting the id of the user who logs into the system
            var user = await _userManager.FindByIdAsync(userId);// All other informations for user
            var roles = await _userManager.GetRolesAsync(user);
            // if user is admin
            if (roles.Count != 0)
            {
                if (roles[0].ToString() == "Admin")
                {
                    Todos = await _context.Todos.ToListAsync();// List all todos in the database
                }
            }
                
            // if user is not admin
            else
            {
                Todos = await _context.Todos.Where(u => u.UserId.Equals(userId))
                                            .Where(t => t.IsApproved==true).ToListAsync();
            }
            
        }
    }
}
