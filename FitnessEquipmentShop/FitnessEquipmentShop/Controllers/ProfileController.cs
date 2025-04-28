using FitnessEquipmentShop.Data.Models.Entities;
using FitnessEquipmentShop.Web.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessEquipmentShop.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Email = user.Email,
                CurrentPicturePath = user.ProfilePicturePath
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            // Update basic info
            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;

            // Upload profile picture
            if (model.ProfilePicture != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images/profiles");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }
                user.ProfilePicturePath = "/images/profiles/" + uniqueName;
            }

            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ProfileViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                TempData["PasswordChanged"] = "Password successfully changed!";
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction("Index");
        }
    }

}
