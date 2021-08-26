using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Medico;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Bussines.Factories.IFactories;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;
        private readonly IApiOperationResultFactory _apiOpResultFactory;


        public MedicoRepository(
            ApplicationDbContext context,
            IMapper mapper,
            IEntityValidator entityValidator,
            IApiOperationResultFactory apiOpResultFactory) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _entityValidator = entityValidator;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public async Task<ApiOperationResult> UpdateMedicoAsync(string id, MedicoRequestDto medicoRequestDto)
        {
            if (await CheckCedulaExistsForEditAsync(medicoRequestDto.Cedula, id))
                return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "Ya existe otro doctor registrado con la cédula que ingresó");

            if (!await CheckRelatedEntityIdsExistsAsync(medicoRequestDto.EspecialidadesId))
                return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "No existe una de las especialidades asignadas");

            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (medicoDb is null)
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound, "No se encontró el doctor ha actualizar");

            _mapper.Map(medicoRequestDto, medicoDb);
            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Doctor actualizado con éxito");
        }

        public async Task<bool> CheckCedulaExistsForCreateAsync(string numCedula)
        {
            return await _entityValidator
                .CheckValueExistsAsync<Medico>("Cedula", numCedula);
        }

        public async Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string id)
        {
            return await _entityValidator
                .CheckValueExistsForEditAsync<Medico>("Cedula", numCedula, id);
        }

        public async Task<bool> CheckRelatedEntityIdsExistsAsync(List<string> entityIds)
        {
            return await _entityValidator
                .CheckRelatedEntityIdsExists<Especialidad>(entityIds);
        }
    }
}