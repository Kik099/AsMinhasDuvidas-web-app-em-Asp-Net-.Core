using System;
using System.ComponentModel.DataAnnotations.Schema;
using AsMinhasDuvidas.Areas.Identity;

namespace AsMinhasDuvidas.Models
{
    public class Post
    {

        public int Id { get; set; }
        public string conteudo { get; set; }
        public int ForumID { get; set; }
        public string UserID { get; set; }

        public DateTime data { get; set; }
        [ForeignKey("ForumID")]
        public virtual Forum Forum { get; set; }
        [ForeignKey("UserID")]
        public virtual ApplicationUser user { get; set; }
    }
}
