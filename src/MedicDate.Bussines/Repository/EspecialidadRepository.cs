using System.Threading.Tasks;
using AutoMapper;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using static System.Net.HttpStatusCode;

namespace MedicDate.Bussines.Repository
{
    public class EspecialidadRepository : Repository<Especialidad>, IEspecialidadRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public EspecialidadRepository(
            ApplicationDbContext context,
            IMapper mapper,
            IApiOperationResultFactory apiOpResultFactory
            ) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public async Task<ApiOperationResult> UpdateEspecialidadAsync(string id,
            EspecialidadRequestDto especialidadDto)
        {
            var especialidadDb = await FindAsync(id);

            if (especialidadDb is null)
                return _apiOpResultFactory.CreateErrorApiOperationResult(NotFound,
                    "No se encontró la especialidad");

            _mapper.Map(especialidadDto, especialidadDb);
            await _context.SaveChangesAsync();

            return _apiOpResultFactory.CreateSuccessApiOperationResult(OK,
                "Especialidad actualizada con éxito");
        }
    }
}