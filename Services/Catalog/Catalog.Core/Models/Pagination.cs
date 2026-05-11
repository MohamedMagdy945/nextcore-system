namespace Catalog.Core.Models
{
    public class Pagination<T> where T : class
    {
        public Pagination()
        {
            PageIndex = 1;
            PageSize = 10;
            TotalCount = 0;
            Data = new List<T>();
        }
        public Pagination(int pageIndex, int pageSize, int totalCount, IReadOnlyList<T> data)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize < 1 ? 10 : pageSize;
            TotalCount = totalCount < 0 ? 0 : totalCount;
            Data = data ?? new List<T>();
        }
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public IReadOnlyList<T> Data { get; set; }
    }
}