using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Grupo;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Grupo;

public partial class GrupoList
{
   private const string GetUrl = "api/Grupo/listarConPaginacion";

   private readonly OpRoutes _opRoutes = new()
   { AddUrl = "grupoCrear", EditUrl = "grupoEditar", GetUrl = GetUrl };

   private readonly string[] _propNames = { "Nombre" };
   private readonly string[] _tableHeaders = { "Nombre" };

   private List<GrupoResponseDto>? _grupoList;
   private int _totalCount;

   [Inject]
   public IBaseListComponentOperations BaseListComponentOps { get; set; } =
     default!;

   protected override async Task OnInitializedAsync()
   {
      var result =
        await BaseListComponentOps.LoadItemListAsync<GrupoResponseDto>(GetUrl);

      if (result.Succeded)
      {
         _grupoList = result.ItemList;
         _totalCount = result.TotalCount;
      }
   }

   private async Task DeleteGrupo(string idString)
   {
      const string deleteUrl = "api/Grupo/eliminar";
      var result =
        await BaseListComponentOps.DeleteItemAndLoadDataList<GrupoResponseDto>(idString,
          deleteUrl, GetUrl);

      if (result.Succeded)
      {
         _grupoList = result.ItemList;
         _totalCount = result.TotalCount;
      }
   }
}