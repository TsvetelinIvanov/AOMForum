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
    public class PostVotesController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public PostVotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostVotes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PostVotes.Include(p => p.Author).Include(p => p.Post);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PostVotes/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PostVotes == null)
            {
                return NotFound();
            }

            var postVote = await _context.PostVotes
                .Include(p => p.Author)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postVote == null)
            {
                return NotFound();
            }

            return View(postVote);
        }

        // GET: PostVotes/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId");
            return View();
        }

        // POST: PostVotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,PostId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PostVote postVote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postVote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postVote.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postVote.PostId);
            return View(postVote);
        }

        // GET: PostVotes/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PostVotes == null)
            {
                return NotFound();
            }

            var postVote = await _context.PostVotes.FindAsync(id);
            if (postVote == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postVote.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postVote.PostId);
            return View(postVote);
        }

        // POST: PostVotes/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Type,PostId,AuthorId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] PostVote postVote)
        {
            if (id != postVote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postVote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostVoteExists(postVote.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", postVote.AuthorId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "AuthorId", postVote.PostId);
            return View(postVote);
        }

        // GET: PostVotes/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PostVotes == null)
            {
                return NotFound();
            }

            var postVote = await _context.PostVotes
                .Include(p => p.Author)
                .Include(p => p.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postVote == null)
            {
                return NotFound();
            }

            return View(postVote);
        }

        // POST: PostVotes/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PostVotes == null)
            {
                return Problem("Entity set 'ApplicationDbContext.PostVotes'  is null.");
            }
            var postVote = await _context.PostVotes.FindAsync(id);
            if (postVote != null)
            {
                _context.PostVotes.Remove(postVote);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostVoteExists(int id)
        {
          return _context.PostVotes.Any(e => e.Id == id);
        }
    }
}