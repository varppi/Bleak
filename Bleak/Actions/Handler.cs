using Bleak.Database;
using Bleak.Models;
using Microsoft.AspNetCore.Identity;

namespace Bleak
{
    public partial class Actions
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        public Actions(
            ApplicationDbContext context, 
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) { 
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
    }
}
