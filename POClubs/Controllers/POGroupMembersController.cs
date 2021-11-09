using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POClubs.Data;
using POClubs.Models;

namespace POClubs.Controllers
{
    public class POGroupMembersController : Controller
    {
        private readonly POClubsContext _context;

        public POGroupMembersController(POClubsContext context)
        {
            _context = context;
        }

        // GET: POGroupMembers
        public async Task<IActionResult> Index(int ArtistId)
        {
            //Checking for session or querystring
            var artistId = SessionChecker(ArtistId);
            if (artistId == 0)
            {
                TempData["Message"] = "Kindly select Artist to view Group member!";
                return RedirectToAction("Index", "POArtist");
            }

            var pOClubsContext = _context.GroupMember
            .Include(g => g.ArtistIdGroupNavigation)
            .Include(g => g.ArtistIdGroupNavigation.NameAddress)
            .Include(g => g.ArtistIdMemberNavigation)
            .Where(g => g.ArtistIdGroup == artistId);

            List<GroupMember> model = await pOClubsContext.ToListAsync();
            if (model.Any())
            {
                return base.View(model);
            }

            return RedirectToAction("GroupsforArtist", "POGroupMembers", new { artistId });
            //return await GroupsforArtist(ArtistId);
        }

        public async Task<IActionResult> GroupsforArtist(int ArtistId)
        {
            TempData["Message"] = "The artist is an individual, not a group, so here’s their historic group memberships";

            var pOClubsContext = _context.GroupMember
                        .Include(g => g.ArtistIdGroupNavigation)
                        .Include(g => g.ArtistIdMemberNavigation)
                        .Include(g => g.ArtistIdMemberNavigation.NameAddress)
                        .Where(g => g.ArtistIdMember == ArtistId).OrderBy(g => g.DateLeft).ThenBy(g => g.DateJoined);
            List<GroupMember> model = await pOClubsContext.ToListAsync();
            if (model.Any())
            {
                return base.View(model);
            }

            TempData["Message"] = "The artist is neither a group nor a group member, but they can become a group";
            return RedirectToAction("Create", "POGroupMembers");
        }

        public int SessionChecker(int artistId = 0)
        {
            if (artistId != 0)
            {
                HttpContext.Session.SetInt32("ArtistId", artistId);
                return artistId;
            }
            if (HttpContext.Session.GetInt32("ArtistId") != 0)
            {
                var heldId = HttpContext.Session.GetInt32("ArtistId");
                return (int)heldId;
            }
            return 0;
        }

        // GET: POGroupMembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // GET: POGroupMembers/Create
        public IActionResult Create()
        {
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId");
            var artistMember = _context.Artist
            .Include(a => a.NameAddress)
            .Select(a => new
            {
                a.ArtistId,
                fullName = $"{a.NameAddress.FirstName} {a.NameAddress.LastName}"
            }).ToList();
            ViewData["ArtistIdMember"] = new SelectList(artistMember, "ArtistId", "fullName");
            return View();
        }

        // POST: POGroupMembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (ModelState.IsValid)
            {
                groupMember.DateJoined = DateTime.Now;
                groupMember.DateLeft = null;
                _context.Add(groupMember);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "POArtist");
            }

            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            var artistMember = _context.Artist
            .Include(a => a.NameAddress)
            .Select(a => new
            {
                a.ArtistId,
                fullName = $"{a.NameAddress.FirstName} {a.NameAddress.LastName}"
            }).ToList();
            ViewData["ArtistIdMember"] = new SelectList(artistMember, "ArtistId", "fullName", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // GET: POGroupMembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember.FindAsync(id);
            if (groupMember == null)
            {
                return NotFound();
            }
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            var artistMember = _context.Artist
            .Include(a => a.NameAddress)
            .Select(a => new
            {
                a.ArtistId,
                fullName = $"{a.NameAddress.FirstName} {a.NameAddress.LastName}"
            }).ToList();
            ViewData["ArtistIdMember"] = new SelectList(artistMember, "ArtistId", "fullName", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // POST: POGroupMembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistIdGroup,ArtistIdMember,DateJoined,DateLeft")] GroupMember groupMember)
        {
            if (id != groupMember.ArtistIdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupMemberExists(groupMember.ArtistIdGroup))
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
            ViewData["ArtistIdGroup"] = new SelectList(_context.Artist, "ArtistId", "ArtistId", groupMember.ArtistIdGroup);
            var artistMember = _context.Artist
            .Include(a => a.NameAddress)
            .Select(a => new
            {
                a.ArtistId,
                fullName = $"{a.NameAddress.FirstName} {a.NameAddress.LastName}"
            }).ToList();
            ViewData["ArtistIdMember"] = new SelectList(artistMember, "ArtistId", "fullName", groupMember.ArtistIdMember);
            return View(groupMember);
        }

        // GET: POGroupMembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupMember = await _context.GroupMember
                .Include(g => g.ArtistIdGroupNavigation)
                .Include(g => g.ArtistIdMemberNavigation)
                .FirstOrDefaultAsync(m => m.ArtistIdGroup == id);
            if (groupMember == null)
            {
                return NotFound();
            }

            return View(groupMember);
        }

        // POST: POGroupMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupMember = await _context.GroupMember.FindAsync(id);
            _context.GroupMember.Remove(groupMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupMemberExists(int id)
        {
            return _context.GroupMember.Any(e => e.ArtistIdGroup == id);
        }
    }
}
