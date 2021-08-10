using MedicDate.Client.Data.HttpRepository.IHttpRepository;
using MedicDate.Client.Services.IServices;
using MedicDate.Models.DTOs;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

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

            var response = await HttpRepo.Get<ApiResponseDto<T>>(getUrl + filterRequestQuery);

            if (response is null)
            {
                return;
            }

            if (response.Error)
            {
                NotificationService.ShowError("Error!", "Error al cargar los datos");
            }
            else
            {
                ItemList = response.Response.DataResult;
                TotalCount = response.Response.TotalCount;
            }
        }

        protected async Task DeleteItem(string idString, string deleteUrl, string getUrl)
        {
            var httpResp = await HttpRepo.Delete($"{deleteUrl}/{idString}");

            if (httpResp.Error)
            {
                NotificationService.ShowError("Error!", await httpResp.GetResponseBody());
            }
            else
            {
                NotificationService.ShowSuccess("Operación Exitosa!", await httpResp.GetResponseBody());

                await LoadItemListAsync(getUrl);
            }
        }
    }
}