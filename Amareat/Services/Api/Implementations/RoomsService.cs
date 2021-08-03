using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.API.Responses.Rooms;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class RoomsService : IRoomsService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;
        private readonly ICrashTokenService _crashTokenService;

        #endregion

        public RoomsService(
            ICrashReporting crashReporting,
            IApiClient apiClient,
            ICrashTokenService crashTokenService)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
            _crashTokenService = crashTokenService;
        }

        public async Task<bool> EditRoom(EditRoom room, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PutAsync($"{ConstantGlobal.Rooms}editRoom", room, cancellationToken);

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
                await _crashReporting.TrackApiError(ex);
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

        public async Task<Room> GetRoom(string id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Rooms}getRoom?_id={id}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Room>(response);
            }
            catch (NoInternetConnectionException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (ApiErrorException ex)
            {
                await _crashReporting.TrackApiError(ex);
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

        public async Task<RoomList> GetRooms(string status, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Rooms}getRooms?status={status}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<RoomList>(response);
            }
            catch (NoInternetConnectionException ex)
            {
                Debug.WriteLine(ex);
                throw ex;
            }
            catch (ApiErrorException ex)
            {
                await _crashReporting.TrackApiError(ex);
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

        public async Task<bool> SaveRoom(SaveRoom room, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PostAsync($"{ConstantGlobal.Rooms}saveRoom", room, cancellationToken);

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
                await _crashReporting.TrackApiError(ex);
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
