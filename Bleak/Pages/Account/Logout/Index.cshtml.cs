using Bleak.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bleak.Pages.Account.Logout
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        public IndexModel(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        public IActionResult OnGet()
        {
            _signInManager.SignOutAsync().Wait();
            return Redirect("/");
        }
    }
}
