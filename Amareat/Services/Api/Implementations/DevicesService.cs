using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Devices;
using Amareat.Models.API.Responses.Devices;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class DevicesService : IDevicesService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;
        private readonly ICrashTokenService _crashTokenService;

        #endregion

        public DevicesService(
            ICrashReporting crashReporting,
            IApiClient apiClient,
            ICrashTokenService crashTokenService)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
            _crashTokenService = crashTokenService;
        }

        public async Task<bool> EditDevice(EditDevice device, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PutAsync($"{ConstantGlobal.Devices}editDevice", device, cancellationToken);

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

        public async Task<Device> GetDevice(string id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Devices}editDevice?_id={id}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Device>(response);
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

        public async Task<DeviceList> GetDevices(string status, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Devices}getDevices?status={status}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<DeviceList>(response);
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

        public async Task<DeviceListWithRoom> GetDevicesByBuilding(string status, string idBuilding, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Devices}getDevicesByBuilding?status={status}&idBuilding={idBuilding}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<DeviceListWithRoom>(response);
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

        public async Task<bool> SaveDevice(SaveDevice device, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PostAsync($"{ConstantGlobal.Devices}saveDevice", device, cancellationToken);

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
