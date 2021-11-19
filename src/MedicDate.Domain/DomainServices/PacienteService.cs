using System.Net;
using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Paciente;
using MedicDate.Utility;

namespace MedicDate.Bussines.DomainServices;

public class PacienteService : IPacienteService
{
   private readonly IEntityValidator _entityValidator;
   private readonly IPacienteRepository _pacienteRepo;

   public PacienteService(IEntityValidator entityValidator, IPacienteRepository pacienteRepo)
   {
      _entityValidator = entityValidator;
      _pacienteRepo = pacienteRepo;
   }

   public async Task<bool> ValidateNumHistoriaForCreateAsync(
    string numHistoria)
   {
      return await _entityValidator.CheckValueExistsAsync<Paciente>(
        "NumHistoria", numHistoria);
   }

   public async Task<bool> ValidatCedulaForCreateAsync(string numeroCedula)
   {
      return await _entityValidator.CheckValueExistsAsync<Paciente>(
        "Cedula", numeroCedula);
   }

   public async Task<bool> ValidateCedulaForEditAsync(string numCedula
     , string id)
   {
      return await
        _entityValidator.CheckValueExistsForEditAsync<Paciente>("Cedula"
          , numCedula, id);
   }

   public async Task<bool> ValidateNumHistoriaForEditAsync(
     string numHistoria, string id)
   {
      return await
        _entityValidator.CheckValueExistsForEditAsync<Paciente>(
          "NumHistoria"
          , numHistoria, id);
   }

   public async Task<OperationResult> ValidatePacienteForCreate(
     string numHistoria, string numCedula)
   {
      if (await ValidatCedulaForCreateAsync(numCedula))
         return OperationResult.Error(HttpStatusCode.BadRequest
           , "Ya existe otro paciente registrado con la cédula ingresada");

      if (await ValidateNumHistoriaForCreateAsync(numHistoria))
         return OperationResult.Error(HttpStatusCode.BadRequest
           , "Ya existe otro paciente registrado con el número de historia ingresado");

      return OperationResult.Success();
   }

   public async Task<OperationResult> ValidatePacienteForEdit(
     string numHistoria, string numCedula, string id)
   {
      if (await ValidateCedulaForEditAsync(numCedula, id))
         return OperationResult.Error(HttpStatusCode.BadRequest
           , "Ya existe otro paciente registrado con la cédula ingresada");

      if (await ValidateNumHistoriaForEditAsync(numHistoria, id))
         return OperationResult.Error(HttpStatusCode.BadRequest
           , "Ya existe otro paciente registrado con el número de historia ingresado");

      return OperationResult.Success();
   }

   public async Task<List<PacienteMonthReviewDto>>
      GetPacientesMonthRegisterationReview(int requestedYear)
   {
      return new List<PacienteMonthReviewDto>
      {
         new()
         {
            RegistrationDate = Sd.Januay,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.Januay && x.DateRegistered < Sd.February
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.February,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.February && x.DateRegistered < Sd.March
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.March,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.March && x.DateRegistered < Sd.April
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.April,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.April && x.DateRegistered < Sd.May
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.May,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.May && x.DateRegistered < Sd.June
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.June,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.June && x.DateRegistered < Sd.July
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.July,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.July && x.DateRegistered < Sd.August
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.August,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.August && x.DateRegistered < Sd.September
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.September,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.September && x.DateRegistered < Sd.October
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.October,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.October && x.DateRegistered < Sd.November
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.November,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.November && x.DateRegistered < Sd.December
            && x.DateRegistered.Year == requestedYear)
         },
         new()
         {
            RegistrationDate = Sd.December,
            TotalRegisteration = await _pacienteRepo.CountResourcesAsync(x =>
            x.DateRegistered >= Sd.December && x.DateRegistered.Year == requestedYear)
         },
      };
   }
}