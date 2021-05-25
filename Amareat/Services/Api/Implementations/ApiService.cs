using System;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;

namespace Amareat.Services.Api.Implementations
{
    public class ApiService : IApiService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;

        #endregion

        public ApiService(
            ICrashReporting crashReporting,
            IApiClient apiClient)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
        }

        #region Public Methods

        #endregion
    }
}
