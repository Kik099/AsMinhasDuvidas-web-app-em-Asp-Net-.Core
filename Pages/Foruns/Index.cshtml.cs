using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Models;
using AsMinhasDuvidas.Data;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.Foruns
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(AsMinhasDuvidas.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Forum> Forum { get; set; }

        public async Task OnGetAsync(string nome, string checkbox)
        {
            if(!String.IsNullOrEmpty(nome) && !String.IsNullOrEmpty(checkbox))
                Forum = await _context.Forum.Where(s=>s.user.UserName == User.Identity.Name && s.Titulo.ToLower().Contains(nome.ToLower())).ToListAsync();

            else if(!String.IsNullOrEmpty(nome))
                Forum = await _context.Forum.Where(s =>  s.Titulo.ToLower().Contains(nome.ToLower())).ToListAsync();
            else if(!String.IsNullOrEmpty(checkbox))
                Forum = await _context.Forum.Where(s => s.user.UserName == User.Identity.Name ).ToListAsync();
            else
                Forum = await _context.Forum.ToListAsync();



        }
    }
}
