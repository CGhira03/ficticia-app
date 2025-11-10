using System.ComponentModel.DataAnnotations;

namespace Ficticia.Domain.Entities
{
    public class PersonAttribute
    {
        public int Id { get; set; }

        [Required]
        public int PersonId { get; set; }

        [Required]
        public int AttributeTypeId { get; set; }

        [Required, MaxLength(200)]
        public string Value { get; set; } = string.Empty;

        // Navegacion
        public Person? Person { get; set; }
        public AttributeType? AttributeType { get; set; }
    }
}