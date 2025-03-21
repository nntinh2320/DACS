﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DACS.Data;
using DACS.Models;
using DACS.Repositories;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;

namespace DACS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        public EmployeeController(IEmployeeRepository employeeRepository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _employeeRepository = employeeRepository;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var NhanVien = await _userManager.GetUsersInRoleAsync("Employee");//chọn role Employee
            var nhanvienId = NhanVien.Select(u => u.Id);
            var all_Nhanvien = from s in _context.User
                               where nhanvienId.Contains(s.Id)
                               select s;

            if (!String.IsNullOrEmpty(searchString))//Input có thông tin thì xuất ra
            {
                string lowercaseSearchString = searchString.ToLower();
                all_Nhanvien = all_Nhanvien.Where(s => s.FullName.ToLower().Contains(lowercaseSearchString)
                                                        || s.Email.ToLower().Contains(lowercaseSearchString)
                                                        || s.UserName.ToLower().Contains(lowercaseSearchString));

            }
            return View(await all_Nhanvien.ToListAsync());
        }

        [HttpPost]
        public async Task<JsonResult> GetSearchEMValue(string search)
        {
            var NhanVien = await _userManager.GetUsersInRoleAsync("Employee");
            var NVResult = NhanVien.Where(x => x.FullName.ToLower().Contains(search))
                                        .Select(x => new {
                                            label = x.FullName.ToLower(),
                                            value = x.FullName.ToLower()
                                        }).ToList();
            return Json(NVResult);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, ApplicationUser employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingEmployee = await _employeeRepository.GetByIdAsync(id); // Giả định có phương thức GetByIdAsync

                existingEmployee.FullName = employee.FullName;
                existingEmployee.UserName = employee.UserName;
                existingEmployee.Email = employee.Email;

                await _employeeRepository.UpdateAsync(existingEmployee);

                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _employeeRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Deletes an employee
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _employeeRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> LockAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Khóa tài khoản
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            if (result.Succeeded)
            {
                TempData["Message"] = "Khoá Tài Khoản " + user + " Thành Công";
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> UnlockAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            // Mở khóa tài khoản
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
            if (result.Succeeded)
            {
                TempData["Message"] = "Mở Khoá Tài Khoản " + user + " Thành Công";
                return RedirectToAction("Index");
            }
            return View("Error");
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (!ModelState.IsValid)
                return NotFound();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action("ResetPassword", "Employee", new { token, email = user.Email }, Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();
            bool emailResponse = emailHelper.SendEmailPasswordReset(user.Email, link);

            if (emailResponse)
                return Json(new { showAlert = true });
            else
            {
                // log email failed 
            }
            return View(email);
        }

        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return View(model);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                RedirectToAction("ResetPasswordConfirmation");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                return View();
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
    }
}
