using MedicDate.API.DTOs.AppRole;
using MedicDate.API.DTOs.AppUser;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserList
    {
        [Inject]
        public IBaseListComponentOperations BaseListComponentOps { get; set; }

        [Inject]
        public INotificationService NotificationService { get; set; }

        [Inject]
        public IHttpRepository HttpRepo { get; set; }

        private IEnumerable<AppUserResponseDto> _userList;
        private int _totalCount = 0;
        private static string GetUrl = "api/Usuario/listarConPaginacion?traerRoles=true";
        private List<RoleResponseDto> _roleList = new();

        private readonly string[] _propNames = { "Nombre", "Apellidos", "Email", "PhoneNumber" };
        private readonly string[] _headers = { "Nombre", "Apellidos", "Email", "Teléfono" };

        private readonly OpRoutes _opRoutes = new()
        { AddUrl = "usuarioCrear", EditUrl = "usuarioEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            var result = await BaseListComponentOps.LoadItemListAsync<AppUserResponseDto>(GetUrl);

            if (result.Succeded)
            {
                _userList = result.ItemList;
                _totalCount = result.TotalCount;
            }

            var httpResponse = await HttpRepo.Get<List<RoleResponseDto>>("api/Usuario/roles");

            if (!httpResponse.Error)
            {
                _roleList = httpResponse.Response;
            }
        }

        private async Task LockUser(string userId)
        {
            var httpResp = await HttpRepo.Post("api/Account/lock", userId);

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!",
                    await httpResp.GetResponseBody());

                var result = await BaseListComponentOps.LoadItemListAsync<AppUserResponseDto>(GetUrl);

                if (result.Succeded)
                {
                    _userList = result.ItemList;
                    _totalCount = result.TotalCount;
                }
            }
        }

        private async Task UnlockUser(string userId)
        {
            var httpResp = await HttpRepo.Post("api/Account/unlock", userId);

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!", await httpResp.GetResponseBody());

                var result = await BaseListComponentOps.LoadItemListAsync<AppUserResponseDto>(GetUrl);

                if (result.Succeded)
                {
                    _userList = result.ItemList;
                    _totalCount = result.TotalCount;
                }
            }
        }

        private async Task DeleteUser(string id)
        {
            var result = await BaseListComponentOps.DeleteItem<AppUserResponseDto>(id, "api/Usuario/eliminar", GetUrl);

            if (result.Succeded)
            {
                _userList = result.ItemList;
                _totalCount = result.TotalCount;
            }
        }

        private async Task FilterByRole(object value)
        {
            try
            {
                var rolId = value is string ? value.ToString() : null;

                var result = await BaseListComponentOps.LoadItemListAsync<AppUserResponseDto>(GetUrl, "&filterRolId=", rolId);

                if (result.Succeded)
                {
                    _userList = result.ItemList;
                    _totalCount = result.TotalCount;
                }
            }
            catch (Exception)
            {
                NotificationService.ShowError("Error!", "Error al obtener el Id del rol");
            }
        }
    }
}