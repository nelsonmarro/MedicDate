using AutoMapper;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Mapper;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Test.Shared
{
    public class BaseDbTest
    {
        protected IMapper BuildMapper()
        {
            var config = new MapperConfiguration
            (
                options => options.AddProfile(new MapperProfile())
            );

            return config.CreateMapper();
        }

        protected ApplicationDbContext BuildDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(dbName).Options;

            return new ApplicationDbContext(options);
        }
    }
}
