using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Requests.History;
using Amareat.Models.API.Responses.History;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class HistoryService : IHistoryService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;
        private readonly ICrashTokenService _crashTokenService;

        #endregion

        public HistoryService(
            ICrashReporting crashReporting,
            IApiClient apiClient,
            ICrashTokenService crashTokenService)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
            _crashTokenService = crashTokenService;
        }

        public async Task<HistoryList> GetHistory(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.History}getHistory", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<HistoryList>(response);
            }
            catch (NoInternetConnectionException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (ApiErrorException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (RefreshTokenException ex)
            {
                await _crashTokenService.TrackRefreshTokenException(ex);
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }

            throw new ApiErrorException();
        }

        public async Task<bool> SaveHistory(SaveHistory history, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PostAsync($"{ConstantGlobal.History}saveHistory", history, cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return false;
                }

                return true;
            }
            catch (NoInternetConnectionException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (ApiErrorException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (RefreshTokenException ex)
            {
                await _crashTokenService.TrackRefreshTokenException(ex);
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }

            throw new ApiErrorException();
        }
    }
}
