using Ficticia.Application.DTOs;
using Ficticia.Application.Exceptions;
using Ficticia.Application.Interfaces;
using Ficticia.Domain.Entities;
using Ficticia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ficticia.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly FicticiaDbContext _context;

        public PersonService(FicticiaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PersonDto>> GetFilteredAsync(string? name, bool? isActive, int? minAge, int? maxAge)
        {
            var query = _context.Persons
                .Include(p => p.Attributes)
                .ThenInclude(a => a.AttributeType)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(p => p.FullName.Contains(name));

            if (isActive.HasValue)
                query = query.Where(p => p.IsActive == isActive.Value);

            if (minAge.HasValue)
                query = query.Where(p => p.Age >= minAge.Value);

            if (maxAge.HasValue)
                query = query.Where(p => p.Age <= maxAge.Value);

            var persons = await query.ToListAsync();
            return persons.Select(MapToDto);
        }

        public async Task<PersonDto> GetByIdAsync(int id)
        {
            var person = await _context.Persons
                .Include(p => p.Attributes)
                .ThenInclude(a => a.AttributeType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                throw new BusinessException($"No se encontró la persona con ID {id}");

            return MapToDto(person);
        }

        public async Task<PersonDto> CreateAsync(PersonDto dto)
        {
            // Validación de unicidad
            if (await _context.Persons.AnyAsync(p => p.Identification == dto.Identification))
                throw new BusinessException("Ya existe una persona con esa identificación.");

            if (dto.Age < 0 || dto.Age > 120)
                throw new BusinessException("La edad debe estar entre 0 y 120.");

            var person = new Person
            {
                FullName = dto.FullName,
                Identification = dto.Identification,
                Age = dto.Age,
                Gender = dto.Gender,
                IsActive = dto.IsActive,
                Attributes = dto.Attributes.Select(a => new PersonAttribute
                {
                    AttributeTypeId = a.AttributeTypeId,
                    Value = a.Value
                }).ToList()
            };

            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return MapToDto(person);
        }

        public async Task<PersonDto> UpdateAsync(int id, PersonDto dto)
        {
            var person = await _context.Persons
                .Include(p => p.Attributes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
                throw new BusinessException($"No se encontró la persona con ID {id}");

            if (await _context.Persons.AnyAsync(p => p.Identification == dto.Identification && p.Id != id))
                throw new BusinessException("Ya existe otra persona con esa identificación.");

            if (dto.Age < 0 || dto.Age > 120)
                throw new BusinessException("La edad debe estar entre 0 y 120.");

            person.FullName = dto.FullName;
            person.Identification = dto.Identification;
            person.Age = dto.Age;
            person.Gender = dto.Gender;
            person.IsActive = dto.IsActive;

            // Actualizar atributos
            person.Attributes.Clear();
            foreach (var attr in dto.Attributes)
            {
                person.Attributes.Add(new PersonAttribute
                {
                    AttributeTypeId = attr.AttributeTypeId,
                    Value = attr.Value
                });
            }

            await _context.SaveChangesAsync();
            return MapToDto(person);
        }

        public async Task DeleteAsync(int id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
                throw new BusinessException("Persona no encontrada.");

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
        }

        // --- Mapeo Entity -> DTO ---
        private static PersonDto MapToDto(Person p) =>
            new PersonDto
            {
                Id = p.Id,
                FullName = p.FullName,
                Identification = p.Identification,
                Age = p.Age,
                Gender = p.Gender,
                IsActive = p.IsActive,
                Attributes = p.Attributes.Select(a => new PersonAttributeDto
                {
                    AttributeTypeId = a.AttributeTypeId,
                    Value = a.Value
                }).ToList()
            };
    }
}
