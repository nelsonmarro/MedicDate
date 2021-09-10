using AutoMapper;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Test.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.DataAccess.Repository;
using MedicDate.DataAccess.Repository.IRepository;
using Xunit;

namespace MedicDate.API.Controllers
{
    public class CitaControllerTest : BaseDbTest
    {
        private readonly IMapper _mapper;
        private ICitaRepository _citaRepo;
        private CitaController _sut;
        private static string _dbName = Guid.NewGuid().ToString();
        private readonly ApplicationDbContext _context;

        private readonly List<Paciente> _pacientesCita = new()
        {
            new()
            {
                Id = "e32c6cdc-b2b3-4935-a3dd-6913c7e68fd7",
                Nombres = "Nelson",
                Apellidos = "Marro",
                Sexo = "Masculino",
                NumHistoria = "0001",
                Cedula = "1757078579",
                FechaNacimiento = new DateTime(1999, 1, 9),
                Email = "nelsonmarro99@gmail.com"
            },
            new()
            {
                Id = "377c3c02-ccdc-4eb6-8922-6f9c8d02361b",
                Nombres = "Maria",
                Apellidos = "Pacheco",
                Sexo = "Femenino",
                NumHistoria = "0002",
                Cedula = "0599687546",
                FechaNacimiento = new DateTime(1998, 3, 27),
                Email = "majopc98@gmail.com"
            }
        };

        private readonly List<Medico> _medicosCita = new()
        {
            new Medico
            {
                Id = "096f6292-0dbc-4dde-9777-512b98088559",
                Nombre = "Aileen",
                Apellidos = "Torres",
                Cedula = "1757078579",
                PhoneNumber = "0999079590"
            },
            new Medico
            {
                Id = "693eaaca-3fa0-48f6-8dd3-b69174bb9bbb",
                Nombre = "Gabi",
                Apellidos = "Noriega",
                Cedula = "0655856743",
                PhoneNumber = "0999079594"
            }
        };

        private readonly List<Actividad> _actividadesCita = new List<Actividad>
        {
            new() {Nombre = "Act 1"},
            new() {Nombre = "Act 2"}
        };

        public static TheoryData<string, string> PacienteAndMedicoIds = new()
        {
            {"e32c6cdc-b2b3-4935-a3dd-6913c7e68fd7", null},
            {null, "693eaaca-3fa0-48f6-8dd3-b69174bb9bbb"},
            {"e32c6cdc-b2b3-4935-a3dd-6913c7e68fd7", "693eaaca-3fa0-48f6-8dd3-b69174bb9bbb"},
        };

        public CitaControllerTest()
        {
            _mapper = BuildMapper();

            _context = BuildDbContext(_dbName);
            CreateCitaRelatedEntities(_context).GetAwaiter().GetResult();
        }

        public class GetAllCitasWithPagingAsync : CitaControllerTest
        {
            [Theory]
            [MemberData(nameof(PacienteAndMedicoIds))]
            public async Task Should_return_the_data_properly_filter_by_the_passed_filter_ids(string pacienteId,
                string medicoId)
            {
                //Arrange
                var context1 = BuildDbContext(_dbName);
                await CreateTestCitaList(context1);

                var context2 = BuildDbContext(_dbName);
                _citaRepo = new CitaRepository(context2, _mapper);
                _sut = new CitaController(_citaRepo, _mapper);

                //Act
                var result = await _sut.GetAllCitasWithPagingAsync(0, 10,
                    medicoId, pacienteId);

                var citaResponseList = result.Value.DataResult;

                //Assert
                if (pacienteId is not null && medicoId is null)
                {
                    Assert.Collection(citaResponseList,
                        cita =>
                        {
                            Assert.Equal("Pendiente 1", cita.Estado);
                            Assert.Equal(_pacientesCita[0].Id, cita.Paciente.Id);
                        },
                        cita =>
                        {
                            Assert.Equal("Pendiente 2", cita.Estado);
                            Assert.Equal(_pacientesCita[0].Id, cita.Paciente.Id);
                        });
                }

                if (pacienteId is null && medicoId is not null)
                {
                    Assert.Collection(citaResponseList,
                        cita =>
                        {
                            Assert.Equal("Pendiente 2", cita.Estado);
                            Assert.Equal(_medicosCita[1].Id, cita.Medico.Id);
                        },
                        cita =>
                        {
                            Assert.Equal("Pendiente 3", cita.Estado);
                            Assert.Equal(_medicosCita[1].Id, cita.Medico.Id);
                        });
                }

                if (pacienteId is not null && medicoId is not null)
                {
                    Assert.Collection(citaResponseList,
                        cita =>
                        {
                            Assert.Equal("Pendiente 2", cita.Estado);

                            Assert.Equal(_medicosCita[1].Id, cita.Medico.Id);
                            Assert.Equal(_pacientesCita[0].Id, cita.Paciente.Id);
                        });
                }
            }
        }

        private async Task CreateCitaRelatedEntities(ApplicationDbContext context)
        {
            if (!await context.Medico.AnyAsync())
            {
                await context.Medico.AddRangeAsync(_medicosCita);
                await context.Paciente.AddRangeAsync(_pacientesCita);
                await context.Actividad.AddRangeAsync(_actividadesCita);
                await context.SaveChangesAsync();
            }
        }

        private async Task CreateTestCitaList(ApplicationDbContext context)
        {
            var citas = new List<Cita>
            {
                new Cita
                {
                    Estado = "Pendiente 1",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(2),
                    MedicoId = _medicosCita[0].Id,
                    PacienteId = _pacientesCita[0].Id,
                },

                new Cita
                {
                    Estado = "Pendiente 2",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(2),
                    MedicoId = _medicosCita[1].Id,
                    PacienteId = _pacientesCita[0].Id,
                },

                new Cita
                {
                    Estado = "Pendiente 3",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddDays(2),
                    MedicoId = _medicosCita[1].Id,
                    PacienteId = _pacientesCita[1].Id,
                },
            };

            if (!await context.Cita.AnyAsync())
            {
                await context.Cita.AddRangeAsync(citas);
                await context.SaveChangesAsync();
            }
        }
    }
}