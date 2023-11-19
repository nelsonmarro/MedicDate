using AutoMapper;
using MedicDate.DataAccess.Repository.IRepository;
using MedicDate.Shared.Models.Common.Interfaces;
using MedicDate.Test.Shared;
using MedicDate.Utility.Interfaces;
using Xunit;

namespace MedicDate.Bussiness.Repository;

public abstract class BaseRepositoryTest<TEntity> : BaseDbTest
  where TEntity : class, IId, new()
{
  protected readonly string DbName = Guid.NewGuid().ToString();
  protected readonly IMapper Mapper;
  protected IRepository<TEntity>? BaseSut;
  protected List<TEntity> EntityList = new();
  protected TEntity ToAddEntity = new();

  public BaseRepositoryTest()
  {
    Mapper = BuildMapper();
  }

  [Fact]
  public async Task FindAsync_should_return_one_record()
  {
    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    var result = await BaseSut.FindAsync(EntityList.Select(x => x.Id).First());

    Assert.NotNull(result);
  }

  [Fact]
  public async Task GetAllAsync_should_return_all_records()
  {
    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    var result = await BaseSut.GetAllAsync();

    Assert.Equal(3, result.Count);
  }

  [Fact]
  public async Task GetAllWithPagingAsync_should_return_all_records_properly_paginated()
  {
    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    var result = await BaseSut.GetAllWithPagingAsync(pageIndex: 0, pageSize: 2);

    Assert.Equal(2, result.Count);
  }

  [Fact]
  public async Task FirstOrDefaultAsync_should_return_one_record_based_on_a_filter()
  {
    var entityId = EntityList.Select(x => x.Id).First();

    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    var result = await BaseSut.FirstOrDefaultAsync(x => x.Id == entityId);

    Assert.Equal(result?.Id, entityId);
  }

  [Fact]
  public async Task AddAsync_should_create_a_record_in_db()
  {
    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    await BaseSut.AddAsync(ToAddEntity);
    await BaseSut.SaveAsync();

    var context = BuildDbContext(DbName);
    var dbSet = context.Set<TEntity>();

    var result = await dbSet.FindAsync(ToAddEntity.Id);

    Assert.NotNull(result);
  }

  [Fact]
  public async Task RemoveAsync_should_delete_a_record_with_the_passed_Id()
  {
    var entityId = EntityList.Select(x => x.Id).First();

    if (BaseSut is null)
      throw new ArgumentNullException(nameof(BaseSut));

    var result = await BaseSut.RemoveAsync(entityId);
    await BaseSut.SaveAsync();

    Assert.Equal(1, result);

    var context = BuildDbContext(DbName);
    var dbSet = context.Set<TEntity>();
    var entity = await dbSet.FindAsync(entityId);

    Assert.Null(entity);
  }
}
