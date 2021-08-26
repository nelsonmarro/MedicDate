using MedicDate.API.DTOs.Grupo;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Grupo
{
	public partial class GrupoEdit
	{
		[Inject] public IHttpRepository HttpRepo { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public INotificationService NotificationService { get; set; }

		[Parameter] public string Id { get; set; }

		private bool _isBussy;

		private GrupoRequestDto _grupoModel;

		protected override async Task OnParametersSetAsync()
		{
			var httpResp = await HttpRepo
				.Get<GrupoRequestDto>($"api/Grupo/obtenerParaEditar/{Id}");

			if (!httpResp.Error)
			{
				_grupoModel = httpResp.Response;
			}
		}

		private async Task EditGrupo()
		{
			_isBussy = true;

			var httpResp = await HttpRepo.Put($"api/Grupo/editar/{Id}", _grupoModel);

			_isBussy = false;

			if (!httpResp.Error)
			{
				NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

				NavigationManager.NavigateTo("grupoList");
			}
		}
	}
}
