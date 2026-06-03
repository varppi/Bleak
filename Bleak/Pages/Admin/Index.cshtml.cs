using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bleak.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public class SiteConfigurationForm
        {
            public string? MasterPassword { get; set; }
            public string? AnonymousViewing { get; set; }
        }

        private readonly Actions _actions;
        public IndexModel(Actions actions)
        {
            _actions = actions;
        }

        [BindProperty]
        public string? Message { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost(SiteConfigurationForm form) {
            _actions.SetSiteConfig(new Models.Site
            {
                MasterPassword = form.MasterPassword,
                AnonymousViewing = form.AnonymousViewing == "on",
            }, User);
            return Page();
        }
    }
}
