using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace webmvc.Models
{
    public class Article {
        [Key]
        public int ID {get;set;}
        [StringLength(200)]
        [Required]
        [Column(TypeName = "nvarchar")]
        public string Name {get;set;}
    }
}