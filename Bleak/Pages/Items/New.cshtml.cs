using Bleak.Helpers;
using Bleak.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages.Items
{

    public class NewItemForm
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public IFormFile? Upload { get; set; }
    }

    public class NewModel : PageModel
    {
        private readonly Actions _actions;
        public NewModel(Actions actions)
        {
            _actions = actions;
        }

        public IActionResult OnGet()
        {
            if (User.Identity is null ? true : !User.Identity.IsAuthenticated) return Redirect("/account/login");
            return Page();
        }

        public IActionResult OnPost(NewItemForm form) {
            if (User.Identity is null ? true : !User.Identity.IsAuthenticated) return Redirect("/account/login");
            if (form.Upload is null) return Page();
            var readStream = form.Upload.OpenReadStream();
            var buffer = new byte[readStream.Length];
            var n = readStream.Read(buffer);
            buffer = buffer.Take(n).ToArray();
            var item = new Item
            {
                Uploader = User.Identity.Name,
                Data = buffer,
                Title = form.Name ?? "unnamed",
                Filename = form.Upload.FileName,
                ContentType = form.Upload.ContentType,
                Type = Files.GetFileType(form.Upload)
            };
            var id = _actions.CreateItem(item); 
            return Redirect("/items");
        }
    }
}
