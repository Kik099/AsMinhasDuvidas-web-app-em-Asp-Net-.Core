using System;
using System.Collections.Generic;
using System.Text;
using AsMinhasDuvidas.Areas.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AsMinhasDuvidas.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AsMinhasDuvidas.Models.Curso> Curso { get; set; }
        public DbSet<AsMinhasDuvidas.Models.MatriculaProfessor> MatriculaProfessor { get; set; }

        public DbSet<AsMinhasDuvidas.Models.MatriculaAluno> MatriculaAluno { get; set; }
        public DbSet<AsMinhasDuvidas.Models.Duvida> Duvida { get; set; }
        public DbSet<AsMinhasDuvidas.Models.ApoiaDuvida> ApoiaDuvida { get; set; }
        public DbSet<AsMinhasDuvidas.Models.Cadeira> Cadeira { get; set; }
        public DbSet<AsMinhasDuvidas.Models.Forum> Forum { get; set; }
        public DbSet<AsMinhasDuvidas.Models.ForumResposta> Post { get; set; }

    }
}
