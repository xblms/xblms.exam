namespace XBLMS.Dto
{
    public class ItemResult<T> where T : class
    {
        public T Item { get; set; }
    }
}
