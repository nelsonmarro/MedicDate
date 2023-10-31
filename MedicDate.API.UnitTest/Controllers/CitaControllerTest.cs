using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Domain.DomainServices.IDomainServices;
using MedicDate.Shared.Models.Cita;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class CitaControllerTest
{
  public class GetCitaDetailsAsync : CitaControllerTest
  {
    [Fact]
    public async Task DebeRetornarLaCitaRequeridaPorElId()
    {
      // Preparacion
      var autoMapperConfig = new MapperConfiguration(
        cfg => cfg.CreateMap<Cita, CitaDetailsDto>().ReverseMap()
      );
      var mapper = autoMapperConfig.CreateMapper();

      var tempCita = new Cita
      {
        Id = "705ee4f2-8485-4ca3-86ba-a61b7b049a71",
        Estado = "Por Confirmar",
        FechaFin = DateTime.Now.AddHours(1),
        FechaInicio = DateTime.Now
      };

      var citaRepoMock = new Mock<ICitaRepository>();
      var citaServiceMock = new Mock<ICitaService>();
      citaRepoMock
        .Setup(
          x =>
            x.FirstOrDefaultAsync(
              It.IsAny<Expression<Func<Cita, bool>>>(),
              It.IsAny<string>(),
              It.IsAny<bool>()
            )
        )
        .ReturnsAsync(tempCita);
      citaRepoMock.Setup(x => x.ResourceExists(It.IsAny<string>())).ReturnsAsync(true);
      var citaRepo = citaRepoMock.Object;
      var citaService = citaServiceMock.Object;

      var citaController = new CitaController(citaRepo, mapper, citaService);

      // Prueba
      var result = await citaController.GetCitaDetailsAsync(tempCita.Id);

      // Comprobación
      result.Value.Should().NotBeNull();
      result.Value.Should().BeEquivalentTo(mapper.Map<CitaDetailsDto>(tempCita));
    }
  }
}
