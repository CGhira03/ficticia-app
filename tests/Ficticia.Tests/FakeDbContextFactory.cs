using Ficticia.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ficticia.Tests
{
	public static class FakeDbContextFactory
	{
		public static FicticiaDbContext CreateDbContext(string dbName)
		{
			var options = new DbContextOptionsBuilder<FicticiaDbContext>()
				.UseInMemoryDatabase(databaseName: dbName)
				.Options;

			return new FicticiaDbContext(options);
		}
	}
}
