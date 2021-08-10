using MedicDate.Client.Components;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs.Grupo;
using MedicDate.Models.DTOs.Paciente;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Paciente
{
    public class PacienteListBase : BaseListComponent<PacienteResponse>
    {
        private const string GetUrl = "api/Paciente/listarConPaginacion?traerGrupos=true";
        protected List<GrupoResponse> Grupos = new();

        protected readonly string[] PropNames =
        {
            "Nombres", "Apellidos", "Edad", "Sexo", "Cedula", "Email",
            "Telefono", "Direccion", "NumHistoria"
        };

        protected readonly string[] Headers =
        {
            "Nombres", "Apellidos", "Edad", "Sexo", "Cédula", "Email",
            "Teléfono", "Dirección", "Num. Historia"
        };

        protected readonly OpRoutes OpRoutes = new()
        { AddUrl = "pacienteCrear", EditUrl = "pacienteEditar", GetUrl = GetUrl };

        protected override async Task OnInitializedAsync()
        {
            await LoadItemListAsync(GetUrl);

            var httpResponse = await HttpRepo.Get<List<GrupoResponse>>("api/Grupo/listar");

            if (!httpResponse.Error)
            {
                Grupos = httpResponse.Response;
            }
        }

        protected async Task DeleteMedico(string id)
        {
            await DeleteItem(id, "api/Paciente/eliminar", GetUrl);
        }

        protected async Task FilterByGrupo(object value)
        {
            try
            {
                var grupoId = value?.ToString() ?? "";

                await LoadItemListAsync(GetUrl, "&filtrarGrupoId=", grupoId);
            }
            catch (Exception)
            {
                NotificationService.ShowError("Error!", "Error al obtener el Id del grupo");
            }
        }
    }
}