using AutoMapper;
using MedicDate.Bussines.Helpers;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Models.DTOs.Medico;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedicDate.API.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Especialidad, EspecialidadResponse>();
            CreateMap<EspecialidadRequest, Especialidad>().ReverseMap();

            CreateMap<Medico, MedicoResponse>()
                .ForMember(x => x.Especialidades, opts => opts.MapFrom(MapEspecialidades));
            CreateMap<MedicoRequest, Medico>()
                .ForMember(x => x.MedicosEspecialidades, opts => opts.MapFrom(MapMedicosEspecialidades));

            CreateMap<Medico, MedicoRequest>()
                .ForMember(x => x.EspecialidadesId, opts => opts.MapFrom(MapEspecialidadesIds));
        }

        private List<int> MapEspecialidadesIds(Medico medico, MedicoRequest medicoRequest)
        {
            var result = new List<int>();

            if (medico.MedicosEspecialidades is null)
            {
                return result;
            }

            result.AddRange(medico.MedicosEspecialidades.Select(x => x.EspecialidadId));

            return result;
        }

        private List<EspecialidadResponse> MapEspecialidades(Medico medico, MedicoResponse medicoResponse)
        {
            var result = new List<EspecialidadResponse>();

            if (medico.MedicosEspecialidades is null)
            {
                return result;
            }

            result.AddRange(medico.MedicosEspecialidades.Select(x => new EspecialidadResponse
            {
                Id = x.EspecialidadId,
                NombreEspecialidad = x.Especialidad.NombreEspecialidad
            }));

            return result;
        }

        private List<MedicoEspecialidad> MapMedicosEspecialidades(MedicoRequest medicoRequest, Medico medico)
        {
            var result = new List<MedicoEspecialidad>();

            if (medicoRequest.EspecialidadesId is null || medicoRequest.EspecialidadesId.Count == 0)
            {
                return result;
            }

            result.AddRange(medicoRequest.EspecialidadesId.Select(especialidadId => new MedicoEspecialidad
            {
                EspecialidadId = especialidadId
            }));

            return result;
        }
    }
}