using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Paciente;
using Moq;
using Xunit;

namespace MedicDate.API.Controllers;

public class PacienteControllerTest
{
    public class GetAllPacienteAsync : PacienteControllerTest
    {
        [Fact]
        public async Task DebeRetornarTodosLosPacientesGuardados()
        {
            // Preparacion
            var autoMapperConfig = new MapperConfiguration(cfg =>
            cfg.CreateMap<Paciente, PacienteCitaResponseDto>().ReverseMap());
            var mapper = autoMapperConfig.CreateMapper();

            var tempPacientes = new List<Paciente>
            {
                new Paciente
                {
                    Nombres = "Nelson",
                    Apellidos = "Marro",
                    Cedula = "1757078579",
                    Direccion = "Latacunga",
                    Email = "nelsonmarro99@gmail.com",
                    FechaNacimiento = new DateTime(day:9, month: 1, year: 1999),
                    NumHistoria = "N001",
                    Sexo = "Masculino",
                    Telefono = "0996266715"
                },
                new Paciente
                {
                    Nombres = "Maria",
                    Apellidos = "Pacheco",
                    Cedula = "0566787764",
                    Direccion = "Latacunga",
                    Email = "nelsonmarro66@gmail.com",
                    FechaNacimiento = new DateTime(day:9, month: 1, year: 1999),
                    NumHistoria = "N002",
                    Sexo = "Femenino",
                    Telefono = "0996266715"
                }
            };

            var pacienteRepoMock = new Mock<IPacienteRepository>();
            pacienteRepoMock.Setup(x => x.GetAllAsync(
                It.IsAny<System.Linq.Expressions.Expression<Func<Paciente, bool>>>(),
                It.IsAny<Func<IQueryable<Paciente>, IOrderedQueryable<Paciente>>>(),
                It.IsAny<string>(),
                It.IsAny<bool>()
                )).ReturnsAsync(tempPacientes);

            var pacienteController = new
                PacienteController(pacienteRepoMock.Object, mapper);

            // Prueba
            var result = await pacienteController.GetAllPacienteAsync();

            // Comprobación
            result.Value.Should().NotBeNull();
            result.Value.Should().NotBeEmpty();
            result.Value.Should().HaveCount(2);
            result.Value.Should().SatisfyRespectively(
                first =>
                {
                    first.Apellidos.Should().Be("Marro");
                },
                second =>
                {
                    second.Apellidos.Should().Be("Pacheco");
                });
        }
    }
}
