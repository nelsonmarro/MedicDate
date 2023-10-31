using System.Linq.Dynamic.Core;
using MedicDate.DataAccess;
using MedicDate.Domain.ApplicationServices.IApplicationServices;
using MedicDate.Utility.Validators;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace MedicDate.Domain.ApplicationServices;

public class EntityValidator : IEntityValidator
{
  private readonly ApplicationDbContext _context;

  public EntityValidator(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<bool> CheckValueExistsAsync<TEntity>(
    string propName,
    object value) where TEntity : class
  {
    if (!PropertyValidator.IsValidProperty<TEntity>(propName)) return false;

    var dbSet = _context.Set<TEntity>();

    return value switch
    {
      string valueString => await dbSet.AnyAsync(
        $"{propName} == \"{valueString}\""),
      int valueInt => await dbSet.AnyAsync($"{propName} == {valueInt}"),
      _ => throw new
        ArgumentException("El parametro \"value\" solo puede ser int o string")
    };
  }

  public async Task<bool> CheckValueExistsForEditAsync<TEntity>(
    string propName,
    object value,
    string resourceId) where TEntity : class
  {
    if (!PropertyValidator.IsValidProperty<TEntity>(propName)) return false;

    var dbSet = _context.Set<TEntity>();

    return value switch
    {
      string valueString => await dbSet.AnyAsync(
        $"{propName} == \"{value}\" && Id != \"{resourceId}\""),
      int valueInt => await dbSet.AnyAsync(
        $"{propName} == {value} && Id != \"{resourceId}\""),
      _ => throw new
        ArgumentException("El parametro \"value\" solo puede ser int o string")
    };
  }

  public async Task<bool> CheckRelatedEntityIdsExists<TRelatedEntity>(
    List<string> entityIds) where TRelatedEntity : class
  {
    var dbSet = _context.Set<TRelatedEntity>();

    var entityIdsDb = await dbSet
      .Where("x => @0.Contains(x.Id)", entityIds)
      .Select("x => x.Id")
      .ToDynamicListAsync<string>();

    return entityIdsDb.Count == entityIds.Count;
  }
}