using System.ComponentModel.DataAnnotations;

namespace Ficticia.Domain.Entities
{
    public class AttributeType
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(250)]
        public string Description { get; set; } = string.Empty;
    }
}