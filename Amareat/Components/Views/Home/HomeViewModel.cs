using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Views.Buildings.Home;
using Amareat.Components.Views.History;
using Amareat.Components.Views.Profile;
using Amareat.Services.Preferences.Interfaces;

namespace Amareat.Components.Views.Home
{
    public class HomeViewModel : BaseVm
    {
        private IPreferenceService _preferenceService;
        private BuildingListViewModel _buildingListViewModel;
        private ChangesHistoryViewModel _changesHistoryViewModel;
        private ProfileViewModel _profileViewModel;

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
            IPreferenceService preferenceService)
        {
            BuildingListViewModel = buildingListViewModel;
            ChangesHistoryViewModel = changesHistoryViewModel;
            ProfileViewModel = profileViewModel;
            _preferenceService = preferenceService;
        }

        public async override Task Init()
        {
            await BuildingListViewModel.Init();
            await ChangesHistoryViewModel.Init();
            await ProfileViewModel.Init();
        }
    }
}
