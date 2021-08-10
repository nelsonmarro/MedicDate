using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using MedicDate.Client.Components;
using MedicDate.Models.DTOs.Grupo;


namespace MedicDate.Client.Pages.Grupo
{
    public class GrupoListBase : BaseListComponent<GrupoResponse>, IDisposable
    {
        [Inject] public IHttpInterceptorService HttpInterceptor { get; set; }

        protected readonly string[] TableHeaders = {"Nombre"};
        protected readonly string[] PropNames = {"Nombre"};
        protected const string GetUrl = "api/Grupo/listarConPaginacion";

        protected readonly OpRoutes OpRoutes = new()
            {AddUrl = "grupoCrear", EditUrl = "grupoEditar", GetUrl = GetUrl};

        protected override async Task OnInitializedAsync()
        {
            HttpInterceptor.RegisterEvent();
            await LoadItemListAsync(GetUrl);
        }

        protected async Task DeleteGrupo(string idString)
        {
            const string deleteUrl = "api/Grupo/eliminar";
            await DeleteItem(idString, deleteUrl, GetUrl);
        }

        public void Dispose()
        {
            HttpInterceptor.DisposeEvent();
        }
    }
}
