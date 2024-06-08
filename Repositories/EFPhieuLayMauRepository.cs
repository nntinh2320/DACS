/*using NCKH.Data;*/
using DACS.Data;
using DACS.Models;
using Microsoft.EntityFrameworkCore;
using DACS.Repositories;

namespace WebsiteFashion.Repositories
{
    public class EFPhieuLayMauRepository : IPhieuLayMauRepository
    {
        private readonly ApplicationDbContext _context;
        public EFPhieuLayMauRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<PhieuLayMau>> GetAllAsync()
        {
            return await _context.PhieuLayMaus.ToListAsync();
        }
        public async Task<PhieuLayMau> GetByIdAsync(int id)
        {
            return await _context.PhieuLayMaus.FindAsync(id);
        }
        public async Task AddAsync(PhieuLayMau phieuLayMau)
        {
            _context.PhieuLayMaus.Add(phieuLayMau);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(PhieuLayMau phieuLayMau)
        {
            _context.PhieuLayMaus.Update(phieuLayMau);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var phieuLayMau = await _context.PhieuLayMaus.FindAsync(id);
            _context.PhieuLayMaus.Remove(phieuLayMau);
            await _context.SaveChangesAsync();
        }
    }
}