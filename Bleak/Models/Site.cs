using System.ComponentModel.DataAnnotations;

namespace Bleak.Models
{
    public class Site
    {
        [Key]
        public string? Id { get; set; }
        public string? MasterPassword { get; set; }
        public bool? AnonymousViewing { get; set; }
    }
}
