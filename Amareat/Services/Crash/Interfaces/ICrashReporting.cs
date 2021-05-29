using System;
using System.Collections.Generic;

namespace Amareat.Services.Crash.Interfaces
{
    public interface ICrashReporting
    {
        void TrackError(Exception ex, IDictionary<string, string> properties = null);
    }
}
