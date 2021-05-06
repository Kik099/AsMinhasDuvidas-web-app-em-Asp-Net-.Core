using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using AsMinhasDuvidas.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace AsMinhasDuvidas.ScheduledTasks
{
    public class EmailJob : IJob
    {
        private readonly IServiceProvider _provider;
       
        public EmailJob(IServiceProvider provider)
        {
            _provider = provider;
          
        }

        public IConfiguration Configuration { get; }
        public void SendEmail(string ToEmail)
        {
            using (var message = new MailMessage(Configuration.GetConnectionString("email"), ToEmail))
            {
                message.Subject = "AsMinhasDuvidas Aviso";
                message.Body = "Tem questoes para responder do dia de hoje: " + DateTime.Now;
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
        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                // Resolve the Scoped service
                var service = scope.ServiceProvider.GetService<ApplicationDbContext>();
                var roleProfessor =  await service.Roles.Where(s => s.Name == "professor").ToListAsync();
                var Teacher = await service.UserRoles.Where(s => s.RoleId == roleProfessor.First().Id).ToListAsync();
               
                //Por cada Professor que existe vamos verificar quais sao os professores que tem Questoes por responder nesse mesmo dia
                //Para isso o que fazemos é percorrer cada professor e verificamos quais sao as cadeiras que esse mesmo professor lecciona
                //Depois disso Com uma lista das cadeiras que o professor lecciona, vamos verificar na base de dados das Questoes quais sao as
                //questoes que correspondem ao dia atual(DateTime.Now.Date) e se corresponde a uma cadeira que o professor lecciona
                int aux = 0;
                foreach(var teacher in Teacher)
                {
                    
                    var subjects = await service.MatriculaProfessor.Where(s => s.UserID == teacher.UserId).ToListAsync();
                    aux = 0;
                    foreach (var subject in subjects)
                    {
                        var data = DateTime.Now;
                        var questions = await service.Duvida.Where(s => s.CadeiraID == subject.CadeiraID && s.Data.Date==data.Date).ToListAsync();
                        
                        if (questions.Count() > 0)
                        {
                            aux += 1;
                        }//Caso na variavel var existe alguma questao é pq existe uma cadeira que o proffesor lecciona
                         //que tem perguntas para respnder 

                        

                    }
                    if (aux > 0)
                    {
                        var user = await service.Users.FindAsync(teacher.UserId);
                       
                        SendEmail(user.Email);
                    }


                }



            }
           
            
        }

        
    }
}
