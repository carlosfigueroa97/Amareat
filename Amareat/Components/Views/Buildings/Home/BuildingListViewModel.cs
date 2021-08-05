using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Views.Rooms.Home;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Models.Wrappers;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Navigation.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Buildings.Home
{
    public class BuildingListViewModel : BaseVm
    {
        #region Private Properties

        private INavigationService _navigationService;
        private IBuildingsService _buildingsService;
        private ICrashReporting _crashReporting;

        private CancellationToken _cancellationToken => new CancellationTokenSource().Token;

        private bool _isEmpty;

        private Building _selectedItem;

        #endregion

        #region Public Properties

        public Command GetBuildingsCommand { get; set; }
        public Command SelectedItemCommand { get; set; }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }

        public Building SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<Building> BuildingList
            => BuildingsListMainMenuWrapper.BuildingList;

        #endregion

        public BuildingListViewModel(
            INavigationService navigationService,
            IBuildingsService buildingsService,
            ICrashReporting crashReporting)
        {
            _navigationService = navigationService;
            _buildingsService = buildingsService;
            _crashReporting = crashReporting;

            InitCommand();
        }

        #region Private Methods

        private void InitCommand()
        {
            GetBuildingsCommand = new Command(async () => await OnGetBuildingsAsync());
            SelectedItemCommand = new Command(async (object obj) => await OnSelectedItemAsync(obj));
        }

        private async Task OnSelectedItemAsync(object obj)
        {
            if (obj is null)
            {
                return;
            }

            SelectedItem = obj as Building;

            await _navigationService
                .NavigateTo<RoomListViewModel, Building>(SelectedItem);

            SelectedItem = null;
        }

        private async Task OnGetBuildingsAsync()
        {
            try
            {
                IsBusy = true;
                IsEmpty = false;

                var response = await _buildingsService.GetBuildings("0", _cancellationToken);

                if (response?.Data?.Count == 0)
                {
                    IsEmpty = true;
                    BuildingsListMainMenuWrapper.BuildingList = 
                        new ObservableCollection<Building>();
                }
                else
                {
                    BuildingsListMainMenuWrapper.BuildingList = 
                        new ObservableCollection<Building>(response.Data);
                }
                OnPropertyChanged(nameof(BuildingList));
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
