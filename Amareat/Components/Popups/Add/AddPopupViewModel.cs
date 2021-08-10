using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Components.Popups.Building;
using Amareat.Components.Popups.User;
using Amareat.Helpers;
using Amareat.Models.Wrappers;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Interfaces;
using MvvmHelpers.Commands;
using Model = Amareat.Models.API.Responses.Buildings;

namespace Amareat.Components.Popups.Add
{
    public class AddPopupViewModel : BaseVm
    {
        #region Properties & Commands

        #region Private properties

        private readonly IPreferenceService _preferenceService;
        private readonly IPopupNavigationService _popupNavigationService;
        private readonly ICrashReporting _crashReporting;

        private AddMainMenuWrapper _itemSelected;
        private BuildingPickerWrapper _pickerWrapper;

        #endregion

        #region Public properties

        public Command ItemSelectedCommand { get; set; }

        public ObservableCollection<AddMainMenuWrapper> AddList { get; set; }

        public AddMainMenuWrapper ItemSelected
        {
            get => _itemSelected;
            set => SetProperty(ref _itemSelected, value);
        }

        public ObservableCollection<Model.Building> BuildingList
            => BuildingsListMainMenuWrapper.BuildingList;

        public BuildingPickerWrapper PickerWrapper
        {
            get => _pickerWrapper;
            set => SetProperty(ref _pickerWrapper, value);
        }

        #endregion

        #endregion

        public AddPopupViewModel(
            IPreferenceService preferenceService,
            IPopupNavigationService popupNavigationService,
            ICrashReporting crashReporting)
        {
            _preferenceService = preferenceService;
            _popupNavigationService = popupNavigationService;
            _crashReporting = crashReporting;

            InitList();

            ItemSelectedCommand = new Command(async () =>
                await ExecuteItemSelectedCommandAsync());
        }

        #region Methods

        #region Private methods

        private void InitList()
        {
            AddList = new ObservableCollection<AddMainMenuWrapper>() {
                new AddMainMenuWrapper
                {
                    ItemText = Resources.AddUser,
                    BackgrounStackColor = Colors.WhiteColor,
                    ItemStyle = Helpers.Styles.NormalPopupLabel,
                    InvokeView = OnUserVm
                },
                new AddMainMenuWrapper
                {
                    ItemText = Resources.AddBuilding,
                    BackgrounStackColor = Colors.WhiteColor,
                    ItemStyle = Helpers.Styles.NormalPopupLabel,
                    InvokeView = OnBuildingVm
                },
                new AddMainMenuWrapper
                {
                    ItemText = Resources.AddRoom,
                    BackgrounStackColor = Colors.WhiteColor,
                    ItemStyle = Helpers.Styles.NormalPopupLabel,
                    InvokeView = OnRoomVm
                },
                new AddMainMenuWrapper
                {
                    ItemText = Resources.AddDevice,
                    BackgrounStackColor = Colors.WhiteColor,
                    ItemStyle = Helpers.Styles.NormalPopupLabel,
                    InvokeView = OnDeviceVm
                },
                new AddMainMenuWrapper
                {
                    ItemText = Resources.Cancel,
                    BackgrounStackColor = Colors.SecondLightBlueSkyColor,
                    ItemStyle = Helpers.Styles.CancelPopupLabel,
                    InvokeView = OnCancelVm
                }
            };

            OnPropertyChanged(nameof(AddList));
        }

        async Task ExecuteItemSelectedCommandAsync()
        {
            try
            {
                if (ItemSelected == null)
                {
                    return;
                }

                await ItemSelected.InvokeView.Invoke();
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
            finally
            {
                ItemSelected = null;
            }
        }

        private async Task OnUserVm()
        {
            await _popupNavigationService.PresentPopupPage<AddUserViewModel>();
        }

        private async Task OnBuildingVm()
        {
            await _popupNavigationService.PresentPopupPage<AddedBuildingViewModel>();
        }

        private async Task OnRoomVm()
        {
            PreparateData();

            await _popupNavigationService.
                PresentPopupPage<AddedBuildingRoomViewModel, 
                BuildingPickerWrapper>(PickerWrapper);
        }

        private async Task OnDeviceVm()
        {
            // TODO: Change view
            await _popupNavigationService.PresentPopupPage<AddUserViewModel>();
        }

        private async Task OnCancelVm()
        {
            await _popupNavigationService.DismissPopupPage();
        }

        private void PreparateData()
        {
            PickerWrapper = new BuildingPickerWrapper
            {
                IsCalledFromAddPopup = true,
            };

            PickerWrapper.Data = 
                new List<Model.Building>();

            foreach (var item in BuildingList) 
            {
                PickerWrapper.Data.Add(item);
            }
        }

        #endregion

        #region Public methods
        #endregion

        #endregion
    }
}
