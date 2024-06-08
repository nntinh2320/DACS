/*using NCKH.Data;*/
using DACS.Data;
using DACS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

{
    public class EFPhieuLayMauRepository : IPhieuLayMauRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPhieuLayMauRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        {
            return await _context.PhieuLayMaus.ToListAsync();
        }
        {
            return await _context.PhieuLayMaus.FindAsync(id);
        }
        {
        }
        {
            _context.PhieuLayMaus.Update(phieuLayMau);
            await _context.SaveChangesAsync();
        }
        {
            var phieuLayMau = await _context.PhieuLayMaus.FindAsync(id);
            _context.PhieuLayMaus.Remove(phieuLayMau);
            await _context.SaveChangesAsync();
        }
    }
}
