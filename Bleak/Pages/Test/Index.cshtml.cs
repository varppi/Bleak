using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Bleak.Pages.Test
{
    public class TestSubmit
    {
        public string Message { get; set; }
    }

    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost(TestSubmit submit)
        {
            Console.WriteLine(submit.Message);
            return new JsonResult(new { Message = submit.Message});
        }
    }
}
