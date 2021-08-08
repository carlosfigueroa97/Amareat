using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.BindingData;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.Wrappers;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Popups.Building
{
    public class AddedBuildingRoomViewModel : 
        BaseViewModel<BindingBuildingAndRoomList>
    {
        #region Properties & Commands
        
        #region Private Properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        private string _roomName;
        //private string _idBuilding;
        //private string _buildingName;
        private ObservableCollection<BindingBuildingAndRoom> 
            _buildingList;
        private string _selectedBuilding;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }
        public Command AddRoom { get; set; }

        public string RoomName
        {
            get => _roomName;
            set => SetProperty(ref _roomName, value);
        }

        /*public string IdBuilding
        {
            get => _idBuilding;
            set => SetProperty(ref _idBuilding, value); 
        }

        public string BuildingName
        {
            get => _buildingName;
            set => SetProperty(ref _buildingName, value);
        }*/

        public ObservableCollection<BindingBuildingAndRoom> 
            BuildingList 
        { 
            get => _buildingList; 
            set => SetProperty(ref _buildingList, value); 
        }

        public string SelectedBuilding
        {
            get => _selectedBuilding;
            set => SetProperty(ref _selectedBuilding, value); 
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
                await VerifyData());
        }

        #region Public Methods

        public override Task Init(BindingBuildingAndRoomList data)
        {
            /*BuildingName = data.BuildingName;
            IdBuilding = data.IdBuilding;*/
            BuildingList = 
                new ObservableCollection<BindingBuildingAndRoom>();

            foreach (var item in data.BindingBuildingList)
            {
                BuildingList.Add(item);
            }

            return base.Init();
        }

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

        async Task ExecuteSaveRoomCommand()
        {
            //TODO: Call the saving method 
        }

        #endregion

        #region Private Methods

        private async Task VerifyData()
        {
            if (BuildingList.Count == 1)
                await ExecuteAddRoomCommand();
            else
                await ExecuteSaveRoomCommand(); 
        }

        #endregion
    }
}
