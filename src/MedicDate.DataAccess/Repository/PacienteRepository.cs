using System.Threading.Tasks;
using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Paciente;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository
{
    public class PacienteRepository : Repository<Paciente>, IPacienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PacienteRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult> UpdatePacienteAsync(string id,
            PacienteRequestDto pacienteRequestDto)
        {
            var pacienteDb = await _context.Paciente
                .Include(x => x.GruposPacientes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pacienteDb is null)
                return OperationResult.Error(NotFound,
                    "No se encontró el paciente a actualizar");

            _mapper.Map(pacienteRequestDto, pacienteDb);
            await _context.SaveChangesAsync();

            return OperationResult.Error(OK,
                "Paciente actualizado con éxito");
        }
    }
}