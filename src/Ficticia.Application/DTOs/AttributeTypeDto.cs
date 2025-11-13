namespace Ficticia.Application.DTOs
{
    public class AttributeTypeDto
    {
        public int Id { get; set; } 
        public int PersonId { get; set; }
        public string Type { get; set; } = string.Empty; 
        public string Value { get; set; } = string.Empty;
    }
}