using Ficticia.Infrastructure.Persistence;
using Ficticia.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ficticia.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOnly")]
    public class AttributeTypesController : ControllerBase
    {
        private readonly FicticiaDbContext _context;

        public AttributeTypesController(FicticiaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.AttributeTypes.ToListAsync();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AttributeType type)
        {
            _context.AttributeTypes.Add(type);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), new { id = type.Id }, type);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AttributeType type)
        {
            var existing = await _context.AttributeTypes.FindAsync(id);
            if (existing == null) return NotFound();

            existing.Type = type.Type;
            existing.Value = type.Value;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.AttributeTypes.FindAsync(id);
            if (type == null) return NotFound();

            _context.AttributeTypes.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
