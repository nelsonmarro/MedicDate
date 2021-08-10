using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MedicDate.DataAccess.Models;
using MedicDate.Models.DTOs.Actividad;
using MedicDate.Models.DTOs.AppUser;
using MedicDate.Models.DTOs.Auth;
using MedicDate.Models.DTOs.Especialidad;
using MedicDate.Models.DTOs.Grupo;
using MedicDate.Models.DTOs.Medico;
using MedicDate.Models.DTOs.Paciente;

namespace MedicDate.Bussines.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Especialidad, EspecialidadResponse>();
            CreateMap<EspecialidadRequest, Especialidad>().ReverseMap();

            CreateMap<Grupo, GrupoResponse>();
            CreateMap<Grupo, GrupoRequest>().ReverseMap();

            CreateMap<Actividad, ActividadResponse>();
            CreateMap<Actividad, ActividadRequest>().ReverseMap();

            CreateMap<Paciente, PacienteRequest>()
                .ForMember(x => x.GruposId, opts => opts.MapFrom(MapGruposIds));

            CreateMap<PacienteRequest, Paciente>()
                .ForMember(x => x.GruposPacientes, opts => opts.MapFrom(MapGruposPacientes));

            CreateMap<Paciente, PacienteResponse>()
                .ForMember(x => x.Grupos, opts => opts.MapFrom(MapGrupos));

            CreateMap<Medico, MedicoResponse>()
                .ForMember(x => x.Especialidades, opts => opts.MapFrom(MapEspecialidades));
            CreateMap<MedicoRequest, Medico>()
                .ForMember(x => x.MedicosEspecialidades, opts => opts.MapFrom(MapMedicosEspecialidades));

            CreateMap<Medico, MedicoRequest>()
                .ForMember(x => x.EspecialidadesId, opts => opts.MapFrom(MapEspecialidadesIds));

            CreateMap<ApplicationUser, AppUserResponse>()
                .ForMember(x => x.Roles, opts => opts.MapFrom(MapRolesResponse));

            CreateMap<AppUserRequest, ApplicationUser>()
                .ForMember(x => x.UserRoles, opts =>
                    opts.MapFrom(MapUserRoles));

            CreateMap<ApplicationUser, AppUserRequest>()
                .ForMember(x => x.Roles, opts => opts.MapFrom(MapRolesRequest));
        }

        private List<RoleResponse> MapRolesRequest(ApplicationUser applicationUser,
            AppUserRequest appUserRequest)
        {
            var result = new List<RoleResponse>();

            if (applicationUser.UserRoles is null)
            {
                return result;
            }

            result.AddRange(applicationUser.UserRoles.Select(x =>
                new RoleResponse
                {
                    Id = x.RoleId,
                    Nombre = x.Role.Name,
                    Descripcion = x.Role.Descripcion
                }));

            return result;
        }

        private List<ApplicationUserRole> MapUserRoles(AppUserRequest appUserRequest,
            ApplicationUser applicationUser)
        {
            var result = new List<ApplicationUserRole>();

            if (appUserRequest.Roles is null)
            {
                return result;
            }

            result.AddRange(appUserRequest.Roles.Select(x => new ApplicationUserRole
            {
                RoleId = x.Id
            }));

            return result;
        }

        private List<RoleResponse> MapRolesResponse(ApplicationUser applicationUser,
            AppUserResponse appUserResponse)
        {
            var result = new List<RoleResponse>();

            if (applicationUser.UserRoles is null)
            {
                return result;
            }

            result.AddRange(applicationUser.UserRoles.Select(x =>
                new RoleResponse
                {
                    Id = x.RoleId,
                    Nombre = x.Role.Name,
                    Descripcion = x.Role.Descripcion
                }));

            return result;
        }

        private List<GrupoResponse> MapGrupos(Paciente paciente, PacienteResponse pacienteResponse)
        {
            var result = new List<GrupoResponse>();

            if (paciente.GruposPacientes is null ||
                paciente.GruposPacientes.Any(x => x.Grupo == null))
            {
                return result;
            }

            result.AddRange(paciente.GruposPacientes.Select(x => new GrupoResponse
            {
                Id = x.GrupoId,
                Nombre = x.Grupo.Nombre
            }));

            return result;
        }

        private List<GrupoPaciente> MapGruposPacientes(PacienteRequest pacienteRequest, Paciente paciente)
        {
            var result = new List<GrupoPaciente>();

            if (pacienteRequest.GruposId is null || pacienteRequest.GruposId.Count == 0)
            {
                return result;
            }

            result.AddRange(pacienteRequest.GruposId.Select(grupoId => new GrupoPaciente
            {
                GrupoId = grupoId
            }));

            return result;
        }

        private List<string> MapGruposIds(Paciente paciente, PacienteRequest pacienteRequest)
        {
            var result = new List<string>();

            if (paciente.GruposPacientes is null)
            {
                return result;
            }

            result.AddRange(paciente.GruposPacientes.Select(x => x.GrupoId));

            return result;
        }

        private List<string> MapEspecialidadesIds(Medico medico, MedicoRequest medicoRequest)
        {
            var result = new List<string>();

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

            if (medico.MedicosEspecialidades is null ||
                medico.MedicosEspecialidades.Any(x => x.Especialidad == null))
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