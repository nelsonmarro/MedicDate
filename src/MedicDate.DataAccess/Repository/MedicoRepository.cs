using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Medico;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MedicoRepository(
            ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper; ;
        }

        public async Task<OperationResult> UpdateMedicoAsync(string id,
            MedicoRequestDto medicoRequestDto)
        {
            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (medicoDb is null)
                return OperationResult.Error(NotFound,
                    "No se encontró el doctor ha actualizar");

            _mapper.Map(medicoRequestDto, medicoDb);
            await _context.SaveChangesAsync();

            return OperationResult.Success(OK, "Doctor actualizado con éxito");
        }
    }
}