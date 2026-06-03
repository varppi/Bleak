using Bleak.Models;
using System.Security.Claims;

namespace Bleak
{
    public partial class Actions
    {
        public bool UpdateItem(
            string itemId, 
            Item updatedItem, 
            ClaimsPrincipal claim)
        {
            var item = GetItem(itemId);
            if (item is null) return false;
            if (HasAccessToItem(claim, itemId) < 2) return false;
            item.Title = updatedItem.Title;
            _context.SaveChanges();
            return true;    
        }

        public bool SetSiteConfig(Site siteConf, ClaimsPrincipal claim)
        {
            var user = GetUserByClaim(claim);
            if (user is null) return false;
            if (user.UserName != "admin") return false;

            _context.SiteConf.RemoveRange(_context.SiteConf);

            siteConf.Id = Guid.NewGuid().ToString();

            _context.SiteConf.Add(siteConf);
            _context.SaveChanges();
            return true;
        }
    }
}
