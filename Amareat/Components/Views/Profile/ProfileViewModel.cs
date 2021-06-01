using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Responses.Users;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Profile
{
    public class ProfileViewModel : BaseVm
    {

        #region Private Properties

        private IUsersService _usersService;
        private ICrashReporting _crashReporting;

        private bool _isEmpty;
        private User _user;

        #endregion

        #region Public Properties

        public Command GetUserProfileCommand { get; set; }

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
            ICrashReporting crashReporting)
        {
            _usersService = usersService;
            _crashReporting = crashReporting;

            InitCommand();
        }

        #region Private Methods

        private void InitCommand()
        {
            GetUserProfileCommand = new Command(async () => await OnGetUserProfileAsync());
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
