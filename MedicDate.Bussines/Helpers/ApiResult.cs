using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using MedicDate.Utility;

namespace MedicDate.Bussines.Helpers
{
    public class ApiResult<T> where T : class
    {
        private ApiResult(
            object dataResult,
            int count,
            int pageIndex,
            int pageSize)
        {
            DataResult = dataResult;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }

        #region Methods

        public static ApiResult<T> Create(
            List<T> sourceList,
            int pageIndex,
            int pageSize,
            string sortColumn = null,
            string sortOrder = null)
        {
            var queryable = sourceList.AsQueryable();
            var count = queryable.Count();

            if (!string.IsNullOrEmpty(sortColumn) && PropertyValidator.IsValidProperty<T>(sortColumn))
            {
                sortOrder = !string.IsNullOrEmpty(sortOrder) && sortOrder.ToUpper() == "ASC" ? "ASC" : "DESC";

                queryable = queryable.OrderBy($"{sortColumn} {sortOrder}");
            }

            var responseList = queryable
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            return new ApiResult<T>(
                responseList,
                count,
                pageIndex,
                pageSize);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The data result
        /// </summary>
        public object DataResult { get; private set; }

        /// <summary>
        /// Zero-based index of current page.
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// Number of items contained in each page.
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// Total items count
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// Total pages count
        /// </summary>
        public int TotalPages { get; private set; }

        #endregion
    }
}