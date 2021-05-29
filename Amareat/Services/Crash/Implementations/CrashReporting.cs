using System;
using System.Collections.Generic;
using System.Diagnostics;
using Amareat.Services.Crash.Interfaces;

namespace Amareat.Services.Crash.Implementations
{
    public class CrashReporting : ICrashReporting
    {
        public CrashReporting()
        {
        }

        public void TrackError(Exception ex, IDictionary<string, string> properties = null)
        {
            Debug.WriteLine($"Track error message: {ex}");
            Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, properties);
        }
    }
}
