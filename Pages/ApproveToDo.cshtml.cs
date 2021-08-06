using System.Collections.Generic;
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
    [Authorize(Roles="Admin")]
    public class ApproveToDoModel : PageModel
    {
        private readonly ApplicationDbContext _context;// Database connection
        private readonly UserManager<WebUser> _userManager; // To access Microsoft Identity Class

        // Constructor
        public ApproveToDoModel(UserManager<WebUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IList<ToDo> Todos { get; set; }

        [BindProperty]

        public ToDo Todo { get; set; }
        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);// Getting the id of the user who logs into the system
            var user = await _userManager.FindByIdAsync(userId);// All other informations for user
            var roles = await _userManager.GetRolesAsync(user);

            if (roles[0].ToString()== "Admin")
            {
                Todos = await _context.Todos.ToListAsync();// List all todos in the database
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var TaskFromDb = await _context.Todos.FindAsync(Todo.Id);
            TaskFromDb.IsApproved = true;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
