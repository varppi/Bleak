using Microsoft.AspNetCore.Identity;

namespace Bleak.Helpers
{
    public static class Errors
    {
        public static string Access { get; set; } = "Access denied";
        public static string Internal { get; set; } = "Internal server error";
        public static string Unknown { get; set; } = "Something went wrong";
        public static string FromIdentityResults(IdentityResult result)
            => $"ERROR:\n{string.Join("\n-", result.Errors.Select(x => x.Description))}";
    }
}
