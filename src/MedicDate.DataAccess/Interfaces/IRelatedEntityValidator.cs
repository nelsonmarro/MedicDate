using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.DataAccess.Interfaces
{
    public interface IRelatedEntityValidator
    {
        Task<bool> CheckRelatedEntityIdsExistsAsync(List<string> entityIds);
    }
}
