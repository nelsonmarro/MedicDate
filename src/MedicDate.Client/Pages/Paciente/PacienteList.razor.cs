using MedicDate.API.DTOs.Grupo;
using MedicDate.API.DTOs.Paciente;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Paciente
{
	public partial class PacienteList
	{
		[Inject]
		public IBaseListComponentOperations BaseListComponentOps { get; set; }

		[Inject]
		public IHttpRepository HttpRepo { get; set; }

		private IEnumerable<PacienteResponseDto> _pacienteList;
		private int _totalCount = 0;
		private const string GetUrl = "api/Paciente/listarConPaginacion?traerGrupos=true";
		private List<GrupoResponseDto> _grupoList = new();

		private readonly string[] _propNames =
		{
			"Nombres", "Apellidos", "Edad", "Sexo", "Cedula", "Email",
			"Telefono", "Direccion", "NumHistoria"
		};

		private readonly string[] _headers =
		{
			"Nombres", "Apellidos", "Edad", "Sexo", "Cédula", "Email",
			"Teléfono", "Dirección", "Num. Historia"
		};

		private readonly OpRoutes _opRoutes = new()
		{ AddUrl = "pacienteCrear", EditUrl = "pacienteEditar", GetUrl = GetUrl };

		protected override async Task OnInitializedAsync()
		{
			var result = await BaseListComponentOps
				.LoadItemListAsync<PacienteResponseDto>(GetUrl);

			if (result.Succeded)
			{
				_pacienteList = result.ItemList;
				_totalCount = result.TotalCount;
			}

			var httpResponse = await HttpRepo.Get<List<GrupoResponseDto>>("api/Grupo/listar");

			if (!httpResponse.Error)
			{
				_grupoList = httpResponse.Response;
			}
		}

		private async Task DeleteMedico(string id)
		{
			var result = await BaseListComponentOps.DeleteItem<PacienteResponseDto>(id, "api/Paciente/eliminar", GetUrl);

			if (result.Succeded)
			{
				_pacienteList = result.ItemList;
				_totalCount = result.TotalCount;
			}
		}

		private async Task FilterByGrupo(object value)
		{
			var grupoId = value?.ToString() ?? "";

			var result = await BaseListComponentOps
				.LoadItemListAsync<PacienteResponseDto>(GetUrl, "&filtrarGrupoId=", grupoId);

			if (result.Succeded)
			{
				_pacienteList = result.ItemList;
				_totalCount = result.TotalCount;
			}
		}
	}
}