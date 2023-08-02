using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using webmvc.Models;
using webmvc.Models.Contact;
using Microsoft.AspNetCore.Authorization;
using webmvc.Data;

namespace webmvc.Areas.Contact.Controllers
{
    [Area("Contact")]
    [Authorize(Roles =RoleName.Administrator)]
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;

        public ContactController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Contact
        [HttpGet("/admin/contact")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.contacts.ToListAsync());
        }

        // GET: Contact/Details/5
        [HttpGet("/admin/contact/detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactt = await _context.contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactt == null)
            {
                return NotFound();
            }

            return View(contactt);
        }

        // GET: Contact/Create
        [HttpGet("/contact/")]
        [AllowAnonymous]
        public IActionResult SendContact()
        {
            return View();
        }
        [TempData]
        public string StatusMessage{get;set;}

        // POST: Contact/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/contact/")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContact([Bind("FullName,Email,Message,Phone")] Contactt contactt)
        {
            if (ModelState.IsValid)
            {
                contactt.DateSent= DateTime.Now;
                _context.Add(contactt);
                await _context.SaveChangesAsync();

                StatusMessage ="Lien he cua ban da dc gui";
                return RedirectToAction("Index","Home");
            }
            return View(contactt);
        }

  

        // GET: Contact/Delete/5
         [HttpGet("/admin/contact/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contactt = await _context.contacts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contactt == null)
            {
                return NotFound();
            }

            return View(contactt);
        }

        // POST: Contact/Delete/5
        [HttpPost ("/admin/contact/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contactt = await _context.contacts.FindAsync(id);
            _context.contacts.Remove(contactt);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
