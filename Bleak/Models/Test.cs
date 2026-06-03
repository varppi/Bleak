using System.ComponentModel.DataAnnotations;

namespace Bleak.Models
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    public class Test
    {
        [Required]
        [Key]
        public string? Id { get; set; }
        [Required]
        public string Message { get; set; }
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
