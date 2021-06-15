using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Amareat.Exceptions;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;

namespace Amareat.Services.Crash.Implementations
{
    public class CrashReporting : ICrashReporting
    {

        private readonly IPopupNavigationService _popupNavigationService;

        public CrashReporting(IPopupNavigationService popupNavigationService)
        {
            _popupNavigationService = popupNavigationService;
        }

        public async Task TrackApiError(ApiErrorException ex, IDictionary<string, string> properties = null)
        {
            Debug.WriteLine("-----------Track Crash API Error Exception-----------");
            Debug.WriteLine($"Message: {ex.Message}");
            Debug.WriteLine($"Response: {ex.ApiMessageResponse}");
            Debug.WriteLine($"Status Code: {ex.StatusCode}");
            Debug.WriteLine($"Reason Phrase: {ex.ReasonPhrase}");
            await _popupNavigationService.ShowToastDialog(ex.ReasonPhrase, 1500);
        }

        public void TrackError(Exception ex, IDictionary<string, string> properties = null)
        {
            Debug.WriteLine($"Track error message: {ex}");
            Microsoft.AppCenter.Crashes.Crashes.TrackError(ex, properties);
        }
    }
}
