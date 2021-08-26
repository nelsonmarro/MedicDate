using AutoMapper;
using MedicDate.API.DTOs.Actividad;
using MedicDate.API.DTOs.AppRole;
using MedicDate.API.DTOs.AppUser;
using MedicDate.API.DTOs.Archivo;
using MedicDate.API.DTOs.Cita;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.API.DTOs.Grupo;
using MedicDate.API.DTOs.Medico;
using MedicDate.API.DTOs.Paciente;
using MedicDate.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MedicDate.Bussines.Mapper
{
	public class MapperProfile : Profile
	{
		public MapperProfile()
		{
			CreateMap<Especialidad, EspecialidadResponseDto>();
			CreateMap<EspecialidadRequestDto, Especialidad>().ReverseMap();

			CreateMap<Grupo, GrupoResponseDto>();
			CreateMap<Grupo, GrupoRequestDto>().ReverseMap();

			CreateMap<Actividad, ActividadResponseDto>();
			CreateMap<Actividad, ActividadRequestDto>().ReverseMap();

			CreateMap<Paciente, PacienteRequestDto>()
				.ForMember(x => x.GruposId, opts => opts.MapFrom(MapGruposIds));

			CreateMap<PacienteRequestDto, Paciente>()
				.ForMember(x => x.GruposPacientes, opts => opts.MapFrom(MapGruposPacientes));

			CreateMap<Paciente, PacienteResponseDto>()
				.ForMember(x => x.Grupos, opts => opts.MapFrom(MapGrupos));

			CreateMap<Medico, MedicoResponseDto>()
				.ForMember(x => x.Especialidades, opts => opts.MapFrom(MapEspecialidades));
			CreateMap<MedicoRequestDto, Medico>()
				.ForMember(x => x.MedicosEspecialidades, opts => opts.MapFrom(MapMedicosEspecialidades));

			CreateMap<Medico, MedicoRequestDto>()
				.ForMember(x => x.EspecialidadesId, opts => opts.MapFrom(MapEspecialidadesIds));

			CreateMap<ApplicationUser, AppUserResponseDto>()
				.ForMember(x => x.Roles, opts => opts.MapFrom(MapRolesResponse));

			CreateMap<AppUserRequestDto, ApplicationUser>()
				.ForMember(x => x.UserRoles, opts =>
					opts.MapFrom(MapUserRoles));

			CreateMap<ApplicationUser, AppUserRequestDto>()
				.ForMember(x => x.Roles, opts => opts.MapFrom(MapRolesRequest));

			CreateMap<Paciente, PacienteCitaResponseDto>();
			CreateMap<Medico, MedicoCitaResponseDto>();
			CreateMap<Archivo, ArchivoResponseDto>();
			CreateMap<ArchivoRequestDto, Archivo>().ReverseMap();

			CreateMap<Cita, CitaResponseDto>()
				.ForMember(x => x.ActividadesCita, opts => opts.MapFrom(MapActividadesCitaRes));

			CreateMap<Cita, CitaRequestDto>()
				.ForMember(x => x.ActividadesCita, opts => opts.MapFrom(MapActividadesCitaReq));

			CreateMap<CitaRequestDto, Cita>()
				.ForMember(x => x.ActividadesCita, opts => opts.MapFrom(MapActividadesCitaCreate));

		}

		private List<ActividadCita> MapActividadesCitaCreate(CitaRequestDto citaRequestDto, Cita cita)
		{
			var result = new List<ActividadCita>();

			if (citaRequestDto.ActividadesCita is null)
			{
				return result;
			}

			result.AddRange(citaRequestDto.ActividadesCita.Select(x => new ActividadCita
			{
				ActividadId = x.ActividadId,
				ActividadTerminada = x.ActividadTerminada,
				Detalles = x.Detalles
			}));

			return result;
		}

		private List<ActividadCitaDto> MapActividadesCitaRes(Cita cita, CitaResponseDto citaResponseDto)
		{
			return MapActividadesCita(cita, citaResponseDto);
		}

		private List<ActividadCitaDto> MapActividadesCitaReq(Cita cita, CitaRequestDto citaRequestDto)
		{
			return MapActividadesCita(cita, citaRequestDto);
		}

		private List<ActividadCitaDto> MapActividadesCita<T>(Cita cita, T citaDto)
		{
			var result = new List<ActividadCitaDto>();

			if (cita.ActividadesCita is null)
			{
				return result;
			}

			result.AddRange(cita.ActividadesCita.Select(x => new ActividadCitaDto
			{
				ActividadId = x.ActividadId,
				ActividadTerminada = x.ActividadTerminada,
				Detalles = x.Detalles
			}));

			return result;
		}

		private List<RoleResponseDto> MapRolesRequest(ApplicationUser applicationUser,
			AppUserRequestDto appUserRequestDto)
		{
			var result = new List<RoleResponseDto>();

			if (applicationUser.UserRoles is null)
			{
				return result;
			}

			result.AddRange(applicationUser.UserRoles.Select(x =>
				new RoleResponseDto
				{
					Id = x.RoleId,
					Nombre = x.Role.Name,
					Descripcion = x.Role.Descripcion
				}));

			return result;
		}

		private List<ApplicationUserRole> MapUserRoles(AppUserRequestDto appUserRequestDto,
			ApplicationUser applicationUser)
		{
			var result = new List<ApplicationUserRole>();

			if (appUserRequestDto.Roles is null) return result;

			result.AddRange(appUserRequestDto.Roles.Select(x => new ApplicationUserRole
			{
				RoleId = x.Id
			}));

			return result;
		}

		private List<RoleResponseDto> MapRolesResponse(ApplicationUser applicationUser,
			AppUserResponseDto appUserResponseDto)
		{
			var result = new List<RoleResponseDto>();

			if (applicationUser.UserRoles is null)
			{
				return result;
			}

			result.AddRange(applicationUser.UserRoles.Select(x =>
				new RoleResponseDto
				{
					Id = x.RoleId,
					Nombre = x.Role.Name,
					Descripcion = x.Role.Descripcion
				}));

			return result;
		}

		private List<GrupoResponseDto> MapGrupos(Paciente paciente, PacienteResponseDto pacienteResponseDto)
		{
			var result = new List<GrupoResponseDto>();

			if (paciente.GruposPacientes is null ||
				paciente.GruposPacientes.Any(x => x.Grupo == null))
			{
				return result;
			}

			result.AddRange(paciente.GruposPacientes.Select(x => new GrupoResponseDto
			{
				Id = x.GrupoId,
				Nombre = x.Grupo.Nombre
			}));

			return result;
		}

		private List<GrupoPaciente> MapGruposPacientes(PacienteRequestDto pacienteRequestDto, Paciente paciente)
		{
			var result = new List<GrupoPaciente>();

			if (pacienteRequestDto.GruposId is null || pacienteRequestDto.GruposId.Count == 0) return result;

			result.AddRange(pacienteRequestDto.GruposId.Select(grupoId => new GrupoPaciente
			{
				GrupoId = grupoId
			}));

			return result;
		}

		private List<string> MapGruposIds(Paciente paciente, PacienteRequestDto pacienteRequestDto)
		{
			var result = new List<string>();

			if (paciente.GruposPacientes is null)
			{
				return result;
			}

			result.AddRange(paciente.GruposPacientes.Select(x => x.GrupoId));

			return result;
		}

		private List<string> MapEspecialidadesIds(Medico medico, MedicoRequestDto medicoRequestDto)
		{
			var result = new List<string>();

			if (medico.MedicosEspecialidades is null)
			{
				return result;
			}

			result.AddRange(medico.MedicosEspecialidades.Select(x => x.EspecialidadId));

			return result;
		}

		private List<EspecialidadResponseDto> MapEspecialidades(Medico medico, MedicoResponseDto medicoResponseDto)
		{
			var result = new List<EspecialidadResponseDto>();

			if (medico.MedicosEspecialidades is null ||
				medico.MedicosEspecialidades.Any(x => x.Especialidad == null))
			{
				return result;
			}

			result.AddRange(medico.MedicosEspecialidades.Select(x => new EspecialidadResponseDto
			{
				Id = x.EspecialidadId,
				NombreEspecialidad = x.Especialidad.NombreEspecialidad
			}));

			return result;
		}

		private List<MedicoEspecialidad> MapMedicosEspecialidades(MedicoRequestDto medicoRequestDto, Medico medico)
		{
			var result = new List<MedicoEspecialidad>();

			if (medicoRequestDto.EspecialidadesId is null || medicoRequestDto.EspecialidadesId.Count == 0)
				return result;

			result.AddRange(medicoRequestDto.EspecialidadesId.Select(especialidadId => new MedicoEspecialidad
			{
				EspecialidadId = especialidadId
			}));

			return result;
		}
	}
}