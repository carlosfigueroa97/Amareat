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
using Amareat.Services.Api.Interfaces;

namespace Amareat.Components.Popups.Building
{
    public class AddedBuildingRoomViewModel : 
        BaseViewModel<BuildingPickerWrapper>
    {
        #region Properties & Commands
        
        #region Private Properties

        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;
        private readonly IRoomsService _roomsService;

        private string _roomName;
        private ObservableCollection<Model.Building> 
            _buildingList;
        private bool _isCalledFromAddPopup;
        private Model.Building _selectedBuilding;
        private bool _isRoomNameEmpty;
        private string _errorRoomNameMessage;
        private bool _isPickerEmpty;
        private string _errorPickerEmptyMessage;

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

        public bool IsCallledFromAddPopup
        {
            get => _isCalledFromAddPopup;
            set => SetProperty(ref _isCalledFromAddPopup, value); 
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

        public bool IsPickerEmpty
        {
            get => _isPickerEmpty;

            set
            {
                SetProperty(ref _isPickerEmpty, value);
                OnPropertyChanged(nameof(BuildingList));
            }
        }

        public string ErrorPickerEmptyMessage
        {
            get => _errorPickerEmptyMessage;

            set
            {
                SetProperty(ref _errorPickerEmptyMessage, value);
                OnPropertyChanged(nameof(BuildingList)); 
            }
                
        }

        #endregion

        #endregion

        public AddedBuildingRoomViewModel(
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting,
            IRoomsService roomsService)
        {
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;
            _roomsService = roomsService;

            ClosePopup = new Command(async () =>
                await ExecuteClosePopupCommand());
            AddRoom = new Command(async () =>
                await VerifyData());
        }

        #region Public Methods

        public override Task Init(BuildingPickerWrapper listData)
        {
            BuildingList = 
                new ObservableCollection<Model.Building>();

            foreach (var item in listData.Data)
            {
                BuildingList.Add(item);
            }

            IsCallledFromAddPopup = listData.IsCalledFromAddPopup;

            if (BuildingList.Count >= 1)
                SelectedBuilding = BuildingList[0];
            else
                ExecuteNoBuildingCommand();

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
			try
			{
                var IsEmpty = RoomNameEmpty();
                if (IsEmpty) return;
                RoomName = RoomName.Trim();

                var model = new SaveRoom
                {
                    IdBuilding = SelectedBuilding.Id,
                    Name = RoomName,
                };

                var isRoomSaved = await _roomsService.
                    SaveRoom(model, default);
				
                if (!isRoomSaved) 
                {
                    await _popupNavigationService.
                        ShowErrorDialog(Resources.RoomNotSaved,
                        Resources.PleaseContactAdministrator);
                    return;
                }

                await ExecuteClosePopupCommand();

                await _popupNavigationService.
                    ShowToastDialog(Resources.RoomSaved, 2000);
			}
			catch (Exception ex)
			{
                _crashReporting.TrackError(ex);
			}
        }

        void ExecuteNoBuildingCommand()
        {
            IsPickerEmpty = true;
            ErrorPickerEmptyMessage = Resources.BuildingEmptyMessage;
        }

        #endregion

        #region Private Methods

        private async Task VerifyData()
        {
            if (!IsCallledFromAddPopup)
                //Add Room into Room List
                await ExecuteAddRoomCommand();
            else 
                //Save Room into database
                await ExecuteSaveRoomCommand();
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
