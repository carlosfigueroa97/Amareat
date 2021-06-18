using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Buildings;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class BuildingsService : IBuildingsService
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IApiClient _apiClient;
        private readonly ICrashTokenService _crashTokenService;

        #endregion

        public BuildingsService(
            ICrashReporting crashReporting,
            IApiClient apiClient,
            ICrashTokenService crashTokenService)
        {
            _crashReporting = crashReporting;
            _apiClient = apiClient;
            _crashTokenService = crashTokenService;
        }

        public async Task<bool> EditBuilding(EditBuilding editBuilding, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PutAsync($"{ConstantGlobal.Buildings}editBuilding", editBuilding, cancellationToken);

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
            catch(RefreshTokenException ex)
            {
                await _crashTokenService.TrackRefreshTokenException(ex);
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }

            throw new ApiErrorException();
        }

        public async Task<Building> GetBuilding(string id, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Buildings}getBuilding?_id={id}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<Building>(response);
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

        public async Task<BuildingList> GetBuildings(string status, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Buildings}getBuildings?status={status}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BuildingList>(response);
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

        public async Task<bool> SaveBuilding(SaveBuilding building, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.PostAsync($"{ConstantGlobal.Buildings}saveBuilding", building, cancellationToken);

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

        public async Task<BuildingList> SearchBuilding(string searchValue, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _apiClient.GetAsync($"{ConstantGlobal.Buildings}searchBuilding?searchValue={searchValue}", cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return null;
                }

                return JsonConvert.DeserializeObject<BuildingList>(response);
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
