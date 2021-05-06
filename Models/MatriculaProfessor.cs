using System;
using System.ComponentModel.DataAnnotations.Schema;
using AsMinhasDuvidas.Areas.Identity;

namespace AsMinhasDuvidas.Models
{
    public class MatriculaProfessor
    {
        public MatriculaProfessor() { }
        public int ID { get; set; }
        public int CadeiraID { get; set; }
        public string UserID { get; set; }
        [ForeignKey("CadeiraID")]
        public virtual Cadeira cadeira { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser user { get; set; }
    }
}