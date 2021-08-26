using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicDate.Bussines.Interfaces
{
    public interface IRelatedEntityValidator
    {
        Task<bool> CheckRelatedEntityIdsExistsAsync(List<string> entityIds);
    }
}
