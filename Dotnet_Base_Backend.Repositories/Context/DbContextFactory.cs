using Microsoft.EntityFrameworkCore;

namespace Dotnet_Base_Backend.Repositories.Context
{
    public static class DbContextFactory
    {
        public static T? Create<T>(string dataBaseName) where T : DbContext
        {
            var options = new DbContextOptionsBuilder<T>()
                .UseInMemoryDatabase(dataBaseName)
                .Options;

            var context = Activator.CreateInstance(typeof(T), options) as T;

            context?.Database.EnsureCreated();

            return context;
        }
    }
}
