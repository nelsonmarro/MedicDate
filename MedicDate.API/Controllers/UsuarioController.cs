using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using Mailjet.Client.Resources.SMS;
using System;
using System.Drawing;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResponseDto<AppUserResponse>>> GetAllUsersAsync
        (
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerRoles = false,
            [FromQuery] string filterRolId = null
        )
        {
            var count = await _unitOfWork.AppUserRepo.CountResourcesAsync();

            if (traerRoles)
            {
                var usersWithRoles = await _unitOfWork.AppUserRepo
                    .GetUsersWithRoles(filterRolId, pageIndex, pageSize);

                return new ApiResponseDto<AppUserResponse>()
                {
                    DataResult = usersWithRoles,
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalCount = count
                };
            }

            var usersDb = await _unitOfWork.AppUserRepo
                .GetAllWithPagingAsync(
                    isTracking: false,
                    pageIndex: pageIndex,
                    pageSize: pageSize,
                    orderBy: x =>
                        x.OrderBy(u => u.Nombre));

            return new ApiResponseDto<AppUserResponse>()
            {
                DataResult = _mapper.Map<List<AppUserResponse>>(usersDb),
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalCount = count
            };
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<AppUserRequest>> GetPutAsync(string id)
        {
            var response = await _unitOfWork.AppUserRepo.GetUserForEdit(id);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return response.Data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserResponse>> GetUserById(string id)
        {
            var userDb = await _unitOfWork.AppUserRepo.FirstOrDefaultAsync(x => x.Id == id);

            if (userDb is null)
            {
                return NotFound($"No se encotro el usuario con id : {id}");
            }

            return _mapper.Map<AppUserResponse>(userDb);
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, AppUserRequest appUserRequest)
        {
            var resp = await _unitOfWork.AppUserRepo.EditUserAsync(id, appUserRequest);

            return resp.ActionResult;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleResponse>>> GetRolesAsync()
        {
            var rolesListDb = await _unitOfWork.AppUserRepo.GetRolesAsync();

            return rolesListDb.Select(x => new RoleResponse
            {
                Id = x.Id,
                Nombre = x.Name,
                Descripcion = x.Descripcion
            }).ToList();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteUserAsync(string id)
        {
            var esMasterResp = await _unitOfWork.AppUserRepo.CheckIfUserIsWebMasterAsync(id);

            if (!esMasterResp.IsSuccess)
            {
                return esMasterResp.ActionResult;
            }

            if (esMasterResp.Data)
            {
                return BadRequest("No puede eliminar al usuario Web Master");
            }

            var resp = await _unitOfWork.AppUserRepo.DeleteUserAsync(id);

            return resp.ActionResult;
        }
    }
}