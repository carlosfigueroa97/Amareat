using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Helpers;
using Amareat.Models.API.Requests.Devices;
using Amareat.Models.API.Requests.History;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Models.API.Responses.Devices;
using Amareat.Models.Wrappers;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.PopupNavigation.Interfaces;
using Amareat.Services.Preferences.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Rooms.Home
{
    public class RoomListViewModel : BaseViewModel<Building>
    {
        #region Private Properties

        private IDevicesService _devicesService;
        private ICrashReporting _crashReporting;
        private IPopupNavigationService _popupNavigationService;
        private IHistoryService _historyService;
        private IPreferenceService _preferenceService;

        private Building _building;

        private ObservableCollection<GroupDeviceWithRoomsWrapper> _deviceList;

        private bool _isEmpty;

        private Models.API.Responses.Devices.Device _deviceSelected;

        #endregion

        #region Public Properties

        public Command GetDataCommand { get; set; }
        public Command ChangePowerCommand { get; set; }

        public Building Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        public ObservableCollection<GroupDeviceWithRoomsWrapper> DeviceList
        {
            get => _deviceList;
            set => SetProperty(ref _deviceList, value);
        }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }

        public Models.API.Responses.Devices.Device DeviceSelected
        {
            get => _deviceSelected;
            set => SetProperty(ref _deviceSelected, value);
        }

        #endregion

        public RoomListViewModel(
            IDevicesService devicesService,
            ICrashReporting crashReporting,
            IPopupNavigationService popupNavigationService,
            IHistoryService historyService,
            IPreferenceService preferenceService)
        {
            _devicesService = devicesService;
            _crashReporting = crashReporting;
            _popupNavigationService = popupNavigationService;
            _historyService = historyService;
            _preferenceService = preferenceService;

            InitCommand();
        }

        public override Task Init(Building parameter)
        {
            Building = parameter;
            return base.Init(parameter);
        }

        #region Private Methods

        private void InitCommand()
        {
            GetDataCommand = new Command(async () => await OnGetDataAsync());
            ChangePowerCommand = new Command(async (obj) => await OnChangePowerAsync(obj));
        }

        private async Task OnChangePowerAsync(object obj)
        {
            try
            {
                IsBusy = true;

                if(obj == null)
                {
                    return;
                }

                DeviceSelected = (Models.API.Responses.Devices.Device)obj;

                var model = new EditDevice {
                    Id = DeviceSelected.Id,
                    Name = DeviceSelected.Name,
                    Status = DeviceSelected.Status,
                    Value = !DeviceSelected.Value
                };

                var response = await _devicesService.EditDevice(model, default);

                if (response)
                {
                    foreach (var devices in DeviceList)
                    {
                        foreach (var item in devices)
                        {
                            if (item.Id == DeviceSelected.Id)
                            {
                                item.Value = !DeviceSelected.Value;
                            }
                        }
                    }

                    await OnSaveChange();

                    OnPropertyChanged(nameof(DeviceList));
                }
                else
                {
                    await _popupNavigationService
                        .ShowErrorDialog(Resources.Error, Resources.AnUnexpectedErrorHasOcurred);
                }
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
            finally
            {
                DeviceSelected = null;
                IsBusy = false;
            }
        }

        private async Task OnSaveChange()
        {
            var idUser = _preferenceService.GetPreference(ConstantGlobal.IdUser);

            var history = new SaveHistory
            {
                IdBuilding = DeviceSelected.IdBuilding,
                Change = !DeviceSelected.Value,
                IdDevice = DeviceSelected.Id,
                IdRoom = DeviceSelected.IdRoom,
                IdUser = idUser.ToString()
            };

            await _historyService.SaveHistory(history, default);
        }

        private async Task OnGetDataAsync()
        {
            try
            {
                List<DeviceWithRoom> devices = new List<DeviceWithRoom>();
                IsBusy = true;
                IsEmpty = false;

                var response = await _devicesService
                    .GetDevicesByBuilding("0", Building.Id, default);

                if(response?.Data?.Count == 0)
                {
                    IsEmpty = true;
                    DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>();
                }
                else
                {
                    var devicesByRooms = response
                        .Data
                        .Select(x => new GroupDeviceWithRoomsWrapper(x.Room, x.Devices));

                    DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>(devicesByRooms);
                }

                OnPropertyChanged(nameof(DeviceList));
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

        #region Public Methods

        #endregion
    }
}
