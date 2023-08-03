using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webmvc.Models;
using webmvc.Models.Blog;
using Microsoft.AspNetCore.Identity;
using webmvc.Utilities;

using webmvc.Areas.Blog.Models;

namespace webmvc.Areas.Blog.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _appUser;

        public PostController(AppDbContext context, UserManager<AppUser> appUser)
        {
            _context = context;
            _appUser = appUser;
        }
        [TempData]
        public string StatusMessage {get;set;}

        // GET: Post
        public async Task<IActionResult> Index([FromQuery(Name ="p")]int currenpage, int pagesize)
        {
          var post = _context.posts.Include(c=>c.Author)
                    .OrderByDescending(p=> p.DateUpdated);
                    int totalPost = await post.CountAsync();
                    if(pagesize <= 0)   pagesize =10;
                    int countPages = (int)Math.Ceiling((double)totalPost / pagesize);  

                    if(currenpage > countPages) currenpage = countPages;
                    if(currenpage < 1) currenpage =1; 

                    var pagingModel = new PagingModel()
                    {
                        currentpage = currenpage,
                        countpages = countPages,
                        generateUrl = (pageNumber) => Url.Action("Index", new {
                                p = pageNumber,
                                pagesize =pagesize
                        })
                    };
                    ViewBag.pagingModel = pagingModel;
                    ViewBag.totalPost = totalPost;
                    ViewBag.PostIndex = (currenpage -1) *pagesize;

                    var postInPage = await post.Skip((currenpage - 1) * pagesize)
                            .Take(pagesize)
                            .Include(p => p.PostCategories)
                            .ThenInclude(pc=> pc.category).ToListAsync();
                           
                //  model.totalUsers = await qr.CountAsync();
                // model.countPages = (int)Math.Ceiling((double)model.totalUsers / model.ITEMS_PER_PAGE);

                // if (model.currentPage < 1)
                //     model.currentPage = 1;
                // if (model.currentPage > model.countPages)
                //     model.currentPage = model.countPages;

                // var qr1 = qr.Skip((model.currentPage - 1) * model.ITEMS_PER_PAGE)
                //             .Take(model.ITEMS_PER_PAGE)
                //             .Select(u => new UserAndRole() {
                //                 Id = u.Id,
                //                 UserName = u.UserName,
                //             });
        
            
            return View( postInPage);
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public async Task< IActionResult> Create()
        {
           var category = await _context.categories.ToListAsync();
           new MultiSelectList(category, "Id","Title");
           ViewData["categories"] = new MultiSelectList(category, "Id","Title");
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Descreption,Slug,Content,Published,categoryID ")] CreatePostModel post)
        {
               var category = await _context.categories.ToListAsync();
             ViewData["categories"] = new MultiSelectList(category, "Id","Title");
              if(post.Slug == null)
              {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
              }

              if (await _context.posts.AnyAsync( p=> p.Slug == post.Slug)) 
              {
                ModelState.AddModelError("Slug", "Nhập chuỗi ký tự khác");
                return View(post);
              }
            if (ModelState.IsValid)
            {
             
             
                var user = await _appUser.GetUserAsync(this.User);
                 post.DateCreated = post.DateUpdated = DateTime.Now;

                  post.AuthorId = user.Id; 
                  
                _context.Add(post);

                if(post.categoryID != null)
                {   
                    foreach (var cateID in post.categoryID)
                    {
                        _context.Add(new PostCategory(){
                            categoryID  = cateID,
                            post = post
                        });
                    }
                }
                await _context.SaveChangesAsync();
                StatusMessage = " Vua tao bai viet moi";
                return RedirectToAction(nameof(Index));
            }
          
            return View(post);
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // var post = await _context.posts.FindAsync(id);
            var post = await _context.posts.Include(p=> p.PostCategories).FirstOrDefaultAsync(p=>p.ID == id);
            if (post == null)
            {
                return NotFound();
            }
            var postEdit = new CreatePostModel()
            {
                 ID = post.ID,
                Title = post.Title,
               
                Content = post.Content,
                Descreption = post.Descreption,
                Slug = post.Slug,
                Published = post.Published,
                categoryID = post.PostCategories.Select(pc => pc.categoryID).ToArray()

            };
             var category = await _context.categories.ToListAsync();
             ViewData["categories"] = new MultiSelectList(category, "Id","Title");
            return View(postEdit);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Title,Descreption,Slug,Content,Published,categoryID")] CreatePostModel post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            var category = await _context.categories.ToListAsync();
             ViewData["categories"] = new MultiSelectList(category, "Id","Title");
               if(post.Slug == null)
              {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
              }

              if (await _context.posts.AnyAsync( p=> p.Slug == post.Slug)) 
              {
                ModelState.AddModelError("Slug", "Nhập chuỗi ký tự khác");
                return View(post);
              }

            if (ModelState.IsValid)
            {
                try
                {
                    var postUpdate = await _context.posts.Include(p=> p.PostCategories).FirstOrDefaultAsync(p=>p.ID == id);
                   if(postUpdate == null)
                   {
                         return NotFound();
                   }
                  postUpdate.Title = post.Title;
                  postUpdate.Descreption = post.Descreption;
                   postUpdate.Content = post.Content;
                    postUpdate.Published = post.Published;
                     postUpdate.Slug = post.Slug;
                      postUpdate.DateUpdated = DateTime.Now;

                    if(post.categoryID == null) post.categoryID = new int []{};

                    var oldCateId = postUpdate.PostCategories.Select(c =>c.categoryID).ToArray();
                    var newCateIDS = post.categoryID;
                      var removeCatePosts = from PostCategory in postUpdate.PostCategories
                                          where (!newCateIDS.Contains(PostCategory.categoryID))
                                          select PostCategory;
                    _context.postCategories.RemoveRange(removeCatePosts);

                    var addCateIds = from CateId in newCateIDS
                                     where !oldCateId.Contains(CateId)
                                     select CateId;

                     foreach (var CateId in addCateIds)
                     {
                         _context.postCategories.Add(new PostCategory(){
                             postID = id,
                             categoryID = CateId
                         });
                     }      

                    _context.Update(postUpdate);

                    await _context.SaveChangesAsync();
                }


                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.appUsers, "Id", "Id", post.AuthorId);
            return View(post);
        }

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.posts.FindAsync(id);
            _context.posts.Remove(post);
            await _context.SaveChangesAsync();
            StatusMessage = "Bạn vừa xóa " + post.Title;
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.posts.Any(e => e.ID == id);
        }
    }
}
