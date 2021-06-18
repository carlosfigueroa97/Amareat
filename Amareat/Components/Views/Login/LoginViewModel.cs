using System;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Views.Home;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Users;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Login
{
    public class LoginViewModel : BaseVm
    {

        #region Private Properties

        private IPopupNavigationService _popupNavigationService;
        private ICrashReporting _crashReporting;
        private IUsersService _usersService;
        private INavigationService _navigationService;
        private IPreferenceService _preferenceService;

        private string _user = string.Empty;
        private string _password = string.Empty;

        private CancellationToken _cancellationToken => new CancellationTokenSource().Token;

        #endregion

        #region Public Properties

        public Command ForgotPasswordCommand { get; set; }
        public Command SignInCommand { get; set; }

        public string User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        #endregion

        public LoginViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting,
            IUsersService usersService,
            INavigationService navigationService,
            IPreferenceService preferenceService)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;
            _usersService = usersService;
            _navigationService = navigationService;
            _preferenceService = preferenceService;

            InitCommands();
        }

        #region Private Methods

        private void InitCommands()
        {
            ForgotPasswordCommand = new Command(async () => await OnForogotPasswordAsync()) ;
            SignInCommand = new Command(async () => await OnSignInAsync());
        }

        private async Task OnForogotPasswordAsync()
        {
            await _popupNavigationService
                .ShowErrorDialog(null,
                Resources.PleaseContactAdministrator);
        }

        private async Task OnSignInAsync()
        {
            try
            {
                IsBusy = true;

                if (!CheckCredentials())
                {
                    await _popupNavigationService
                        .ShowErrorDialog(Resources.Error,
                        Resources.CheckYourDataWell);
                    return;
                }

                var signIn = new SignIn
                {
                    Username = User,
                    Password = Password
                };

                var response = await _usersService.SignIn(signIn, _cancellationToken);

                if (!response)
                {
                    await _popupNavigationService
                        .ShowErrorDialog(Resources.WrongCredentials,
                        Resources.CheckYourDataWell);
                    return;
                }

                _preferenceService.IsUserLoggedIn = true;

                await _navigationService.SetNewNavigationPage<HomeViewModel>();
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

        private bool CheckCredentials()
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
