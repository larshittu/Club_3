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
    //The CountryController that contain all the method and Action Result
    public class POCountryController : Controller
    {
        private readonly POClubsContext _context;

        //The constructor for intialize
        public POCountryController(POClubsContext context)
        {
            _context = context;
        }

        //Default Action Page of the Country and list of instrument
        // GET: Country list
        public async Task<IActionResult> Index()
        {
            return View(await _context.Country.OrderBy(c => c.Name).ToListAsync());
        }

        //Detail Action Page of the selected country from the list by ID
        // GET: Country/Details/5
        public async Task<IActionResult> Details(string id)
        {
            HttpContext.Session.SetString("CountryCode", id);

            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            var pOClubsContext = await _context.Province.Include(p => p.CountryCodeNavigation).Where(p => p.CountryCode == id).ToListAsync();
            ViewBag.Prinvinces = pOClubsContext;
            return View(country);
        }

        //Creat Action Page new Country 
        // GET: Country/Create
        public IActionResult Create()
        {
            return View();
        }

        //Creat Action Page new country submission
        // POST: Country/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax,ProvinceTerminology")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        //Edit Action Page of the selected country from the list by ID
        // GET: Country/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        //Edit Action Page of the selected Country from the list by ID for submission
        // POST: Country/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CountryCode,Name,PostalPattern,PhonePattern,FederalSalesTax,ProvinceTerminology")] Country country)
        {
            if (id != country.CountryCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryCode))
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
            return View(country);
        }

        //Delete Action Page of the Country Style from the list by ID
        // GET: Country/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var country = await _context.Country
                .FirstOrDefaultAsync(m => m.CountryCode == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        //Delete Action Page of the selected Country from the list by ID Confirmation
        // POST: Country/Delete/5
        // Delete country by id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //return True or False on existing Country
        private bool CountryExists(string id)
        {
            return _context.Country.Any(e => e.CountryCode == id);
        }
    }
}
