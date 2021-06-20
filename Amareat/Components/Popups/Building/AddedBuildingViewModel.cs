using System;
using System.Threading.Tasks;
using Amareat.Components.Base;
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
        private bool _isListViewVisible;

        #endregion

        #region Public Properties

        public Command ClosePopup { get; set; }

        public Command AddRoomCommand { get; set; }
        
        public string BuildingName
        {
            get => _buildingName;
            set => SetProperty(ref _buildingName, value);
        }

        public bool IsListViewVisible
        {
            get => _isListViewVisible;
            set => SetProperty(ref _isListViewVisible, value);
        }

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
