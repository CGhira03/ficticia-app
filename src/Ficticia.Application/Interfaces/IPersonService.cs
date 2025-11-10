using Ficticia.Application.DTOs;

namespace Ficticia.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetFilteredAsync(string? name, bool? isActive, int? minAge, int? maxAge);
        Task<PersonDto> GetByIdAsync(int id);
        Task<PersonDto> CreateAsync(PersonDto dto);
        Task<PersonDto> UpdateAsync(int id, PersonDto dto);
        Task DeleteAsync(int id);
    }
}