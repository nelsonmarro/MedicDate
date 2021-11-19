using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Especialidad;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class EspecialidadRepository : Repository<Especialidad>,
  IEspecialidadRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;

  public EspecialidadRepository(ApplicationDbContext context,
    IMapper mapper) : base(context)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<OperationResult> UpdateEspecialidadAsync(string id,
    EspecialidadRequestDto especialidadDto)
  {
    var especialidadDb = await FindAsync(id);

    if (especialidadDb is null)
      return OperationResult.Error(NotFound,
        "No se encontró la especialidad");

    _mapper.Map(especialidadDto, especialidadDb);
    await _context.SaveChangesAsync();

    return OperationResult.Success(OK,
      "Especialidad actualizada con éxito");
  }
}