using AutoMapper;
using MedicDate.DataAccess.Entities;
using MedicDate.Shared.Models.Actividad;
using MedicDate.Shared.Models.AppRole;
using MedicDate.Shared.Models.AppUser;
using MedicDate.Shared.Models.Archivo;
using MedicDate.Shared.Models.Cita;
using MedicDate.Shared.Models.Especialidad;
using MedicDate.Shared.Models.Grupo;
using MedicDate.Shared.Models.Medico;
using MedicDate.Shared.Models.Paciente;

namespace MedicDate.DataAccess.Mapper;

public class MapperProfile : Profile
{
  public MapperProfile()
  {
    CreateMap<Especialidad, EspecialidadResponseDto>();
    CreateMap<EspecialidadRequestDto, Especialidad>()
      .ForMember(x => x.MedicosEspecialidades, opts => opts.Ignore())
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ReverseMap();

    CreateMap<Grupo, GrupoResponseDto>();
    CreateMap<Grupo, GrupoRequestDto>().ReverseMap();

    CreateMap<Actividad, ActividadResponseDto>();
    CreateMap<Actividad, ActividadRequestDto>().ReverseMap();

    CreateMap<Paciente, PacienteRequestDto>()
      .ForMember(x => x.GruposId, opts => opts.MapFrom(MapGruposIds));

    CreateMap<PacienteRequestDto, Paciente>()
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ForMember(x => x.Citas, opts => opts.Ignore())
      .ForMember(x => x.GruposPacientes,
        opts => opts.MapFrom(MapGruposPacientes));

    CreateMap<Paciente, PacienteResponseDto>()
      .ForMember(x => x.Grupos, opts => opts.MapFrom(MapGrupos));

    CreateMap<Paciente, PacienteCitaResponseDto>()
      .ForMember(x => x.FullInfo, opts => opts.Ignore());

    CreateMap<Medico, MedicoResponseDto>()
      .ForMember(x => x.Especialidades,
        opts => opts.MapFrom(MapEspecialidades));

    CreateMap<MedicoRequestDto, Medico>()
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ForMember(x => x.Citas, opts => opts.Ignore())
      .ForMember(x => x.MedicosEspecialidades,
        opts => opts.MapFrom(MapMedicosEspecialidades));

    CreateMap<Medico, MedicoRequestDto>()
      .ForMember(x => x.EspecialidadesId,
        opts => opts.MapFrom(MapEspecialidadesIds));

    CreateMap<Medico, MedicoCitaResponseDto>()
      .ForMember(x => x.FullInfo, opts => opts.Ignore());

    CreateMap<ApplicationUser, AppUserResponseDto>()
      .ForMember(x => x.Roles,
        opts => opts.MapFrom(MapRolesResponse));

    CreateMap<AppUserRequestDto, ApplicationUser>()
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ForMember(x => x.RefreshToken, opts => opts.Ignore())
      .ForMember(x => x.RefreshTokenExpiryTime, opts => opts.Ignore())
      .ForMember(x => x.UserName, opts => opts.Ignore())
      .ForMember(x => x.NormalizedUserName, opts => opts.Ignore())
      .ForMember(x => x.NormalizedEmail, opts => opts.Ignore())
      .ForMember(x => x.SecurityStamp, opts => opts.Ignore())
      .ForMember(x => x.ConcurrencyStamp, opts => opts.Ignore())
      .ForMember(x => x.PhoneNumberConfirmed, opts => opts.Ignore())
      .ForMember(x => x.TwoFactorEnabled, opts => opts.Ignore())
      .ForMember(x => x.LockoutEnd, opts => opts.Ignore())
      .ForMember(x => x.LockoutEnabled, opts => opts.Ignore())
      .ForMember(x => x.PasswordHash, opts => opts.Ignore())
      .ForMember(x => x.AccessFailedCount, opts => opts.Ignore())
      .ForMember(x => x.UserRoles, opts =>
        opts.MapFrom(MapUserRoles));

    CreateMap<ApplicationUser, AppUserRequestDto>()
      .ForMember(x => x.Roles, opts => opts.MapFrom(MapRolesRequest));

    CreateMap<Archivo, ArchivoResponseDto>();
    CreateMap<CreateArchivoRequestDto, Archivo>()
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ForMember(x => x.CitaId, opts => opts.Ignore())
      .ForMember(x => x.Cita, opts => opts.Ignore())
      .ReverseMap();

    CreateMap<Cita, CitaDetailsDto>()
      .ForMember(x => x.ActividadesCita,
        opts => opts.MapFrom(MapActividadesCitaDetails));

    CreateMap<Cita, CitaRequestDto>()
      .ForMember(x => x.ActividadesCita,
        opts => opts.MapFrom(MapActividadesCitaForEdit));

    CreateMap<CitaRequestDto, Cita>()
      .ForMember(x => x.Id, opts => opts.Ignore())
      .ForMember(x => x.Paciente, opts => opts.Ignore())
      .ForMember(x => x.Medico, opts => opts.Ignore())
      .ForMember(x => x.ActividadesCita,
        opts => opts.MapFrom(MapActividadesCitaForCreate));

    CreateMap<Cita, CitaCalendarDto>()
      .ForMember(x => x.InfoCita, opt => opt.Ignore());

    CreateMap<ArchivoDbSaveDto, Archivo>();
  }

