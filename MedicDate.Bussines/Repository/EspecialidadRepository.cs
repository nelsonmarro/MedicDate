using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Especialidad;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
                    ErrorActionResult = new NotFoundObjectResult("No se encontró la especialidad")
                };
            }

            _mapper.Map(especialidadDto, especialidadDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>()
            {
                IsSuccess = true
            };
        }
    }
}