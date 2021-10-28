﻿using System.Threading.Tasks;
using Atomic.UnifiedAuth.Localization;
using Atomic.UnifiedAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;

namespace Atomic.UnifiedAuth.Pages.Account
{
    public class ResetPassword : PageModel
    {
        private readonly IStringLocalizer<AccountResource> _localizer;
        private readonly UserManager<AppUser> _userManager;

        public ResetPassword(
            UserManager<AppUser> userManager,
            IStringLocalizer<AccountResource> localizer
        )
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        [BindProperty]
        public ResetPasswordModel Input { get; set; }

        public bool PasswordReset { get; set; }

        public IActionResult OnGet(string code, string userId)
        {
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(userId))
            {
                return BadRequest(_localizer["Wrong password reset link."]);
            }

            Input = new ResetPasswordModel
            {
                UserId = userId,
                Code = code,
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var user = await _userManager.FindByIdAsync(Input.UserId);
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);

            if (result.Succeeded)
            {
                PasswordReset = true;
                return Page();
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}