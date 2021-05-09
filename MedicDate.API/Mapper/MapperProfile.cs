using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;

namespace MedicDate.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Especialidad, EspecialidadResponse>();
            CreateMap<EspecialidadRequest, Especialidad>().ReverseMap();
            CreateMap<ApiResult<Especialidad, EspecialidadResponse>, ApiResponseDto<EspecialidadResponse>>();
        }
    }
}