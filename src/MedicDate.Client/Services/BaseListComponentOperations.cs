using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Helpers;
using MedicDate.Client.Services.IServices;
using MedicDate.Shared.Models.Common;

namespace MedicDate.Client.Services
{
   public class BaseListComponentOperations : IBaseListComponentOperations
   {
      private readonly IHttpRepository _httpRepo;
      private readonly INotificationService _notificationService;

      public BaseListComponentOperations(IHttpRepository httpRepo,
          INotificationService notificationService)
      {
         _httpRepo = httpRepo;
         _notificationService = notificationService;
      }

      public async Task<ResourceListComponentResult<T>> LoadItemListAsync<T>
      (
          string getUrl,
          string? filterQuery = null,
          string? filterData = null
      ) where T : class
      {
         var filterRequestQuery = "";

         if (!string.IsNullOrEmpty(filterQuery)
             && !string.IsNullOrEmpty(filterData)
             && filterData != "0")
         {
            filterRequestQuery = filterQuery + filterData;
         }

         var response = await _httpRepo.Get<PaginatedResourceListDto<T>>(getUrl + filterRequestQuery);

         if (!response.Error)
         {
            if (response.Response is not null)
            {
               return new ResourceListComponentResult<T>
               {
                  Succeded = true,
                  ItemList = response.Response.DataResult,
                  TotalCount = response.Response.TotalCount,
               };
            }
         }

         return new ResourceListComponentResult<T>
         {
            Succeded = false,
            ItemList = new List<T>(),
         };
      }

      public async Task<ResourceListComponentResult<T>> DeleteItem<T>(string idString, string deleteUrl, string getUrl) where T : class
      {
         var httpResp = await _httpRepo.Delete($"{deleteUrl}/{idString}");

         if (!httpResp.Error)
         {
            _notificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

            return await LoadItemListAsync<T>(getUrl);
         }

         return new ResourceListComponentResult<T>
         {
            Succeded = false,
            ItemList = new List<T>()
         };
      }
   }
}
