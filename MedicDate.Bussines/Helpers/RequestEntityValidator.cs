using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using MedicDate.DataAccess.Data;
using MedicDate.Utility;
using Microsoft.EntityFrameworkCore.DynamicLinq;

namespace MedicDate.Bussines.Helpers
{
    public class RequestEntityValidator<T> where T : class
    {
        public static async Task<bool> CheckValueExistsAsync(ApplicationDbContext context, string propName,
            object value)
        {
            if (!PropertyValidator.IsValidProperty<T>(propName)) return false;

            var dbSet = context.Set<T>();

            return value switch
            {
                string valueString => await dbSet.AnyAsync($"{propName} == \"{valueString}\""),
                int valueInt => await dbSet.AnyAsync($"{propName} == {valueInt}"),
                _ => throw new
                    ArgumentException("El parametro \"value\" solo puede ser int o string")
            };
        }

        public static async Task<bool> CheckValueExistsForEditAsync(
            ApplicationDbContext context, string propName,
            object value, string resourceId)
        {
            if (!PropertyValidator.IsValidProperty<T>(propName)) return false;

            var dbSet = context.Set<T>();

            return value switch
            {
                string valueString => await dbSet.AnyAsync($"{propName} == \"{value}\" && Id != \"{resourceId}\""),
                int valueInt => await dbSet.AnyAsync($"{propName} == {value} && Id != \"{resourceId}\""),
                _ => throw new
                    ArgumentException("El parametro \"value\" solo puede ser int o string")
            };
        }

        public static async Task<bool> CheckRelatedEntityIdExists(ApplicationDbContext context, List<string> entityIds)
        {
            var dbSet = context.Set<T>();

            var entityIdsDb = await dbSet
                .Where("x => @0.Contains(x.Id)", entityIds)
                .Select("x => x.Id")
                .ToDynamicListAsync<string>();

            return entityIdsDb.Count != entityIds.Count;
        }
    }
}
