using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Utility;

namespace MedicDate.Bussines.Repository
{
    public class EspecialidadRepository : Repository<Especialidad>, IEspecialidadRepository
    {
        private readonly IMapper _mapper;

        public EspecialidadRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> UpdateEspecialidad(int id, EspecialidadRequest especialidadDto)
        {
            var especialidadDb = await FindAsync(id);

            if (especialidadDb is null)
            {
                return new DataResponse<string>()
                {
                    Sussces = false,
                    Message = "No se encontró la especialidad"
                };
            }

            _mapper.Map(especialidadDto, especialidadDb);
            await SaveAsync();

            return new DataResponse<string>()
            {
                Sussces = true,
                Message = "Especialidad actualizada con éxito"
            };
        }
    }
}