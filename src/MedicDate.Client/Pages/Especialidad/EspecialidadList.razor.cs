using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Especialidad;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Especialidad;

public partial class EspecialidadList
{
   private const string GetUrl = "api/Especialidad/listarConPaginacion";

   private readonly OpRoutes _opRoutes = new()
   {
      AddUrl = "especialidadCrear", EditUrl = "especialidadEditar",
      GetUrl = GetUrl
   };

   private readonly string[] _propNames = { "NombreEspecialidad" };
   private readonly string[] _tableHeaders = { "Nombre" };

   private List<EspecialidadResponseDto>? _especialidadList;
   private int _totalCount;

   [Inject]
   public IBaseListComponentOperations BaseListComponentOps { get; set; } =
     default!;

   protected override async Task OnInitializedAsync()
   {
      var result =
        await BaseListComponentOps
          .LoadItemListAsync<EspecialidadResponseDto>(GetUrl);

      if (result.Succeded)
      {
         _especialidadList = result.ItemList;
         _totalCount = result.TotalCount;
      }
   }

   private async Task DeleteEspecialidad(string id)
   {
      var result =
        await BaseListComponentOps.DeleteItem<EspecialidadResponseDto>(id,
          "api/Especialidad/eliminar", GetUrl);

      if (result.Succeded)
      {
         _especialidadList = result.ItemList;
         _totalCount = result.TotalCount;
      }
   }
}