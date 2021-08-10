using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.Client.Pages.Especialidad
{
    public class EspecialidadListBase : BaseListComponent<EspecialidadResponse>, IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        protected readonly string[] TableHeaders = {"Nombre"};
        protected readonly string[] PropNames = {"NombreEspecialidad"};
        protected const string GetUrl = "api/Especialidad/listarConPaginacion";

        protected readonly OpRoutes OpRoutes = new()
            {AddUrl = "especialidadCrear", EditUrl = "especialidadEditar", GetUrl = GetUrl};

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();
            await LoadItemListAsync(GetUrl);
        }

        protected async Task DeleteEspecialidad(string id)
        {
            await DeleteItem(id, "api/Especialidad/eliminar", GetUrl);
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}