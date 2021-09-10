using AutoMapper;
using MedicDate.API.DTOs.AppRole;
using MedicDate.API.DTOs.AppUser;
using MedicDate.API.DTOs.Common;
using MedicDate.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Repository.IRepository;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : BaseController<ApplicationUser>
    {
        private readonly IAppUserRepository _appUserRepo;
        private readonly IMapper _mapper;

        public UsuarioController(IAppUserRepository appUserRepo, IMapper mapper)
            : base(appUserRepo, mapper)
        {
            _appUserRepo = appUserRepo;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async
            Task<ActionResult<PaginatedResourceListDto<AppUserResponseDto>>>
            GetAllUsersAsync
            (
                int pageIndex = 0,
                int pageSize = 10,
                [FromQuery] bool traerRoles = false,
                [FromQuery] string filterRolId = null
            )
        {
            var includeProperties = traerRoles
                ? "UserRoles.Role"
                : "";

            if (!string.IsNullOrEmpty(filterRolId))
                return await GetAllWithPagingAsync<AppUserResponseDto>
                (
                    pageIndex,
                    pageSize,
                    includeProperties,
                    x => x.UserRoles.Any(ur => ur.RoleId == filterRolId)
                );

            return await GetAllWithPagingAsync<AppUserResponseDto>
            (
                pageIndex,
                pageSize,
                includeProperties
            );
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserResponseDto>> GetUserById(
            string id)
        {
            return await GetByIdAsync<AppUserResponseDto>(id, "UserRoles.Role");
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<AppUserRequestDto>> GetPutUserAsync(
            string id)
        {
            return await GetByIdAsync<AppUserRequestDto>(id, "UserRoles.Role");
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id,
            AppUserRequestDto appUserRequestDto)
        {
            var resp =
                await _appUserRepo.UpdateUserAsync(id, appUserRequestDto);

            return resp.Succeeded
                ? resp.SuccessResult
                : resp.ErrorResult;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleResponseDto>>> GetRolesAsync()
        {
            var rolesListDb = await _appUserRepo.GetRolesAsync();

            return rolesListDb.Select(x => new RoleResponseDto
            {
                Id = x.Id,
                Nombre = x.Name,
                Descripcion = x.Descripcion
            }).ToList();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteUserAsync(string id,
            [FromServices] IUserService userService)
        {
            var esMasterResp =
                await userService.CheckIfUserIsWebMasterAsync(id);

            if (!esMasterResp.Succeeded)
            {
                return esMasterResp.ErrorResult;
            }

            if (esMasterResp.DataResult)
                return BadRequest("No puede eliminar al usuario Web Master");

            return await DeleteResourceAsync(id);
        }
    }
}