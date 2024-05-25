using DACS.Data;
using DACS.Models;
using Microsoft.AspNetCore.Identity;

namespace DACS.Repositories
{
    public class EFEmployeeRepository : IEmployeeRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public EFEmployeeRepository(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var employees = await _userManager.GetUsersInRoleAsync("Employee");
            return employees;
        }

        public async Task<ApplicationUser> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        public async Task UpdateAsync(ApplicationUser employee)
        {
            await _userManager.UpdateAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
