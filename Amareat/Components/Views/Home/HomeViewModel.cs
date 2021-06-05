using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Amareat.Components.Base;
using Amareat.Components.Views.Buildings.Home;
using Amareat.Components.Views.History;
using Amareat.Components.Views.Profile;
using Amareat.Services.Preferences.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Components.Popups.Add;
using Amareat.Services.Crash.Interfaces;
using System;

namespace Amareat.Components.Views.Home
{
    public class HomeViewModel : BaseVm, INotifyPropertyChanged
    {
        private IPreferenceService _preferenceService;
        private BuildingListViewModel _buildingListViewModel;
        private ChangesHistoryViewModel _changesHistoryViewModel;
        private ProfileViewModel _profileViewModel;
        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        public Command<string> TapCommand { get; set; }

        public BuildingListViewModel BuildingListViewModel
        {
            get => _buildingListViewModel;
            private set => SetProperty(ref _buildingListViewModel, value);
        }

        public ChangesHistoryViewModel ChangesHistoryViewModel
        {
            get => _changesHistoryViewModel;
            private set => SetProperty(ref _changesHistoryViewModel, value);
        }

        public ProfileViewModel ProfileViewModel
        {
            get => _profileViewModel;
            private set => SetProperty(ref _profileViewModel, value);
        }

        public bool IsAdmin => _preferenceService.IsAdmin;

        public HomeViewModel(
            BuildingListViewModel buildingListViewModel,
            ChangesHistoryViewModel changesHistoryViewModel,
            ProfileViewModel profileViewModel,
            IPreferenceService preferenceService,
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting)
        {
            BuildingListViewModel = buildingListViewModel;
            ChangesHistoryViewModel = changesHistoryViewModel;
            ProfileViewModel = profileViewModel;
            _preferenceService = preferenceService;
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;

            TapCommand = new Command<string>(async (param) =>
                await ExecuteTapCommand(param));
        }

        public async override Task Init()
        {
            await BuildingListViewModel.Init();
            await ChangesHistoryViewModel.Init();
            await ProfileViewModel.Init();
        }

        async Task ExecuteTapCommand(string optionClicked)
        {
            try
            {
                if (optionClicked == "AddPopup")
                {
                    await _popupNavigationService.PresentPopupPage<AddPopupViewModel>();
                }
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }
    }
}
