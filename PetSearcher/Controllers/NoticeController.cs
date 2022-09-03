using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetSearcher.Helper;
using PetSearcher.Models;
using PetSearcher.Services;

namespace PetSearcher.Controllers
{
    
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INoticeService _noticeService;

        public NoticeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, INoticeService noticeService)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _noticeService = noticeService;
        }

        // GET: Notice

        public async Task<IActionResult> Index()
        {
            ViewBag.PhoneNumbers = _noticeService.GetUsersPhonesList();

            return _context.Notices != null ? 
                          View(await _context.Notices.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
        }

        // GET: Notice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // GET: Notice/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.PetTypesList = HelperClass.GetRolesForDropDown();
            return View();
        }

        // POST: Notice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KindOfPet,Name,Description,Location,ImagePath,ImageFile,UserId")] Notice notice)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(notice.ImageFile.FileName);
                string fileExtension = Path.GetExtension(notice.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + fileExtension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                notice.ImagePath = path;
                notice.ImageName = fileName;
                using(var filestream = new FileStream(path, FileMode.Create))
                {
                    await notice.ImageFile.CopyToAsync(filestream);
                }
                notice.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                   



                _context.Add(notice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(notice);
        }

        // GET: Notice/Edit/5
        [Authorize(Roles ="Support")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices.FindAsync(id);
            if (notice == null)
            {
                return NotFound();
            }
            return View(notice);
        }

        // POST: Notice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Support")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KindOfPet,Name,Description,ImagePath,UserId")] Notice notice)
        {
            if (id != notice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(notice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoticeExists(notice.Id))
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
            return View(notice);
        }

        // GET: Notice/Delete/5
        [Authorize(Roles = "Support")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var notice = await _context.Notices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (notice == null)
            {
                return NotFound();
            }

            return View(notice);
        }

        // POST: Notice/Delete/5
        [Authorize(Roles = "Support")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Notices == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Notices'  is null.");
            }
            var notice = await _context.Notices.FindAsync(id);
            if (notice != null)
            {
                if (System.IO.File.Exists(notice.ImagePath))
                {
                    System.IO.File.Delete(notice.ImagePath);
                }
                _context.Notices.Remove(notice);
                
            }
            

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticeExists(int id)
        {
          return (_context.Notices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
