using Microsoft.EntityFrameworkCore;
using System;

namespace CleverCore.Data.EF.Test
{
    public class ContextFactory
    {
        public static AppDbContext CreateInMemoryDbContext()
        {
            DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }
    }
}