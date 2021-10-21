using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace MedicDate.Bussiness.Repository
{
    public class ActividadRepositoryTest : BaseRepositoryTest<Actividad>
    {
        private IActividadRepository? _sut;

        public ActividadRepositoryTest()
        {
            EntityList = new List<Actividad>()
            {
                new() {Nombre = "Act 1"},
                new() {Nombre = "Act 2"},
                new() {Nombre = "Act 3"},
            };

            ToAddEntity = new Actividad { Nombre = "Act 4" };

            var context = BuildDbContext(DbName);
            if (context is not null)
            {
                context.Actividad.AddRange(EntityList);
                context.SaveChangesAsync().GetAwaiter().GetResult();

                context = BuildDbContext(DbName);
                BaseSut = new Repository<Actividad>(context);
            }
        }

        [Fact]
        public async Task UpdateActividadAsync_should_update_an_activity()
        {
            var actividadId = EntityList.Select(x => x.Id).Last();
            var context = BuildDbContext(DbName);

            _sut = new ActividadRepository(context, Mapper);

            var result = await _sut.UpdateActividadAsync(actividadId,
                new ActividadRequestDto { Nombre = "New Act 3" });

            var successResult = result.SuccessResult as GenericActionResult;

            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, successResult?.HttpStatusCode);

            var context2 = BuildDbContext(DbName);
            var existEditedEntity =
                await context.Actividad.AnyAsync(x => x.Nombre == "New Act 3");

            Assert.True(existEditedEntity);
        }
    }
}