using MedicDate.Bussines.Repository;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Bussiness.Repository
{
    public class CitaRepositoryTest : BaseRepositoryTest<Cita>
    {
        private readonly ApplicationDbContext _context;
        private ICitaRepository _sut;
        private readonly Paciente _pacienteCita = new()
        {
            Nombres = "Nelson",
            Apellidos = "Marro",
            Sexo = "Masculino",
            NumHistoria = "0001",
            Cedula = "1757078579",
            FechaNacimiento = new DateTime(1999, 1, 9),
            Email = "nelsonmarro99@gmail.com"
        };

        private readonly Medico _medicoCita = new()
        {
            Nombre = "Aileen",
            Apellidos = "Torres",
            Cedula = "1757078579",
            PhoneNumber = "0999079590"
        };

        private readonly Actividad _activdadCita = new()
        {
            Nombre = "Act 1"
        };

        private readonly Archivo _archivoCuta = new()
        {
            RutaArchivo = "cualquier ruta",
            Descripcion = "cualquier descripcion"
        };

        public CitaRepositoryTest()
        {
            _context = BuildDbContext(dbName);
            CreateMedicoAndPacineteForCita(_context).GetAwaiter().GetResult();

            entityList = new List<Cita>()
            {
                new()
                {
                    Estado = "Cancelada",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now,
                    MedicoId = _medicoCita.Id,
                    PacienteId = _pacienteCita.Id
                },
                new()
                {
                    Estado = "Acabada",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now.AddHours(2),
                    MedicoId = _medicoCita.Id,
                    PacienteId = _pacienteCita.Id
                },
                new()
                {
                    Estado = "No Asistió",
                    FechaInicio = DateTime.Now,
                    FechaFin = DateTime.Now,
                    MedicoId = _medicoCita.Id,
                    PacienteId = _pacienteCita.Id
                },
            };
            _context.Cita.AddRange(entityList);
            _context.SaveChangesAsync().GetAwaiter().GetResult();

            toAddEntity = new Cita
            {
                Estado = "Pendiente",
                FechaInicio = DateTime.Now,
                FechaFin = DateTime.Now.AddDays(2),
                MedicoId = _medicoCita.Id,
                PacienteId = _pacienteCita.Id,
                Archivos = new List<Archivo>
                {
                    _archivoCuta
                },
                ActividadesCita = new List<ActividadCita>()
                {
                    new ActividadCita
                    {
                        Actividad = _activdadCita,
                        ActividadTerminada = false,
                        Detalles = "culaquier detalle"
                    }
                }
            };

            _context = BuildDbContext(dbName);
            sut = new Repository<Cita>(_context);

        }

        private async Task CreateMedicoAndPacineteForCita(ApplicationDbContext context)
        {
            await _context.Medico.AddAsync(_medicoCita);
            await _context.Paciente.AddAsync(_pacienteCita);

            await _context.SaveChangesAsync();
        }
    }
}
