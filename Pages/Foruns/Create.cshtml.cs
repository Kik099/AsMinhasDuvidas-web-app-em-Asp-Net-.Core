using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.Foruns
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;

        public CreateModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Forum Forum { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Forum.data = DateTime.Now;
            var user = await _context.Users.Where(s => s.UserName == User.Identity.Name).ToListAsync();
            Forum.UserID = user.First().Id;
            Forum.Aberto = true;
            _context.Forum.Add(Forum);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
