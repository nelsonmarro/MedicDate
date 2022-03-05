using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Especialidad;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class EspecialidadControllerTest
{
    public class GetEspecialidadAsync : EspecialidadControllerTest
    {
        [Fact]
        public async Task DebeRetornarLaEspecialidadRequeridaPorElId()
        {
            // Preparacion
            var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<Especialidad, EspecialidadResponseDto>().ReverseMap());
            var mapper = autoMapperConfig.CreateMapper();

            var tempEspecialidad = new Especialidad
            {
                Id = "705ee4f2-8485-4ca3-86ba-a61b7b049a71",
                NombreEspecialidad = "Odontología"
            };

            var especialidadRepoMock = new Mock<IEspecialidadRepository>();
            especialidadRepoMock.Setup(x => x.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(tempEspecialidad);
            especialidadRepoMock.Setup(x => x.ResourceExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var especialidadRepo = especialidadRepoMock.Object;

            var especialidadController = new EspecialidadController(especialidadRepo, mapper);

            // Prueba
            var result = await especialidadController.GetEspecialidadAsync(tempEspecialidad.Id);

            // Comprobación
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(mapper.Map<EspecialidadResponseDto>(tempEspecialidad));
        }
    }
}
