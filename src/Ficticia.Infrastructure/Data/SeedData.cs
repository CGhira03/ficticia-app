using Ficticia.Domain.Entities;
using Ficticia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ficticia.Infrastructure.Data
{
    public static class SeedData
    {
        public static void Initialize(FicticiaDbContext context)
        {
            // Revisar si ya hay atributos para no duplicar
            if (context.AttributeTypes.Any()) return;

            var attributes = new List<AttributeType>
        {
            new AttributeType { Type = "¿Maneja?" },
            new AttributeType { Type = "¿Usa lentes?" },
            new AttributeType { Type = "¿Es diabético?" },
            new AttributeType { Type = "¿Tiene hipertensión?" },
            new AttributeType { Type = "¿Fuma?" },
            new AttributeType { Type = "¿Consume alcohol regularmente?" },
            new AttributeType { Type = "¿Ha tenido cirugías importantes?" },
            new AttributeType { Type = "¿Ha sido hospitalizado en el último año?" },
            new AttributeType { Type = "¿Tiene enfermedades crónicas?" },
            new AttributeType { Type = "¿Realiza actividad física regularmente?" }
        };

            context.AttributeTypes.AddRange(attributes);
            context.SaveChanges();
        }
    }
}
