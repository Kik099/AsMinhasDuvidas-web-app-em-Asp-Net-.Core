using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Mail;
using System.Net;

namespace AsMinhasDuvidas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public IConfiguration Configuration { get; }
        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    TempData["erro"] = "CONTA INEXISTENTE OU NÃO CONFIRMADA!!";
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./ForgotPassword");
                }

                // For more information on how to enable account confirmation and password reset please 
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);
                using (var message = new MailMessage(Configuration.GetConnectionString("email"), Input.Email))
                {
                    message.Subject = "AsMinhasDuvidas repor password";
                    message.Body = $"Repor password: {callbackUrl}";
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
                            Console.WriteLine("Erro no envio de email (forgot password)");
                        }

                    }
                }


                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset Password",
                    $"Repor palavrapasse:'{HtmlEncoder.Default.Encode(callbackUrl)}");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
