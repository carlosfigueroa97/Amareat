using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Connection.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.SecureStorage.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Api.Implementations
{
    public class ApiClient : IApiClient
    {
        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IConnectivityService _connectivityService;
        private readonly ISecureStorage _secureStorage;
        private readonly IUsersService _usersService;

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

        #endregion

        public ApiClient(
            ICrashReporting crashReporting,
            IConnectivityService connectivityService,
            ISecureStorage secureStorage,
            IUsersService usersService)
        {
            _crashReporting = crashReporting;
            _connectivityService = connectivityService;
            _secureStorage = secureStorage;
            _usersService = usersService;
        }

        #region Public Methods

        public async Task<string> GetAsync(string url, CancellationToken cancellatonToken)
        {
            HttpResponseMessage httpResponseMessage = null;

            if (!_connectivityService.UserHasInternetConnection)
            {
                throw new NoInternetConnectionException();
            }

            try
            {
                cancellatonToken.ThrowIfCancellationRequested();

                if (!HttpClient.DefaultRequestHeaders.Contains(ConstantGlobal.Authorization))
                {
                    string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, ConstantGlobal.Bearer + token);
                }

                httpResponseMessage = await HttpClient.GetAsync(url, cancellatonToken).ConfigureAwait(false);

                cancellatonToken.ThrowIfCancellationRequested();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }

                if(httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseToken = await RefreshUserToken();
                    if (responseToken)
                    {
                        return await GetAsync(url, cancellatonToken);
                    }

                    await CatchRefreshTokenException(httpResponseMessage);
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

        public async Task<string> PostAsync(string url, object item, CancellationToken cancellatonToken, bool isAuthorizedCall = true)
        {
            HttpResponseMessage httpResponseMessage = null;

            if (!_connectivityService.UserHasInternetConnection)
            {
                throw new NoInternetConnectionException();
            }

            try
            {
                cancellatonToken.ThrowIfCancellationRequested();

                if (isAuthorizedCall && !HttpClient.DefaultRequestHeaders.Contains(ConstantGlobal.Authorization))
                {
                    string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, ConstantGlobal.Bearer + token);
                }

                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                httpResponseMessage = await HttpClient.PostAsync(url, content, cancellatonToken).ConfigureAwait(false);

                cancellatonToken.ThrowIfCancellationRequested();

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return await httpResponseMessage.Content.ReadAsStringAsync();
                }

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseToken = await RefreshUserToken();
                    if (responseToken)
                    {
                        return await PostAsync(url, item, cancellatonToken, isAuthorizedCall);
                    }

                    await CatchRefreshTokenException(httpResponseMessage);
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

                if (httpResponseMessage.StatusCode == HttpStatusCode.Unauthorized)
                {
                    var responseToken = await RefreshUserToken();
                    if (responseToken)
                    {
                        return await PutAsync(url, item, cancellationToken, isAuthorizedCall);
                    }

                    await CatchRefreshTokenException(httpResponseMessage);
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

        private async Task<bool> RefreshUserToken()
        {
            var cancellationToken = new CancellationTokenSource().Token;
            var response = await _usersService.RefreshUserToken(cancellationToken);
            return response;
        }

        private async Task CatchRefreshTokenException(HttpResponseMessage httpResponseMessage)
        {
            var message = await httpResponseMessage.Content.ReadAsStringAsync();

            throw new RefreshTokenException
            {
                StatusCode = httpResponseMessage.StatusCode,
                ReasonPhrase = httpResponseMessage.ReasonPhrase,
                ApiMessageResponse = message
            };
        }

        #endregion
    }
}
