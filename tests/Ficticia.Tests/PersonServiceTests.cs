using Ficticia.Application.DTOs;
using Ficticia.Application.Exceptions;
using Ficticia.Application.Services;
using Ficticia.Infrastructure.Persistence;
using Ficticia.Domain.Entities;
using Xunit;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // ✅ agregado

namespace Ficticia.Tests
{
    public class PersonServiceTests
    {
        private readonly FicticiaDbContext _context;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _context = FakeDbContextFactory.CreateDbContext("FicticiaTestDb");
            _context.Database.EnsureDeleted(); // 👈 limpia la base
            _context.Database.EnsureCreated();
            _service = new PersonService(_context);
        }

        [Fact(DisplayName = "Crear persona correctamente")]
        public async Task CreatePerson_ShouldWork()
        {
            var dto = new PersonDto
            {
                FullName = "Juan Pérez",
                Identification = "123456",
                Age = 30,
                Gender = "M",
                IsActive = true
            };

            var result = await _service.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Juan Pérez", result.FullName);
            Assert.Equal(1, await _context.Persons.CountAsync()); // ✅ funciona ahora
        }

        [Fact(DisplayName = "No permitir duplicar identificación")]
        public async Task CreatePerson_ShouldFail_WhenDuplicateIdentification()
        {
            var dto = new PersonDto
            {
                FullName = "Ana",
                Identification = "ABC123",
                Age = 25,
                Gender = "F"
            };

            await _service.CreateAsync(dto);

            var dto2 = new PersonDto
            {
                FullName = "Carla",
                Identification = "ABC123",
                Age = 40,
                Gender = "F"
            };

            await Assert.ThrowsAsync<BusinessException>(() => _service.CreateAsync(dto2));
        }

        [Fact(DisplayName = "Actualizar persona correctamente")]
        public async Task UpdatePerson_ShouldWork()
        {
            var dto = new PersonDto
            {
                FullName = "Pedro",
                Identification = "987",
                Age = 35,
                Gender = "M"
            };

            var created = await _service.CreateAsync(dto);

            created.FullName = "Pedro Gómez";
            var updated = await _service.UpdateAsync(created.Id, created); // ✅ sin .Value

            Assert.Equal("Pedro Gómez", updated.FullName);
        }

        [Fact(DisplayName = "Eliminar persona correctamente")]
        public async Task DeletePerson_ShouldRemoveFromDb()
        {
            var dto = new PersonDto
            {
                FullName = "Sofía",
                Identification = "555",
                Age = 29,
                Gender = "F"
            };

            var created = await _service.CreateAsync(dto);
            await _service.DeleteAsync(created.Id); // ✅ sin .Value

            Assert.Empty(_context.Persons);
        }

        [Fact(DisplayName = "Filtrar por nombre debe devolver coincidencias")]
        public async Task FilterByName_ShouldReturnResults()
        {
            _context.Persons.Add(new Person { FullName = "Laura", Identification = "L1", Age = 28 });
            _context.Persons.Add(new Person { FullName = "Lucía", Identification = "L2", Age = 30 });
            await _context.SaveChangesAsync();

            var result = await _service.GetFilteredAsync("Lau", null, null, null);

            Assert.Single(result);
        }
    }
}
