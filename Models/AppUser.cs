using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace webmvc.Models
{
    public class AppUser : IdentityUser
    {
        [Column(TypeName ="nvarchar")]
        [StringLength(150)]
        public string HomeAdress {get;set;}
        
        [DataType(DataType.Date)]
        public DateTime Brirthday {get;set;}
    }
}