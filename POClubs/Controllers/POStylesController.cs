using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using POClubs.Data;
using POClubs.Models;

namespace POClubs.Controllers
{
    //The StylesController that contain all the method and Action Result
    public class POStylesController : Controller
    {
        private readonly POClubsContext _context;

        //The constructor for intialize
        public POStylesController(POClubsContext context)
        {
            _context = context;
        }

        //Default Action Page of the Style and list of instrument
        // GET: Styles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Style.ToListAsync());
        }

        //Detail Action Page of the selected style from the list by ID
        // GET: Styles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .FirstOrDefaultAsync(m => m.StyleName == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        //Creat Action Page new Style 
        // GET: Styles/Create
        public IActionResult Create()
        {
            return View();
        }

        //Creat Action Page new Style submission
        // POST: Styles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StyleName,Description")] Style style)
        {
            if (ModelState.IsValid)
            {
                _context.Add(style);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(style);
        }


        //Edit Action Page of the selected Style from the list by ID
        // GET: Styles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style.FindAsync(id);
            if (style == null)
            {
                return NotFound();
            }
            return View(style);
        }

        //Edit Action Page of the selected Style from the list by ID for submission
        // POST: Styles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StyleName,Description")] Style style)
        {
            if (id != style.StyleName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(style);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StyleExists(style.StyleName))
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
            return View(style);
        }

        //Delete Action Page of the selected Style from the list by ID
        // GET: Styles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var style = await _context.Style
                .FirstOrDefaultAsync(m => m.StyleName == id);
            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        //Delete Action Page of the selected Style from the list by ID Confirmation
        // POST: Styles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var style = await _context.Style.FindAsync(id);
            _context.Style.Remove(style);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //return True or False on existing style
        private bool StyleExists(string id)
        {
            return _context.Style.Any(e => e.StyleName == id);
        }
    }
}
