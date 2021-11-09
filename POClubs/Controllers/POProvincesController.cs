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
    //The POProvincesController that contain all the method and Action Result
    public class POProvincesController : Controller
    {
        private readonly POClubsContext _context;

        //The constructor for intialize
        public POProvincesController(POClubsContext context)
        {
            _context = context;
        }

        //Default Action Page of the Country and list of instrument
        // GET: POProvinces
        public async Task<IActionResult> Index(string countryCode)
        {
            //Checking for session or querystring
            var code = SessionChecker(countryCode);
            if (string.IsNullOrEmpty(code))
            {
                TempData["Message"] = "Select a country to view province!";
                return RedirectToAction("Index", "POCountry");
            }

            var pOClubsContext = await _context.Province.Include(p => p.CountryCodeNavigation).Where(p => p.CountryCode == code).ToListAsync();
            return View(pOClubsContext);
        }

        //Checking for session or querystring method
        public string SessionChecker(string countryCode = null)
        {
            if (!string.IsNullOrEmpty(countryCode))
            {
                HttpContext.Session.SetString("CountryCode", countryCode);
                return countryCode;
            }
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("CountryCode")))
            {
                var heldcountryCode = HttpContext.Session.GetString("CountryCode");
                return heldcountryCode;
            }
            return null;
        }

        //Detail Action Page of the selected Province from the list by ID
        // GET: POProvinces/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        //Creat Action Page new Province 
        // GET: POProvinces/Create
        public IActionResult Create()
        {
            //Checking for session or querystring            
            if (string.IsNullOrEmpty(SessionChecker()))
            {
                TempData["Message"] = "Select a country to view province!";
                return RedirectToAction("Index", "POCountry");
            }

            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode");
            ViewData["CountryCode"] = HttpContext.Session.GetString("CountryCode");
            return View();
        }


        //Creat Action Page new Province submission
        // POST: POProvinces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getCode = await _context.Province.Where(p => p.ProvinceCode == province.ProvinceCode).ToListAsync();
                    if (getCode.Any())
                    {
                        TempData["Message"] = "Province Code exist, Kindly provide a new province code.";
                        return View(province);
                    }

                    var getName = await _context.Province.Where(p => p.Name == province.Name.Trim()).ToListAsync();
                    if (getName.Any())
                    {
                        TempData["Message"] = "Province Name exist, Kindly provide a new Name.";
                        return View(province);
                    }

                    province.CountryCode = HttpContext.Session.GetString("CountryCode");
                    _context.Add(province);
                    await _context.SaveChangesAsync();
                    // return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "POProvinces", new { CountryCode = province.CountryCode });
                }
                // ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);

                ViewData["CountryCode"] = HttpContext.Session.GetString("CountryCode");
                province.CountryCode = HttpContext.Session.GetString("CountryCode");
                return View(province);

            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.ToString();
                return View(province);
            }
        }

        //Edit Action Page of the selected province from the list by ID
        // GET: POProvinces/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(SessionChecker()))
            {
                TempData["Message"] = "Select a country to view province!";
                return RedirectToAction("Index", "POCountry");
            }

            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province.FindAsync(id);
            if (province == null)
            {
                return NotFound();
            }
            //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
            ViewData["CountryCode"] = HttpContext.Session.GetString("CountryCode");
            return View(province);
        }


        //Edit Action Page of the selected province from the list by ID for submission
        // POST: POProvinces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProvinceCode,Name,CountryCode,SalesTaxCode,SalesTax,IncludesFederalTax,FirstPostalLetter")] Province province)
        {
            try
            {
                if (id != province.ProvinceCode)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        //province.CountryCode = HttpContext.Session.GetString("CountryCode");
                        _context.Update(province);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProvinceExists(province.ProvinceCode))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //return RedirectToAction(nameof(Index));
                    return RedirectToAction("Index", "POProvinces", new { CountryCode = province.CountryCode });
                }
                //ViewData["CountryCode"] = new SelectList(_context.Country, "CountryCode", "CountryCode", province.CountryCode);
                ViewData["CountryCode"] = HttpContext.Session.GetString("CountryCode");
                province.CountryCode = HttpContext.Session.GetString("CountryCode");
                return View(province);
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return View(province);
            }
        }

        //Delete Action Page of the selected Province from the list by ID
        // GET: POProvinces/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var province = await _context.Province
                .Include(p => p.CountryCodeNavigation)
                .FirstOrDefaultAsync(m => m.ProvinceCode == id);
            if (province == null)
            {
                return NotFound();
            }

            return View(province);
        }

        //Delete Action Page of the selected Province from the list by ID Confirmation
        // POST: POProvinces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var province = await _context.Province.FindAsync(id);
            _context.Province.Remove(province);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //return True or False on existing Province
        private bool ProvinceExists(string id)
        {
            return _context.Province.Any(e => e.ProvinceCode == id);
        }
    }
}
