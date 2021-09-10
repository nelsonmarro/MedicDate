using MedicDate.API.DTOs.Common;
using MedicDate.Client.Helpers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MedicDate.Client.Components
{
    public partial class RadzenGenericGrid<TItem>
    {
        [Parameter] public IEnumerable<TItem> ItemList { get; set; }
        [Parameter] public int TotalCount { get; set; }
        [Parameter] public string[] Headers { get; set; } = Array.Empty<string>();

        [Parameter] public string[] PropNames { get; set; } = Array.Empty<string>();

        [Parameter] public RenderFragment CustomGridCols { get; set; }

        [Parameter] public bool AllowFilter { get; set; } = true;
        [Parameter] public bool AllowColumnResize { get; set; } = true;
        [Parameter] public FilterMode FilterMode { get; set; } = FilterMode.Simple;

        [Parameter] public AllowCrudOps AllowCrudOps { get; set; } = new();

        [Parameter] public OpRoutes OpRoutes { get; set; }

        [Parameter] public EventCallback<string> OnDeleteData { get; set; }

        private RadzenDataGrid<TItem> _dataGrid;
        private int _pageSize = 10;
        private IEnumerable<TItem> _itemList;
        private bool _callLoadData;

        protected override void OnAfterRender(bool firstRender)
        {
            _callLoadData = !firstRender;
        }

        protected override void OnParametersSet()
        {
            if (ItemList is not null)
            {
                _itemList = ItemList;
            }
        }

        private async Task LoadData(LoadDataArgs args = null, int pageIndex = 0, int pageSize = 10)
        {
            if (!_callLoadData)
            {
                return;
            }

            var url = OpRoutes.GetUrl.Contains("?")
                ? $"{OpRoutes.GetUrl}&pageIndex={pageIndex}&pageSize={pageSize}"
                : $"{OpRoutes.GetUrl}?pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpRepository.Get<PaginatedResourceListDto<TItem>>(url);

            if (!response.Error)
            {
                _itemList = response.Response.DataResult;
                _pageSize = response.Response.PageSize;
                TotalCount = response.Response.TotalCount;
            }

            if (args is not null)
            {
                var query = _itemList.AsQueryable();
                if (!string.IsNullOrEmpty(args.Filter))
                {
                    query = query.Where(args.Filter);
                }

                if (!string.IsNullOrEmpty(args.OrderBy))
                {
                    query = query.OrderBy(args.OrderBy);
                }

                _itemList = query.ToList();
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