namespace Ficticia.Web.Models
{
    public class PersonFilterViewModel
    {
        // Filtros
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        // Rol actual (para permisos)
        public string Role { get; set; } = string.Empty;

        // Resultado
        public List<PersonDto> Persons { get; set; } = new();
    }
}

