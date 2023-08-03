using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;



namespace webmvc.Models.Blog
{
     [Table("Post")]
    public class Post
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 6, ErrorMessage = "Phai dai tu {1} den {2}")]
        public string Title { get; set; }

        [Display(Name = "Mô tả ngắn")]
        public string Descreption { get; set; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [Required(ErrorMessage = "Phải thiết lập chuỗi URL")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string Slug { set; get; }

        [Display(Name = "Nội dung")]
        public string Content { get; set; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }

        // [Required]
        [Display(Name = "Tác giả")]
        public string AuthorId { set; get; }
        [ForeignKey("AuthorId")]
        [Display(Name = "Tác giả")]
        public AppUser Author { set; get; }

        [Display(Name = "Ngày tạo")]
        public DateTime DateCreated { set; get; }

        [Display(Name = "Ngày cập nhật")]
        public DateTime DateUpdated { set; get; }

         public List<PostCategory>  PostCategories { get; set; }
    }
}