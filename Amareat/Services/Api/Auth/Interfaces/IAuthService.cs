using System;
using System.Threading;
using System.Threading.Tasks;

namespace Amareat.Services.Api.Auth.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RefreshUserToken(CancellationToken cancellationToken);
    }
}
