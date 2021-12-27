using MedicDate.Client.Components;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Grupo;
using MedicDate.Shared.Models.Paciente;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace MedicDate.Client.Pages.Paciente;

public partial class PacienteList
{
   private const string GetUrl =
     "api/Paciente/listarConPaginacion?traerGrupos=true";

   private readonly RenderFragment<object> _especialidadesDialogContent = obj =>
     builder =>
     {
        var grupo = (GrupoResponseDto) obj;

        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "mx-3 my-2");
        builder.AddContent(3, grupo.Nombre);
        builder.CloseElement();
     };

   private readonly string[] _headers =
   {
    "Nombres", "Apellidos", "Edad", "Sexo", "Cédula", "Email",
    "Teléfono", "Dirección", "Num. Historia"
  };

   private readonly OpRoutes _opRoutes = new()
   { AddUrl = "pacienteCrear", EditUrl = "pacienteEditar", GetUrl = GetUrl };

   private readonly string[] _propNames =
   {
    "Nombres", "Apellidos", "Edad", "Sexo", "Cedula", "Email",
    "Telefono", "Direccion", "NumHistoria"
  };

   private List<GrupoResponseDto> _grupoList = new();

   private List<PacienteResponseDto>? _pacienteList;
   private int _totalCount;

   [Inject]
   public IBaseListComponentOperations BaseListComponentOps { get; set; } =
     default!;

   [Inject] public IHttpRepository HttpRepo { get; set; } = default!;

   [Inject] public DialogService DialogService { get; set; } = default!;

   protected override async Task OnInitializedAsync()
   {
      var result = await BaseListComponentOps
        .LoadItemListAsync<PacienteResponseDto>(GetUrl);

      if (result.Succeded)
      {
         _pacienteList = result.ItemList;
         _totalCount = result.TotalCount;
      }

      var httpResponse =
        await HttpRepo.Get<List<GrupoResponseDto>>("api/Grupo/listar");

      if (!httpResponse.Error)
         if (httpResponse.Response is not null)
            _grupoList = httpResponse.Response;
   }

   private async Task DeleteMedico(string id)
   {
      var result =
        await BaseListComponentOps.DeleteItemAndLoadDataList<PacienteResponseDto>(id,
          "api/Paciente/eliminar", GetUrl);

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
        .LoadItemListAsync<PacienteResponseDto>(GetUrl, "&filtrarGrupoId=",
          grupoId);

      if (result.Succeded)
      {
         _pacienteList = result.ItemList;
         _totalCount = result.TotalCount;
      }
   }

   private async Task OpenGruposDialog()
   {
      await DialogService.OpenAsync<RadzenGenericDialog>
      ("",
        new Dictionary<string, object>
        {
        {"Heading", "Grupos"},
        {"ItemList", _grupoList.ToArray()},
        {"ListBodyContent", _especialidadesDialogContent}
        });
   }
}