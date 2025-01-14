﻿using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Grupo;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Pages.Grupo
{
  public partial class GrupoCreate
  {
    [Inject] public IHttpRepository HttpRepo { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public INotificationService NotificationService { get; set; } = default!;

    private bool _isBussy;

    private readonly GrupoRequestDto _grupoModel = new();

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