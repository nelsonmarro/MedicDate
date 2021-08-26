using MedicDate.API.DTOs.Actividad;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Actividad
{
	public partial class ActividadCreate
	{
		[Inject] public IHttpRepository HttpRepo { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }
		[Inject] public INotificationService NotificationService { get; set; }

		private bool _isBussy;

		private readonly ActividadRequestDto _actividadModel = new();

		private async Task CreateActividad()
		{
			_isBussy = true;

			var httpResp = await HttpRepo.Post("api/Actividad/crear", _actividadModel);

			_isBussy = false;

			if (!httpResp.Error)
			{
				NotificationService.ShowSuccess("Operación Exitosa!", "Actividad creada con éxito");

				NavigationManager.NavigateTo("actividadList");
			}
		}
	}
}
