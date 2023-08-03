using webmvc.Models.Blog;
using System.ComponentModel.DataAnnotations;


namespace webmvc.Areas.Blog.Models
{
    public class CreatePostModel : Post
    {
        [Display(Name ="Chuyên mục")]
        public int[] categoryID {get;set;}
    }
}