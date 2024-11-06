using System.ComponentModel.DataAnnotations;

namespace SimpleCRUD_JWTAuthn.Model.Dto_s
{
    public class ItemDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [MaxLength(80)]
        public string? Description { get; set; }

        [Required]
        public bool InStock { get; set; }

    }
}
