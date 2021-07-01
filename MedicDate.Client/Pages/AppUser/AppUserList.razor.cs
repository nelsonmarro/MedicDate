using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Models.DTOs.Medico;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Client.Pages.Medico;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.AppUser;


namespace MedicDate.Client.Pages.AppUser
{
    public partial class AppUserList : IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private static string _getUrl = "api/Usuario/listarConPaginacion?traerRoles=true";
        private List<AppUserResponse> _usuariosList;
        private int _totalCount;
        private List<RolResponse> _roles = new();

        private readonly PermitirOpCrud _permitirOp = new()
            {PermitirAgregar = true, PermitirEditar = true, PermitirEliminar = true};

        private readonly RutasOp _rutasOp = new()
            {AgregarUrl = "usuarioCrear", EditarUrl = "usuarioEditar", GetUrl = _getUrl};

        private async Task CargarUsuariosList(string filterRolId = null)
        {
            var filtrarPorRolQuery = "";

            if (!string.IsNullOrEmpty(filterRolId))
            {
                filtrarPorRolQuery = $"&filterRolId={filterRolId}";
            }

            var response = await HttpRepo.Get<ApiResponseDto<AppUserResponse>>($"{_getUrl}{filtrarPorRolQuery}");

            if (response is null)
            {
                return;
            }

            if (response.Error)
            {
                NotificationService.ShowError("Error!", "Error al cargar los datos");
            }
            else
            {
                _usuariosList = response.Response.DataResult;
                _totalCount = response.Response.TotalCount;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();

            var httpResponse = await HttpRepo.Get<List<RolResponse>>("api/Usuario/roles");

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
                _roles = httpResponse.Response;
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await CargarUsuariosList();
        }

        private async Task EliminarUser(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var httpResp = await HttpRepo.Delete($"api/Usuario/eliminar/{id}");

                if (httpResp is null)
                {
                    return;
                }

                if (httpResp.Error)
                {
                    NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
                }
                else
                {
                    NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                    await CargarUsuariosList();
                }
            }
        }

        private async Task FiltrarPorRol(object value)
        {
            try
            {
                var rolId = value is string ? value.ToString() : null;

                await CargarUsuariosList(rolId);
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