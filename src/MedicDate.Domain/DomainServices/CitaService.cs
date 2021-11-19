using MedicDate.Bussines.ApplicationServices.IApplicationServices;
using MedicDate.Bussines.DomainServices.IDomainServices;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Cita;
using MedicDate.Utility;

namespace MedicDate.Bussines.DomainServices;

public class CitaService : ICitaService
{
   private readonly ICitaRepository _citaRepo;
   private readonly IEntityValidator _entityValidator;

   public CitaService(ICitaRepository citaRepo, IEntityValidator entityValidator)
   {
      _citaRepo = citaRepo;
      _entityValidator = entityValidator;
   }

   public async Task<List<CitaRegisteredQuarterReviewDto>> GetCitasAnualQuarterReview(int requestedYear)
   {
      return new List<CitaRegisteredQuarterReviewDto>
      {
         new()
         {
           Quarter = "T1",
           TotalCitas = await _citaRepo.CountResourcesAsync(x =>
           x.FechaInicio >= Sd.Januay && x.FechaInicio < Sd.April
           && x.FechaInicio.Year == requestedYear),
         },
         new()
         {
           Quarter = "T2",
           TotalCitas = await _citaRepo.CountResourcesAsync(x =>
           x.FechaInicio >= Sd.April && x.FechaInicio < Sd.July
           && x.FechaInicio.Year == requestedYear),
         },
         new()
         {
           Quarter = "T3",
           TotalCitas = await _citaRepo.CountResourcesAsync(x =>
           x.FechaInicio >= Sd.July && x.FechaInicio < Sd.October
           && x.FechaInicio.Year == requestedYear),
         },
         new()
         {
           Quarter = "T4",
           TotalCitas = await _citaRepo.CountResourcesAsync(x =>
           x.FechaInicio >= Sd.October
           && x.FechaInicio.Year == requestedYear),
         },
      };
   }

   public async Task<List<CitaEstadoMonthReviewDto>>
      GetCitasMonthReviewByEstado(string estadoName, int requestedYear)
   {
      return new List<CitaEstadoMonthReviewDto>
      {
               new()
               {
                  RegisterationDate = Sd.Januay,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.Januay && x.FechaInicio < Sd.February
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.February,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.February && x.FechaInicio < Sd.March
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.March,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.March && x.FechaInicio < Sd.April
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.April,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.April && x.FechaInicio < Sd.May
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.May,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.May && x.FechaInicio < Sd.June
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.June,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.June && x.FechaInicio < Sd.July
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.July,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.July && x.FechaInicio < Sd.August
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.August,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.August && x.FechaInicio < Sd.September
                  && x.FechaInicio.Year == requestedYear)
               },
               new()
               {
                  RegisterationDate = Sd.September,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.September && x.FechaInicio < Sd.October
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.October,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.October && x.FechaInicio < Sd.November
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.November,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.November && x.FechaInicio < Sd.December
                  && x.FechaInicio.Year == requestedYear && estadoName == x.Estado)
               },
               new()
               {
                  RegisterationDate = Sd.December,
                  NombreEstado = estadoName,
                  TotalCitas = await _citaRepo.CountResourcesAsync(x =>
                  x.FechaInicio >= Sd.December && x.FechaInicio.Year == requestedYear
                  && estadoName == x.Estado)
               },
      };
   }
}
