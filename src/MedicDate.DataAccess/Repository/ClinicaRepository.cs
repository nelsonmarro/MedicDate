using MedicDate.DataAccess.Entities;
using MedicDate.DataAccess.Helpers;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Clinica;

namespace MedicDate.DataAccess.Repository;

public class ClinicaRepository : Repository<Clinica>, IClinicaRepository
{
    private readonly ApplicationDbContext _context;

    public ClinicaRepository(ApplicationDbContext context)
        : base(context)
    {
        _context = context;
    }

    public Task<OperationResult> UpdateClinicaAsync(string id, ClinicaRequestDto clinicaRequest)
    {
        var clinicaDb =
    }
}
