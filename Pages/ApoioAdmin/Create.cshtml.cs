
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.ApoioAdmin
{
    [Authorize(Roles = "admin")]
    public class CreateModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;

        public CreateModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CursoID"] = new SelectList(_context.Curso, "ID", "Name");

            return Page();
        }

        [BindProperty]
        public Cadeira Cadeira { get; set; }
        public string StatusMessage { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_context.Cadeira.Where(S => S.ID == Cadeira.ID).Count() > 0)
            {
                StatusMessage = "Já existe uma cadeira com o Id selecionado";
                ViewData["CursoID"] = new SelectList(_context.Curso, "ID", "Name");

                return Page();
            }

           
             _context.Cadeira.Add(Cadeira);
             await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
