using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Models.API.Responses.Users;
using Amareat.Services.Api.Auth.Interfaces;
using Amareat.Services.Connection.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.SecureStorage.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Auth.Implementations
{
    public class AuthService : IAuthService
    {

        private ICrashReporting _crashReporting;
        private ICrashTokenService _crashTokenService;
        private IConnectivityService _connectivityService;
        private ISecureStorage _secureStorage;

        public AuthService(
            ICrashReporting crashReporting,
            ICrashTokenService crashTokenService,
            IConnectivityService connectivityService,
            ISecureStorage secureStorage)
        {
            _crashReporting = crashReporting;
            _crashTokenService = crashTokenService;
            _connectivityService = connectivityService;
            _secureStorage = secureStorage;
        }

        private static HttpClient _httpClient;
        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient
                    {
                        BaseAddress = new Uri(ConstantGlobal.ApiUrl),
                        Timeout = TimeSpan.FromSeconds(10)
                    };
                }

                return _httpClient;
            }
        }

        public async Task<string> PutAsync(string url, object item, CancellationToken cancellationToken, bool isAuthorizedCall = true)
        {
            HttpResponseMessage httpResponseMessage = null;

            if (!_connectivityService.UserHasInternetConnection)
            {
                throw new NoInternetConnectionException();
            }

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (isAuthorizedCall && !HttpClient.DefaultRequestHeaders.Contains(ConstantGlobal.Authorization))
                {
                    string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, ConstantGlobal.Bearer + token);
                }

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponseMessage = await HttpClient.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (OperationCanceledException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
                throw ex;
            }

            throw new ApiErrorException
            {
                StatusCode = httpResponseMessage == null ? System.Net.HttpStatusCode.Ambiguous : httpResponseMessage.StatusCode
            };
        }

        public async Task<bool> RefreshUserToken(CancellationToken cancellationToken)
        {
            try
            {
                var response = await PutAsync($"{ConstantGlobal.Users}refreshToken", null, cancellationToken);

                if (string.IsNullOrEmpty(response))
                {
                    return false;
                }

                var tokenModel = JsonConvert.DeserializeObject<RefreshToken>(response);

                await _secureStorage.SetValue(KeysSecureStorage.Token, tokenModel.Token);

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
