using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Shared.Models.Medico;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class MedicoControllerTest
{
    public class GetMedicoByIdAsync : MedicoControllerTest
    {
        [Fact]
        public async Task DebeRetornarElMedicoRequeridoPorElId()
        {
            // Preparacion
            var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<Medico, MedicoResponseDto>().ReverseMap());
            var mapper = autoMapperConfig.CreateMapper();

            var tempMedico = new Medico
            {
                Id = "705ee4f2-8485-4ca3-86ba-a61b7b049a71",
                Nombre = "Nelson",
                Apellidos = "Marro",
                Cedula = "1757078579"
            };

            var medicoRepoMock = new Mock<IMedicoRepository>();
            medicoRepoMock.Setup(x => x.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(tempMedico);
            medicoRepoMock.Setup(x => x.ResourceExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var medicoRepo = medicoRepoMock.Object;

            var medicoController = new MedicoController(medicoRepo, mapper);

            // Prueba
            var result = await medicoController.GetMedicoByIdAsync(tempMedico.Id);

            // Comprobación
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(mapper.Map<MedicoResponseDto>(tempMedico));
        }
    }
}
