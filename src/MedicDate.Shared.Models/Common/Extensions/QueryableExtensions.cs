﻿namespace MedicDate.Shared.Models.Common.Extensions;

public static class QueryableExtensions
{
  public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable,
    int pageIndex = 0,
    int pageSize = 10)
  {
    return queryable
      .Skip(pageIndex * pageSize)
      .Take(pageSize);
  }
}