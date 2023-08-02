using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace webmvc.Models.Contact
{
    public class Contactt 
    {
        [Key]
        public int Id{get;set;}
        [Required(ErrorMessage ="Phai nhap {0}")]
        [StringLength(200)]
        [Column(TypeName ="nvarchar")]
        [Display(Name ="Ho va Ten")]
        public string FullName {get;set;}
        [Required(ErrorMessage ="Phai nhap {0}")]
        [StringLength(200)]
        [EmailAddress(ErrorMessage = "Phai la dia chi email")]
        public string Email{get;set;}
        public DateTime DateSent{get;set;} // muon nhan gia tri null thi DateTime them dau ? => DateTime?
         [Display(Name ="Noi Dung")]
        public string Message {get;set;}
         
        [StringLength(200)]
        [Phone(ErrorMessage ="Phai la so dien thoai")]
        [Display(Name ="So Dien Thoai")]
        public string Phone {get;set;}
    }
}