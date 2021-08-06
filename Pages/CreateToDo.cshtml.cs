using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StarCleaningService_Identity.Data;
using StarCleaningService_Identity.Model;

namespace StarCleaningService_Identity.Pages
{
    [Authorize]
    public class CreateToDoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<WebUser> _userManager;
        public CreateToDoModel(UserManager<WebUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [BindProperty]
        public string TodoTask { get; set; }

        [BindProperty]
        public DateTime TodoDeadline { get; set; }

        [BindProperty]
        public string TodoCategory { get; set; }

        [BindProperty]
        public string TodoPriority { get; set; }
        public async Task FillDropDownLists()
        {
            var allCategories = await (from c in _context.TodoCategories orderby c.Category select c.Category).Distinct().ToListAsync();
            var categoryItems = new SelectList(allCategories).ToList();

            //list for categories
            ViewData["DDListCat"] = categoryItems;

            //populate a DrowDownList using ViewData without database
            List<SelectListItem> DDListPri = new()
            {
                new SelectListItem { Text = "Not important", Value = "Not important" },
                new SelectListItem { Text = "Important", Value = "Important" },
                new SelectListItem { Text = "Very important", Value = "Very important" }
            };

            // list for priorities
            ViewData["DDListPri"] = DDListPri;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await FillDropDownLists();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//will give the user's userId
            var userName = User.FindFirstValue(ClaimTypes.Name);//will give the user's userName

            ToDo todo = new();
            todo.IsActive = true;
            todo.IsApproved = false;
            todo.Timestamp = DateTime.Now;
            todo.Task = TodoTask;
            todo.Deadline = TodoDeadline;
            todo.Category = TodoCategory;
            todo.Priority = TodoPriority;

            todo.UserId = userId;
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();//send database
            await FillDropDownLists();//update DropDownLists
            return RedirectToPage("./CreateToDo");
        }
    }
}
