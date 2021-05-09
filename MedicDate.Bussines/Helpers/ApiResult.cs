using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace MedicDate.Bussines.Helpers
{
    public class ApiResult<TSource, TResponse>
    {
        private ApiResult(
            List<TResponse> dataResult,
            int count,
            int pageIndex,
            int pageSize)
        {
            DataResult = dataResult;
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = count;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
        }

        #region Methods

        public static ApiResult<TSource, TResponse> Create(
            List<TSource> source,
            int pageIndex,
            int pageSize,
            IMapper mapper)
        {
            var count = source.Count();

            source = source
                .Skip(pageIndex * pageSize)
                .Take(pageSize).ToList();

            var dataResult = mapper.Map<List<TResponse>>(source);

            return new ApiResult<TSource, TResponse>(
                dataResult,
                count,
                pageIndex,
                pageSize);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The data result
        /// </summary>
        public List<TResponse> DataResult { get; private set; }

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