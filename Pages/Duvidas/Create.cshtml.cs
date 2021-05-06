using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace AsMinhasDuvidas.Pages.Duvidas
{
    [Authorize(Roles = "aluno")]
    public class CreateModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;

        public CreateModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            var cadeirasinscritas = _context.MatriculaAluno.Where(S => S.user.UserName == User.Identity.Name).Select(s=>s.cadeira);

            ViewData["CadeiraID"] = new SelectList(cadeirasinscritas, "ID", "Name");
            return Page();
        }

        [BindProperty]
        public Duvida Duvida { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Duvida.Data = DateTime.Now;
            var id= _context.Duvida.Count() + 1;
            Duvida.ID = id;
            var user = (from s in _context.Users select s).Where(s => s.UserName == User.Identity.Name).ToList();
            Duvida.UserID = user.First().Id;
            _context.Duvida.Add(Duvida);
            await _context.SaveChangesAsync();


            ApoiaDuvida duvida = new ApoiaDuvida()
            {
                UserID = user.First().Id,
                DuvidaID = id
            };

            _context.ApoiaDuvida.Add(duvida);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
