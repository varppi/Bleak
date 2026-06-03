using System.ComponentModel.DataAnnotations;

namespace Bleak.Models
{
    public class Item
    {
        [Required]
        [Key]
        public string? Id { get; set; }

        [Required] 
        public int? Type { get; set; } // 0 = text, 1 = pdf, 2 = video, 3 = audio, 4 = unknown
        [Required]
        public string? ContentType { get; set; }
        [Required]
        public string? Filename { get; set; }

        [Required]
        public string? Uploader { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Title { get; set; }
        [Required]
        public byte[]? Data { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
