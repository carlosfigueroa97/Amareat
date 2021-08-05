using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.Wrappers;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Popups.Building
{
    public class AddedBuildingRoomViewModel : BaseVm
    {
        #region Properties & Commands
        
        #region Private Properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        private string _roomName;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }
        public Command AddRoom { get; set; }

        public string RoomName
        {
            get => _roomName;
            set => SetProperty(ref _roomName, value);
        }

        #endregion

        #endregion

        public AddedBuildingRoomViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;

            ClosePopup = new Command(async () =>
                await ExecuteClosePopupCommand());
            AddRoom = new Command(async () =>
                await ExecuteAddRoomCommand());
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

        }

        async Task ExecuteAddRoomCommand()
        {
            try
            {
                if (!string.IsNullOrEmpty(RoomName))
                {
                    var model = new SimpleRoom
                    {
                        Name = RoomName 
                    };

                    RoomsToAddWrapper.RoomsToSaveList.Add(model);

                    RoomsToAddWrapper.RoomsFlagsWrapper.IsListViewVisible = true;
                    RoomsToAddWrapper.RoomsFlagsWrapper.IsLabelVisible = false;

                    await ExecuteClosePopupCommand();
                }
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
