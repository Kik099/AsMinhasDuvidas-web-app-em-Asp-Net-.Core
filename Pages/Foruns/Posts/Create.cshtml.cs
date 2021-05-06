using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsMinhasDuvidas.Models;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Data;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.Foruns.Posts
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
           
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Forum = await _context.Forum.Where(s => s.Id == id)
                .Include(d => d.user).ToListAsync();

            return Page();
        }

        public IList<Forum> Forum { get; set; }

        [BindProperty]
        public ForumResposta Post { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Post.UserID = _context.Users.Where(s => s.UserName == User.Identity.Name).First().Id;
            Post.ForumID = id;
            _context.Post.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index",new { id=id});
        }
    }
}
