using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Helpers;
using Model = Amareat.Models.API.Responses.Buildings;
using Amareat.Models.API.Requests.Buildings;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.Wrappers;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Popups.Building
{
    public class AddedBuildingViewModel : BaseVm
    {
        #region Properties & Commands

        #region Private Properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;
        private IBuildingsService _buildingsService;
        private CancellationToken _cancellationToken =
            new CancellationTokenSource().Token;

        private string _buildingName;
        private bool _isBuildingNameEmpty;
        private string _errorBuildingNameMessage = string.Empty;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }
        public Command AddRoomCommand { get; set; }
        public Command SaveBuildingCommand { get; set; }

        public string BuildingName
        {
            get => _buildingName;
            set => SetProperty(ref _buildingName, value);
        }

        public bool IsBuildingNameEmpty
        {
            get => _isBuildingNameEmpty;

            set
            {
                SetProperty(ref _isBuildingNameEmpty, value);
                OnPropertyChanged(nameof(BuildingName));
            }
        }

        public string ErrorBuildingNameMessage
        {
            get => _errorBuildingNameMessage;

            set
            {
                SetProperty(ref _errorBuildingNameMessage, value);
                OnPropertyChanged(nameof(BuildingName));
            }
        }

        public ObservableCollection<SimpleRoom> RoomsToSaveList
            => RoomsToAddWrapper.RoomsToSaveList;

        public RoomsFlagsWrapper RoomsFlagsWrapper =>
            RoomsToAddWrapper.RoomsFlagsWrapper;

        #endregion

        #endregion

        public AddedBuildingViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting,
            IBuildingsService buildingsService)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;
            _buildingsService = buildingsService;

            ClosePopup = new Command(async () =>
                await ExecuteClosePopupCommand());
            AddRoomCommand = new Command(async () =>
                await ExecuteAddRoomCommand());
            SaveBuildingCommand = new Command(async () =>
                await ExecuteValidateData());

            InitializeRoomsWrapper();
        }

        #region Public Methods

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
            finally
            {
                if (RoomsToSaveList.Count > 0)
                    RoomsToSaveList.Clear();

                RoomsToAddWrapper.RoomsFlagsWrapper.IsListViewVisible = false;
                RoomsToAddWrapper.RoomsFlagsWrapper.IsLabelVisible = true;
            }

        }

        async Task ExecuteAddRoomCommand()
        {
            try
            {
                await _popupNavigationService.
                    PresentPopupPage<AddedBuildingRoomViewModel>();
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        async Task ExecuteValidateData()
        {
            try
            {
                if (string.IsNullOrEmpty(BuildingName))
                {
                    InitializeErrorMessage();

                    IsBuildingNameEmpty = true;
                    ErrorBuildingNameMessage = Resources.BuildingNameEmpty;

                    return;
                }

                BuildingName = BuildingName.TrimEnd();

                await ExecuteSaveBuildingCommand();

            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        async Task ExecuteSaveBuildingCommand()
        {
            try
            {
                List<SimpleRoom> RoomsToSaveList =
                    new List<SimpleRoom>(RoomsToAddWrapper.RoomsToSaveList);

                var BuildingToSave = new SaveBuilding
                {
                    Name = BuildingName,
                    Rooms = RoomsToSaveList 
                };

                var newBuilding = await _buildingsService.
                    SaveBuilding(BuildingToSave, _cancellationToken);

                if (newBuilding.Data is null)
                {
                    await _popupNavigationService
                       .ShowErrorDialog(Resources.BuildingNotSaved,
                       Resources.PleaseContactAdministrator);
                    return;
                }

                RegisterNewBuilding(newBuilding.Data);

                await ExecuteClosePopupCommand();

                await _popupNavigationService.
                    ShowToastDialog(Resources.BuildingSaved, 2000);
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        #endregion

        #region Private Methods

        private void InitializeRoomsWrapper()
        {
            if (RoomsToAddWrapper.RoomsToSaveList is null)
            {
                RoomsToAddWrapper.RoomsToSaveList = new ObservableCollection<SimpleRoom>();
            }

            if (RoomsToAddWrapper.RoomsFlagsWrapper is null)
            {
                RoomsToAddWrapper.RoomsFlagsWrapper = new RoomsFlagsWrapper(false, true);
            }
        }

        private void InitializeErrorMessage()
        {
            IsBuildingNameEmpty = false;
            ErrorBuildingNameMessage = string.Empty;
        }

        private void RegisterNewBuilding(Model.Building BuildingToSave)
        {
            BuildingsListMainMenuWrapper.BuildingList.Add(BuildingToSave);
        }

        #endregion
    }
}
