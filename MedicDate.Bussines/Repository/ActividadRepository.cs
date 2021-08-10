using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Actividad;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Repository
{
    public class ActividadRepository : Repository<Actividad>, IActividadRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ActividadRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> UpdateActividadAsync(string actId, ActividadRequest actRequest)
        {
            var actividadDb = await FindAsync(actId);

            if (actividadDb is null)
            {
                return new DataResponse<string>
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encontró la actividad a actualizar")
                };
            }

            _mapper.Map(actRequest, actividadDb);

            await _context.SaveChangesAsync();

            return new DataResponse<string>
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Actividad actualizada con éxito")
            };
        }
    }
}