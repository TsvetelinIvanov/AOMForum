using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AOMForum.Data;
using AOMForum.Data.Models;

namespace AOMForum.Web.Controllers
{
    public class CommentReportsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CommentReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CommentReports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CommentReports.Include(c => c.Author).Include(c => c.Comment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CommentReports/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommentReports == null)
            {
                return NotFound();
            }

            var commentReport = await _context.CommentReports
                .Include(c => c.Author)
                .Include(c => c.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentReport == null)
            {
                return NotFound();
            }

            return View(commentReport);
        }

        // GET: CommentReports/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId");
            return View();
        }

        // POST: CommentReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Content,CommentId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] CommentReport commentReport)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentReport);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentReport.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentReport.CommentId);
            return View(commentReport);
        }

        // GET: CommentReports/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommentReports == null)
            {
                return NotFound();
            }

            var commentReport = await _context.CommentReports.FindAsync(id);
            if (commentReport == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentReport.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentReport.CommentId);
            return View(commentReport);
        }

        // POST: CommentReports/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Content,CommentId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] CommentReport commentReport)
        {
            if (id != commentReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentReport);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentReportExists(commentReport.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentReport.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentReport.CommentId);
            return View(commentReport);
        }

        // GET: CommentReports/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommentReports == null)
            {
                return NotFound();
            }

            var commentReport = await _context.CommentReports
                .Include(c => c.Author)
                .Include(c => c.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentReport == null)
            {
                return NotFound();
            }

            return View(commentReport);
        }

        // POST: CommentReports/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommentReports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CommentReports'  is null.");
            }
            var commentReport = await _context.CommentReports.FindAsync(id);
            if (commentReport != null)
            {
                _context.CommentReports.Remove(commentReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentReportExists(int id)
        {
          return _context.CommentReports.Any(e => e.Id == id);
        }
    }
}