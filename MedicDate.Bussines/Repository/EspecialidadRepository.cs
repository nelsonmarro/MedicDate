using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.Repository
{
    public class EspecialidadRepository : Repository<Especialidad>, IEspecialidadRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EspecialidadRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> UpdateEspecialidad(string id, EspecialidadRequest especialidadDto)
        {
            var especialidadDb = await FindAsync(id);

            if (especialidadDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encontró la especialidad")
                };
            }

            _mapper.Map(especialidadDto, especialidadDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>()
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Especialidad actualizada con éxito")
            };
        }
    }
}