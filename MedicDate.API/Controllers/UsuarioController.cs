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
        public async Task<ActionResult<ApiResponseDto<AppUserResponse>>> GetAllUsersAsync
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
                var usersConRoles = await _userRepo.GetUsersWithRoles(filterRolId);

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

        [HttpPost("crear")]
        public async Task<ActionResult> PostAsync(RegisterRequest registerRequest)
        {
            var resp = await _userRepo.CreateUserAsync(registerRequest);

            return resp.ActionResult;
        }

        [HttpPut("editar/{id}")]
        public async Task<ActionResult> PutAsync(string id, AppUserRequest appUserRequest)
        {
            var resp = await _userRepo.EditUserAsync(id, appUserRequest);

            return resp.ActionResult;
        }

        [HttpPost("lockUnlock/{id}")]
        public async Task<ActionResult> LockUnlockUserAsync(string id)
        {
            var resp = await _userRepo.LockUnlockUserAsync(id);

            return resp.ActionResult;
        }

        [HttpPost("confirmEmail")]
        public async Task<ActionResult> ConfirmEmailAsync(ConfirmEmailRequest confirmEmailRequest)
        {
            var response = await _userRepo.ConfirmEmailAsync(confirmEmailRequest);

            return !response.IsSuccess ? response.ActionResult : Ok();
        }

        [HttpPost("sendConfirmationEmail")]
        public async Task<ActionResult> ResendConfirmEmailAsync([FromBody] string userEmail)
        {
            var response = await _userRepo.SendConfirmEmailAsync(userEmail);

            return !response.IsSuccess ? response.ActionResult : Ok();
        }

        [HttpPost("sendChangeEmailToken")]
        public async Task<ActionResult> SendChangeEmailTokenAsync(ChangeEmailModel changeEmailModel)
        {
            var response = await _userRepo.SendChangeEmailTokenAsync(changeEmailModel);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return Ok();
        }

        [HttpPost("changeEmail/{userId}")]
        public async Task<ActionResult> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel)
        {
            var response = await _userRepo.ChangeEmailAsync(userId, changeEmailModel);

            if (!response.IsSuccess)
            {
                return response.ActionResult;
            }

            return Ok();
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