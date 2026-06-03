using Bleak.Pages.Account.Register;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages.Account.Login
{
    public class LoginForm
    {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }
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
        public async Task<IActionResult> OnPost(LoginForm form)
        {
            if (form.UserName is null || form.Password is null) return Page();
            var result = await _actions.Login(form.UserName, form.Password);
            if (result is not null && result.Succeeded)
            {
                Message = "logged in successfully!";
                return Redirect("/");
            }

            Message = "ERROR: Invalid username or password";
            return Page();
        }
    }
}
