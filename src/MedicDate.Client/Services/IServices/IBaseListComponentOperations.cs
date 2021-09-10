using MedicDate.Client.Helpers;
using System.Threading.Tasks;

namespace MedicDate.Client.Services.IServices
{
    public interface IBaseListComponentOperations
    {
        public Task<ResourceListComponentResult<T>> LoadItemListAsync<T>
            (
            string getUrl,
            string filterQuery = null,
            string filterData = null
            ) where T : class;

        public Task<ResourceListComponentResult<T>> DeleteItem<T>(string idString, string deleteUrl, string getUrl) where T : class;
    }
}
