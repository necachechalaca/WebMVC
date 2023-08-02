using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webmvc.Models.Blog;
using webmvc.Models;
using Microsoft.AspNetCore.Authorization;
using webmvc.Data;


namespace webmvc.Areas.Blog.Controllers
{
     [Authorize(Roles = RoleName.Administrator)]
    [Area("Blog")]
    [Route("admin/blog/category/[action]/{id?}")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Category
        public async Task<IActionResult> Index()
        {
           var qr = (from  c in _context.categories select c)
                    .Include(c=>c.ParentCategory)
                    .Include(c=> c.CategoryChildren);

            var categories = (await qr.ToListAsync()).Where(c=>c.ParentCategory == null)
                            .ToList();

            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        public void CreateSelectItem(List<Category> source, List<Category> des, int level)
        {
             string prefix = string.Concat( Enumerable.Repeat("*",level));
            foreach (var category in source)
            {
                // category.Title =prefix + category.Title;

                des.Add(new Category(){
                    Id =category.Id,
                    Title = prefix +" " +category.Title
                });
                if(category.CategoryChildren?.Count >0) 
                {
                    CreateSelectItem(category.CategoryChildren.ToList(),des,level +1);
                }

            }
        }

        // GET: Category/Create
        public async Task< IActionResult> Create()
        {
            var qr = (from c in _context.categories  select c)
                        .Include(c=>c.ParentCategory)
                        .Include(c=>c.CategoryChildren);
                        
            var  categories = (await qr.ToListAsync())
                            
                                .ToList();
            categories.Insert(0,new Category(){
                Id =-1,
                Title = "Không có danh mục cha"
            });   
            var item = new List<Category>();        

            CreateSelectItem(categories,item,0);
            var SelectList = new SelectList(categories, "Id", "Title");

            ViewData["ParentCategoryId"] = SelectList;
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
               if(category.ParentCategoryId ==-1) category.ParentCategoryId = null;
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
               var qr = (from c in _context.categories  select c)
                        .Include(c=>c.ParentCategory)
                        .Include(c=>c.CategoryChildren);
            var  categories = (await qr.ToListAsync())
                               
                                .ToList();

            categories.Insert(0,new Category(){
                Id =-1,
                Title = "Không có danh mục cha"
            });
            var item = new List<Category>();        

            CreateSelectItem(categories,item,0);                    
            var SelectList = new SelectList(categories, "Id", "Title");
            ViewData["ParentCategoryId"] = SelectList;
            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
              var qr = (from c in _context.categories  select c)
                        .Include(c=>c.ParentCategory)
                        .Include(c=>c.CategoryChildren);
            var  categories = (await qr.ToListAsync())
                               
                                .ToList();

            categories.Insert(0,new Category(){
                Id =-1,
                Title = "Không có danh mục cha"
            });
            var item = new List<Category>();        

            CreateSelectItem(categories,item,0);                    
            var SelectList = new SelectList(categories, "Id", "Title");
            ViewData["ParentCategoryId"] =           SelectList;

            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
           var qr = (from c in _context.categories  select c)
                        .Include(c=>c.ParentCategory)
                        .Include(c=>c.CategoryChildren);
            var  categories = (await qr.ToListAsync())
                               
                                .ToList();

            categories.Insert(0,new Category(){
                Id =-1,
                Title = "Không có danh mục cha"
            });
            var item = new List<Category>();        

            CreateSelectItem(categories,item,0);                    
            var SelectList = new SelectList(categories, "Id", "Title");
            ViewData["ParentCategoryId"] = SelectList;
            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
           var category = await _context.categories.
           Include(c=>c.CategoryChildren).FirstOrDefaultAsync(c =>c.Id ==id);
           if(category == null)
           {
            return NotFound();
           }
           foreach (var cCatrgory in category.CategoryChildren)
           {
                cCatrgory.ParentCategoryId = category.ParentCategoryId;
           }

             _context.categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.categories.Any(e => e.Id == id);
        }
    }
}
