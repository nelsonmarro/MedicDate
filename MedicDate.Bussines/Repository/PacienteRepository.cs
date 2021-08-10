using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Data;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Paciente;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MedicDate.Bussines.Repository
{
    public class PacienteRepository : Repository<Paciente>, IPacienteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PacienteRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CheckCedulaExistsAsync(string numCedula)
        {
            return await RequestEntityValidator<Paciente>
                .CheckValueExistsAsync(_context, "Cedula", numCedula);
        }

        public async Task<bool> CheckCedulaExistsForEditAsync(string numCedula, string id)
        {
            return await RequestEntityValidator<Paciente>
                .CheckValueExistsForEditAsync(_context, "Cedula", numCedula,
                    id);
        }

        public Task<bool> CheckRelatedEntityIdExistsAsync(List<string> entityIds)
        {
            return RequestEntityValidator<Grupo>.CheckRelatedEntityIdExists(_context, entityIds);
        }

        public async Task<DataResponse<string>> UpdatePacienteAsync(string id, PacienteRequest pacienteRequest)
        {
            if (await CheckCedulaExistsForEditAsync(pacienteRequest.Cedula, id))
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("Ya existe otro paciente registrado con la cédula que ingresó")
                };
            }

            if (await CheckNumHistoriaExistsAsync(pacienteRequest.NumHistoria, true, id))
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult =
                        new BadRequestObjectResult(
                            "Ya existe otro paciente registrado con el número de historia que ingresó")
                };
            }

            if (await CheckRelatedEntityIdExistsAsync(pacienteRequest.GruposId))
            {
                return new DataResponse<string>()
                {
                    IsSuccess = false,
                    ActionResult = new BadRequestObjectResult("No existe uno de los grupos asignados")
                };
            }

            var pacienteDb = await _context.Paciente
            .Include(x => x.GruposPacientes)
            .FirstOrDefaultAsync(x => x.Id == id);

            if (pacienteDb is null)
            {
                return new DataResponse<string>
                {
                    IsSuccess = false,
                    ActionResult = new NotFoundObjectResult("No se encontró el paciente a actualizar")
                };
            }

            _mapper.Map(pacienteRequest, pacienteDb);
            await _context.SaveChangesAsync();

            return new DataResponse<string>
            {
                IsSuccess = true,
                ActionResult = new OkObjectResult("Paciente actualizado con éxito")
            };
        }

        public async Task<bool> CheckNumHistoriaExistsAsync(string numHistoria, bool isEdit = false, string id = null)
        {
            if (isEdit && !string.IsNullOrEmpty(id))
            {
                return await RequestEntityValidator<Paciente>
                    .CheckValueExistsForEditAsync(_context, "NumHistoria",
                        numHistoria, id);
            }

            return await RequestEntityValidator<Paciente>
                .CheckValueExistsAsync(_context, "NumHistoria", numHistoria);
        }
    }
}
