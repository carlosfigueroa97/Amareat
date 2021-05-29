using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Requests.TypeDevices;
using Amareat.Models.API.Responses.TypeDevices;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class TypeDevicesService : ITypeDevicesService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;
        private readonly ICrashTokenService _crashTokenService;

        #endregion

        public TypeDevicesService(
            ICrashReporting crashReporting,
            IApiClient apiClient,
            ICrashTokenService crashTokenService)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
            _crashTokenService = crashTokenService;
        }

        public async Task<TypeDeviceList> GetTypeDevices(string status, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.TypeDevices}getTypeDevices?status={status}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<TypeDeviceList>(response);
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

        public async Task<bool> SaveTypeDevice(SaveTypeDevice typeDevice, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PostAsync($"{ConstantGlobal.TypeDevices}saveTypeDevice", typeDevice, cancellationToken);

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
