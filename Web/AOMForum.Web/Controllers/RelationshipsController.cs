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
    public class RelationshipsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public RelationshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Relationships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Relationships.Include(r => r.Follower).Include(r => r.Leader);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Relationships/Details/1
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Relationships == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationships
                .Include(r => r.Follower)
                .Include(r => r.Leader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // GET: Relationships/Create
        public IActionResult Create()
        {
            ViewData["FollowerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["LeaderId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Relationships/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaderId,FollowerId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Relationship relationship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(relationship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FollowerId"] = new SelectList(_context.Users, "Id", "Id", relationship.FollowerId);
            ViewData["LeaderId"] = new SelectList(_context.Users, "Id", "Id", relationship.LeaderId);
            return View(relationship);
        }

        // GET: Relationships/Edit/1
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Relationships == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationships.FindAsync(id);
            if (relationship == null)
            {
                return NotFound();
            }
            ViewData["FollowerId"] = new SelectList(_context.Users, "Id", "Id", relationship.FollowerId);
            ViewData["LeaderId"] = new SelectList(_context.Users, "Id", "Id", relationship.LeaderId);
            return View(relationship);
        }

        // POST: Relationships/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaderId,FollowerId,IsDeleted,DeletedOn,Id,CreatedOn,ModifiedOn")] Relationship relationship)
        {
            if (id != relationship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(relationship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelationshipExists(relationship.Id))
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
            ViewData["FollowerId"] = new SelectList(_context.Users, "Id", "Id", relationship.FollowerId);
            ViewData["LeaderId"] = new SelectList(_context.Users, "Id", "Id", relationship.LeaderId);
            return View(relationship);
        }

        // GET: Relationships/Delete/1
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Relationships == null)
            {
                return NotFound();
            }

            var relationship = await _context.Relationships
                .Include(r => r.Follower)
                .Include(r => r.Leader)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relationship == null)
            {
                return NotFound();
            }

            return View(relationship);
        }

        // POST: Relationships/Delete/1
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Relationships == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Relationships'  is null.");
            }
            var relationship = await _context.Relationships.FindAsync(id);
            if (relationship != null)
            {
                _context.Relationships.Remove(relationship);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RelationshipExists(int id)
        {
          return _context.Relationships.Any(e => e.Id == id);
        }
    }
}