using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Utility;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MedicoRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> ExisteEspecialidadIdParaCrearMedico(List<int> especialidadesIds)
        {
            var especialidadesIdsDb = await _context.Especialidad
                .Where(x => especialidadesIds.Contains(x.Id))
                .Select(x => x.Id).ToListAsync();

            return !(especialidadesIdsDb.Count != especialidadesIds.Count);
        }

        public async Task<TResponse> GetMedicoConEspecialidades<TResponse>(int medicoId)
        {
            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .ThenInclude(x => x.Especialidad)
                .FirstOrDefaultAsync(x => x.Id == medicoId);

            return _mapper.Map<TResponse>(medicoDb);
        }

        public async Task<TRequest> GetMedicoParaEdicion<TRequest>(int medicoId)
        {
            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .FirstOrDefaultAsync(x => x.Id == medicoId);

            return _mapper.Map<TRequest>(medicoDb);
        }

        public async Task<DataResponse<string>> UpdateMedicoAsync(int id, MedicoRequest medicoRequest)
        {
            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (medicoDb is null)
            {
                return new DataResponse<string>()
                {
                    Sussces = false,
                    Message = "No se encontró el médico ha actualizar"
                };
            }

            _mapper.Map(medicoRequest, medicoDb);
            await SaveAsync();

            return new DataResponse<string>()
            {
                Sussces = true,
                Message = "Médico actualizado exitosamente"
            };
        }
    }
}