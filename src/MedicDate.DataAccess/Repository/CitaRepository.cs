using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Cita;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
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

        public async Task<OperationResult> UpdateCitaAsync(string citaId,
            CitaRequestDto citaRequestDto)
        {
            var citaDb = await FirstOrDefaultAsync(filter: x => x.Id == citaId,
                "ActividadesCita");

            if (citaRequestDto.Archivos is not null &&
                citaRequestDto.Archivos.Count > 0)
            {
                var archivos = await _context.Archivo.ToListAsync();
                _context.Archivo.RemoveRange(archivos);
                await _context.SaveChangesAsync();
            }

            _mapper.Map(citaRequestDto, citaDb);
            await SaveAsync();

            return OperationResult.Success(OK, "Cita actualizada con éxito");
        }
    }
}