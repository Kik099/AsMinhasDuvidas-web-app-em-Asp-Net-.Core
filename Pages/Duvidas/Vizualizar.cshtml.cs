using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Areas.Identity;
using AsMinhasDuvidas.Models;
using AsMinhasDuvidas.Funcoes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsMinhasDuvidas.Pages.Duvidas
{
    [Authorize(Roles = "aluno, professor")]
    public class VizualizarModel : PageModel
    {

        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public VizualizarModel(AsMinhasDuvidas.Data.ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IList<Duvida> Duvidas { get; set; }

        [BindProperty]
        public ApoiaDuvida ApoiaDuvida { get; set; }
        public async Task OnGetAsync(int? id)
        {



            Duvidas = await _context.Duvida.Where(S=>S.ID==id).Include(s=>s.cadeira).Include(s => s.user).ToListAsync();

        }
        public async Task<IActionResult> OnPostDislikeAsync(int id)
        {
            var user = _context.Users.Where(s => s.UserName == User.Identity.Name).ToList();
            var duvida = _context.ApoiaDuvida.Where(s => s.DuvidaID == id).Where(s => s.UserID == user.First().Id).ToList();
            ApoiaDuvida = await _context.ApoiaDuvida.FindAsync(duvida.First().ID);
            _context.ApoiaDuvida.Remove(ApoiaDuvida);
            await _context.SaveChangesAsync();


            return RedirectToPage("./Vizualizar", new { id = id });
        }

        public async Task<IActionResult> OnPostLikeAsync(int id)
        {
            var user = _context.Users.Where(s => s.UserName == User.Identity.Name).ToList();
            ApoiaDuvida duvida = new ApoiaDuvida()
            {
                UserID= user.First().Id,
                 DuvidaID = id
             };
            
              _context.ApoiaDuvida.Add(duvida);
            await _context.SaveChangesAsync();




            return RedirectToPage("./Vizualizar", new { id = id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Duvida = await _context.Duvida.FindAsync(id);

            if (Duvida != null )
            {
                _context.Duvida.Remove(Duvida);
                await _context.SaveChangesAsync();
            }

            var matriculas = await _context.ApoiaDuvida.Where(S => S.DuvidaID == id).ToListAsync();
            foreach(var mat in matriculas)
            {
                _context.ApoiaDuvida.Remove(mat);
            }
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}