using MedicDate.API.DTOs.Actividad;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MedicDate.Bussines.Repository
{
    public class ActividadRepositoryTest : BaseRepositoryTest<Actividad>
    {
        private readonly ApplicationDbContext _context;
        private IActividadRepository _sut;

        public ActividadRepositoryTest()
        {
            entityList = new List<Actividad>()
            {
                new Actividad() {Nombre = "Act 1"},
                new Actividad() {Nombre = "Act 2"},
                new Actividad() {Nombre = "Act 3"},
            };

            toAddEntity = new Actividad() { Nombre = "Act 4" };

            _context = BuildDbContext(dbName);
            _context.Actividad.AddRange(entityList);
            _context.SaveChangesAsync().GetAwaiter().GetResult();

            _context = BuildDbContext(dbName);
            sut = new Repository<Actividad>(_context);
        }

        [Fact]
        public async Task UpdateActividadAsync_should_update_an_activity()
        {
            var actividadId = entityList.Select(x => x.Id).Last();
            var context = BuildDbContext(dbName);

            _sut = new ActividadRepository(context, mapper, apiOpResultFactory);

            var result = await _sut.UpdateActividadAsync(actividadId, new ActividadRequestDto { Nombre = "New Act 3" });

            Assert.True(result.IsSuccess);
            Assert.Equal(HttpStatusCode.OK, result.SuccessActionResult.HttpStatusCode);

            var context2 = BuildDbContext(dbName);
            var existEditedEntity = await context.Actividad.AnyAsync(x => x.Nombre == "New Act 3");

            Assert.True(existEditedEntity);
        }
    }

}
