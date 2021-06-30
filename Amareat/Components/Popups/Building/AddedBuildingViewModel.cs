using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.Wrappers;
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

        private string _buildingName;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }

        public Command AddRoomCommand { get; set; }
        
        public string BuildingName
        {
            get => _buildingName;
            set => SetProperty(ref _buildingName, value);
        }

        public ObservableCollection<SimpleRoom> RoomsToSaveList
            => RoomsToAddWrapper.RoomsToSaveList;

        public RoomsFlagsWrapper RoomsFlagsWrapper =>
            RoomsToAddWrapper.RoomsFlagsWrapper;

        #endregion

        #endregion

        public AddedBuildingViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;

            ClosePopup = new Command(async () =>
                await ExecuteClosePopupCommand());
            AddRoomCommand = new Command(async () =>
                await ExecuteAddRoomCommand());

            if(RoomsToAddWrapper.RoomsToSaveList is null)
            {
                RoomsToAddWrapper.RoomsToSaveList = new ObservableCollection<SimpleRoom>();
            }

            if (RoomsToAddWrapper.RoomsFlagsWrapper is null)
            {
                RoomsToAddWrapper.RoomsFlagsWrapper = new RoomsFlagsWrapper(false, true);
            }
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

        #endregion

        #region Private Methods
        #endregion
    }
}
