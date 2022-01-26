using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Clinica;
using static System.Net.HttpStatusCode;

namespace MedicDate.DataAccess.Repository;

public class ClinicaRepository : Repository<Clinica>, IClinicaRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ClinicaRepository(ApplicationDbContext context, IMapper mapper)
        : base(context)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<OperationResult> UpdateClinicaAsync(string id, ClinicaRequestDto clinicaRequest)
    {
        var clinicaDb = await _context.Clinica.FindAsync(id);

        if (clinicaDb is null)
        {
            return OperationResult.Error(NotFound,
                "No se encotró la clínica requerida");
        }

        _mapper.Map(clinicaRequest, clinicaDb);
        await _context.SaveChangesAsync();

        return OperationResult.Success(OK,
            "Clínica editada con éxito");
    }
}
