using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs.AppUser;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.AppUser
{
    public class AppUserListBase : BaseListComponent<AppUserResponse>
    {
        protected static string GetUrl = "api/Usuario/listarConPaginacion?traerRoles=true";
        protected List<RoleResponse> Roles = new();
        protected string[] PropNames = { "Nombre", "Apellidos", "Email", "PhoneNumber" };
        protected string[] Headers = { "Nombre", "Apellidos", "Email", "Teléfono" };

        protected readonly OpRoutes OpRoutes = new()
        { AddUrl = "usuarioCrear", EditUrl = "usuarioEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemListAsync(GetUrl);

            var httpResponse = await HttpRepo.Get<List<RoleResponse>>("api/Usuario/roles");

            if (!httpResponse.Error)
            {
                Roles = httpResponse.Response;
            }
        }

        protected async Task LockUnlock(string userId)
        {
            var httpResp = await HttpRepo.Post("api/Account/lockUnlock", userId);

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación exitosa!", await httpResp.GetResponseBody());

                await LoadItemListAsync(GetUrl);
            }
        }

        protected async Task DeleteUser(string id)
        {
            await DeleteItem(id, "api/Usuario/eliminar", GetUrl);
        }

        protected async Task FilterByRole(object value)
        {
            try
            {
                var rolId = value is string ? value.ToString() : null;

                await LoadItemListAsync(GetUrl, "&filterRolId=", rolId);
            }
            catch (Exception)
            {
                NotificationService.ShowError("Error!", "Error al obtener el Id del rol");
            }
        }
    }
}