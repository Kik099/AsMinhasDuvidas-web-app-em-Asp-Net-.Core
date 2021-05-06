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
using Moq;

namespace AsMinhasDuvidas.Pages.Duvidas
{
    [Authorize(Roles = "aluno, professor")]
    public class IndexModel : PageModel
    {

        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(AsMinhasDuvidas.Data.ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public string DataSort { get; set; }

        public string VerTodas { get; set; }
        public string MinhasDuvidasSort { get; set; }

        public string CurrentFilterTodas { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentFilterCadeira { get; set; }
        public string SearchStringCadeira { get; set; }

        public SelectList Cadeiras { get; set; }
        public string CurrentFilterData { get; set; }

        public string CurrentSort { get; set; }
        public DateTime DataFim { get; set; }
        public DateTime DataInicio { get; set; }
        public PaginatedList<Duvida> Duvidas { get; set; }
        public IQueryable<Duvida> DuvidaIQ { get; set; }
        [BindProperty]
        public ApoiaDuvida ApoiaDuvida { get; set; }
        [BindProperty]
        public string Topico { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [DataType(DataType.Date)]
            public string dataInicio { get; set; }

            [DataType(DataType.Date)]
            public string dataFim { get; set; }

            public string cadeira { get; set; }

        }

        public async Task OnGetAsync(string sortOrder, string currentFilter, int? pageIndex, string searchStringTopico, string searchStringCadeira, DateTime? From, DateTime? To)
        {
            if (sortOrder != null && CurrentSort != null)
                CurrentSort = sortOrder;
            if (sortOrder == null && CurrentSort != null)
                sortOrder = CurrentSort;
            if (sortOrder != null && CurrentSort == null)
                CurrentSort = sortOrder;



            // using System;
            DataSort = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            VerTodas = String.IsNullOrEmpty(sortOrder) ? "todas" : "";
            MinhasDuvidasSort = sortOrder == "minhasduvidasativas" ? "minhasduvidas" : "minhasduvidasativas"; ;
            if (DuvidaIQ == null)
            {
                if (User.IsInRole("professor"))
                {
                    List<int> values = _context.MatriculaProfessor.Where(s => s.user.UserName == User.Identity.Name).Select(s => s.CadeiraID).ToList();
                    DuvidaIQ = (from s in _context.Duvida
                                select s).Include(q => q.cadeira).Include(q => q.user);


                    DuvidaIQ = DuvidaIQ.Where(p => values.Contains(p.CadeiraID));
                }
                if (User.IsInRole("aluno"))
                {
                    List<int> values = _context.MatriculaAluno.Where(s => s.user.UserName == User.Identity.Name).Select(s => s.CadeiraID).ToList();
                    DuvidaIQ = (from s in _context.Duvida
                                select s).Include(q => q.cadeira).Include(q => q.user);
                    DuvidaIQ = DuvidaIQ.Where(p => values.Contains(p.CadeiraID));

                }

              


            }

            switch (sortOrder)
            {
                case "date_desc":
                    DuvidaIQ = DuvidaIQ.OrderByDescending(s => s.Data);
                    break;
                case "minhasduvidas":

                    DuvidaIQ = (from s in _context.Duvida
                                select s).Include(q => q.cadeira).Include(q => q.user);

                    break;
                case "minhasduvidasativas":
                    DuvidaIQ = _context.ApoiaDuvida.Where(s => s.user.UserName == User.Identity.Name).Select(s=>s.duvida);

                    
                    break;
                case "todas":
                    if (User.IsInRole("professor"))
                    {
                        List<int> values = _context.MatriculaProfessor.Where(s => s.user.UserName == User.Identity.Name).Select(s => s.CadeiraID).ToList();
                        DuvidaIQ = (from s in _context.Duvida
                                    select s).Include(q => q.cadeira).Include(q => q.user);


                        DuvidaIQ = DuvidaIQ.Where(p => values.Contains(p.CadeiraID));
                    }
                    if (User.IsInRole("aluno"))
                    {
                        List<int> values = _context.MatriculaAluno.Where(s => s.user.UserName == User.Identity.Name).Select(s => s.CadeiraID).ToList();
                        DuvidaIQ = (from s in _context.Duvida
                                    select s).Include(q => q.cadeira).Include(q => q.user);


                        DuvidaIQ = DuvidaIQ.Where(p => values.Contains(p.CadeiraID));


                    }

                    break;

                default:
                    DuvidaIQ = DuvidaIQ;
                    break;
            }

            if (!String.IsNullOrEmpty(searchStringTopico))
            {
                DuvidaIQ = DuvidaIQ.Where(s => s.Topico.ToLower().Contains(searchStringTopico.ToLower()));


            }
            if (!String.IsNullOrEmpty(searchStringCadeira))
            {
                DuvidaIQ = DuvidaIQ.Where(s => s.cadeira.Name.ToLower().Contains(searchStringCadeira.ToLower()));


            }
            if (From != null)
            {
                DuvidaIQ = DuvidaIQ.Where(s => s.Data.Date >= From.Value);


            }
            if (To != null)
            {
                DuvidaIQ = DuvidaIQ.Where(s => s.Data.Date <= To.Value);


            }
            int pageSize = 4;

            Duvidas = await PaginatedList<Duvida>.CreateAsync(DuvidaIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
           


        }
        public async Task<IActionResult> OnPostDislikeAsync(int id)
        {
           
            var user = _context.Users.Where(s => s.UserName == User.Identity.Name).ToList();
            var duvida = _context.ApoiaDuvida.Where(s => s.DuvidaID == id).Where(s => s.UserID == user.First().Id).ToList();
            ApoiaDuvida = await _context.ApoiaDuvida.FindAsync(duvida.First().ID);
            
            _context.ApoiaDuvida.Remove(ApoiaDuvida);
            await _context.SaveChangesAsync();


            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostLikeAsync(int id)
        {
            var user = _context.Users.Where(s => s.UserName == User.Identity.Name).ToList();

            ApoiaDuvida duvida = new ApoiaDuvida()
            {
                UserID = user.First().Id,
                DuvidaID = id
            };

            _context.ApoiaDuvida.Add(duvida);
            await _context.SaveChangesAsync();



            return RedirectToPage();
        }

       

    }


}