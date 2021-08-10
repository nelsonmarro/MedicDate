using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Linq.Dynamic.Core;
using MedicDate.Client.Helpers;
using MedicDate.Models.DTOs;

namespace MedicDate.Client.Components
{
    public partial class RadzenGenericGrid<TItem>
    {
        [Parameter]
        public List<TItem> ItemList { get; set; }

        [Parameter] public string[] Headers { get; set; } = { };

        [Parameter]
        public string[] PropNames { get; set; } = {};

        [Parameter]
        public RenderFragment NullItemList { get; set; }

        [Parameter]
        public RenderFragment CustomGridCols { get; set; }

        [Parameter]
        public bool AllowFilter { get; set; } = true;
        [Parameter]
        public bool AllowColumnResize { get; set; } = true;
        [Parameter]
        public FilterMode FilterMode { get; set; } = FilterMode.Simple;

        [Parameter] public AllowCrudOps AllowCrudOps { get; set; } = new();

        [Parameter]
        public OpRoutes OpRoutes { get; set; }

        [Parameter]
        public EventCallback<string> OnDeleteData { get; set; }

        [Parameter]
        public int TotalCount { get; set; }

        private RadzenDataGrid<TItem> _dataGrid;
        private int _pageSize = 10;

        private async Task LoadData(LoadDataArgs args = null, int pageIndex = 0, int pageSize = 10)
        {
            var url = OpRoutes.GetUrl.Contains("?")
                ? $"{OpRoutes.GetUrl}&pageIndex={pageIndex}&pageSize={pageSize}"
                : $"{OpRoutes.GetUrl}?pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpRepository.Get<ApiResponseDto<TItem>>(url);

            if (response.Error)
            {
                _notificationService.ShowError("Error!", await response.GetResponseBody());
            }
            else
            {
                ItemList = response.Response.DataResult;
                _pageSize = response.Response.PageSize;
                TotalCount = response.Response.TotalCount;
                if (args is not null)
                {
                    var query = ItemList.AsQueryable();
                    if (!string.IsNullOrEmpty(args.Filter))
                    {
                        query = query.Where(args.Filter);
                    }

                    if (!string.IsNullOrEmpty(args.OrderBy))
                    {
                        query = query.OrderBy(args.OrderBy);
                    }

                    ItemList = query.ToList();
                    await InvokeAsync(StateHasChanged);
                }
            }
        }

        private async Task OnDropDownChange(object value)
        {
            _pageSize = (int)value;
            await LoadData(null, 0, _pageSize);
            _dataGrid.GoToPage(0);
        }
    }
}