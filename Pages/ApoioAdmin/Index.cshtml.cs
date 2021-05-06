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

namespace AsMinhasDuvidas.Pages.ApoioAdmin  
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly AsMinhasDuvidas.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;

      

        public IList<Cadeira> Cadiras { get;set; }
        public PaginatedList<Cadeira> Cadeiras { get; set; }
        public PaginatedList<Curso> Cursos { get; set; }
        [BindProperty]
        public string Filtro { get; set; }

        [BindProperty]
        public MatriculaProfessor Mat { get; set; }
        public string CurrentFilter { get; set; }
        public PaginatedList<MatriculaAluno> Matriculado { get; set; }
        public int tab { get; set; }
        public IndexModel(AsMinhasDuvidas.Data.ApplicationDbContext context, ILogger<IndexModel> logger)
        {
            _context = context;
            _logger = logger;

        }
        public async Task OnGetAsync( string currentFilter, int? Sec_Aluno_IDCadeira, int? Sec_Aluno_IDAluno, int? pageIndex, int? Sec_Aluno_IDCurso, string Sec_Aluno_NomeCadeira, int? Sec_Cadeira_ID, string Sec_Cadeira_Nome, int? Sec_Cadeira_IDCurso, int? Sec_Curso_ID, string Sec_Curso_Nome, int? id)
        {
            if (Filtro != null )
            {
                pageIndex = 1;
            }
            if (id == null)
                tab = 1;

            else
                tab = (int)id;

            if (!(Equals(Filtro, "cadeira") || Equals(Filtro, "curso") || Equals(Filtro, "lecciona")))
                Filtro = "cadeira";
            if (Sec_Aluno_IDAluno != null || Sec_Aluno_IDCadeira != null || Sec_Aluno_IDCurso != null || !String.IsNullOrEmpty(Sec_Aluno_NomeCadeira))
                tab = 4;
            if (Sec_Cadeira_ID != null || Sec_Cadeira_IDCurso != null || !String.IsNullOrEmpty(Sec_Cadeira_Nome))
                tab = 1;
            if (Sec_Curso_ID != null ||  !String.IsNullOrEmpty(Sec_Curso_Nome))
                tab = 2;
            Cadeiras = await ProcurarCadeiras(Sec_Cadeira_ID, Sec_Cadeira_Nome, Sec_Cadeira_IDCurso, pageIndex);
            Cursos = await ProcurarCursos(Sec_Curso_ID, Sec_Curso_Nome, pageIndex);
            Matriculado = await ProcurarLeccionadas(Sec_Aluno_IDCadeira, Sec_Aluno_IDCurso, Sec_Aluno_IDAluno, Sec_Aluno_NomeCadeira, pageIndex);
           
        }

        public async Task<IActionResult> OnPostDeletecadeiraAsync(int id)
        {
            var cadeira = _context.Cadeira.Where(s=>s.ID==id).First();
            var duvida = _context.Duvida.Where(s => s.CadeiraID == id).ToList();
            foreach (var item in duvida)
            {
                var apoiaduvida = _context.ApoiaDuvida.Where(s => s.DuvidaID == item.ID);
                foreach (var apoia in apoiaduvida)
                {
                    _context.ApoiaDuvida.Remove(apoia);
                }
                _context.Duvida.Remove(item);
            }
            _context.Cadeira.Remove(cadeira);
            await _context.SaveChangesAsync();



            return RedirectToPage("./Index", new { id = 1 });
        }
        public async Task<IActionResult> OnPostDeletecursoAsync(int id)
        {
            
            var cadeira = _context.Cadeira.Where(s => s.CursoID == id).ToList();
            foreach(var cad in cadeira) {
                var matriculaaluno = _context.MatriculaAluno.Where(s => s.CadeiraID == cad.ID).ToList();
                foreach (var item in matriculaaluno)
                {
                    _context.MatriculaAluno.Remove(item);
                }
                var matriculaprof = _context.MatriculaProfessor.Where(s => s.CadeiraID == cad.ID).ToList();
                foreach (var item in matriculaprof)
                {
                    _context.MatriculaProfessor.Remove(item);
                }
                var duvida = _context.Duvida.Where(s => s.CadeiraID == id).ToList();
                foreach (var item in duvida)
                {
                    var apoiaduvida = _context.ApoiaDuvida.Where(s => s.DuvidaID == item.ID);
                    foreach(var apoia in apoiaduvida)
                    {   
                        _context.ApoiaDuvida.Remove(apoia);
                    }
                    _context.Duvida.Remove(item);
                }
                _context.Cadeira.Remove(cad);
            }
            _context.Curso.Remove(_context.Curso.Where(S => S.ID == id).First());
            await _context.SaveChangesAsync();
           
            return RedirectToPage("./Index", new { id = 2 });
        }
        public async Task<IActionResult> OnPostDeletealunoAsync(int id)
        {
            var user = _context.MatriculaAluno.Where(s => s.ID == id).First();
            var matriculaaluno = _context.MatriculaAluno.Where(s => s.UserID ==user.UserID).ToList();
            
                _context.MatriculaAluno.Remove(user);
            
            var duvida = _context.Duvida.Where(s => s.UserID == user.UserID && s.CadeiraID==user.CadeiraID).ToList();
            foreach (var item in duvida)
            {
                var apoiaduvida = _context.ApoiaDuvida.Where(s => s.DuvidaID == item.ID);
                foreach (var apoia in apoiaduvida)
                {
                    _context.ApoiaDuvida.Remove(apoia);
                }
                _context.Duvida.Remove(item);
            }
            var apoiaduvidaaluno= _context.ApoiaDuvida.Where(s => s.UserID == user.UserID && s.duvida.CadeiraID == user.CadeiraID);
            foreach(var item in apoiaduvidaaluno)
            {
                _context.ApoiaDuvida.Remove(item);
            }

           
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index",new { id=4 });
        }
        private async Task<PaginatedList<MatriculaAluno>> ProcurarLeccionadas(int? sec_Aluno_IDCadeira, int? sec_Aluno_IDCurso, int? sec_Aluno_IDAluno, string sec_Aluno_NomeCadeira, int? pageIndex)
        {
            var cadeiras  = _context.MatriculaAluno.Include(q => q.cadeira).Include(q => q.user);
            

            if (sec_Aluno_IDCadeira!=null)
            {
                Filtro = "lecciona";
                int id = (int)sec_Aluno_IDCadeira;
                cadeiras = cadeiras.Where(s => s.CadeiraID == id).Include(q => q.cadeira).Include(q => q.user);
            }
            if (!String.IsNullOrEmpty(sec_Aluno_NomeCadeira))
            {
                Filtro = "lecciona";
                cadeiras = cadeiras.Where(s => s.cadeira.Name.ToLower().Contains(sec_Aluno_NomeCadeira.ToLower())).Include(q => q.cadeira).Include(q => q.user);
            }
            if (sec_Aluno_IDCurso != null)
            {
                Filtro = "lecciona";
                int id = (int)sec_Aluno_IDCurso;
                cadeiras = cadeiras.Where(s => s.CadeiraID == id).Include(q => q.cadeira).Include(q => q.user);
            }
            if (sec_Aluno_IDAluno != null)
            {
                Filtro = "lecciona";
                int id = (int)sec_Aluno_IDAluno;
                cadeiras = cadeiras.Where(s => s.UserID == id.ToString()).Include(q => q.cadeira).Include(q => q.user);
            }
            int pageSize = 4;
            return await PaginatedList< MatriculaAluno>.CreateAsync(cadeiras.AsNoTracking(), pageIndex ?? 1, pageSize);

        }

        public async Task<IActionResult> OnPostAsync()
        {

            var user = _context.MatriculaProfessor.Where(s => s.UserID == Mat.UserID && s.CadeiraID==Mat.CadeiraID);
          
             
            if (_context.MatriculaProfessor.Where(s => s.UserID == Mat.UserID && s.CadeiraID == Mat.CadeiraID).Count() != 0)
            {
                    _context.MatriculaProfessor.Remove(user.First());
                    await _context.SaveChangesAsync();


            }
            else if((_context.Cadeira.Where(s => s.ID == Mat.CadeiraID).Count() == 0)&& (_context.Users.Where(s => s.Id == Mat.UserID).Count() == 0))
                TempData["erro"] = "NÃO EXISTE A CADEIRA NEM O PROFESSOR COM OS IDS ESCOLHIDOS ";
            else if(_context.Cadeira.Where(s => s.ID == Mat.CadeiraID).Count() == 0)
                TempData["erro"] = "NÃO EXISTE A CADEIRA COM O ID ESCOLHIDO";
            else if(_context.Users.Where(s=>s.Id== Mat.UserID).Count() == 0)
                TempData["erro"] = "NÃO EXISTE O PROFESSOR COM O ID ESCOLHIDO";

            return RedirectToPage(new { id=3});
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
