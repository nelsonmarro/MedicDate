using System.Collections.Generic;
using System.Threading.Tasks;
using MedicDate.API.DTOs;
using MedicDate.API.DTOs.Common;
using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using Microsoft.AspNetCore.Components;

namespace MedicDate.Client.Components
{
    public class BaseListComponent<T> : ComponentBase
    {
        [Inject] protected INotificationService NotificationService { get; set; }
        [Inject] protected IHttpRepository HttpRepo { get; set; }

        protected List<T> ItemList;
        protected int TotalCount;

        protected async Task LoadItemListAsync
        (
            string getUrl,
            string filterQuery = null,
            string filterData = null
        )
        {
            var filterRequestQuery = "";

            if (!string.IsNullOrEmpty(filterQuery)
                && !string.IsNullOrEmpty(filterData)
                && filterData != "0")
            {
                filterRequestQuery = filterQuery + filterData;
            }

            var response = await HttpRepo.Get<PaginatedResourceListDto<T>>(getUrl + filterRequestQuery);

            if (!response.Error)
            {
                ItemList = response.Response.DataResult;
                TotalCount = response.Response.TotalCount;
            }
        }

        protected async Task DeleteItem(string idString, string deleteUrl, string getUrl)
        {
            var httpResp = await HttpRepo.Delete($"{deleteUrl}/{idString}");

            if (!httpResp.Error)
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                await LoadItemListAsync(getUrl);
            }
        }
    }
}