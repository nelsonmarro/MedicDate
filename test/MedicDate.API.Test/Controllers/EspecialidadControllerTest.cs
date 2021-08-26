﻿using AutoMapper;
using MedicDate.API.DTOs.Especialidad;
using MedicDate.Bussines.Factories;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Helpers;
using MedicDate.Bussines.Repository;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.DataAccess.Entities;
using MedicDate.Test.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MedicDate.API.Controllers
{
	public class EspecialidadControllerTest : BaseDbTest
	{
		private readonly IMapper _mapper;
		private IEspecialidadRepository _especialidadRepo;
		private EspecialidadController _sut;
		private readonly IApiOperationResultFactory _apiOpResultFactory;

		private readonly List<Especialidad> _tempEspecialidadList = new()
		{
			new Especialidad { NombreEspecialidad = "Esp 3" },
			new Especialidad { NombreEspecialidad = "Esp 1" },
			new Especialidad { NombreEspecialidad = "Esp 4" },
			new Especialidad { NombreEspecialidad = "Esp 2" }
		};

		protected EspecialidadControllerTest()
		{
			_mapper = BuildMapper();
			_apiOpResultFactory = new ApiOperationResultFactory();
		}

		public class GetAllEspecialidadesWithPagingAsync : EspecialidadControllerTest
		{
			[Theory]
			[InlineData(2)]
			public async Task Should_return_all_specialities_properly_paginated(int pageSize)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				context_1.Especialidad.AddRange(_tempEspecialidadList);
				await context_1.SaveChangesAsync();

				var tempContext = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(tempContext, _mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.GetAllEspecialidadesWithPagingAsync(0, pageSize);

				//Assert

				//Check correct count of total pages
				Assert.Equal(2, result.Value.TotalPages);

				//Check correct count of specialityes
				Assert.Equal(4, result.Value.TotalCount);
			}
		}

		public class GetAllEspecialidadesAsync : EspecialidadControllerTest
		{
			[Fact]
			public async Task Should_return_all_specialities_in_db()
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				context_1.Especialidad.AddRange(_tempEspecialidadList);
				await context_1.SaveChangesAsync();

				var context_2 = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context_2, _mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.GetAllEspecialidadesAsync();

				//Assert

				Assert.Equal(4, result.Value.Count);
			}

			[Fact]
			public async Task Should_return_all_specialities_sorted_in_ascending_order()
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				context_1.Especialidad.AddRange(_tempEspecialidadList);
				await context_1.SaveChangesAsync();

				var context_2 = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context_2, _mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.GetAllEspecialidadesAsync();

				//Assert
				Assert.Collection(result.Value,
					esp => Assert.Equal("Esp 1", esp.NombreEspecialidad),
					esp => Assert.Equal("Esp 2", esp.NombreEspecialidad),
					esp => Assert.Equal("Esp 3", esp.NombreEspecialidad),
					esp => Assert.Equal("Esp 4", esp.NombreEspecialidad)
				);
			}
		}

		public class GetPutEspecialidadAsync : EspecialidadControllerTest
		{
			private readonly Especialidad _tempEspecialidad = new()
			{ NombreEspecialidad = "Esp 1" };

			[Theory]
			[InlineData("invalidId")]
			public async Task Should_response_404_if_no_speciality_with_the_given_Id_exists(string invalidId)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context,
					_mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);
				var result = await _sut.GetPutEspecialidadAsync(invalidId);

				var notFoundResult = result.Result as NotFoundObjectResult;

				//Assert
				Assert.NotNull(notFoundResult);
			}

			[Fact]
			public async Task Should_return_the_speciality_with_the_given_id()
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				await context_1.Especialidad.AddAsync(_tempEspecialidad);
				await context_1.SaveChangesAsync();

				var validEspecialidadId = await context_1.Especialidad.Select(x => x.Id)
					.FirstOrDefaultAsync();

				var context_2 = BuildDbContext(dbName);
				_especialidadRepo = new EspecialidadRepository(context_2, _mapper, _apiOpResultFactory);
				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.GetPutEspecialidadAsync(validEspecialidadId);

				//Assert
				Assert.Equal(_tempEspecialidad.NombreEspecialidad, result.Value.NombreEspecialidad);
			}
		}

		public class GetEspecialidadAsync : EspecialidadControllerTest
		{
			private readonly Especialidad _tempEspecialidad = new()
			{ NombreEspecialidad = "Esp 1" };

			[Theory]
			[InlineData("invalidId")]
			public async Task Should_response_404_if_no_speciality_with_the_given_Id_exists(string invalidId)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context,
					_mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);
				var result = await _sut.GetEspecialidadAsync(invalidId);

				var notFoundResult = result.Result as NotFoundObjectResult;

				//Assert
				Assert.NotNull(notFoundResult);
			}

			[Fact]
			public async Task Should_return_the_speciality_with_the_given_id()
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				await context_1.Especialidad.AddAsync(_tempEspecialidad);
				await context_1.SaveChangesAsync();

				var validEspecialidadId = await context_1.Especialidad.Select(x => x.Id)
					.FirstOrDefaultAsync();

				var context_2 = BuildDbContext(dbName);
				_especialidadRepo = new EspecialidadRepository(context_2, _mapper, _apiOpResultFactory);
				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.GetEspecialidadAsync(validEspecialidadId);

				//Assert
				Assert.Equal(validEspecialidadId, result.Value.Id);
			}
		}

		public class CreateEspecialidadAsync : EspecialidadControllerTest
		{
			public static TheoryData<EspecialidadRequestDto> _newEspecialidad = new()
			{
				{ new EspecialidadRequestDto { NombreEspecialidad = "Esp 1" } }
			};

			[Theory]
			[MemberData(nameof(_newEspecialidad))]
			public async Task Should_create_a_speciality_in_db(EspecialidadRequestDto newEspecialidad)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context_1,
					_mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);
				var result = await _sut.CreateEspecialidadAsync(newEspecialidad);

				var createdResult = result as CreatedAtRouteResult;

				//Assert
				Assert.NotNull(createdResult);

				var context_2 = BuildDbContext(dbName);
				var cantidad = await context_2.Especialidad.CountAsync();

				Assert.Equal(1, cantidad);
			}
		}

		public class UpdateEspecialidadAsync : EspecialidadControllerTest
		{
			private readonly Especialidad _tempEspecialidad = new()
			{
				NombreEspecialidad = "Esp 1"
			};

			public static TheoryData<EspecialidadRequestDto> _specialityForUpdate = new()
			{
				{ new EspecialidadRequestDto { NombreEspecialidad = "Esp-123" } }
			};

			[Theory]
			[MemberData(nameof(_specialityForUpdate))]
			public async Task Should_update_a_speciality_with_the_given_Id(EspecialidadRequestDto specialityRequest)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				await context_1.Especialidad.AddAsync(_tempEspecialidad);
				await context_1.SaveChangesAsync();

				var context_2 = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context_2,
					_mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.UpdateEspecialidadAsync(_tempEspecialidad.Id, specialityRequest);

				if (result is not GenericActionResult successResult)
					throw new NullReferenceException(nameof(successResult));

				//Assert
				Assert.Equal(HttpStatusCode.OK, successResult.HttpStatusCode);

				var context_3 = BuildDbContext(dbName);
				var updatedEspExists = await context_3.Especialidad
					.AnyAsync(x => x.NombreEspecialidad == specialityRequest.NombreEspecialidad);

				Assert.True(updatedEspExists, "La speciality debió ser actualizada");
			}
		}

		public class DeleteEspecialidadAsync : EspecialidadControllerTest
		{
			private readonly Especialidad _tempEspecialidad = new()
			{
				NombreEspecialidad = "Esp 1"
			};

			[Theory]
			[InlineData("invalidId")]
			public async Task Should_return_404_when_trying_deleting_an_nonExistent_speciality(string specialityId)
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				_especialidadRepo = new EspecialidadRepository(context_1, _mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);

				var result = await _sut.DeleteEspecialidadAsync(specialityId);
				var notFoundResult = result as NotFoundObjectResult;

				//Assert
				Assert.NotNull(notFoundResult);
			}

			[Fact]
			public async Task Should_delete_a_speciality_with_the_given_Id()
			{
				//Arrange
				var dbName = Guid.NewGuid().ToString();
				var context_1 = BuildDbContext(dbName);

				await context_1.Especialidad.AddAsync(_tempEspecialidad);
				await context_1.SaveChangesAsync();

				var context_2 = BuildDbContext(dbName);
				_especialidadRepo = new EspecialidadRepository(context_2, _mapper, _apiOpResultFactory);

				//Act
				_sut = new EspecialidadController(_especialidadRepo, _mapper);
				var result = await _sut.DeleteEspecialidadAsync(_tempEspecialidad.Id);

				var okResult = result as OkObjectResult;

				//Assert
				Assert.NotNull(okResult);

				var context_3 = BuildDbContext(dbName);
				var especialidadesCount = await context_3.Especialidad.CountAsync();

				Assert.Equal(0, especialidadesCount);
			}
		}
	}
}