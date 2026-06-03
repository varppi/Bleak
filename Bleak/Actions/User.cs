using Bleak.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bleak
{
    public partial class Actions
    {

        private AppUser? GetUserByName(string username)
        {
            return _userManager.FindByNameAsync(username)
                .GetAwaiter()
                .GetResult();  
        }

        private AppUser? GetUserByClaim(ClaimsPrincipal claim)
        {
            return claim.Identity is null 
                ? null 
                : GetUserByName(claim.Identity.Name??"");
        }

        public bool UserExists(string username)
            => GetUserByName(username) is not null;

        public async Task<IdentityResult> RegisterUser(string username, string password)
        {
            var user = new AppUser
            {
                UserName = username,
                Email = "none@none.none",
            };
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<SignInResult?> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null) return null;
            return await _signInManager.PasswordSignInAsync(user, password, true, false);
        }

        public bool VerifyPassword(ClaimsPrincipal claim, string? password)
        {
            var user = GetUserByClaim(claim);
            if (user is null) return false;
            return _userManager.CheckPasswordAsync(user, password??"")
                .GetAwaiter()
                .GetResult();
        }

        public async Task<IdentityResult?> ChangePassword(
            ClaimsPrincipal claim, 
            string? passwordCurrent, 
            string? passwordNew)
        {
            var user = GetUserByClaim(claim);
            if (user is null) return null;
            return await _userManager.ChangePasswordAsync(
                user, 
                passwordCurrent??"", 
                passwordNew??"");
        }

        public bool DeleteAccount(
            ClaimsPrincipal claim)
        {
            var user = GetUserByClaim(claim);
            if (user is null) return false;
            GetItems()
                .Where(item => item.Uploader == user.UserName)
                .Select(item => DeleteItem(item.Id ?? "", claim));
            var result = _userManager.DeleteAsync(user).GetAwaiter().GetResult();
            _signInManager.SignOutAsync().Wait();
            return result.Succeeded;
        }

        // 0 = no access
        // 1 = read access
        // 2 = read/write access
        public int HasAccessToItem(ClaimsPrincipal claim, string itemId)
        {
            var user = GetUserByClaim(claim);
            if (user is null) return 0;

            var item = GetItem(itemId);
            if (item is null) return 0;
            if (item.Uploader == user.UserName) return 2;
            if (claim.Identity?.Name == "admin") return 2;
            return 1;
        }
    }
}
