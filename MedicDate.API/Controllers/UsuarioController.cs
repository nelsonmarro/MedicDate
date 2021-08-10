using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.AppUser;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<ApplicationUser>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsuarioController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
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
            if (!string.IsNullOrEmpty(filterRolId))
            {
                return await GetAllWithPagingAsync<AppUserResponse>
                (
                    pageIndex,
                    pageSize,
                    traerRoles,
                    "UserRoles.Role",
                    x => x.UserRoles.Any(ur => ur.RoleId == filterRolId)
                );
            }

            return await GetAllWithPagingAsync<AppUserResponse>
            (
                pageIndex,
                pageSize,
                traerRoles,
                "UserRoles.Role"
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserResponse>> GetUserById(string id)
        {
            return await GetByIdAsync<AppUserResponse>(id,
                true, "UserRoles.Role");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<AppUserRequest>> GetPutAsync(string id)
        {
            return await GetByIdAsync<AppUserRequest>(id,
                true, "UserRoles.Role");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, AppUserRequest appUserRequest)
        {
            var resp = await _unitOfWork.AppUserRepo.EditUserAsync(id, appUserRequest);

            return resp.IsSuccess
                ? Ok("Usuario actualizado con éxito")
                : resp.ErrorActionResult;
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
            var esMasterResp = await _unitOfWork.AppUserRepo
                .CheckIfUserIsWebMasterAsync(id);

            if (!esMasterResp.IsSuccess)
            {
                return esMasterResp.ErrorActionResult;
            }

            if (esMasterResp.Data)
            {
                return BadRequest("No puede eliminar al usuario Web Master");
            }

            return await DeleteResourceAsync(id);
        }
    }
}