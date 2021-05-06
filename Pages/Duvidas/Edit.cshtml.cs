using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Data;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.Duvidas
{
    [Authorize(Roles = "aluno, admin")]
    public class EditModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;

        public EditModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Duvida Duvida { get; set; }
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["CadeiraID"] = new SelectList(_context.Cadeira, "ID", "Name");

            Duvida = await _context.Duvida
                .Include(d => d.cadeira)
                .Include(d => d.user).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);
     
            
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

             if (!ModelState.IsValid)
            {
                return Page();
            }
            _context.Attach(Duvida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DuvidaExists(Duvida.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DuvidaExists(int id)
        {
            return _context.Duvida.Any(e => e.ID == id);
        }
    }
}
