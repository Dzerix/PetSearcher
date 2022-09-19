using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PetSearcher.Helper;
using PetSearcher.Models;
using PetSearcher.Models.ViewModels;
using PetSearcher.Services;

namespace PetSearcher.Controllers
{
    
    public class NoticeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INoticeService _noticeService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NoticeController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, INoticeService noticeService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _noticeService = noticeService;
            _userManager = userManager;
        }

        // GET: Notice

        public async Task<IActionResult> Index()
        {
            ViewBag.PhoneNumbers = _noticeService.GetUsersPhonesList();
            ViewBag.UserId = _userManager.GetUserId(HttpContext.User);

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
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Notices == null)
            {
                return NotFound();
            }

            var DBnotice = await _context.Notices.FindAsync(id);
            if (DBnotice == null)
            {
                return NotFound();
            }
            EditNoticeViewModel notice = new EditNoticeViewModel();
            notice.Location = DBnotice.Location;
            notice.Name = DBnotice.Name;
            notice.Description = DBnotice.Description;
            return View(notice);
        }

        // POST: Notice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, EditNoticeViewModel notice)
        {
            //if (id != notice.Id)
            //{
            //    return NotFound();
            //}
            if (_context.Notices == null)
            {
                return NotFound();
            }
            var DBnotice = await _context.Notices.FindAsync(id);
            if ( DBnotice != null)
            {
                
                DBnotice.Name = notice.Name;
                DBnotice.Description = notice.Description;
                DBnotice.Location = notice.Location;
               
            }
            else
            {
                return NotFound();
            }
            if (_userManager.GetUserId(HttpContext.User) == DBnotice.UserId || User.IsInRole("Support"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(DBnotice);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!NoticeExists(id))
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
            }
            else
            {
                return NotFound();
            }
            return View(notice);
        }

        // GET: Notice/Delete/5
        [Authorize]
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
        [Authorize]
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
                if (_userManager.GetUserId(HttpContext.User) == notice.UserId || User.IsInRole("Support"))
                {
                    if (System.IO.File.Exists(notice.ImagePath))
                    {
                        System.IO.File.Delete(notice.ImagePath);
                    }
                    _context.Notices.Remove(notice);
                }
                else
                {
                    return Problem("Error: You are not owner of this notice.");
                }
            }
            

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoticeExists(int id)
        {
          return (_context.Notices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [Authorize]
        public IActionResult MyNotices()
        {
            ViewBag.PhoneNumbers = _noticeService.GetUsersPhonesList();
            var UserID = _userManager.GetUserId(HttpContext.User);
            var NoticeList = new List<Notice>();
            if (_context.Notices != null)
            { 
                foreach (var notice in _context.Notices)
                {
                    if(notice.UserId == UserID)
                    {
                        NoticeList.Add(notice);
                    }
                }


            }
            return View(NoticeList);
        }
    }
}
