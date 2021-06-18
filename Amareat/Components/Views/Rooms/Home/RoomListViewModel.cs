using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Newtonsoft.Json;
using SocketIOClient;
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

        private SocketIO _socket;

        #endregion

        #region Public Properties

        //public Command GetDataCommand { get; set; }
        public Command ChangePowerCommand { get; set; }
        public Command ConnectSocketCommand { get; set; }
        public Command DisconnectSocketCommand { get; set; }

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

        public SocketIO Socket
        {
            get => _socket;
            set => SetProperty(ref _socket, value);
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
            //GetDataCommand = new Command(async () => await OnGetDataAsync());
            ChangePowerCommand = new Command(async (obj) => await OnChangePowerAsync(obj));
            ConnectSocketCommand = new Command(async () => await OnConnectSocketAsync());
            DisconnectSocketCommand = new Command(async () => await OnDisconnectSocketAsync());
        }

        private async Task OnDisconnectSocketAsync()
        {
            try
            {
                if (Socket.Connected)
                {
                    await Socket.DisconnectAsync();
                }

                Socket.OnConnected -= Client_OnConnected;
                Socket.OnError -= Client_OnError;
                Socket.OnReconnecting -= Client_OnReconnecting;
                Socket.OnReconnectFailed -= Client_OnReconnectFailed;
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        private async Task OnConnectSocketAsync()
        {
            try
            {
                IsBusy = true;

                var socketOptions = new SocketIOOptions
                {
                    Reconnection = true,
                    ReconnectionDelay = 30,
                    ReconnectionDelayMax = 40,
                    AllowedRetryFirstConnection = true,
                    EIO = 4
                };

                Socket = new SocketIO(ConstantGlobal.ApiUrl, socketOptions);

                Socket.OnConnected += Client_OnConnected;
                Socket.OnError += Client_OnError;
                Socket.OnReconnecting += Client_OnReconnecting;
                Socket.OnReconnectFailed += Client_OnReconnectFailed;

                if (Socket.Disconnected)
                {
                    await Socket.ConnectAsync();
                }    
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

        private void Client_OnReconnectFailed(object sender, Exception e)
        {
#if DEBUG
            Debug.WriteLine($"Error to reconnect: {e.Message}");
#endif
        }

        private void Client_OnReconnecting(object sender, int e)
        {
#if DEBUG
            Debug.WriteLine("Trying reconnect");
#endif
        }

        private void Client_OnError(object sender, string e)
        {
#if DEBUG
            Debug.WriteLine($"Error to connect: {e}");
#endif
        }

        private void Client_OnConnected(object sender, EventArgs e)
        {
#if DEBUG
            Debug.WriteLine("Connection successfully");
#endif
            var socket = (SocketIO)sender;
            var model = new DevicesByBuilding
            {
                IdBuilding = Building.Id,
                Status = "0"
            };

#if DEBUG
            Debug.WriteLine(socket.Id);
#endif

            if (socket.Connected)
            {
                var item = JsonConvert.SerializeObject(model);

                socket.EmitAsync("client data buildings", item);

                socket.On("get devices by buildings", (response) =>
                {
                    try
                    {
                        var json = response.GetValue().GetRawText();

#if DEBUG
                        Debug.WriteLine(json);
#endif
                        var data = JsonConvert.DeserializeObject<DeviceListWithRoom>(json.ToString());
                        if (data?.Data?.Count == 0)
                        {
                            IsEmpty = true;
                            DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>();
                        }
                        else
                        {
                            if(DeviceList is null)
                            {
                                var devicesByRooms = data
                                .Data
                                .Select(x => new GroupDeviceWithRoomsWrapper(x.Room, x.Devices))
                                .ToList();

                                DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>(devicesByRooms);
                            }
                            else
                            {
                                foreach (var elem in data.Data)
                                {
                                    foreach (var deviceList in DeviceList)
                                    {
                                        foreach (var device in deviceList)
                                        {
                                            if(elem.Room.Id == device.IdRoom)
                                            {
                                                device.Value = elem.Devices
                                                .FirstOrDefault(x => x.Id == device.Id).Value;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _crashReporting.TrackError(ex);
                    }
                });
            }
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

        // TODO: Temporarily disabled
        //private async Task OnGetDataAsync()
        //{
        //    try
        //    {
        //        List<DeviceWithRoom> devices = new List<DeviceWithRoom>();
        //        IsBusy = true;
        //        IsEmpty = false;

        //        var response = await _devicesService
        //            .GetDevicesByBuilding("0", Building.Id, default);

        //        if(response?.Data?.Count == 0)
        //        {
        //            IsEmpty = true;
        //            DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>();
        //        }
        //        else
        //        {
        //            var devicesByRooms = response
        //                .Data
        //                .Select(x => new GroupDeviceWithRoomsWrapper(x.Room, x.Devices));

        //            DeviceList = new ObservableCollection<GroupDeviceWithRoomsWrapper>(devicesByRooms);
        //        }

        //        OnPropertyChanged(nameof(DeviceList));
        //    }
        //    catch (Exception ex)
        //    {
        //        _crashReporting.TrackError(ex);
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        #endregion

        #region Public Methods

        #endregion
    }
}
