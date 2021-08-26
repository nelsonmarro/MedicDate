using MedicDate.API.DTOs.Grupo;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Client.Pages.Grupo
{
	public partial class GrupoList
	{
		[Inject]
		public IBaseListComponentOperations BaseListComponentOps { get; set; }

		private IEnumerable<GrupoResponseDto> _grupoList;
		private int _totalCount = 0;
		private readonly string[] _tableHeaders = { "Nombre" };
		private readonly string[] _propNames = { "Nombre" };
		private const string GetUrl = "api/Grupo/listarConPaginacion";

		private readonly OpRoutes _opRoutes = new()
		{ AddUrl = "grupoCrear", EditUrl = "grupoEditar", GetUrl = GetUrl };

		protected override async Task OnInitializedAsync()
		{
			var result = await BaseListComponentOps.LoadItemListAsync<GrupoResponseDto>(GetUrl);

			if (result.Succeded)
			{
				_grupoList = result.ItemList;
				_totalCount = result.TotalCount;
			}
		}

		private async Task DeleteGrupo(string idString)
		{
			const string deleteUrl = "api/Grupo/eliminar";
			var result = await BaseListComponentOps.DeleteItem<GrupoResponseDto>(idString, deleteUrl, GetUrl);

			if (result.Succeded)
			{
				_grupoList = result.ItemList;
				_totalCount = result.TotalCount;
			}
		}
	}
}
