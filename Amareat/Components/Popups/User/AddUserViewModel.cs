using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
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
        private string _errorPasswordMessage = string.Empty;
        private bool _isPasswordWrong;
        private string _errorEmailMessage = string.Empty;
        private bool _isEmailWrong;

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

        public string ErrorPasswordMessage
        {
            get => _errorPasswordMessage;

            set
            {
                SetProperty(ref _errorPasswordMessage, value);
                OnPropertyChanged(nameof(Password));
            }
        }

        public bool IsPasswordWrong
        {
            get => _isPasswordWrong;

            set
            {
                SetProperty(ref _isPasswordWrong, value);
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorEmailMessage
        {
            get => _errorEmailMessage;

            set
            {
                SetProperty(ref _errorEmailMessage, value);
                OnPropertyChanged(nameof(Email));
            }
        }

        public bool IsEmailWrong
        {
            get => _isEmailWrong;

            set
            {
                SetProperty(ref _isEmailWrong, value);
                OnPropertyChanged(nameof(Email));
            }
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
                InitializeErrorMessages();

                if (string.IsNullOrEmpty(User) ||
                    string.IsNullOrEmpty(Password) ||
                    string.IsNullOrEmpty(Email))
                {
                    await _popupNavigationService
                        .ShowErrorDialog(Resources.Error,
                        Resources.CheckYourDataWell);
                    return;
                }

                User = User.TrimEnd();
                Email = Email.TrimEnd();

                bool validPassword = ValidatePassword(Password);
                bool validEmail = ValidateEmail(Email);

                if (!validPassword || !validEmail)
                {
                    return;
                }

                await SaveNewUser();

            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        private void InitializeErrorMessages()
        {
            IsPasswordWrong = false;
            ErrorPasswordMessage = string.Empty;
            IsEmailWrong = false;
            ErrorEmailMessage = string.Empty;
        }

        private bool ValidatePassword(string password)
        {
            if (password.Length <= 6)
            {
                IsPasswordWrong = true;
                ErrorPasswordMessage = Resources.ShortPasswordError;
                return false;
            }

            return true;
        }

        private bool ValidateEmail(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                IsEmailWrong = true;
                ErrorEmailMessage = Resources.InvalidEmailError;
            }

            return false;
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

                await ExecuteClosePopupCommand();

                await _popupNavigationService.ShowToastDialog(Resources.UserSaved, 2000);
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        #endregion
    }
}
