using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Requests.Rooms;
using Model = Amareat.Models.API.Responses.Buildings;
using Amareat.Models.Wrappers;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using MvvmHelpers.Commands;
using Amareat.Helpers;

namespace Amareat.Components.Popups.Building
{
    public class AddedBuildingRoomViewModel : 
        BaseViewModel<Model.BuildingList>
    {
        #region Properties & Commands
        
        #region Private Properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        private string _roomName;
        private ObservableCollection<Model.Building> 
            _buildingList;
        private Model.Building _selectedBuilding;
        private bool _isRoomNameEmpty;
        private string _errorRoomNameMessage;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }
        public Command AddRoom { get; set; }

        public string RoomName
        {
            get => _roomName;
            set => SetProperty(ref _roomName, value);
        }

        public ObservableCollection<Model.Building> 
            BuildingList 
        { 
            get => _buildingList; 
            set => SetProperty(ref _buildingList, value); 
        }

        public Model.Building SelectedBuilding
        {
            get => _selectedBuilding;

            set
            {
                SetProperty(ref _selectedBuilding, value);
                OnPropertyChanged();
            }
        }

        public bool IsRoomNameEmpty
        {
            get => _isRoomNameEmpty;
            
            set
            {
                SetProperty(ref _isRoomNameEmpty, value);
                OnPropertyChanged(nameof(RoomName)); 
            } 
        }

        public string ErrorRoomNameMessage
        {
            get => _errorRoomNameMessage;
            
            set
            {
                SetProperty(ref _errorRoomNameMessage, value);
                OnPropertyChanged(nameof(RoomName));
            }
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

        public override Task Init(Model.BuildingList listData)
        {
            BuildingList = 
                new ObservableCollection<Model.Building>();

            foreach (var item in listData.Data)
            {
                BuildingList.Add(item);
            }

            if (BuildingList.Count >= 1)
                SelectedBuilding = BuildingList[0];
            //else
            //    SelectedBuilding = Resources.NoBuildingFound;

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
                var IsEmpty = RoomNameEmpty();
                if (IsEmpty) return;
                RoomName = RoomName.Trim();

                var model = new SimpleRoom
                {
                    Name = RoomName 
                };

                RoomsToAddWrapper.RoomsToSaveList.Add(model);

                RoomsToAddWrapper.RoomsFlagsWrapper.IsListViewVisible = true;
                RoomsToAddWrapper.RoomsFlagsWrapper.IsLabelVisible = false;

                await ExecuteClosePopupCommand();
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

        async Task ExecuteNoBuildingCommand()
        {
            //TODO: Add code when no building is found 
        }

        #endregion

        #region Private Methods

        private async Task VerifyData()
        {
            if (BuildingList.Count == 1)
                await ExecuteAddRoomCommand();
            else if (BuildingList.Count > 1)
                await ExecuteSaveRoomCommand();
            else
                await ExecuteNoBuildingCommand();
        }

        private bool RoomNameEmpty() 
        {
            if(string.IsNullOrEmpty(RoomName))
            {
                InitializeErrorMessage();

                IsRoomNameEmpty = true;
                ErrorRoomNameMessage = Resources.RoomNameEmpty;

                return IsRoomNameEmpty;
            }

            InitializeErrorMessage();
            return IsRoomNameEmpty;
        }

        private void InitializeErrorMessage() 
        {
            IsRoomNameEmpty = false;
            ErrorRoomNameMessage = string.Empty;
        }

        #endregion
    }
}
