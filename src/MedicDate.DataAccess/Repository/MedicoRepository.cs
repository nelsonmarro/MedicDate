using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Medico;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.DataAccess.Services.IServices;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;

        public MedicoRepository(
            ApplicationDbContext context, IMapper mapper,
            IEntityValidator entityValidator) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _entityValidator = entityValidator;
        }

        public async Task<OperationResult> UpdateMedicoAsync(string id,
            MedicoRequestDto medicoRequestDto)
        {
            if (await CheckCedulaExistsForEditAsync(medicoRequestDto.Cedula,
                id))
                return OperationResult.Error(BadRequest,
                    "Ya existe otro doctor registrado con la cédula que ingresó");

            if (!await CheckRelatedEntityIdsExistsAsync(medicoRequestDto
                .EspecialidadesId))
                return OperationResult.Error(BadRequest,
                    "No existe una de las especialidades asignadas");

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

        public async Task<bool> CheckCedulaExistsForCreateAsync(
            string numCedula)
        {
            return await _entityValidator
                .CheckValueExistsAsync<Medico>("Cedula", numCedula);
        }

        public async Task<bool> CheckCedulaExistsForEditAsync(string numCedula,
            string id)
        {
            return await _entityValidator
                .CheckValueExistsForEditAsync<Medico>("Cedula", numCedula, id);
        }

        public async Task<bool> CheckRelatedEntityIdsExistsAsync(
            List<string> entityIds)
        {
            return await _entityValidator
                .CheckRelatedEntityIdsExists<Especialidad>(entityIds);
        }
    }
}