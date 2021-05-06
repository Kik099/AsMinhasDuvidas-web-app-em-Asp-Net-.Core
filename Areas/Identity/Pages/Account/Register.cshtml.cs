using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AsMinhasDuvidas.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, RoleManager<IdentityRole> _roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            roleManager = _roleManager;
        }


       
        

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        [BindProperty]
        public InputModel Input { get; set; }
        public class InputModel
        {
            [Required(ErrorMessage = "Código é necessário")]
            [Display(Name = "Código do Aluno/Professor")]
            public string ID { get; set; }
            [Required(ErrorMessage = "Nome é necessário")]
            [Display(Name = "Nome")]
            public string Name { get; set; }
            [Required(ErrorMessage = "Email é necessário")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "A password é necessário")]
            [StringLength(100, ErrorMessage = "O tamanho da password tem de conter no minimo 4 caracteres e no máximo 100", MinimumLength = 4)]
            [Display(Name = "Password")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "As passwords não correspondem.")]
            public string ConfirmPassword { get; set; }
            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ViewData["role"] = new SelectList(Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
   
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            ViewData["role"] = new SelectList(Enum.GetValues(typeof(Roles)).Cast<Roles>().Select(v => new SelectListItem
            {
                Text = v.ToString(),
                Value = ((int)v).ToString()
            }).ToList(), "Value", "Text");
            if (ModelState.IsValid)
            {
                if (_userManager.FindByIdAsync(Input.ID).Result != null)
                {
                    TempData["id"] = "->Id já existe";
                        return Page();

                }
                else if (_userManager.FindByEmailAsync(Input.Email).Result != null)
                {
                    TempData["email"] = "->Email já existe";
                    return Page();

                }
                else { 
                    


                var user = new ApplicationUser { Id=Input.ID,UserName = Input.Email, Email = Input.Email,Name=Input.Name };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    if (Input.Role == "2")
                    {
                        await _userManager.AddToRoleAsync(user, "aluno");

                    }
                    if (Input.Role == "3")
                    {


                        var resultw = await _userManager.AddToRoleAsync(user, "professor");

                    }
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                   
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }

                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                }

            }
             
            // If we got this far, something failed, redisplay form
            return Page();
        }

       
    }
}
