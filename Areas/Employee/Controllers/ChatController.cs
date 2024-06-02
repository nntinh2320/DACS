using DACS.Data;
using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Globalization;

namespace DACS.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class ChatController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Display the form to create a new PhieuLayMau
        public IActionResult CreatePhieuLayMau()
        {
            ViewBag.DongSongs = _context.DongSongs.ToList();
            return View();
        }

        public JsonResult GetViTriLayMaus(string dongSongId)
        {
            var viTriLayMaus = _context.ViTriLayMaus
                .Where(v => v.DongSong.Id == dongSongId)
                .ToList();
            return Json(viTriLayMaus);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePhieuLayMau(PhieuLayMau phieuLayMau, string viTriLayMauId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Convert string date to DateTime
                    //phieuLayMau.Date = DateTime.ParseExact(phieuLayMau.Date.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    phieuLayMau.Date = DateTime.Now;
                    // Assign other properties
                    phieuLayMau.EmployeeId = _userManager.GetUserId(User); // Get current user ID
                    phieuLayMau.ViTriLayMauId = viTriLayMauId;

                    // Add to database
                    _context.PhieuLayMaus.Add(phieuLayMau);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(CreateCTCacChat), new { phieuLayMauId = phieuLayMau.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewBag.DongSongs = _context.DongSongs.ToList();
            return View(phieuLayMau);
        }


        // Display the form to create new CTCacChat entries
        public IActionResult CreateCTCacChat(int phieuLayMauId)
        {
            ViewBag.Chats = _context.Chats.ToList();
            ViewBag.PhieuLayMauId = phieuLayMauId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCTCacChat(int phieuLayMauId, List<CTCacChat> CTCacChats)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var ctcacChat in CTCacChats)
                    {
                        ctcacChat.PhieuLayMauId = phieuLayMauId;
                    }

                    _context.CTCacChats.AddRange(CTCacChats);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            ViewBag.Chats = _context.Chats.ToList();
            ViewBag.PhieuLayMauId = phieuLayMauId;
            return View(CTCacChats);
        }

        // Existing Index action
        public async Task<IActionResult> Index()
        {
            var chats = await _context.Chats
                .Include(c => c.CTCacChats)
                    .ThenInclude(ctc => ctc.PhieuLayMau)
                        .ThenInclude(plm => plm.ViTriLayMau)
                            .ThenInclude(vtm => vtm.DongSong)
                .OrderBy(c => c.Name)
                .ToListAsync();

            foreach (var chat in chats)
            {
                foreach (var ctc in chat.CTCacChats)
                {
                    ctc.PhieuLayMau.Employee = await _userManager.FindByIdAsync(ctc.PhieuLayMau.EmployeeId);
                }
            }

            return View(chats);
        }

    }
}
