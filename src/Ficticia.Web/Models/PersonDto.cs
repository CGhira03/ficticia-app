namespace Ficticia.Web.Models
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Identification { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public List<PersonAttribute> Attributes { get; set; } = new();
    }

    public class PersonAttribute
    {
        public string Type { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
