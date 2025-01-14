﻿using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MedicDate.API.Helpers;

public class CustomValueProviderFactory : IValueProviderFactory
{
    public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var query = context.ActionContext.HttpContext.Request.Query;
        if (query != null && query.Count > 0)
        {
            var valueProvider = new QueryStringValueProvider(
              BindingSource.Query,
              query,
              CultureInfo.CurrentCulture);

            context.ValueProviders.Add(valueProvider);
        }

        return Task.CompletedTask;
    }
}