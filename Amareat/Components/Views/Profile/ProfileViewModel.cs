using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Views.Login;
using Amareat.Helpers;
using Amareat.Models.API.Responses.Users;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Interfaces;
using Amareat.Services.SecureStorage.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Profile
{
    public class ProfileViewModel : BaseVm
    {

        #region Private Properties

        private IUsersService _usersService;
        private ICrashReporting _crashReporting;
        private IPopupNavigationService _popupNavigationService;
        private ISecureStorage _secureStorage;
        private IPreferenceService _preferenceService;
        private INavigationService _navigationService;

        private bool _isEmpty;
        private User _user;

        #endregion

        #region Public Properties

        public Command GetUserProfileCommand { get; set; }
        public Command LogoutCommand { get; set; }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }

        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        #endregion

        public ProfileViewModel(
            IUsersService usersService,
            ICrashReporting crashReporting,
            IPopupNavigationService popupNavigationService,
            ISecureStorage secureStorage,
            IPreferenceService preferenceService,
            INavigationService navigationService)
        {
            _usersService = usersService;
            _crashReporting = crashReporting;
            _popupNavigationService = popupNavigationService;
            _secureStorage = secureStorage;
            _preferenceService = preferenceService;
            _navigationService = navigationService;

            InitCommand();
        }

        #region Private Methods

        private void InitCommand()
        {
            GetUserProfileCommand = new Command(async () => await OnGetUserProfileAsync());
            LogoutCommand = new Command(async () => await OnLogoutAsync());
        }

        private async Task OnLogoutAsync()
        {
            try
            {
                IsBusy = true;

                var response = await _usersService.Logout(default);

                if (!response)
                {
                    await _popupNavigationService.ShowErrorDialog(
                        Resources.Error,
                        Resources.AnUnexpectedErrorHasOcurred);
                    return;
                }

                _secureStorage.ResetAllProperties();
                _preferenceService.ResetProperties();

                await _navigationService.SetNewNavigationPage<LoginViewModel>();

            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task OnGetUserProfileAsync()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;

                var response = await _usersService.GetUserProfile(default);

                if(response is null)
                {
                    IsEmpty = true;
                    User = new User();
                }
                else
                {
                    User = response;
                }

                OnPropertyChanged(nameof(User));
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        #endregion
    }
}
