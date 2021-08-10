using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Models.DTOs.AppUser;


namespace MedicDate.Client.Pages.AppUser
{
    public class AppUserListBase : BaseListComponent<AppUserResponse>, IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        protected static string GetUrl = "api/Usuario/listarConPaginacion?traerRoles=true";
        protected List<RoleResponse> Roles = new();
        protected string[] PropNames = { "Nombre", "Apellidos", "Email", "PhoneNumber"};
        protected string[] Headers = { "Nombre", "Apellidos", "Email", "Teléfono"};

        protected readonly OpRoutes OpRoutes = new()
            {AddUrl = "usuarioCrear", EditUrl = "usuarioEditar", GetUrl = GetUrl};

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

            await LoadItemListAsync(GetUrl);

            var httpResponse = await HttpRepo.Get<List<RoleResponse>>("api/Usuario/roles");

            if (httpResponse is null)
            {
                return;
            }

            if (httpResponse.Error)
            {
                NotificationService.ShowError("Error!", "Error al obtener los roles");
            }
            else
            {
                Roles = httpResponse.Response;
            }
        }

        protected async Task LockUnlock(string userId)
        {
            try
            {
                var httpResp = await HttpRepo.Post("api/Account/lockUnlock", userId);

                if (httpResp.Error)
                {
                    NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                }
                else
                {
                    NotificationService.ShowSuccess("Operación exitosa!", await httpResp.GetResponseBody());

                    await LoadItemListAsync(GetUrl);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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


        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}