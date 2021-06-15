using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amareat.Exceptions;

namespace Amareat.Services.Crash.Interfaces
{
    public interface ICrashReporting
    {
        void TrackError(Exception ex, IDictionary<string, string> properties = null);

        Task TrackApiError(ApiErrorException ex, IDictionary<string, string> properties = null);
    }
}
