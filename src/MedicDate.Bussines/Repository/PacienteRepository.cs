using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Paciente;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Bussines.Services.IServices;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class PacienteRepository : Repository<Paciente>, IPacienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEntityValidator _entityValidator;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public PacienteRepository(
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

        public async Task<bool> CheckCedulaExistsForCreateAsync(string numCedula)
        {
            return await _entityValidator
                .CheckValueExistsAsync<Paciente>("Cedula", numCedula);
        }

        public async Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string id)
        {
            return await _entityValidator
                .CheckValueExistsForEditAsync<Paciente>("Cedula", numCedula,
                    id);
        }

        public Task<bool> CheckRelatedEntityIdsExistsAsync(List<string> entityIds)
        {
            return _entityValidator
                .CheckRelatedEntityIdsExists<Grupo>(entityIds);
        }

        public async Task<ApiOperationResult> UpdatePacienteAsync(string id,
            PacienteRequestDto pacienteRequestDto)
        {
            if (await CheckCedulaExistsForEditAsync(pacienteRequestDto.Cedula, id))
                return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "Ya existe otro paciente registrado con la cédula que ingresó");

            if (await CheckNumHistoriaExistsAsync(pacienteRequestDto.NumHistoria, true, id))
                return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "Ya existe otro paciente registrado con el número de historia que ingresó");


            if (!await CheckRelatedEntityIdsExistsAsync(pacienteRequestDto.GruposId))
                return _apiOpResultFactory.CreateErrorApiOperationResult(BadRequest, "No existe uno de los grupos asignados");

            var pacienteDb = await _context.Paciente
                .Include(x => x.GruposPacientes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (pacienteDb is null)
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound, "No se encontró el paciente a actualizar");

            _mapper.Map(pacienteRequestDto, pacienteDb);
            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Paciente actualizado con éxito");
        }

        public async Task<bool> CheckNumHistoriaExistsAsync(string numHistoria, bool isEdit = false, string id = null)
        {
            if (isEdit && !string.IsNullOrEmpty(id))
                return await _entityValidator
                    .CheckValueExistsForEditAsync<Paciente>("NumHistoria",
                        numHistoria, id);

            return await _entityValidator
                .CheckValueExistsAsync<Paciente>("NumHistoria", numHistoria);
        }
    }
}