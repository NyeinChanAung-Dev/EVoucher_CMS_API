using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EVoucher_CMS_API.Helper
{
    public static class PageListHelper
    {
        public static string GetPagingMetadata<T>(PagedList<T> pageList)
        {
            return StringHelper.SerializeObject(new
            {
                CurrentPage = pageList.CurrentPage,
                TotalCount = pageList.TotalCount,
                TotalPages = pageList.TotalPages,
            });
        }
    }

    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }
        public static PagedList<T> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            if (source == null || source.Count() < 1)
                return null;

            var count = source.Count();

            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }

    public class PaginationModel
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        public string GetPagingMetadata()
        {
            return StringHelper.SerializeObject(this);
        }
    }

}
