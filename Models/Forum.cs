using System;
using System.ComponentModel.DataAnnotations.Schema;
using AsMinhasDuvidas.Areas.Identity;

namespace AsMinhasDuvidas.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime data { get; set; }
        public Boolean Aberto { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser user { get; set; }
    }
}
