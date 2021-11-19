using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Grupo;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class GrupoRepository : Repository<Grupo>, IGrupoRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IMapper _mapper;

  public GrupoRepository(ApplicationDbContext context, IMapper mapper) :
    base(context)
  {
    _context = context;
    _mapper = mapper;
  }

  public async Task<OperationResult> UpdateGrupoAsync(string id,
    GrupoRequestDto grupoRequestDto)
  {
    var grupoDb = await FindAsync(id);

    if (grupoDb is null)
      return OperationResult.Error(NotFound,
        "No se encontró el grupo para editar");

    _mapper.Map(grupoRequestDto, grupoDb);
    await _context.SaveChangesAsync();

    return OperationResult.Error(OK, "Grupo actualizado con éxito");
  }
}