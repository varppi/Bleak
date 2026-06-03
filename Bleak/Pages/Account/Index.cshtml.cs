using Bleak.Helpers;
using Bleak.Pages.Account.Register;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Bleak.Pages.Account
{
    public class AccountForm
    {
        public string? PasswordCurrent { get; set; }
        public string? Password { get; set; }
        public string? Password2 { get; set; }
        public bool? Delete { get; set; }
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
        [BindProperty(SupportsGet = true)]
        public bool Verified { get; set; }
        [BindProperty]
        public string? Message { get; set; }
        public async Task<IActionResult> OnPost(AccountForm form)
        {
            // VALIDATE CURRENT password
            var correctPassword = _actions.VerifyPassword(User, form.PasswordCurrent);
            if (!correctPassword)
            {
                Message = "invalid password";
                return Page();
            }

            // DELETE account?
            if (form.Delete??false)
            {
                _actions.DeleteAccount(User);
                return Redirect("/account/logout");
            }

            // CHANGE password
            if (form.Password is not null && form.Password.Length > 3) {

                if (form.Password != form.Password2)
                {
                    Message = "new passwords do not match";
                    return Page();
                }

                var result = await _actions.ChangePassword(User, form.PasswordCurrent, form.Password);
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
                Message = "password changed";
            }



            return Page();
        }
    }
}
