using Ficticia.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ficticia.Infrastructure.Persistence
{
    public class FicticiaDbContext : DbContext
    {
        public FicticiaDbContext(DbContextOptions<FicticiaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons => Set<Person>();
        public DbSet<AttributeType> AttributeTypes => Set<AttributeType>();
        public DbSet<PersonAttribute> PersonAttributes => Set<PersonAttribute>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Índice único para identificación
            modelBuilder.Entity<Person>()
                .HasIndex(p => p.Identification)
                .IsUnique();

            // Relaciones
            modelBuilder.Entity<PersonAttribute>()
                .HasOne(pa => pa.Person)
                .WithMany(p => p.Attributes)
                .HasForeignKey(pa => pa.PersonId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PersonAttribute>()
                .HasOne(pa => pa.AttributeType)
                .WithMany()
                .HasForeignKey(pa => pa.AttributeTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
