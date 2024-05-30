using DACS.Data;
using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TableController : Controller
    {


        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TableController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /*public async Task<IActionResult> Index()
        {
            var model = (from phieuLayMau in _context.PhieuLayMaus
                         join ctPhieuLayMau in _context.CTPhieuLayMaus on phieuLayMau.Id equals ctPhieuLayMau.PhieuLayMauId
                         join viTriLayMau in _context.ViTriLayMaus on ctPhieuLayMau.ViTriLayMauId equals viTriLayMau.Id
                         join dongSong in _context.DongSongs on viTriLayMau.DongSong.Id equals dongSong.Id
                         join ctcacChat in _context.CTCacChats on ctPhieuLayMau.CTPhieuLayMauId equals ctcacChat.CTPhieuLayMauId
                         join chat in _context.Chats on ctcacChat.ChatId equals chat.Id
                         select new TableViewModel
                         {
                             DongSongName = dongSong.Name,
                             PhieuLayMauDate = phieuLayMau.Date,
                             ChatName = chat.Name,
                             Wo = phieuLayMau.Wo,
                             Qo = phieuLayMau.Qo,
                             WQI = ctcacChat.WQI
                         }).ToList();

            return View(model);
        }*/

        public async Task<IActionResult> Test()
        {
            var dongSongList = await _context.ViTriLayMaus.ToListAsync();
            return View(dongSongList);
        }

        // GET: Admin/Table/Add
        [HttpGet]
        public IActionResult AddDongSong()
        {
            return View();
        }

        // POST: Admin/Table/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDongSong(DongSong model)
        {
            if (ModelState.IsValid)
            {
                _context.DongSongs.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

    }


}
