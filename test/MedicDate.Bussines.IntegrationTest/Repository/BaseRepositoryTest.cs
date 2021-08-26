using AutoMapper;
using MedicDate.Bussines.Factories;
using MedicDate.Bussines.Factories.IFactories;
using MedicDate.Bussines.Repository.IRepository;
using MedicDate.Test.Shared;
using MedicDate.Utility.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MedicDate.Bussines.Repository
{
    public abstract class BaseRepositoryTest<TEntity> : BaseDbTest
        where TEntity : class, IId
    {
        protected IRepository<TEntity> sut;
        protected readonly IMapper mapper;
        protected readonly string dbName;
        protected IEnumerable<TEntity> entityList;
        protected IApiOperationResultFactory apiOpResultFactory;
        protected TEntity toAddEntity;

        public BaseRepositoryTest()
        {
            mapper = BuildMapper();
            dbName = Guid.NewGuid().ToString();
            apiOpResultFactory = new ApiOperationResultFactory();
        }

        [Fact]
        public async Task FindAsync_should_return_one_record()
        {
            var result = await sut.FindAsync(entityList.Select(x => x.Id).First());

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetAllAsync_should_return_all_records()
        {
            var result = await sut.GetAllAsync();

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task GetAllWithPagingAsync_should_return_all_records_properly_paginated()
        {
            var result = await sut.GetAllWithPagingAsync(pageIndex: 0, pageSize: 2);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FirstOrDefaultAsync_should_return_one_record_based_on_a_filter()
        {
            var entityId = entityList.Select(x => x.Id).First();

            var result = await sut.FirstOrDefaultAsync(x => x.Id == entityId);

            Assert.Equal(result.Id, entityId);
        }

        [Fact]
        public async Task AddAsync_should_create_a_record_in_db()
        {
            await sut.AddAsync(toAddEntity);
            await sut.SaveAsync();

            var context = BuildDbContext(dbName);
            var dbSet = context.Set<TEntity>();

            var result = await dbSet.FindAsync(toAddEntity.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task RemoveAsync_should_delete_a_record_with_the_passed_Id()
        {
            var entityId = entityList.Select(x => x.Id).First();

            var result = await sut.RemoveAsync(entityId);
            await sut.SaveAsync();

            Assert.Equal(1, result);

            var context = BuildDbContext(dbName);
            var dbSet = context.Set<TEntity>();
            var entity = await dbSet.FindAsync(entityId);

            Assert.Null(entity);
        }
    }
}
