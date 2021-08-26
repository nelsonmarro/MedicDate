using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Grupo;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class GrupoRepository : Repository<Grupo>, IGrupoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public GrupoRepository(
            ApplicationDbContext context,
            IMapper mapper,
            IApiOperationResultFactory apiOpResultFactory) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public async Task<ApiOperationResult> UpdateGrupoAsync(string id, GrupoRequestDto grupoRequestDto)
        {
            var grupoDb = await FindAsync(id);

            if (grupoDb is null)
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound, "No se encontró el grupo para editar");

            _mapper.Map(grupoRequestDto, grupoDb);
            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Grupo actualizado con éxito");
        }
    }
}