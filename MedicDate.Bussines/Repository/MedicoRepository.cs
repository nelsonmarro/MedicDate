using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository
{
    public class MedicoRepository : Repository<Medico>, IMedicoRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public MedicoRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DataResponse<string>> UpdateMedicoAsync(string id, MedicoRequest medicoRequest)
        {
            if (await CheckCedulaExistsForEditAsync(medicoRequest.Cedula, id))
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ErrorActionResult = new BadRequestObjectResult("Ya existe otro doctor registrado con la cédula que ingresó")
                };
            }

            if (await CheckRelatedEntityIdExistsAsync(medicoRequest.EspecialidadesId))
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ErrorActionResult = new BadRequestObjectResult("No existe una de las especialidades asignadas")
                };
            }

            var medicoDb = await _context.Medico
                .Include(x => x.MedicosEspecialidades)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (medicoDb is null)
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ErrorActionResult = new NotFoundObjectResult("No se encontró el médico ha actualizar")
                };
            }

            _mapper.Map(medicoRequest, medicoDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>()
            {
                IsSuccess = true,
                ErrorActionResult = new NoContentResult()
            };
        }

        public async Task<bool> CheckCedulaExistsAsync(string numCedula)
        {
            return await RequestEntityValidator<Medico>
                .CheckValueExistsAsync(_context, "Cedula", numCedula);
        }

        public async Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string id)
        {
            return await RequestEntityValidator<Medico>
                .CheckValueExistsForEditAsync(_context, "Cedula", numCedula, id);
        }

        public async Task<bool> CheckRelatedEntityIdExistsAsync(List<string> entityIds)
        {
            return await RequestEntityValidator<Especialidad>
                .CheckRelatedEntityIdExists(_context, entityIds);
        }
    }
}