using System;
using Amareat.Components.Popups.Add;
using Amareat.Components.Popups.Building;
using Amareat.Components.Popups.Edit;
using Amareat.Components.Popups.Room;
using Amareat.Components.Popups.User;
using Amareat.Components.Views.Buildings.Edit;
using Amareat.Components.Views.Buildings.Home;
using Amareat.Components.Views.Devices;
using Amareat.Components.Views.History;
using Amareat.Components.Views.Home;
using Amareat.Components.Views.Login;
using Amareat.Components.Views.Profile;
using Amareat.Components.Views.Rooms.Edit;
using Amareat.Components.Views.Rooms.Home;
using Amareat.Components.Views.Users;
using Amareat.Helpers;
using Amareat.Services.Api.Auth.Implementations;
using Amareat.Services.Api.Auth.Interfaces;
using Amareat.Services.Api.Implementations;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Connection.Implementations;
using Amareat.Services.Connection.Interfaces;
using Amareat.Services.Crash.Implementations;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Encryption.Implementations;
using Amareat.Services.Encryption.Interfaces;
using Amareat.Services.Localization.Implementations;
using Amareat.Services.Localization.Interfaces;
using Amareat.Services.Messaging.Implementations;
using Amareat.Services.Messaging.Interfaces;
using Amareat.Services.Navigation.Implementations;
using Amareat.Services.Navigation.Interfaces;
using Amareat.Services.PopupNavigation.Implementations;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Implementations;
using Amareat.Services.Preferences.Interfaces;
using Amareat.Services.SecureStorage.Implementations;
using Amareat.Services.SecureStorage.Interfaces;
using Amareat.Utils.ServiceLocator;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace Amareat
{
    public partial class App : Application
    {

        #region Private Properties

        private IServiceLocator _serviceLocator;
        private INavigationService _navigationService;
        private IPopupNavigationService _popupNavigationService;
        private IPreferenceService _preferenceService;

        private static bool _navigationInitialized;

        #endregion

        public App()
        {
            InitializeComponent();
            InitializeDependencyContainer();
            InitNavigation();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        #region Private Methods

        private void InitializeAppCenter()
        {
            string appCenter = string.Format("{0};{1}", ConstantGlobal.AppCenteriOSToken, ConstantGlobal.AppCenterAndroidToken);
            AppCenter.Start(appCenter, typeof(Analytics), typeof(Crashes));
        }

        private void InitializeDependencyContainer()
        {
            _serviceLocator = ServiceLocatorProvider.Instance.Current;
            RegisterDependencies();
            RegisterViewModels();
            _serviceLocator.Init();
        }

        private void RegisterViewModels()
        {
            // Views
            _serviceLocator.Register<EditBuildingDetailViewModel>();
            _serviceLocator.Register<EditBuildingListViewModel>();
            _serviceLocator.Register<BuildingListViewModel>();
            _serviceLocator.Register<EditDeviceDetailViewModel>();
            _serviceLocator.Register<ChangesHistoryViewModel>();
            _serviceLocator.Register<LoginViewModel>();
            _serviceLocator.Register<ProfileViewModel>();
            _serviceLocator.Register<EditRoomDetailViewModel>();
            _serviceLocator.Register<RoomListViewModel>();
            _serviceLocator.Register<EditUserDetailViewModel>();
            _serviceLocator.Register<EditUserListViewModel>();
            _serviceLocator.Register<HomeViewModel>();

            // Popups
            _serviceLocator.Register<AddPopupViewModel>();
            _serviceLocator.Register<AddedBuildingRoomViewModel>();
            _serviceLocator.Register<AddedBuildingViewModel>();
            _serviceLocator.Register<EditViewModel>();
            _serviceLocator.Register<AddedRoomViewModel>();
            _serviceLocator.Register<AddUserViewModel>();
        }

        private void RegisterDependencies()
        {
            _serviceLocator.RegisterSingle<ILocalizationService, LocalizationService>();
            _serviceLocator.RegisterSingle<ISecureStorage, SecureStorage>();
            _serviceLocator.RegisterSingle<IConnectivityService, ConnectivityService>();
            _serviceLocator.RegisterSingle<IAuthService, AuthService>();
            _serviceLocator.RegisterSingle<ICrashReporting, CrashReporting>();
            _serviceLocator.RegisterSingle<ICrashTokenService, CrashTokenService>();
            _serviceLocator.RegisterSingle<IApiClient, ApiClient>();
            _serviceLocator.RegisterSingle<IEncryptionService, EncryptionService>();
            _serviceLocator.RegisterSingle<IUsersService, UsersService>();
            _serviceLocator.RegisterSingle<IBuildingsService, BuildingsService>();
            _serviceLocator.RegisterSingle<IRoomsService, RoomsService>();
            _serviceLocator.RegisterSingle<IDevicesService, DevicesService>();
            _serviceLocator.RegisterSingle<ITypeDevicesService, TypeDevicesService>();
            _serviceLocator.RegisterSingle<IHistoryService, HistoryService>();
            _serviceLocator.RegisterSingle<INavigationService, NavigationService>();
            _serviceLocator.RegisterSingle<IPopupNavigationService, PopupNavigationService>();
            _serviceLocator.RegisterSingle<IMessagingService, MessagingService>();
            _serviceLocator.RegisterSingle<IPreferenceService, PreferenceService>();
        }

        private void BindViewsAndViewModels()
        {
            // Register all the view models with their corresponding pages
            _navigationService.RegisterViewMapping(
                typeof(EditBuildingDetailViewModel),
                typeof(EditBuildingDetailView));
            _navigationService.RegisterViewMapping(
                typeof(EditBuildingListViewModel),
                typeof(EditBuildingListView));
            _navigationService.RegisterViewMapping(
                typeof(BuildingListViewModel),
                typeof(BuildingListView));
            _navigationService.RegisterViewMapping(
                typeof(EditDeviceDetailViewModel),
                typeof(EditDeviceDetailView));
            _navigationService.RegisterViewMapping(
                typeof(ChangesHistoryViewModel),
                typeof(ChangesHistoryView));
            _navigationService.RegisterViewMapping(
                typeof(LoginViewModel),
                typeof(LoginView));
            _navigationService.RegisterViewMapping(
                typeof(ProfileViewModel),
                typeof(ProfileView));
            _navigationService.RegisterViewMapping(
                typeof(EditRoomDetailViewModel),
                typeof(EditRoomDetailView));
            _navigationService.RegisterViewMapping(
                typeof(RoomListViewModel),
                typeof(RoomListView));
            _navigationService.RegisterViewMapping(
                typeof(EditUserDetailViewModel),
                typeof(EditUserDetailView));
            _navigationService.RegisterViewMapping(
                typeof(EditUserListViewModel),
                typeof(EditUserListView));
            _navigationService.RegisterViewMapping(
                typeof(HomeViewModel),
                typeof(HomeView));

            // Register all the popups
            _popupNavigationService.RegisterViewMapping(
                typeof(AddPopupViewModel),
                typeof(AddPopup));
            _popupNavigationService.RegisterViewMapping(
                typeof(AddedBuildingViewModel),
                typeof(AddedBuildingPopup));
            _popupNavigationService.RegisterViewMapping(
                typeof(AddedBuildingRoomViewModel),
                typeof(AddedBuildingRoomPopup));
            _popupNavigationService.RegisterViewMapping(
                typeof(EditViewModel),
                typeof(EditPopup));
            _popupNavigationService.RegisterViewMapping(
                typeof(AddedRoomViewModel),
                typeof(AddedRoomPopup));
            _popupNavigationService.RegisterViewMapping(
                typeof(AddUserViewModel),
                typeof(AddUserPopup));
        }

        private void InitNavigation()
        {
            _navigationService = _navigationService ?? _serviceLocator.Resolve<INavigationService>();
            _popupNavigationService = _popupNavigationService ?? _serviceLocator.Resolve<IPopupNavigationService>();
            _preferenceService = _preferenceService ?? _serviceLocator.Resolve<IPreferenceService>();

            lock (typeof(App))
            {
                if (_navigationInitialized)
                    return;

                _navigationInitialized = true;
            }

            BindViewsAndViewModels();

            NavigationToFirstViewModel();
        }

        private void NavigationToFirstViewModel()
        {
            if (_preferenceService.IsUserLoggedIn)
            {
                _navigationService.SetNewNavigationPage<HomeViewModel>();
            }
            else
            {
                _navigationService.SetNewNavigationPage<LoginViewModel>();
            }
        }

        #endregion
    }
}
