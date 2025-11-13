using System.Collections.Generic;   
using System.ComponentModel.DataAnnotations;

namespace Ficticia.Domain.Entities
{
    public class Person
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(8)]
        public string Identification { get; set; } = string.Empty; 

        [Range(0, 90, ErrorMessage = "La edad debe estar entre 0 y 90.")]
        public int Age { get; set; }

        [Required, MaxLength(20)]
        public string Gender { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public ICollection<PersonAttribute> Attributes { get; set; } = new List<PersonAttribute>();
    }
}