  private List<ActividadCita> MapActividadesCitaForCreate(
    CitaRequestDto citaReqDto, Cita cita)
  {
    var result = new List<ActividadCita>();

    result.AddRange(citaReqDto.ActividadesCita.Select(x =>
       new ActividadCita
       {
          ActividadId = x.ActividadId ?? "",
          ActividadTerminada = x.ActividadTerminada,
          Detalles = x.Detalles,
          CitaId = cita.Id
       }));

    return result;
  }

  private List<ActividadCitaResponseDto> MapActividadesCitaDetails(
    Cita cita,
    CitaDetailsDto citaDetailsDto)
  {
    List<ActividadCitaResponseDto> result = new();

    if (cita.ActividadesCita is null ||
        cita.ActividadesCita.First().Actividad is null)
      return result;

    result.AddRange(cita.ActividadesCita.Select(x =>
      new ActividadCitaResponseDto
      {
        ActividadId = x.ActividadId,
        Nombre = x.Actividad.Nombre,
        ActividadTerminada = x.ActividadTerminada,
        Detalles = x.Detalles
      }));

    return result;
  }

  private List<ActividadCitaRequestDto> MapActividadesCitaForEdit(
    Cita cita, CitaRequestDto citaReqDto)
  {
    var result = new List<ActividadCitaRequestDto>();

    if (cita.ActividadesCita is null) return result;

    result.AddRange(cita.ActividadesCita.Select(x =>
      new ActividadCitaRequestDto
      {
        ActividadId = x.ActividadId,
        ActividadTerminada = x.ActividadTerminada,
        Detalles = x.Detalles
      }));

    return result;
  }

  private List<RoleResponseDto> MapRolesRequest(
    ApplicationUser applicationUser,
    AppUserRequestDto appUserRequestDto)
  {
    var result = new List<RoleResponseDto>();

    if (applicationUser.UserRoles is null) return result;

    result.AddRange(applicationUser.UserRoles.Select(x =>
      new RoleResponseDto
      {
        Id = x.RoleId,
        Nombre = x.Role.Name,
        Descripcion = x.Role.Descripcion
      }));

    return result;
  }

  private List<ApplicationUserRole> MapUserRoles(
    AppUserRequestDto appUserRequestDto,
    ApplicationUser applicationUser)
  {
    var result = new List<ApplicationUserRole>();

    if (appUserRequestDto.Roles is null) return result;

    result.AddRange(appUserRequestDto.Roles.Select(x =>
      new ApplicationUserRole
      {
        RoleId = x.Id
      }));

    return result;
  }

  private List<RoleResponseDto> MapRolesResponse(
    ApplicationUser applicationUser,
    AppUserResponseDto appUserResponseDto)
  {
    var result = new List<RoleResponseDto>();

    if (applicationUser.UserRoles is null) return result;

    result.AddRange(applicationUser.UserRoles.Select(x =>
      new RoleResponseDto
      {
        Id = x.RoleId,
        Nombre = x.Role.Name,
        Descripcion = x.Role.Descripcion
      }));

    return result;
  }

  private List<GrupoResponseDto> MapGrupos(Paciente paciente,
    PacienteResponseDto pacienteResponseDto)
  {
    var result = new List<GrupoResponseDto>();

    if (paciente.GruposPacientes is null ||
        paciente.GruposPacientes.Any(x => x.Grupo == null))
      return result;

    result.AddRange(paciente.GruposPacientes.Select(x =>
      new GrupoResponseDto
      {
        Id = x.GrupoId,
        Nombre = x.Grupo.Nombre
      }));

    return result;
  }

  private List<GrupoPaciente> MapGruposPacientes(
    PacienteRequestDto pacienteRequestDto, Paciente paciente)
  {
    var result = new List<GrupoPaciente>();

    if (pacienteRequestDto.GruposId is null ||
        pacienteRequestDto.GruposId.Count == 0) return result;

    result.AddRange(pacienteRequestDto.GruposId.Select(grupoId =>
      new GrupoPaciente {GrupoId = grupoId}));

    return result;
  }

  private List<string> MapGruposIds(Paciente paciente,
    PacienteRequestDto pacienteRequestDto)
  {
    var result = new List<string>();

    if (paciente.GruposPacientes is null) return result;

    result.AddRange(paciente.GruposPacientes.Select(x => x.GrupoId));

    return result;
  }

  private List<string> MapEspecialidadesIds(Medico medico,
    MedicoRequestDto medicoRequestDto)
  {
    var result = new List<string>();

    if (medico.MedicosEspecialidades is null) return result;

    result.AddRange(
      medico.MedicosEspecialidades.Select(x => x.EspecialidadId));

    return result;
  }

  private List<EspecialidadResponseDto> MapEspecialidades(Medico medico,
    MedicoResponseDto medicoResponseDto)
  {
    var result = new List<EspecialidadResponseDto>();

    if (medico.MedicosEspecialidades is null ||
        medico.MedicosEspecialidades.Any(x => x.Especialidad == null))
      return result;

    result.AddRange(medico.MedicosEspecialidades.Select(x =>
      new EspecialidadResponseDto
      {
        Id = x.EspecialidadId,
        NombreEspecialidad = x.Especialidad.NombreEspecialidad
      }));

    return result;
  }

  private List<MedicoEspecialidad> MapMedicosEspecialidades(
    MedicoRequestDto medicoRequestDto, Medico medico)
  {
    var result = new List<MedicoEspecialidad>();

    if (medicoRequestDto.EspecialidadesId is null ||
        medicoRequestDto.EspecialidadesId.Count == 0)
      return result;

    result.AddRange(medicoRequestDto.EspecialidadesId.Select(
      especialidadId => new MedicoEspecialidad
        {EspecialidadId = especialidadId}));

    return result;
  }
}