using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using webmvc.Models.Blog;


namespace webmvc.Models.Blog
{
    [Table("PostCategory")]
    public class PostCategory 
    {
        public int postID {get;set;}// Fk tham chieu den ban Post

        public int categoryID {get;set;}


        [ForeignKey("categoryID")]  
        public Category category {get;set;}


        [ForeignKey("postID")]
        public  Post post {get;set;}
    }
}