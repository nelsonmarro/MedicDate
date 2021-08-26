using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using System.Threading.Tasks;
using MedicDate.API.DTOs.Actividad;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using MedicDate.Bussines.Factories.IFactories;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class ActividadRepository : Repository<Actividad>, IActividadRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public ActividadRepository(
            ApplicationDbContext context,
            IMapper mapper,
            IApiOperationResultFactory apiOpResultFactory
        ) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public async Task<ApiOperationResult> UpdateActividadAsync(string actId,
            ActividadRequestDto actRequestDto)
        {
            var actividadDb = await FindAsync(actId);

            if (actividadDb is null)
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound,
                    "No se encontró la actividad a actualizar");

            _mapper.Map(actRequestDto, actividadDb);

            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK, "Actividad atualizada con éxito");
        }
    }
}