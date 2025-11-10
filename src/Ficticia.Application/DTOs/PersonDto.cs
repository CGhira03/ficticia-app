namespace Ficticia.Application.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<PersonAttributeDto> Attributes { get; set; } = new();
    }
}
