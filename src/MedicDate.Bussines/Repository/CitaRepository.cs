using AutoMapper;
using MedicDate.API.DTOs.Cita;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess;
using MedicDate.DataAccess.Entities;
using System;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Repository
{
    public class CitaRepository : Repository<Cita>, ICitaRepository
    {
        private readonly IMapper _mapper;
        private readonly IApiOperationResultFactory _apiOpResultFactory;

        public CitaRepository(ApplicationDbContext context, IMapper mapper, IApiOperationResultFactory apiOpResultFactory) : base(context)
        {
            _mapper = mapper;
            _apiOpResultFactory = apiOpResultFactory;
        }

        public Task<ApiOperationResult> UpdateCitaAsync(string citaId, CitaRequestDto citaRequestDto)
        {
            throw new NotImplementedException();
        }
    }
}
