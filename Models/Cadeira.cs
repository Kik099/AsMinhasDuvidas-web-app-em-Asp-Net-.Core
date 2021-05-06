using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsMinhasDuvidas.Models
{
    public class Cadeira
    {
        public Cadeira() { }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ID { get; set; }
        public string Name { get; set; }
        public int CursoID { get; set; }
        [ForeignKey("CursoID")]
        public virtual Curso curso { get; set; }
    }
}
