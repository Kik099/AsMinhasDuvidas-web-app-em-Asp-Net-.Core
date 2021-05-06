using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AsMinhasDuvidas.Areas.Identity;
using AsMinhasDuvidas.Models;

namespace AsMinhasDuvidas.Models
{
    public class ApoiaDuvida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int DuvidaID { get; set; }
        public string UserID { get; set; }
        [ForeignKey("DuvidaID")]
        public virtual Duvida duvida { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser user { get; set; }
    }
}
