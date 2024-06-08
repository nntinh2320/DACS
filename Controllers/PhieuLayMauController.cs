using DACS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DACS.Repositories;

namespace DACS.Controllers
{
    public class PhieuLayMauController : Controller
    {
        private readonly ILogger<PhieuLayMauController> _logger;
        private readonly IPhieuLayMauRepository _phieuLayMau;
        private readonly ApplicationDbContext _context;

        public PhieuLayMauController(ILogger<PhieuLayMauController> logger, IPhieuLayMauRepository phieuLayMau, ApplicationDbContext context)
        {
            _logger = logger;
            _phieuLayMau = phieuLayMau;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var allPhieu = _context.PhieuLayMaus.AsQueryable();
            var phieus = await allPhieu.ToListAsync();
            return View(phieus);
        }
    }
}
