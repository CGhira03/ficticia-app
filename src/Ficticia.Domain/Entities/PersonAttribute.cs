using System.ComponentModel.DataAnnotations;

namespace Ficticia.Domain.Entities
{
    public class PersonAttribute
    {
        public int Id { get; set; }

        // Relación con Person
        public int PersonId { get; set; }
        public Person Person { get; set; } = null!;

        // Relación con AttributeType
        public int AttributeTypeId { get; set; }
        public AttributeType AttributeType { get; set; } = null!;

        public string Value { get; set; } = string.Empty;
    }
}
