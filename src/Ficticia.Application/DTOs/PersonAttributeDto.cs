namespace Ficticia.Application.DTOs
{
    public class PersonAttributeDto
    {
        public int Id { get; set; }

        // 👇 Esta propiedad es la que el servicio está intentando usar
        public int AttributeTypeId { get; set; }

        public string AttributeTypeName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
