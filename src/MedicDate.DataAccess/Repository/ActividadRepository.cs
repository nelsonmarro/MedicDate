using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Actividad;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository
{
    public class ActividadRepository : Repository<Actividad>,
        IActividadRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActividadRepository(ApplicationDbContext context, IMapper mapper)
            : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OperationResult> UpdateActividadAsync(string actId, ActividadRequestDto actRequestDto)
        {
            var actividadDb = await FindAsync(actId);

            if (actividadDb is null)
                return OperationResult.Error(NotFound,
                    "No se encontró la actividad a actualizar");

            _mapper.Map(actRequestDto, actividadDb);

            await SaveAsync();

            return OperationResult.Success(OK,
                "Actividad atualizada con éxito");
        }
    }
}