using DACS.Data;
using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DACS.Repositories
{
    public class EFPhieuLayMauRepository : IPhieuLayMauRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPhieuLayMauRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhieuLayMau>> GetAllPhieuLayMaus()
        {
            return await _context.PhieuLayMaus.ToListAsync();
        }

        public async Task<PhieuLayMau> GetPhieuLayMauById(int id)
        {
            return await _context.PhieuLayMaus.FindAsync(id);
        }

        public async Task AddPhieuLayMau(PhieuLayMau phieuLayMau)
        {
            await _context.PhieuLayMaus.AddAsync(phieuLayMau);
        }
        public async Task<PhieuLayMau> GetPhieuLayMau(int id)
        {
            return await _context.PhieuLayMaus
                .Include(plm => plm.ViTriLayMau)
                .FirstOrDefaultAsync(plm => plm.Id == id);
        }
        public void UpdatePhieuLayMau(PhieuLayMau phieuLayMau)
        {
            _context.PhieuLayMaus.Update(phieuLayMau);
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
