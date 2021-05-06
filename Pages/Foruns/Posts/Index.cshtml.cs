using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AsMinhasDuvidas.Data;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Authorization;

namespace AsMinhasDuvidas.Pages.Foruns.Posts
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<ForumResposta> Post { get;set; }
        public IList<Forum> Forum { get; set; }
       
        public async Task OnGetAsync(int? id)
        {
            Post = await _context.Post.Where(s=>s.Forum.Id==id).ToListAsync();
            Forum= await _context.Forum.Where(s => s.Id == id).ToListAsync();
            
        }
    }
}
