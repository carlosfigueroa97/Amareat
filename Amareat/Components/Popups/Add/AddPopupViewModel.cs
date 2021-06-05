using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Popups.Add
{
    public class AddPopupViewModel : BaseVm
    {
        #region Properties & Commands

        #region Private properties

        private readonly IPreferenceService _preferenceService;
        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        #endregion

        #region Public properties

        public Command<string> TapCommand { get; set; }

        #endregion

        #endregion

        public AddPopupViewModel(
            IPreferenceService preferenceService,
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting)
        {
            _preferenceService = preferenceService;
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;

            TapCommand = new Command<string>(async (param) =>
                await ExecuteTapCommand(param));
        }

        #region Methods

        #region Private methods

        async Task ExecuteTapCommand(string optionClicked)
        {
            try
            {
                if (optionClicked == null)
                    return;

                switch (optionClicked)
                {
                    case "User":
                        break;

                    case "Building":
                        break;

                    case "Room":
                        break;

                    case "Device":
                        break;

                    default:
                        await _popupNavigationService.DismissPopupPage();
                        break;
                }
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }
        #endregion

        #region Public methods
        #endregion

        #endregion
    }
}
