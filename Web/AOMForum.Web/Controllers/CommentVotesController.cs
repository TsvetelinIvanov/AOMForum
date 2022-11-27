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
    public class CommentVotesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public CommentVotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CommentVotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CommentVotes.Include(c => c.Author).Include(c => c.Comment);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CommentVotes/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CommentVotes == null)
            {
                return NotFound();
            }

            var commentVote = await _context.CommentVotes
                .Include(c => c.Author)
                .Include(c => c.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentVote == null)
            {
                return NotFound();
            }

            return View(commentVote);
        }

        // GET: CommentVotes/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId");
            return View();
        }

        // POST: CommentVotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,CommentId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] CommentVote commentVote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentVote.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentVote.CommentId);
            return View(commentVote);
        }

        // GET: CommentVotes/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CommentVotes == null)
            {
                return NotFound();
            }

            var commentVote = await _context.CommentVotes.FindAsync(id);
            if (commentVote == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentVote.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentVote.CommentId);
            return View(commentVote);
        }

        // POST: CommentVotes/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Type,CommentId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] CommentVote commentVote)
        {
            if (id != commentVote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentVoteExists(commentVote.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", commentVote.AuthorId);
            ViewData["CommentId"] = new SelectList(_context.Comments, "Id", "AuthorId", commentVote.CommentId);
            return View(commentVote);
        }

        // GET: CommentVotes/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CommentVotes == null)
            {
                return NotFound();
            }

            var commentVote = await _context.CommentVotes
                .Include(c => c.Author)
                .Include(c => c.Comment)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentVote == null)
            {
                return NotFound();
            }

            return View(commentVote);
        }

        // POST: CommentVotes/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CommentVotes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CommentVotes'  is null.");
            }
            var commentVote = await _context.CommentVotes.FindAsync(id);
            if (commentVote != null)
            {
                _context.CommentVotes.Remove(commentVote);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentVoteExists(int id)
        {
          return _context.CommentVotes.Any(e => e.Id == id);
        }
    }
}