using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Auth;
using System.Drawing;

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
        public async Task<ActionResult<ApiResponseDto<AppUserResponse>>> ListarUsuarios
        (
            int pageIndex = 0,
            int pageSize = 10,
            [FromQuery] bool traerRoles = false,
            [FromQuery] string filterRolId = null
        )
        {
            var apiResponse = new ApiResponseDto<AppUserResponse>();

            if (traerRoles)
            {
                var usersConRoles = await _userRepo.GetUsersConRoles(filterRolId);

                var count = usersConRoles.Count();

                usersConRoles = usersConRoles
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();


                apiResponse.DataResult = usersConRoles;
                apiResponse.TotalCount = count;
                apiResponse.PageIndex = pageIndex;
                apiResponse.PageSize = pageSize;
                apiResponse.TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            }
            else
            {
                var usersDb = await _userRepo.GetAllAsync();

                var result = ApiResult<ApplicationUser, AppUserResponse>.Create
                (
                    usersDb,
                    pageIndex,
                    pageSize,
                    _mapper
                );

                apiResponse.DataResult = result.DataResult;
                apiResponse.TotalCount = result.TotalCount;
                apiResponse.PageIndex = result.PageIndex;
                apiResponse.PageSize = result.PageSize;
                apiResponse.TotalPages = result.TotalPages;
            }

            return apiResponse;
        }

        [HttpGet("obtenerParaEditar/{id}")]
        public async Task<ActionResult<AppUserRequest>> GetUsuarioParaEditar(string id)
        {
            var response = await _userRepo.GetUsuarioParaEditar(id);

            if (!response.Sussces)
            {
                return response.ActionResult;
            }

            return response.Data;
        }

        [HttpPost("crear")]
        public async Task<ActionResult> CrearUsuario(RegisterRequest registerRequest)
        {
            var resp = await _userRepo.CrearUsuarioAsync(registerRequest);

            return resp.ActionResult;
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> EditarUsuario(string id, AppUserRequest appUserRequest)
        {
            var resp = await _userRepo.EditarUsuarioAsync(id, appUserRequest);

            return resp.ActionResult;
        }

        [HttpPost("lockunlock/{id}")]
        public async Task<ActionResult> LockUnlockUser(string id)
        {
            var resp = await _userRepo.LockUnlockUsuarioAsync(id);

            return resp.ActionResult;
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RolResponse>>> GetRoles()
        {
            var rolesListDb = await _userRepo.ObtenerRolesAsync();

            return rolesListDb.Select(x => new RolResponse
            {
                Id = x.Id,
                Nombre = x.Name,
                Descripcion = x.Descripcion
            }).ToList();
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var esMasterResp = await _userRepo.CheckIfUserIsWebMasterAsync(id);

            if (!esMasterResp.Sussces)
            {
                return esMasterResp.ActionResult;
            }

            if (esMasterResp.Data)
            {
                return BadRequest("No puede eliminar al usuario Web Master");
            }

            var resp = await _userRepo.EliminarUsuarioAsync(id);

            return resp.ActionResult;
        }
    }
}