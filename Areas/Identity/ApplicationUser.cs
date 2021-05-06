using System;
using Microsoft.AspNetCore.Identity;

namespace AsMinhasDuvidas.Areas.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
    }
}
