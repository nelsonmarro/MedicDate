using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicDate.Utility.Interfaces;

namespace MedicDate.Client.Components
{
    public partial class SelectableGrid<TItem> where TItem : IId
    {
        [Parameter] public List<TItem> ItemList { get; set; }

        [Parameter] public List<TItem> SelectedItems { get; set; }

        [Parameter] public string[] Headers { get; set; }

        [Parameter] public string[] PropNames { get; set; }

        [Parameter] public bool AllowColumnResize { get; set; } = false;

        private void SelectAll(bool args)
        {
            if (args)
            {
                SelectedItems.AddRange(ItemList);
            }
            else
            {
                SelectedItems.Clear();
            }
        }

        private void SelectOne(bool args, TItem selectedItem)
        {
            if (args)
            {
                SelectedItems.Add(selectedItem);
            }
            else
            {
                SelectedItems.RemoveAll(x => x.Id == selectedItem.Id);
            }
        }
    }
}
