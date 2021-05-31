using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Models.API.Responses.Devices;
using Amareat.Models.Wrappers;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Xamarin.Forms;

namespace Amareat.Components.Views.Rooms.Home
{
    public class RoomListViewModel : BaseViewModel<Building>
    {
        #region Private Properties

        private IDevicesService _devicesService;
        private ICrashReporting _crashReporting;

        private Building _building;

        private ObservableCollection<GroupDeviceWithRoomsWrapper> _deviceList;

        private bool _isEmpty;

        #endregion

        #region Public Properties

        public Command GetDataCommand { get; set; }

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

        #endregion

        public RoomListViewModel(
            IDevicesService devicesService,
            ICrashReporting crashReporting)
        {
            _devicesService = devicesService;
            _crashReporting = crashReporting;

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
