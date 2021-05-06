using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.Extensions.Configuration;

namespace AsMinhasDuvidas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _sender;

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender sender, IConfiguration configuration)
        {
            _userManager = userManager;
            _sender = sender;
            Configuration = configuration;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public IConfiguration Configuration { get; }
        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;
            // Once you add a real email sender, you should remove this code that lets you confirm the account
            
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                    protocol: Request.Scheme);
            
            using (var message = new MailMessage(Configuration.GetConnectionString("email"), Email))
            {
                message.Subject = "AsMinhasDuvidas confirmar email";
                message.Body = $"Ativar  conta: {EmailConfirmationUrl}";
                using (SmtpClient client = new SmtpClient
                {
                    EnableSsl = true,
                    Host = "smtp.gmail.com",
                    Port = 587,
                    Credentials = new NetworkCredential(Configuration.GetConnectionString("email"), Configuration.GetConnectionString("emailpass"))
                })
                {
                    try
                    {
                        client.Send(message);
                    }
                    catch
                    {
                        Console.WriteLine("Erro no envio de email (repor password)");
                    }
                       
                   

                }
            }
            return Page();
        }
    }
}
