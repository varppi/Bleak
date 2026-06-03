using Bleak.Helpers;
using Bleak.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages.Items
{
    public class EditForm
    {
        [Required]
        public string? Title { get; set; }
        [Required]
        public bool? Delete { get; set; }
        [Required]
        public string? Id { get; set; }
    }


    public class EditModel : PageModel
    {
        private readonly Actions _actions;
        public EditModel(Actions actions)
        {
            _actions = actions;
        }

        [BindProperty(SupportsGet = true)]
        public bool Verified { get; set; }

        [BindProperty]
        public string? Message { get; set; }
        public IActionResult OnPost(EditForm form, string id)
        {
            var hasToLogin = _actions.GetConfig()?.AnonymousViewing ?? false;
            if (hasToLogin && !(User.Identity?.IsAuthenticated ?? false)) return Redirect("/account/login");

            Item = _actions.GetItem(id ?? "");
            if (Item is null || Item.Id is null) return Page();
            
            if (form.Delete??false)
            {
                _actions.DeleteItem(Item.Id, User);
                return Redirect("/items");
            }

            // are ANY fields null?
            if (form.Title is null) 
            {
                Message = "some fields are missing";
                return Page();
            }

            var result = _actions.UpdateItem(Item.Id, new Item {
                Title = form.Title,
            }, User);
            Message = "item updated successfully";
            if (!result) Message = "something went wrong. check that everything is valid.";
            return Page();
        }

        public Item? Item;
        public IActionResult OnGet(string id)
        {
            var hasToLogin = _actions.GetConfig()?.AnonymousViewing ?? false;
            if (hasToLogin && !(User.Identity?.IsAuthenticated ?? false)) return Redirect("/account/login");

            Item = _actions.GetItem(id ?? "");
            if (Item is null || Item.Id is null) return Page();

            return Page();
        }
    }
}
