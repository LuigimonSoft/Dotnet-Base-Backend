using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dotnet_Base_Backend.Repositories.Test
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
