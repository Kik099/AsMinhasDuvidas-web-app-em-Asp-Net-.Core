using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Models;
using Microsoft.Extensions.Logging;
using AsMinhasDuvidas.Funcoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AsMinhasDuvidas.Pages.ApoioAluno  
{
    [Authorize(Roles = "aluno")]
    public class IndexModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

      

        public IList<Cadeira> Cadiras { get;set; }
        public PaginatedList<Cadeira> Cadeiras { get; set; }
        public PaginatedList<Curso> Cursos { get; set; }
        [BindProperty]
        public string Filtro { get; set; }
        public int tab { get; set; }
        [BindProperty]
        public int? Id { get; set; }
        [BindProperty]
        public MatriculaAluno Mat { get; set; }
        public string CurrentFilter { get; set; }
        public PaginatedList<MatriculaAluno> Matriculado { get; set; }
        public IndexModel(AsMinhasDuvidas.Data.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task OnGetAsync( string currentFilter, int? procuracurso, int? id, int? Sec_Leccionada_IDCadeira, int? pageIndex, int? Sec_Leccionada_IDCurso, string Sec_Leccionada_NomeCadeira, int? Sec_Cadeira_ID, string Sec_Cadeira_Nome, int? Sec_Cadeira_IDCurso, int? Sec_Curso_ID, string Sec_Curso_Nome)
        {
            if (Filtro != null )
            {
                pageIndex = 1;
            }
            ViewData["Cursos"] = new SelectList(_context.Curso, "ID", "Name");
            if (procuracurso != null)
                ViewData["Cadeiras"] = new SelectList(_context.Cadeira.Where(s => s.CursoID == (int)procuracurso), "ID", "Name");

            if (Equals(currentFilter, "curso"))
                Filtro = "curso";
            if (id == null)
                tab = 1;

            else
                tab = (int)id;
            if (!(Equals(Filtro, "cadeira") || Equals(Filtro, "curso") || Equals(Filtro, "lecciona")))
                Filtro = "cadeira";
            if (Sec_Leccionada_IDCadeira != null || Sec_Leccionada_IDCurso != null ||  !String.IsNullOrEmpty(Sec_Leccionada_NomeCadeira))
                tab = 4;
            if (Sec_Cadeira_ID != null || Sec_Cadeira_IDCurso != null || !String.IsNullOrEmpty(Sec_Cadeira_Nome))
                tab = 1;
            if (Sec_Curso_ID != null || !String.IsNullOrEmpty(Sec_Curso_Nome))
                tab = 2;


            Cadeiras = await ProcurarCadeiras(Sec_Cadeira_ID, Sec_Cadeira_Nome, Sec_Cadeira_IDCurso, pageIndex);
            Cursos = await ProcurarCursos(Sec_Curso_ID, Sec_Curso_Nome, pageIndex);
            Matriculado = await ProcurarLeccionadas(Sec_Leccionada_IDCadeira, Sec_Leccionada_IDCurso, Sec_Leccionada_NomeCadeira, pageIndex);
            
        }

        public async Task<IActionResult> OnPostLikeAsync(int? id)
        {
            var duvida = _context.MatriculaAluno.Where(s => s.ID == id ).ToList();
            _context.MatriculaAluno.Remove(duvida.First());
            await _context.SaveChangesAsync();



            return RedirectToPage("./Index",new { id=4 });
        }
        private async Task<PaginatedList<MatriculaAluno>> ProcurarLeccionadas(int? sec_Leccionada_IDCadeira, int? sec_Leccionada_IDCurso, string sec_Leccionada_NomeCadeira, int? pageIndex)
        {
            var cadeiras  = _context.MatriculaAluno.Where(s=>s.user.UserName==User.Identity.Name).Include(q => q.cadeira).Include(q => q.user);
            

            if (sec_Leccionada_IDCadeira!=null)
            {
                Filtro = "lecciona";
                int id = (int)sec_Leccionada_IDCadeira;
                cadeiras = cadeiras.Where(s => s.CadeiraID == id).Include(q => q.cadeira).Include(q => q.user);
            }
            if (!String.IsNullOrEmpty(sec_Leccionada_NomeCadeira))
            {
                Filtro = "lecciona";
                cadeiras = cadeiras.Where(s => s.cadeira.Name.ToLower().Contains(sec_Leccionada_NomeCadeira.ToLower())).Include(q => q.cadeira).Include(q => q.user);
            }
            if (sec_Leccionada_IDCurso!=null)
            {
                Filtro = "lecciona";
                int id = (int)sec_Leccionada_IDCurso;
                cadeiras = cadeiras.Where(s => s.CadeiraID == id).Include(q => q.cadeira).Include(q => q.user);
            }
            int pageSize = 4;
            return await PaginatedList< MatriculaAluno>.CreateAsync(cadeiras.AsNoTracking(), pageIndex ?? 1, pageSize);

        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var user = _context.Users.Where(s => s.UserName == User.Identity.Name).ToList();

            if(_context.Cadeira.Where(s => s.ID == Mat.CadeiraID).Count() != 0)
            {

                if (_context.MatriculaAluno.Where(s => s.CadeiraID == Mat.CadeiraID && s.UserID == user.First().Id).Count() == 0)
                {
                    Mat.UserID = user.First().Id;
                    _context.MatriculaAluno.Add(Mat);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["erro"] = "JÁ ESTÁ MATRICULADO NA CADEIRA ESCOLHIDA ";
                    return RedirectToPage(new { id = 3, procuracurso = id });
                }

            }


            return RedirectToPage(new { id = 3 });
        }
        public async Task<IActionResult> OnPostCursoAsync(int? id)
        {


            ViewData["Cadeiras"] = new SelectList(await _context.Cadeira.Where(s => s.CursoID == (int)id).ToListAsync(), "ID", "Name");
      


            return RedirectToPage(new { id = 3, procuracurso = id });
        }
        private async Task<PaginatedList<Curso>> ProcurarCursos(int? sec_Curso_ID, string sec_Curso_Nome, int? pageIndex)
        {
           
            var cursos = (from s in _context.Curso
                          select s);

            if (sec_Curso_ID!=null)
            {
                Filtro = "curso";
                int id = (int)sec_Curso_ID;
                cursos = cursos.Where(s => s.ID==id);
            }
            if (!String.IsNullOrEmpty(sec_Curso_Nome))
            {
                Filtro = "curso";
                
                cursos = cursos.Where(s => s.Name.ToLower().Contains(sec_Curso_Nome.ToLower()));
            }
            int pageSize = 4;
            return await PaginatedList<Curso>.CreateAsync(cursos.AsNoTracking(), pageIndex ?? 1, pageSize);  
        }

        private async Task<PaginatedList<Cadeira>> ProcurarCadeiras(int? sec_Cadeira_ID, string sec_Cadeira_Nome, int? sec_Cadeira_IDCurso, int? pageIndex)
        {
            var cadeiras = (from s in _context.Cadeira
                            select s).Include(q => q.curso);


            if (sec_Cadeira_IDCurso!=null)
            {
                Filtro = "cadeira";

                int id = (int)(sec_Cadeira_IDCurso);
                cadeiras = cadeiras.Where(s => s.CursoID == id).Include(q => q.curso);
            }
            if (!String.IsNullOrEmpty(sec_Cadeira_Nome))
            {
                Filtro = "cadeira";
                cadeiras = cadeiras.Where(s => s.Name.ToLower().Contains(sec_Cadeira_Nome.ToLower())).Include(q => q.curso);
            }
            if (sec_Cadeira_ID!=null)
            {
                Filtro = "cadeira";
                int id = (int)sec_Cadeira_ID;
                cadeiras = cadeiras.Where(s => s.ID == id).Include(q => q.curso);
            }
            int pageSize = 4;
            return  await PaginatedList<Cadeira>.CreateAsync(cadeiras.AsNoTracking(), pageIndex ?? 1, pageSize);

        }
    }
}
