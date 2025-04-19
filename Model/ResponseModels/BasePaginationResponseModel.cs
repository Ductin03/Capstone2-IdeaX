namespace IdeaX.Model.ResponseModels
{
    public class BasePaginationResponseModel<T>
    {
        public BasePaginationResponseModel(int pageIndex, int pageSize, int total, IEnumerable<T> items)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Items = items;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}
