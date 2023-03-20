using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookSeller.Data;
using BookSeller.Models;
using Microsoft.AspNetCore.Hosting;
using NuGet.Protocol;
using Microsoft.AspNetCore.Authorization;
using BookSeller.Data.Static;

namespace BookSeller.Controllers
{
	[Authorize(Roles = UserRoles.Admin)]
	public class BooksController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		public BooksController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}
		[AllowAnonymous]

		// GET: Books
		public async Task<IActionResult> Index()
		{
			TempData["Cate"] = 0;
			var appDbContext = _context.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b => b.Category);
			return View(await appDbContext.ToListAsync());
		}
		[AllowAnonymous]

		public IActionResult Filter(string searchString)
		{
			var allBooks = from b in _context.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b => b.Category) select b;
			if (!string.IsNullOrEmpty(searchString))
			{
				var filterBooks = allBooks.Where(s => s.Name.Contains(searchString) || s.Author.FullName.Contains(searchString)).ToList();
				if (filterBooks.Count == 0)
				{
					ViewBag.emty = "Không tìm thấy cuốn sách này";
				}
				return View("Index", filterBooks);
			}
			return View("Index", allBooks.ToList());
		}

		[AllowAnonymous]
		public IActionResult Category(int category)
		{
			var allBooks = from b in _context.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b => b.Category) select b;
			var filterBooks = allBooks.Where(s => ((int)s.CategoryId) == category).ToList();
			if(filterBooks.Count == 0)
			{
				ViewBag.emtyCate = "Không có thể loại bạn vừa tìm";
			}
			TempData["Cate"] = category;
			return View("Index", filterBooks);
		}
		[AllowAnonymous]

		// GET: Books/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Books == null)
			{
				return NotFound();
			}

			var book = await _context.Books
				.Include(b => b.Author)
				.Include(b => b.Publisher)
				.Include(b => b.Category)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (book == null)
			{
				return NotFound();
			}
			var bookPublishers = _context.Books.Where(n => n.Id != book.Id).Where(n => n.AuthorId == book.AuthorId).Take(5);
			ViewBag.bookPublisher = bookPublishers.Count() != 0 ? bookPublishers : null;
			var bookAuthors = _context.Books.Where(n => n.Id != book.Id).Where(n => n.PublisherId == book.PublisherId).Take(5);
			ViewBag.bookAuthor = bookAuthors.Count() != 0 ? bookAuthors : null;
			var bookCates = _context.Books.Where(n => n.CategoryId == book.CategoryId).Where(n => n.Id != book.Id).Take(5);
			ViewBag.bookCate = bookCates.Count() != 0 ? bookCates : null;
			return View(book);
		}
		// GET: Books/Create
		public IActionResult Create()
		{
			ViewBag.AuthorId = new SelectList(_context.Authors.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.PublisherId = new SelectList(_context.Publishers.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(n => n.Name).ToList(), "Id", "Name");
			return View();
		}

		// POST: Books/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,PictureFile,PublishingYear,CategoryId,AuthorId,PublisherId")] Book book)
		{
			if (!ModelState.IsValid)
			{
				string wwwRootPath = _webHostEnvironment.WebRootPath;
				string fileName = Path.GetFileNameWithoutExtension(book.PictureFile.FileName);
				string extension = Path.GetExtension(book.PictureFile.FileName);
				fileName += DateTime.Now.ToString("ddMMyyyy") + extension;
				book.ImageURL = fileName;
				string path = Path.Combine(wwwRootPath + "/Img/Book", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await book.PictureFile.CopyToAsync(fileStream);
				}
				_context.Add(book);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewBag.AuthorId = new SelectList(_context.Authors.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.PublisherId = new SelectList(_context.Publishers.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(n => n.Name).ToList(), "Id", "Name");
			return View(book);
		}

		// GET: Books/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Books == null)
			{
				return NotFound();
			}

			var book = await _context.Books.FindAsync(id);
			if (book == null)
			{
				return NotFound();
			}
			ViewBag.AuthorId = new SelectList(_context.Authors.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.PublisherId = new SelectList(_context.Publishers.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(n => n.Name).ToList(), "Id", "Name");
			ViewBag.Url = book.ImageURL;
			return View(book);
		}

		// POST: Books/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,ImageURL, PublishingYear,CategoryId,AuthorId,PublisherId")] Book book)
		{
			if (id != book.Id)
			{
				return NotFound();
			}

			if (!ModelState.IsValid)
			{
				try
				{
					_context.Update(book);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!BookExists(book.Id))
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
			ViewBag.AuthorId = new SelectList(_context.Authors.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.PublisherId = new SelectList(_context.Publishers.OrderBy(n => n.FullName).ToList(), "Id", "FullName");
			ViewBag.CategoryId = new SelectList(_context.Categories.OrderBy(n => n.Name).ToList(), "Id", "Name");
			return View(book);
		}

		// GET: Books/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Books == null)
			{
				return NotFound();
			}

			var book = await _context.Books
				.Include(b => b.Author)
				.Include(b => b.Publisher)
				.Include(b => b.Category)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (book == null)
			{
				return NotFound();
			}

			return View(book);
		}

		// POST: Books/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Books == null)
			{
				return Problem("Entity set 'AppDbContext.Books'  is null.");
			}
			var book = await _context.Books.FindAsync(id);
			if (book != null)
			{
				_context.Books.Remove(book);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool BookExists(int id)
		{
			return _context.Books.Any(e => e.Id == id);
		}
	}
}
