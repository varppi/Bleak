using Bleak.Models;
using System.Security.Claims;

namespace Bleak
{
    public partial class Actions
    {
        public bool DeleteItem(
            string itemId,
            ClaimsPrincipal claim)
        {
            var item = GetItem(itemId);
            if (item is null) return false;
            if (HasAccessToItem(claim, itemId) < 2) return false;
            _context.Items.Remove(item);
            _context.SaveChanges();
            return true;
        }

    }
}
