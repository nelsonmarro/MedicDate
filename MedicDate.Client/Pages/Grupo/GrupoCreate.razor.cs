using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs.Grupo;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Grupo
{
    public partial class GrupoCreate
    {
        [Inject] public IHttpRepository HttpRepo { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public INotificationService NotificationService { get; set; }

        private bool _isBussy;

        private readonly GrupoRequest _grupoModel = new();

        private async Task CreateGrupo()
        {
            _isBussy = true;

            var httpResp = await HttpRepo.Post("api/Grupo/crear", _grupoModel);

            _isBussy = false;

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", "Grupo creado con éxito");

                NavigationManager.NavigateTo("grupoList");
            }
        }
    }
}
