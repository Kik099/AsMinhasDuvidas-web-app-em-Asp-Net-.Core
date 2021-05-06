using System;
using System.Linq;
using AsMinhasDuvidas.Areas.Identity;
using AsMinhasDuvidas.Data;
using AsMinhasDuvidas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AsMinhasDuvidas
{
    public static class MigrationManager
    {

        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            db.Database.EnsureCreated();
            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedCadeirasAsync(db);
            SeedDuvidasAsync(db);
            SeedApoiaDuvidasAsync(db);
            SeedForumAsync(db);
            SeedFrequentaAsync(db);
            db.SaveChanges();
        }

        private static async void SeedForumAsync(ApplicationDbContext db)
        {
            if (db.Forum.Where(S => S.Id == 1).Count() == 0)
            {
                Forum forum = new Forum();

                forum.UserID = "aluno";
                forum.Titulo = "Funcionamento do site";
                forum.Descricao = "Alguém me poderia explicar como funciona o site?";
                forum.data = DateTime.Now.Date;
                forum.Aberto = true;
                await db.Forum.AddAsync(forum);

            }
            if (db.Forum.Where(S => S.Id == 2).Count() == 0)
            {
                Forum forum = new Forum();

                forum.UserID = "aluno";
                forum.Titulo = "Titulo Forum 1";
                forum.Descricao = "Descrição forum 1";
                forum.data = DateTime.Now.Date;
                forum.Aberto = false;
                await db.Forum.AddAsync(forum);

            }
            if (db.Post.Where(S => S.Id == 1).Count() == 0)
            {
                ForumResposta post = new ForumResposta();

                post.UserID = "aluno2";
                post.ForumID = 2;
                post.data = DateTime.Now.Date;
                post.conteudo = "conteudo do post que pertence ao forum 1";
                await db.Post.AddAsync(post);

            }
            if (db.Post.Where(S => S.Id == 2).Count() == 0)
            {
                ForumResposta post = new ForumResposta();

                post.UserID = "aluno2";
                post.ForumID = 2;
                post.data = DateTime.Now.Date;
                post.conteudo = "conteudo do post que pertence ao forum 2";
                await db.Post.AddAsync(post);

            }

        }


        private static async void SeedApoiaDuvidasAsync(ApplicationDbContext db)
        {
            if (db.ApoiaDuvida.Where(S => S.UserID == "aluno2" && S.DuvidaID == 1).Count() == 0)
            {
                ApoiaDuvida apoia = new ApoiaDuvida();

                apoia.UserID = "aluno2";
                apoia.DuvidaID = 1;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
            if (db.ApoiaDuvida.Where(S => S.UserID == "aluno" && S.DuvidaID == 2).Count() == 0)
            {
                ApoiaDuvida apoia = new ApoiaDuvida();

                apoia.UserID = "aluno";
                apoia.DuvidaID = 2;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
            if (db.ApoiaDuvida.Where(S => S.UserID == "aluno2" && S.DuvidaID == 4).Count() == 0)
            {
                ApoiaDuvida apoia = new ApoiaDuvida();

                apoia.UserID = "aluno2";
                apoia.DuvidaID = 4;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
        }
        private static async void SeedDuvidasAsync(ApplicationDbContext db)
        {
            if (db.Duvida.Where(S => S.ID == 1).Count() == 0)
            {
                Duvida duvida = new Duvida();
                duvida.ID = 1;
                duvida.Pergunta = "Como é que crio uma classe em java?";
                duvida.Resposta = "Reposta exemplo da pergunta 1";
                duvida.Topico = "Java";
                duvida.Data = DateTime.Now.Date;
                duvida.CadeiraID = 1;
                duvida.UserID = "aluno";
                duvida.VizualizarResposta = "False";
                await db.Duvida.AddAsync(duvida);
                ApoiaDuvida apoia = new ApoiaDuvida();
                apoia.UserID= "aluno";
                apoia.DuvidaID = 1;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
            if (db.Duvida.Where(S => S.ID == 2).Count() == 0)
            {
                Duvida duvida = new Duvida();
                duvida.ID = 2;
                duvida.Pergunta = "Pergunta exemplo 2";
                duvida.Resposta = "Reposta exemplo da pergunta 2";
                duvida.Topico = "Topico exemplo 1";
                duvida.Data = DateTime.Now.Date;
                duvida.CadeiraID = 1;
                duvida.UserID = "aluno2";
                duvida.VizualizarResposta = "False";
                await db.Duvida.AddAsync(duvida);
                ApoiaDuvida apoia = new ApoiaDuvida();
                apoia.UserID = "aluno2";
                apoia.DuvidaID = 2;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
            if (db.Duvida.Where(S => S.ID == 3).Count() == 0)
            {
                Duvida duvida = new Duvida();
                duvida.ID = 3;
                duvida.Pergunta = "Qual é o código que transforma uma frase com letras minúsculas em maiúsculas?";
                duvida.Resposta = null;
                duvida.Topico = "Java";
                duvida.Data = DateTime.Now.Date;
                duvida.CadeiraID = 1;
                duvida.UserID = "aluno2";
                duvida.VizualizarResposta = null;
                await db.Duvida.AddAsync(duvida);
                ApoiaDuvida apoia = new ApoiaDuvida();
                apoia.UserID = "aluno2";
                apoia.DuvidaID = 3;
                await db.ApoiaDuvida.AddAsync(apoia);

            }
            if (db.Duvida.Where(S => S.ID == 4).Count() == 0)
            {
                Duvida duvida = new Duvida();
                duvida.ID = 4;
                duvida.Pergunta = "Pergunta exemplo 4";
                duvida.Resposta = "Reposta exemplo da pergunta 4";
                duvida.Topico = "Topico exemplo 1";
                duvida.Data = DateTime.Now.Date;
                duvida.CadeiraID = 2;
                duvida.UserID = "aluno2";
                duvida.VizualizarResposta = "True";
                await db.Duvida.AddAsync(duvida);
                ApoiaDuvida apoia = new ApoiaDuvida();
                apoia.UserID = "aluno";
                apoia.DuvidaID = 4;
                await db.ApoiaDuvida.AddAsync(apoia);
            }
            if (db.Duvida.Where(S => S.ID == 5).Count() == 0)
            {
                Duvida duvida = new Duvida();
                duvida.ID = 5;
                duvida.Pergunta = "Pergunta exemplo 5";
                duvida.Resposta = "Reposta exemplo da pergunta 4";
                duvida.Topico = "Topico exemplo 1";
                duvida.Data = DateTime.Now.Date;
                duvida.CadeiraID = 4;
                duvida.UserID = "aluno2";
                duvida.VizualizarResposta = "True";
                await db.Duvida.AddAsync(duvida);
                
            }
        }

        private static async void SeedFrequentaAsync(ApplicationDbContext db)
        {
            if (db.MatriculaProfessor.Where(S => S.UserID == "professor").Count() == 0)
            {
                MatriculaProfessor mat1 = new MatriculaProfessor();

                mat1.UserID = "professor";
                mat1.CadeiraID = 1;
                db.MatriculaProfessor.Add(mat1);
                var aux = await db.MatriculaProfessor.AddAsync(mat1);
                MatriculaProfessor mat2 = new MatriculaProfessor();

                mat2.UserID = "professor";
                mat2.CadeiraID = 2;
                await db.MatriculaProfessor.AddAsync(mat2);


            }
            if (db.MatriculaAluno.Where(S => S.UserID == "aluno").Count() == 0)
            {

                MatriculaAluno mat2 = new MatriculaAluno();

                mat2.UserID = "aluno";
                mat2.CadeiraID = 2;
                var aux = await db.MatriculaAluno.AddAsync(mat2);
            }
            if (db.MatriculaAluno.Where(s => s.UserID == "aluno" && s.CadeiraID == 1).Count() == 0)
            {
                MatriculaAluno mat1 = new MatriculaAluno();

                mat1.UserID = "aluno";
                mat1.CadeiraID = 1;
                db.MatriculaAluno.Add(mat1);
                await db.MatriculaAluno.AddAsync(mat1);
            }
            if (db.MatriculaAluno.Where(s => s.UserID == "aluno" && s.CadeiraID == 3).Count() == 0)
            {
                MatriculaAluno mat1 = new MatriculaAluno();

                mat1.UserID = "aluno";
                mat1.CadeiraID = 3;
                db.MatriculaAluno.Add(mat1);
                await db.MatriculaAluno.AddAsync(mat1);

            }
            if (db.MatriculaAluno.Where(s => s.UserID == "aluno" && s.CadeiraID == 4).Count() == 0)
            {
                MatriculaAluno mat1 = new MatriculaAluno();

                mat1.UserID = "aluno";
                mat1.CadeiraID = 4;
                db.MatriculaAluno.Add(mat1);
                await db.MatriculaAluno.AddAsync(mat1);

            }
            if (db.MatriculaAluno.Where(s => s.UserID == "aluno" && s.CadeiraID == 5).Count() == 0)
            {
                MatriculaAluno mat1 = new MatriculaAluno();

                mat1.UserID = "aluno";
                mat1.CadeiraID = 5;
                db.MatriculaAluno.Add(mat1);
                await db.MatriculaAluno.AddAsync(mat1);

            }
            if (db.MatriculaAluno.Where(s => s.UserID == "aluno" && s.CadeiraID == 6).Count() == 0)
            {
                MatriculaAluno mat1 = new MatriculaAluno();

                mat1.UserID = "aluno";
                mat1.CadeiraID = 6;
                db.MatriculaAluno.Add(mat1);
                await db.MatriculaAluno.AddAsync(mat1);
            }
        }

        private static async void SeedCadeirasAsync(ApplicationDbContext db)
        {
            if (db.Curso.Where(S => S.ID == 1).Count() == 0)
            {
                Curso c1 = new Curso();
                c1.ID = 1;
                c1.Name = "Engenharia Informática";
                var aux = await db.Curso.AddAsync(c1);


            }
            if (db.Cadeira.Where(s => s.ID == 1).Count() == 0)
            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 1;
                cadeira1.Name = "Programação 1";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }
            if (db.Cadeira.Where(s => s.ID == 2).Count() == 0)
            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 2;
                cadeira1.Name = "Programação 2";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }
            if (db.Cadeira.Where(s => s.ID == 3).Count() == 0)
            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 3;
                cadeira1.Name = "Programação Orientada aos Objectos";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }
            if (db.Cadeira.Where(s => s.ID == 4).Count() == 0)
            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 4;
                cadeira1.Name = "Arquitetura de Computadores 1";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }
            if (db.Cadeira.Where(s => s.ID == 5).Count() == 0)

            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 5;
                cadeira1.Name = "Arquitetura de Computadores 2";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }
            if (db.Cadeira.Where(s => s.ID == 6).Count() == 0)

            {
                Cadeira cadeira1 = new Cadeira();
                cadeira1.ID = 6;
                cadeira1.Name = "Projeto";
                cadeira1.CursoID = 1;
                var aux = await db.Cadeira.AddAsync(cadeira1);
            }

        }

        public static void SeedUsers
    (UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("a39441@ubi.pt").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "a39441@ubi.pt";
                user.Email = "a39441@ubi.pt";
                user.Id = "admin";
                user.Name = "admin";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "admin").Wait();
                }
            }
            if (userManager.FindByNameAsync("professor@ubi.pt").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "professor@ubi.pt";
                user.Email = "professor@ubi.pt";
                user.Id = "professor";
                user.Name = "professor";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "professor").Wait();
                }
            }
            if (userManager.FindByNameAsync("aluno@ubi.pt").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "aluno@ubi.pt";
                user.Email = "aluno@ubi.pt";
                user.Id = "aluno";
                user.Name = "aluno";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "aluno").Wait();
                }
            }
            if (userManager.FindByNameAsync("aluno2@ubi.pt").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "aluno2@ubi.pt";
                user.Email = "aluno2@ubi.pt";
                user.Id = "aluno2";
                user.Name = "aluno2";
                user.EmailConfirmed = true;

                IdentityResult result = userManager.CreateAsync(user, "1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "aluno").Wait();
                }
            }
            


        }

        public static void SeedRoles
    (RoleManager<IdentityRole> roleManager)
        {

            if (!roleManager.RoleExistsAsync("admin").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "admin";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("professor").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "professor";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("aluno").Result)
            {
                IdentityRole role = new IdentityRole();
                role.Name = "aluno";
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
            }
        }
    }

}

