using DACS.Data;
using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using YourNamespace;

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


        public async Task<IActionResult> Index()
        {
            // Get all chat names
            var allChats = _context.Chats.Select(c => c.Name).ToList();

            var samples = _context.CTCacChats
                .Include(ct => ct.Chat)
                .Include(ct => ct.PhieuLayMau)
                    .ThenInclude(plm => plm.ViTriLayMau)
                        .ThenInclude(vtlm => vtlm.DongSong)
                .ToList();

            var groupedSamples = samples.GroupBy(s => new
            {
                s.PhieuLayMau.ViTriLayMau.DongSong.Name,
                ViTriLayMau = s.PhieuLayMau.ViTriLayMau.Id, // assuming Id or some unique identifier for ViTriLayMau
                s.PhieuLayMau.Date,
                s.PhieuLayMau.Wo,
                s.PhieuLayMau.Qo,
                s.WQI,
                s.MucDoONhiem
            })
            .Select(g => new SampleViewModel
            {
                DongSongName = g.Key.Name,
                ViTriLayMau = g.Key.ViTriLayMau,
                PhieuLayMauDate = g.Key.Date,
                Wo = g.Key.Wo,
                Qo = g.Key.Qo,
                WQI = g.Key.WQI,
                MucDo = g.Key.MucDoONhiem,
                ChatValues = allChats.ToDictionary(
                    chatName => chatName,
                    chatName => g.FirstOrDefault(ct => ct.Chat.Name == chatName)?.GiaTri
                )
            })
            .ToList();

            return View(groupedSamples);
        }

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


        // GET: /Table/Add
        public IActionResult Add()
        {
            var model = new AddSampleViewModel
            {
                DongSongs = _context.DongSongs.Select(ds => new SelectListItem
                {
                    Value = ds.Id.ToString(),
                    Text = ds.Name
                }).ToList(),
                ViTriLayMaus = _context.ViTriLayMaus.Select(vt => new SelectListItem
                {
                    Value = vt.Id.ToString(),
                    Text = vt.Id.ToString() // Assuming there is a Name property
                }).ToList(),
                ChatValues = _context.Chats.ToDictionary(c => c.Name, c => (float?)null)
            };

            return View(model);
        }

        // POST: /Table/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(AddSampleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DongSongs = _context.DongSongs.Select(ds => new SelectListItem
                {
                    Value = ds.Id.ToString(),
                    Text = ds.Name
                }).ToList();
                model.ViTriLayMaus = _context.ViTriLayMaus.Select(vt => new SelectListItem
                {
                    Value = vt.Id.ToString(),
                    Text = vt.Id.ToString() // Assuming there is a Name property
                }).ToList();
                model.ChatValues = _context.Chats.ToDictionary(c => c.Name, c => model.ChatValues.ContainsKey(c.Name) ? model.ChatValues[c.Name] : (float?)null);
                return View(model);
            }

            var phieuLayMau = new PhieuLayMau
            {
                Date = model.Date,
                Wo = model.Wo,
                Qo = model.Qo,
                EmployeeId = model.EmployeeId,
                ViTriLayMauId = model.ViTriLayMauId
            };

            _context.PhieuLayMaus.Add(phieuLayMau);
            _context.SaveChanges();

            foreach (var chatValue in model.ChatValues)
            {
                var chatId = _context.Chats.FirstOrDefault(c => c.Name == chatValue.Key)?.Id;
                if (chatId == null) continue;

                var ctCacChat = new CTCacChat
                {
                    PhieuLayMauId = phieuLayMau.Id,
                    ChatId = chatId.Value,
                    GiaTri = chatValue.Value ?? 0,
                    WQI = model.WQI ?? 0,
                    MucDoONhiem = model.MucDoONhiem
                };

                _context.CTCacChats.Add(ctCacChat);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
