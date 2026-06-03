using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bleak.Pages.Items
{
    public class IndexModel : PageModel
    {
        private readonly Actions _actions;

        public IndexModel(Actions actions)
        {
            _actions = actions;
        }

        public IActionResult OnGet()
        {
            var hasToLogin = _actions.GetConfig()?.AnonymousViewing ?? false;
            if (hasToLogin && !(User.Identity?.IsAuthenticated??false)) return Redirect("/account/login");
            return Page();
        }

        [BindProperty(SupportsGet = true)]
        public int? Type { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? PageNo { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? SearchQuery { get; set; }
    }
}
