using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Clinica;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Clinica;

public partial class ClinicaList : ComponentBase
{
    private const string GetUrl = "api/Clinica/listarConPaginacion";

    private readonly OpRoutes _opRoutes = new()
    {
        AddUrl = "clinicaCrear", EditUrl = "clinicaEditar",
        GetUrl = GetUrl
    };

    private readonly string[] _propNames = { "Nombre", "Ruc", "Direccion", "Telefono" };
    private readonly string[] _tableHeaders = { "Nombre", "Ruc", "Dirección", "Teléfono" };

    private List<ClinicaResponseDto>? _clinicaList;
    private int _totalCount;

    [Inject]
    public IBaseListComponentOperations BaseListComponentOps { get; set; }
    = default!;

    protected override async Task OnInitializedAsync()
    {
        var result =
          await BaseListComponentOps
            .LoadItemListAsync<ClinicaResponseDto>(GetUrl);

        if (result.Succeded)
        {
            _clinicaList = result.ItemList;
            _totalCount = result.TotalCount;
        }
    }

    private async Task DeleteClinica(string id)
    {
        var result =
          await BaseListComponentOps.DeleteItem<ClinicaResponseDto>(id,
            "api/Clinica/eliminar", GetUrl);

        if (result.Succeded)
        {
            _clinicaList = result.ItemList;
            _totalCount = result.TotalCount;
        }
    }
}
