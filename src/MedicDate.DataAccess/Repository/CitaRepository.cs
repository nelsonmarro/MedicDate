using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Cita;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository
{
    public class CitaRepository : Repository<Cita>, ICitaRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CitaRepository(ApplicationDbContext context, IMapper mapper) :
            base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CitaCalendarDto>> GetCitasByDateRange(DateTime startDate, DateTime endDate)
        {
            var citasListDb = await _context.Cita.AsNoTracking()
                .Include(x => x.Paciente)
                .Include(x => x.Medico)
                .Where(x => x.FechaInicio >= startDate && x.FechaFin <= endDate)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CitaCalendarDto>>(citasListDb);
        }

        public async Task<OperationResult> UpdateCitaAsync(string citaId,
            CitaRequestDto citaRequestDto)
        {
            var citaDb = await FirstOrDefaultAsync(filter: x => x.Id == citaId,
                "ActividadesCita");

            if (citaRequestDto.Archivos is not null && citaRequestDto.Archivos.Count > 0)
            {
                var archivos = await _context.Archivo.ToListAsync();
                _context.Archivo.RemoveRange(archivos);
                await _context.SaveChangesAsync();
            }

            _mapper.Map(citaRequestDto, citaDb);
            await SaveAsync();

            return OperationResult.Success(OK, "Cita actualizada con éxito");
        }

        public async Task<OperationResult> UpdateEstadoCitaAsync(string id, string newEstado)
        {
            var citaDb = await FirstOrDefaultAsync(x => x.Id == id);

            if (citaDb is null)
            {
                return OperationResult.Error(NotFound, "No se encontro la cita para actualizar");
            }

            citaDb.Estado = newEstado;
            await _context.SaveChangesAsync();

            return OperationResult
            .Success(OK, "Cita actualizada correctamente");
        }
    }
}