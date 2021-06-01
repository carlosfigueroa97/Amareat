using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Helpers;
using Amareat.Services.Api.Auth.Interfaces;
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
        private readonly IAuthService _authService;

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
                        Timeout = TimeSpan.FromSeconds(30)
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
            IAuthService authService)
        {
            _crashReporting = crashReporting;
            _connectivityService = connectivityService;
            _secureStorage = secureStorage;
            _authService = authService;
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

                string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                if(token != null)
                {
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, $"{ConstantGlobal.Bearer} {token}");
                }

                httpResponseMessage = await HttpClient.GetAsync(url, cancellatonToken).ConfigureAwait(false);

                cancellatonToken.ThrowIfCancellationRequested();

                var readString = await httpResponseMessage.Content.ReadAsStringAsync();

#if DEBUG
                Debug.WriteLine(readString);
#endif

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return readString;
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

                string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                if (token != null)
                {
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, $"{ConstantGlobal.Bearer} {token}");
                }

                StringContent content = null;

                if(item != null)
                {
                    var json = JsonConvert.SerializeObject(item);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                httpResponseMessage = await HttpClient.PostAsync(url, content, cancellatonToken).ConfigureAwait(false);

                cancellatonToken.ThrowIfCancellationRequested();

                var readString = await httpResponseMessage.Content.ReadAsStringAsync();
#if DEBUG
                Debug.WriteLine(readString);
#endif

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return readString;
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

                string token = await _secureStorage.GetValue(KeysSecureStorage.Token);
                if (token != null)
                {
                    HttpClient.DefaultRequestHeaders.Clear();
                    HttpClient.DefaultRequestHeaders.Add(ConstantGlobal.Authorization, $"{ConstantGlobal.Bearer} {token}");
                }
                StringContent content = null;

                if(item != null)
                {
                    var json = JsonConvert.SerializeObject(item);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                httpResponseMessage = await HttpClient.PutAsync(url, content, cancellationToken).ConfigureAwait(false);

                cancellationToken.ThrowIfCancellationRequested();

                var readString = await httpResponseMessage.Content.ReadAsStringAsync();

#if DEBUG
                Debug.WriteLine(readString);
#endif

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return readString;
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
            var response = await _authService.RefreshUserToken(cancellationToken);
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
