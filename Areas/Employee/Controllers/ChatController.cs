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
        private readonly IPhieuLayMauRepository _phieuLayMauRepository;
        private readonly ICTCacChatRepository _cTCacChatRepository;

        public ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IPhieuLayMauRepository phieuLayMauRepository, ICTCacChatRepository cTCacChatRepository)
        {
            _context = context;
            _userManager = userManager;
            _phieuLayMauRepository = phieuLayMauRepository;
            _cTCacChatRepository = cTCacChatRepository;
        }

        [HttpGet]
        public IActionResult CreatePhieuLayMau()
        {
            ViewBag.DongSongs = _context.DongSongs.ToList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePhieuLayMau(PhieuLayMau phieuLayMau, string viTriLayMauId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    phieuLayMau.Date = DateTime.Now;
                    phieuLayMau.EmployeeId = _userManager.GetUserId(User); // Get current user ID
                    phieuLayMau.ViTriLayMauId = viTriLayMauId;

                    await _phieuLayMauRepository.AddPhieuLayMau(phieuLayMau);
                    await _phieuLayMauRepository.SaveAsync();

                    return RedirectToAction(nameof(CreateCTCacChat), new { phieuLayMauId = phieuLayMau.Id });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }
            else
            {
                // Capture and log any model state errors for debugging
                var modelStateErrors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in modelStateErrors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }

            ViewBag.DongSongs = _context.DongSongs.ToList();
            return View(phieuLayMau);
        }
        [HttpGet]
        public JsonResult GetViTriLayMaus(string dongSongId)
        {
            var viTriLayMaus = _context.ViTriLayMaus
                .Where(v => v.DongSong.Id == dongSongId)
                .ToList();
            return Json(viTriLayMaus);
        }

        [HttpGet]
        public IActionResult CreateCTCacChat(int phieuLayMauId)
        {
            ViewBag.Chats = _context.Chats.ToList();
            ViewBag.PhieuLayMauId = phieuLayMauId;

            var cTCacChats = new List<CTCacChat>();
            foreach (var chat in ViewBag.Chats as List<Chat>)
            {
                cTCacChats.Add(new CTCacChat
                {
                    ChatId = chat.Id,
                    PhieuLayMauId = phieuLayMauId
                });
            }

            return View(cTCacChats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCTCacChat(int phieuLayMauId, List<CTCacChat> CTCacChats)
        {
            ViewBag.Chats = _context.Chats.ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    int i = 1;
                    foreach (var ctcacChat in CTCacChats)
                    {
                      
                        ctcacChat.ChatId = i;
                        i++;
                        ctcacChat.PhieuLayMauId = phieuLayMauId;
                    }

                    await _cTCacChatRepository.AddCTCacChats(CTCacChats);
                    await _cTCacChatRepository.SaveAsync();

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

        [HttpGet]
        public async Task<IActionResult> EditCTCacChat(int ctcacChatId)
        {
            try
            {
                // Lấy thông tin CTCacChat từ ctcacChatId
                var ctcacChat = await _cTCacChatRepository.GetCTCacChatById(ctcacChatId);
                if (ctcacChat == null)
                {
                    return NotFound();
                }

                // Lấy thông tin phiếu lấy mẫu liên quan
                var phieuLayMau = await _phieuLayMauRepository.GetPhieuLayMau(ctcacChat.PhieuLayMauId);
                if (phieuLayMau == null)
                {
                    return NotFound();
                }

                // Lấy danh sách các dòng sông và vị trí lấy mẫu để đổ vào dropdown
                ViewBag.DongSongs = _context.DongSongs.ToList();
                ViewBag.ViTriLayMaus = _context.ViTriLayMaus
                    .Where(v => v.DongSongId == phieuLayMau.ViTriLayMau.DongSongId)
                    .ToList();

                // Lấy danh sách CTCacChat của phiếu lấy mẫu này
                var cTCacChats = await _cTCacChatRepository.GetAllCTCacChats();
                var filteredCTCacChats = cTCacChats.Where(c => c.PhieuLayMauId == phieuLayMau.Id).ToList();

                // Trả về view với tuple PhieuLayMau và List CTCacChat
                return View(new Tuple<PhieuLayMau, List<CTCacChat>>(phieuLayMau, filteredCTCacChats));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditCTCacChat(Tuple<PhieuLayMau, List<CTCacChat>> model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var phieuLayMau = model.Item1;
                    _phieuLayMauRepository.UpdatePhieuLayMau(phieuLayMau);

                    foreach (var ctcacChat in model.Item2)
                    {
                        _cTCacChatRepository.UpdateCTCacChat(ctcacChat);
                    }

                    await _phieuLayMauRepository.SaveAsync();
                    await _cTCacChatRepository.SaveAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                }
            }

            // Load necessary data again if there are validation errors
            ViewBag.DongSongs = _context.DongSongs.ToList();
            ViewBag.ViTriLayMaus = _context.ViTriLayMaus
                .Where(v => v.DongSongId == model.Item1.ViTriLayMau.DongSongId)
                .ToList();

            return View(model);
        }


        public async Task<IActionResult> Index()
        {
            var chats = await _context.Chats
                .Include(c => c.CTCacChats)
                    .ThenInclude(ctc => ctc.PhieuLayMau)
                        .ThenInclude(plm => plm.ViTriLayMau)
                            .ThenInclude(vtm => vtm.DongSong)
                
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
