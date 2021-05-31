using System.Threading.Tasks;
using Amareat.Components.Base;
using Amareat.Models.API.Responses.Buildings;
using Amareat.Services.Api.Interfaces;

namespace Amareat.Components.Views.Rooms.Home
{
    public class RoomListViewModel : BaseViewModel<Building>
    {
        #region Private Properties

        private IRoomsService _roomsService;
        private IDevicesService _devicesService;

        private Building _building;

        #endregion

        #region Public Properties

        public Building Building
        {
            get => _building;
            set => SetProperty(ref _building, value);
        }

        #endregion

        public RoomListViewModel(
            IRoomsService roomsService,
            IDevicesService devicesService)
        {
            _roomsService = roomsService;
            _devicesService = devicesService;
        }

        public override Task Init(Building parameter)
        {
            Building = parameter;
            return base.Init(parameter);
        }

        #region Private Methods

        #endregion

        #region Public Methods

        #endregion
    }
}
