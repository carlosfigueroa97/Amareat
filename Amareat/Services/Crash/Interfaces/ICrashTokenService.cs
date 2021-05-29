using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amareat.Exceptions;

namespace Amareat.Services.Crash.Interfaces
{
    public interface ICrashTokenService
    {
        Task TrackRefreshTokenException(RefreshTokenException ex, IDictionary<string, string> properties = null);
    }
}
