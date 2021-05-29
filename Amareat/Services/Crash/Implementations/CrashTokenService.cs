using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Amareat.Components.Views.Login;
using Amareat.Exceptions;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.SecureStorage.Interfaces;

namespace Amareat.Services.Crash.Implementations
{
    public class CrashTokenService : ICrashTokenService
    {
        private readonly ISecureStorage _secureStorage;
        private readonly INavigationService _navigationService;
        private readonly IPopupNavigationService _popupNavigationService;

        public CrashTokenService(
            ISecureStorage secureStorage,
            INavigationService navigationService,
            IPopupNavigationService popupNavigationService)
        {
            _secureStorage = secureStorage;
            _navigationService = navigationService;
            _popupNavigationService = popupNavigationService;
        }

        public async Task TrackRefreshTokenException(RefreshTokenException ex, IDictionary<string, string> properties = null)
        {
            Debug.WriteLine("-----------Track Crash Refresh Token-----------");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"Response: {ex.ApiMessageResponse}");
            Debug.WriteLine($"Status Code: {ex.StatusCode}");
            Debug.WriteLine($"Reason Phrase: {ex.ReasonPhrase}");
            _secureStorage.ResetAllProperties();
            await _popupNavigationService.ShowErrorDialog(ex.StatusCode.ToString(), ex.ReasonPhrase);
            await _navigationService.SetNewNavigationPage<LoginViewModel>();
        }
    }
}
