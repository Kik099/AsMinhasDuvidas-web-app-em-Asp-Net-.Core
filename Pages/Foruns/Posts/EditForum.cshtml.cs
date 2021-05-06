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
    public class EditForumModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditForumModel(ApplicationDbContext context)
        {
            _context = context;
           
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Forum= await _context.Forum.Include(d => d.user).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);


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
            _context.Attach(Forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(Forum.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index", new { id=Forum.Id});
        }

        private bool ForumExists(int id)
        {
            return _context.Forum.Any(e => e.Id == id);
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Forum = await _context.Forum.FindAsync(id);

            if (Forum != null)
            {
                _context.Forum.Remove(Forum);
                await _context.SaveChangesAsync();
            }

            var posts = _context.Post.Where(S => S.ForumID == id).ToList();
            foreach(var item in posts)
            {
                
                    _context.Post.Remove(item);
                    await _context.SaveChangesAsync();
                
            }


            return RedirectToPage("/Foruns/Index");
        }

        public async Task<IActionResult> OnPostFecharAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Forum = _context.Forum.Where(s => s.Id == id).First();
            Forum.Aberto = false;
            _context.Attach(Forum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ForumExists(Forum.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/Foruns/Index");
        }
    }
}
