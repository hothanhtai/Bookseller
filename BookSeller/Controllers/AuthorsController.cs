using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSeller.Data;
using BookSeller.Models;
using System.Security.Policy;
using Microsoft.AspNetCore.Authorization;
using BookSeller.Data.Static;

namespace BookSeller.Controllers
{
    //[Authorize(Roles = UserRoles.Admin)]
    public class AuthorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthorsController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        //[AllowAnonymous]
        // GET: Authors
        public async Task<IActionResult> Index()
        {
              return View(await _context.Authors.ToListAsync());
        }
        [AllowAnonymous]
        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            ViewBag.bookAuthor = _context.Books.Where(n => n.AuthorId == author.Id);

            return View(author);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PictureFile,FullName,Bio")] Author author)
        {
            if (!ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(author.PictureFile.FileName);
                string extension = Path.GetExtension(author.PictureFile.FileName);
                fileName += DateTime.Now.ToString("ddMMyyyy") + extension;
                author.ProfilePictureURL = fileName;
                string path = Path.Combine(wwwRootPath + "/Img/Author", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await author.PictureFile.CopyToAsync(fileStream);
                }
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PictureFile,FullName,Bio")] Author author)
        {
            if (id != author.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(author.PictureFile.FileName);
                    string extension = Path.GetExtension(author.PictureFile.FileName);
                    fileName += DateTime.Now.ToString("ddMMyyyy") + extension;
                    author.ProfilePictureURL = fileName;
                    string path = Path.Combine(wwwRootPath + "/Img/Author", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await author.PictureFile.CopyToAsync(fileStream);
                    }
                    _context.Update(author);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorExists(author.Id))
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
            return View(author);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Authors == null)
            {
                return NotFound();
            }

            var author = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Authors == null)
            {
                return Problem("Entity set 'AppDbContext.Authors'  is null.");
            }
            var author = await _context.Authors.FindAsync(id);
            if (author != null)
            {
                _context.Authors.Remove(author);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorExists(int id)
        {
          return _context.Authors.Any(e => e.Id == id);
        }
    }
}
