using Bleak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bleak.Pages.Items
{
    public class DownloadModel : PageModel
    {
        private readonly Actions _actions;
        public DownloadModel(Actions actions)
        {
            _actions = actions;
        }

        public Item? Item;
        public IActionResult OnGet(string id)
        {
            var hasToLogin = _actions.GetConfig()?.AnonymousViewing ?? false;
            if (hasToLogin && !(User.Identity?.IsAuthenticated ?? false)) return Redirect("/account/login");

            var item = _actions.GetItem(id ?? "");
            if (item is null) return Page();
            return File(item.Data ?? [], item.ContentType??"application/octet-stream", item.Filename);
        }
    }
}
