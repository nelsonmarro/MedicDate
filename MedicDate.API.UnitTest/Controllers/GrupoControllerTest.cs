using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.Domain.Entities;
using MedicDate.Domain.Interfaces.DataAccess;
using MedicDate.Shared.Models.Grupo;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class GrupoControllerTest
{
    public class GetGrupoAsync : GrupoControllerTest
    {
        [Fact]
        public async Task DebeRetornarElGrupoRequeridoPorElId()
        {
            // Preparacion
            var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<Grupo, GrupoResponseDto>().ReverseMap());
            var mapper = autoMapperConfig.CreateMapper();

            var tempGrupo = new Grupo
            {
                Id = "705ee4f2-8485-4ca3-86ba-a61b7b049a71",
                Nombre = "IESS"
            };

            var grupoRepoMock = new Mock<IGrupoRepository>();
            grupoRepoMock.Setup(x => x.FindAsync(It.IsAny<string>()))
                .ReturnsAsync(tempGrupo);
            grupoRepoMock.Setup(x => x.ResourceExists(It.IsAny<string>()))
                .ReturnsAsync(true);
            var grupoRepo = grupoRepoMock.Object;

            var grupoController = new GrupoController(grupoRepo, mapper);

            // Prueba
            var result = await grupoController.GetGrupoAsync(tempGrupo.Id);

            // Comprobación
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEquivalentTo(mapper.Map<GrupoResponseDto>(tempGrupo));
        }
    }
}
