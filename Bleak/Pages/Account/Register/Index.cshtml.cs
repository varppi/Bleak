using Bleak.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages.Account.Register
{
    public class RegistrationForm {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? MasterPassword { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly Actions _actions;
        public IndexModel(Actions actions)
        {
            _actions = actions;
        }

        public void OnGet()
        {
        }

        [BindProperty]
        public string? Message { get; set; }
        public async Task<IActionResult> OnPost(RegistrationForm form)
        {
            if (form.UserName is null || form.Password is null) return Page();
            var master = _actions.GetConfig()?.MasterPassword;
            if (master is not null && form.MasterPassword != master)
            {
                Message = "invalid master password";
                return Page();
            }
            var result = await _actions.RegisterUser(form.UserName, form.Password);
            if (result is null)
            {
                Message = Errors.Unknown;
                return Page();
            }
            else if (!result.Succeeded)
            {
                Message = Errors.FromIdentityResults(result);
                return Page();
            }

            Message = "registered successfully!";
            return Redirect("/account/login");
        }
    }
}
