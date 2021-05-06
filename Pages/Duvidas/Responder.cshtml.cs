using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Areas.Identity;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace AsMinhasDuvidas.Pages.Duvidas
{
    [Authorize(Roles = "professor")]
    public class ResponderModel : PageModel
    {

        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResponderModel(AsMinhasDuvidas.Data.ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IConfiguration configuration, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            Configuration = configuration;
        }
        public IList<Duvida> Duvidas { get; set; }
            [BindProperty]
        public Duvida Duvida { get; set; }

        public IConfiguration Configuration { get; }
        public async Task OnGetAsync(int? id)
        {

            Duvida = await _context.Duvida
               .Include(d => d.cadeira)
               .Include(d => d.user).AsNoTracking().FirstOrDefaultAsync(m => m.ID == id);

            Duvidas = await _context.Duvida.Where(S=>S.ID==id).Include(s=>s.cadeira).Include(s => s.user).ToListAsync();

        }

        public void email(string email, string duvida)
        {
            using (var message = new MailMessage(Configuration.GetConnectionString("email"), email))
            {
                message.Subject = "AsMinhasDuvidas Aviso questao respondida";
                message.Body = "A seguinte questão foi respondida (" + duvida+") para vizualisar a resposta diriga-se à página Dúvidas";
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential(Configuration.GetConnectionString("email"), Configuration.GetConnectionString("emailpass"))
                })
                {
                    client.Send(message);

                }
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (Duvida.Resposta==null && Duvida.VizualizarResposta==null)
            {

                return RedirectToPage("./Responder", new { id = Duvida.ID });
            }
            _context.Attach(Duvida).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                IList<ApoiaDuvida> Alunos = await _context.ApoiaDuvida.Where(s => s.DuvidaID == Duvida.ID).Include(s => s.user).ToListAsync();
                foreach(var aluno in Alunos)
                {
                    email(aluno.user.UserName,Duvida.Pergunta);
                }
                   var user = from s in _context.Duvida where s.ID==Duvida.ID
                              select s.user.UserName;
                email(user.First(), Duvida.Pergunta);

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