using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Bussines.Helpers;

namespace MedicDate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAppUserRepository _userRepo;
        private readonly IMapper _mapper;

        public UsuarioController(IAppUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpGet("listarConPaginacion")]
        public async Task<ActionResult<ApiResult<AppUserResponse>>> GetAllUsersAsync
        (
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerRoles = false,
            [FromQuery] string filterRolId = null
        )
        {
            if (traerRoles)
            {
                var usersWithRoles = await _userRepo.GetUsersWithRoles(filterRolId);

                return ApiResult<AppUserResponse>.Create
                (
                    usersWithRoles,
                    pageIndex,
                    pageSize,
                    "Nombre",
                    "ASC"
                );
            }

            var usersDb = await _userRepo.GetAllAsync<AppUserResponse>();

            return ApiResult<AppUserResponse>.Create
            (
                usersDb,
                pageIndex,
                pageSize,
                "Nombre",
                "ASC"
            );
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<AppUserRequest>> GetPutAsync(string id)
        {
            var response = await _userRepo.GetUserForEdit(id);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return response.Data;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserResponse>> GetUserById(string id)
        {
            var userDb = await _userRepo.FirstOrDefaultAsync(x => x.Id == id);

            if (userDb is null)
            {
                return NotFound($"No se encotro el usuario con id : {id}");
            }

            return _mapper.Map<AppUserResponse>(userDb);
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, AppUserRequest appUserRequest)
        {
            var resp = await _userRepo.EditUserAsync(id, appUserRequest);

            return resp.ActionResult;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RoleResponse>>> GetRolesAsync()
        {
            var rolesListDb = await _userRepo.GetRolesAsync();

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
            var esMasterResp = await _userRepo.CheckIfUserIsWebMasterAsync(id);

            if (!esMasterResp.IsSuccess)
            {
                return esMasterResp.ActionResult;
            }

            if (esMasterResp.Data)
            {
                return BadRequest("No puede eliminar al usuario Web Master");
            }

            var resp = await _userRepo.DeleteUserAsync(id);

            return resp.ActionResult;
        }
    }
}