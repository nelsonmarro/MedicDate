using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.DataAccess.Services.IServices
{
    public interface IEntityValidator
    {
        public Task<bool> CheckValueExistsAsync<TEntity>(string propName, object value)
            where TEntity : class;

        public Task<bool> CheckValueExistsForEditAsync<TEntity>(string propName, object value,
            string resourceId) where TEntity : class;

        public Task<bool> CheckRelatedEntityIdsExists<TRelatedEntity>(List<string> entityIds)
            where TRelatedEntity : class;
    }
}