using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AOMForum.Data;
using AOMForum.Data.Models;

namespace AOMForum.Web.Areas.Administration.Controllers
{   
    public class PostReportsController : AdministrationController
    {
        private readonly ApplicationDbContext _context;

        public PostReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Administration/PostReports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PostReports.Include(p => p.Author).Include(p => p.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Administration/PostReports/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostReports == null)
            {
                return NotFound();
            }

            var postReport = await _context.PostReports
                .Include(p => p.Author)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postReport == null)
            {
                return NotFound();
            }

            return View(postReport);
        }

        // GET: Administration/PostReports/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId");
            return View();
        }

        // POST: Administration/PostReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,PostId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PostReport postReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postReport.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postReport.PostId);
            return View(postReport);
        }

        // GET: Administration/PostReports/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostReports == null)
            {
                return NotFound();
            }

            var postReport = await _context.PostReports.FindAsync(id);
            if (postReport == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postReport.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postReport.PostId);
            return View(postReport);
        }

        // POST: Administration/PostReports/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Content,PostId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PostReport postReport)
        {
            if (id != postReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostReportExists(postReport.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postReport.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postReport.PostId);
            return View(postReport);
        }

        // GET: Administration/PostReports/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostReports == null)
            {
                return NotFound();
            }

            var postReport = await _context.PostReports
                .Include(p => p.Author)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postReport == null)
            {
                return NotFound();
            }

            return View(postReport);
        }

        // POST: Administration/PostReports/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PostReports'  is null.");
            }
            var postReport = await _context.PostReports.FindAsync(id);
            if (postReport != null)
            {
                _context.PostReports.Remove(postReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostReportExists(int id)
        {
          return _context.PostReports.Any(e => e.Id == id);
        }
    }
}