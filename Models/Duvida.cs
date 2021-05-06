using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AsMinhasDuvidas.Areas.Identity;

namespace AsMinhasDuvidas.Models
{
    public class Duvida
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ID { get; set; }
        public string Pergunta { get; set; }
        public string Resposta { get; set; }
        public DateTime Data { get; set; }
        public string Topico { get; set; }
        public int CadeiraID { get; set; }
        public string UserID { get; set; }
        [ForeignKey("CadeiraID")]
        public virtual Cadeira cadeira { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser user { get; set; }
        public string VizualizarResposta { get; set; }
    }
}
