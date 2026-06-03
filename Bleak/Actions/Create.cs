using Bleak.Models;

namespace Bleak
{
    public partial class Actions
    {
        public string CreateItem(Item item)
        {
            var id = Guid.NewGuid().ToString();
            item.Id = id;
            item.Created = DateTime.Now;
            item.Modified = DateTime.Now;
            _context.Items.Add(item);
            _context.SaveChanges();
            return id;
        }

    }
}
