using System.Threading;
using System.Threading.Tasks;

namespace Amareat.Services.Api.Interfaces
{
    public interface IApiClient
    {
        Task<string> GetAsync(string url, CancellationToken cancellatonToken);

        Task<string> PostAsync(string url, object item, CancellationToken cancellatonToken, bool isAuthorizedCall = true);

        Task<string> PutAsync(string url, object item, CancellationToken cancellationToken, bool isAuthorizedCall = true);
    }
}
