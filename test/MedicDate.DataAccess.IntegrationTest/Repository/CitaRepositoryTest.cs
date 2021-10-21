using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.Archivo;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Xunit;

namespace MedicDate.Bussiness.Repository
{
    public class CitaRepositoryTest : BaseRepositoryTest<Cita>
    {
        private readonly ApplicationDbContext _context;
        private ICitaRepository? _sut;

        private readonly List<Paciente> _pacientesCita = new()
        {
            new Paciente
            {
                Nombres = "Nelson",
                Apellidos = "Marro",
                Sexo = "Masculino",
                NumHistoria = "0001",
                Cedula = "1757078579",
                FechaNacimiento = new DateTime(1999, 1, 9),
                Email = "nelsonmarro99@gmail.com"
            },
            new Paciente
            {
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
                Nombre = "Aileen",
                Apellidos = "Torres",
                Cedula = "1757078579",
                PhoneNumber = "0999079590"
            },
            new Medico
            {
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

        public CitaRepositoryTest()
        {
            _context = BuildDbContext(DbName);
            CreateCitaRelatedEntities().GetAwaiter().GetResult();

            EntityList = new List<Cita>()
            {
                new()
                {
                    Estado = Sd.ESTADO_CITA_CANCELADA,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now,
                    MedicoId = _medicosCita[0].Id,
                    PacienteId = _pacientesCita[0].Id
                },
                new()
                {
                    Estado = Sd.ESTADO_CITA_ANULADA,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddHours(2),
                    MedicoId = _medicosCita[0].Id,
                    PacienteId = _pacientesCita[0].Id
                },
                new()
                {
                    Estado = Sd.ESTADO_CITA_NOASISTIOPACIENTE,
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now,
                    MedicoId = _medicosCita[0].Id,
                    PacienteId = _pacientesCita[0].Id
                },
            };
            _context.Cita.AddRange(EntityList);
            _context.SaveChangesAsync().GetAwaiter().GetResult();

            ToAddEntity = new Cita
            {
                Estado = Sd.ESTADO_CITA_PORCONFIRMAR,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2),
                MedicoId = _medicosCita[0].Id,
                PacienteId = _pacientesCita[0].Id,
                Archivos = new List<Archivo>
                {
                    new()
                    {
                        RutaArchivo = "cualquier ruta",
                        Descripcion = "cualquier descripcion"
                    }
                },
                ActividadesCita = new List<ActividadCita>()
                {
                    new()
                    {
                        ActividadId = _actividadesCita[0].Id,
                        ActividadTerminada = false,
                        Detalles = "culaquier detalle"
                    }
                }
            };

            _context = BuildDbContext(DbName);
            BaseSut = new Repository<Cita>(_context);
        }

        [Fact]
        public async Task UpdateCitaAsync_should_update_a_cita()
        {
            var context1 = BuildDbContext(DbName);
            var newCita = new Cita
            {
                Estado = Sd.ESTADO_CITA_PORCONFIRMAR,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2),
                MedicoId = _medicosCita[0].Id,
                PacienteId = _pacientesCita[0].Id,
                Archivos = new List<Archivo>
                {
                    new()
                    {
                        RutaArchivo = "cualquier ruta",
                        Descripcion = "cualquier descripcion"
                    }
                },
                ActividadesCita = new List<ActividadCita>()
                {
                    new()
                    {
                        ActividadId = _actividadesCita[0].Id,
                        ActividadTerminada = false,
                        Detalles = "culaquier detalle"
                    }
                }
            };

            await _context.Cita.AddAsync(newCita);
            await _context.SaveChangesAsync();

            var citaUpdate = new CitaRequestDto
            {
                Estado = Sd.ESTADO_CITA_CONFIRMADA,
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(3),
                MedicoId = _medicosCita[1].Id,
                PacienteId = _pacientesCita[1].Id,
                ActividadesCita = new List<ActividadCitaRequestDto>()
                {
                    new()
                    {
                        ActividadId = _actividadesCita[1].Id,
                        ActividadTerminada = false,
                        Detalles = "detalles update"
                    }
                },
                Archivos = new List<ArchivoRequestDto>
                {
                    new()
                    {
                        RutaArchivo = "Nueva ruta",
                        Descripcion = "Nueva desc"
                    }
                }
            };

            _sut = new CitaRepository(context1, Mapper);

            var result = await _sut.UpdateCitaAsync(newCita.Id, citaUpdate);
            var successResult = result.SuccessResult as GenericActionResult;

            Assert.True(result.Succeeded);
            Assert.Equal(HttpStatusCode.OK, successResult?.HttpStatusCode);

            var context2 = BuildDbContext(DbName);
            var citaDb = await context2.Cita
                .AsNoTracking()
                .Include(x => x.Archivos)
                .Include(x => x.ActividadesCita)
                .FirstOrDefaultAsync(x => x.Id == newCita.Id);

            Assert.Equal(citaUpdate.Estado, citaDb?.Estado);

            Assert.Equal(citaUpdate.Archivos.Select(x => x.RutaArchivo).First(),
                citaDb?.Archivos.Select(x => x.RutaArchivo).First());

            Assert.Equal(
                citaUpdate.ActividadesCita.Select(x => x.Detalles).First(),
                citaDb?.ActividadesCita.Select(x => x.Detalles).First());
        }

        private async Task CreateCitaRelatedEntities()
        {
            await _context.Medico.AddRangeAsync(_medicosCita);
            await _context.Paciente.AddRangeAsync(_pacientesCita);
            await _context.Actividad.AddRangeAsync(_actividadesCita);
            await _context.SaveChangesAsync();
        }
    }
}