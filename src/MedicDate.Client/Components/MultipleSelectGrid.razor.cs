using MedicDate.Utility.Interfaces;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace MedicDate.Client.Components;

public partial class MultipleSelectGrid<TItem> where TItem : IId
{
    private RadzenDataGrid<TItem> _dataGrid = default!;
    private IList<TItem> _tempItemList = new List<TItem>();
    private IList<TItem> _tempSelectedItems = new List<TItem>();
    [Parameter] public IList<TItem>? ItemList { get; set; }
    [Parameter] public IList<TItem>? SelectedItems { get; set; }
    [Parameter] public string[]? Headers { get; set; }
    [Parameter] public string[]? PropNames { get; set; }
    [Parameter] public bool AllowColumnResize { get; set; }

    protected override void OnParametersSet()
    {
        if (SelectedItems is not null) _tempSelectedItems = SelectedItems;

        foreach (var selectedItem in _tempSelectedItems)
            _tempItemList.Add(selectedItem);

        if (ItemList is not null)
            foreach (var item in ItemList.Where(item =>
                       _tempItemList.Count != ItemList.Count &&
                       _tempItemList.All(x => x.Id != item.Id)))
                _tempItemList.Add(item);
    }

    private void SelectAll(bool isSelected)
    {
        if (isSelected)
        {
            _tempSelectedItems = _tempItemList;

            if (SelectedItems is not null)
                foreach (var tempSelectedItem in _tempSelectedItems)
                    SelectedItems.Add(tempSelectedItem);
        }
        else
        {
            _tempSelectedItems = new List<TItem>();
            SelectedItems?.Clear();
        }
    }

    private void SelectRow(TItem data)
    {
        _tempSelectedItems.Add(data);
        if (SelectedItems is not null &&
            SelectedItems.All(x => x.Id != data.Id))
            SelectedItems.Add(data);
    }

    private void DeselectRow(TItem data)
    {
        _tempSelectedItems.Remove(data);

        if (SelectedItems is not null)
        {
            var itemToDelete =
              SelectedItems.FirstOrDefault(x => x.Id == data.Id);

            if (itemToDelete is not null) SelectedItems.Remove(itemToDelete);
        }
    }
}