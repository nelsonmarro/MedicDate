using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Shared.Models.Actividad;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class ActividadControllerTest
{
    public class GetActividadAsync : ActividadControllerTest
    {
        [Fact]
        public async Task DebeRetornarLaActividadRequeridaPorElId()
        {
            // Preparacion
            var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<Actividad, ActividadResponseDto>().ReverseMap());
            var mapper = autoMapperConfig.CreateMapper();

            var tempActividad = new Actividad
            {
                Id = "705ee4f2-8485-4ca3-86ba-a61b7b049a71",
                Nombre = "Rinoplastia"
            };

            var actividadRepoMock = new Mock<IActividadRepository>();
            actividadRepoMock.Setup(x => x.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(tempActividad);
            actividadRepoMock.Setup(x => x.ResourceExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var actividadRepo = actividadRepoMock.Object;

            var actividadController = new ActividadController(actividadRepo, mapper);

            // Prueba
            var result = await actividadController.GetActividadAsync(tempActividad.Id);

            // Comprobación
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(mapper.Map<ActividadResponseDto>(tempActividad));
        }
    }
}
