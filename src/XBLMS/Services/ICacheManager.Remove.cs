namespace XBLMS.Services
{
    public partial interface ICacheManager
    {
        void Remove(string key);
        void Clear();
    }
}
