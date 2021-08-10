using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Grupo;
using Microsoft.AspNetCore.Mvc;

namespace MedicDate.Bussines.Repository
{
    public class GrupoRepository : Repository<Grupo>, IGrupoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GrupoRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> UpdateGrupoAsync(string id, GrupoRequest grupoRequest)
        {
            var grupoDb = await _context.Grupo.FindAsync(id);

            if (grupoDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encontró el grupo para editar")
                };
            }

            _mapper.Map(grupoRequest, grupoDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>()
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Grupo actualizado con éxito")
            };
        }
    }
}