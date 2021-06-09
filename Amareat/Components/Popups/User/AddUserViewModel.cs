using System;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Popups.Base;
using Amareat.Components.Popups.Dialogs;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Users;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Encryption.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Popups.User
{
    public class AddUserViewModel : BaseVm
    {
        #region Properties & Commands

        #region Private properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;
        private IUsersService _usersService;
        private IEncryptionService _encryptionService;

        private CancellationToken _cancellationToken =
            new CancellationTokenSource().Token;

        private string _user = string.Empty;
        private string _password = string.Empty;
        private string _email = string.Empty;
        private bool _isAdmin;

        #endregion

        #region Public properties

        public Command ClosePopup { get; set; }
        public Command ValidateData { get; set; }

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

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => SetProperty(ref _isAdmin, value);
        }

        #endregion

        #endregion

        public AddUserViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting,
            IUsersService usersService,
            IEncryptionService encryptionService)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;
            _usersService = usersService;
            _encryptionService = encryptionService;

            ClosePopup = new Command(async () =>
                await ExecuteClosePopupCommand());
            ValidateData = new Command(async () =>
                await ExecuteValidateDataCommand());
        }

        #region Methods

        async Task ExecuteClosePopupCommand()
        {
            try
            {
                await _popupNavigationService.DismissPopupPage();
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        async Task ExecuteValidateDataCommand()
        {
            try
            {
                if (string.IsNullOrEmpty(User) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Email))
                {
                    await _popupNavigationService
                        .ShowErrorDialog(Resources.Error,
                        Resources.CheckYourDataWell);
                    return;
                }

                await SaveNewUser();
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        private async Task SaveNewUser()
        {
            try
            {
                var EncryptedPassword = _encryptionService.Encrypt(Password);

                var UserInfo = new SaveUser
                {
                    Username = User,
                    Password = EncryptedPassword,
                    Email = Email,
                    IsAdmin = IsAdmin
                };

                var isUserSaved =
                    await _usersService.SaveUser(UserInfo, _cancellationToken);

                if (!isUserSaved)
                {
                    await _popupNavigationService
                       .ShowErrorDialog(Resources.UserNotSaved,
                       Resources.PleaseContactAdministrator);
                    return;
                }
                else
                {
                    await ExecuteClosePopupCommand();

                    await _popupNavigationService.ShowToastDialog(Resources.UserSaved, 3000);
                    return;
                }
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        #endregion
    }
}
