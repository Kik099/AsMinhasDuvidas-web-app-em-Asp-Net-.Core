using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsMinhasDuvidas.Models
{
    public class Curso
    {
        
        public Curso() { }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public int ID { get; set; }
        public string Name { get; set; }
    }
}
