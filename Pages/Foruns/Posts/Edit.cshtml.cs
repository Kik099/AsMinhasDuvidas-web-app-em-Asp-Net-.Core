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
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
           
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Post= await _context.Post.Include(d => d.Forum)
                .Include(d => d.user).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            post = await _context.Post.Where(s => s.Id == id)
                .Include(d => d.user).ToListAsync();

            return Page();
        }

        public IList<ForumResposta> post { get; set; }
        [BindProperty]
        public ForumResposta Post { get; set; }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(Post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(Post.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id = Post.ForumID });
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Post = await _context.Post.FindAsync(id);

            if (Post != null)
            {
                _context.Post.Remove(Post);
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./Index", new { id=Post.ForumID});
        }
    }
}
