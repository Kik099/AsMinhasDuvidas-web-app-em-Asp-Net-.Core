using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AsMinhasDuvidas.Data;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.ApoioAdmin
{
    [Authorize(Roles = "admin")]
    public class CriarCursoModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;

        public CriarCursoModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Curso Curso { get; set; }

        public string StatusMessage { get; set; }


        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (_context.Curso.Where(S => S.ID == Curso.ID).Count() > 0)
            {
                StatusMessage = "Já existe um curso com o Id selecionado";
                return Page();
            }
            _context.Curso.Add(Curso);

            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = 2 });
        }
    }
}
