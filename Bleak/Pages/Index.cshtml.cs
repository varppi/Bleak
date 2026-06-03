using Bleak.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages
{
    public class RegistrationForm
    {
        [Required]
        public string? Password { get; set; }
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
            if (form.Password is null) return Page();
            var result = await _actions.RegisterUser("admin", form.Password);
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
            return Redirect("/");
        }
    }
}
