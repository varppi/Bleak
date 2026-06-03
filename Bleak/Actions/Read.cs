using Bleak.Models;

namespace Bleak
{
    public partial class Actions
    {
        public Item? GetItem(string Id)
        {
            return _context.Items.Where(item => item.Id == Id).FirstOrDefault();
        }

        public IEnumerable<Item> GetItems()
        {
            return _context.Items; 
        }

        public Site? GetConfig()
        {
            return _context.SiteConf.FirstOrDefault();
        }
    }
}
