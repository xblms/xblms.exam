namespace XBLMS.Dto
{
    public class ItemRequest<T> where T : class
    {
        public T Item { get; set; }
    }
}
