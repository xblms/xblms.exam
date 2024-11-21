using System.Threading.Tasks;

namespace Datory
{
    public interface IRedis
    {
        string ConnectionString { get; }
        string Host { get; }
        int Port { get; }
        string Password { get; }
        int Database { get; }
        bool AllowAdmin { get; }
        Task<(bool IsConnectionWorks, string ErrorMessage)> IsConnectionWorksAsync();
    }
}
