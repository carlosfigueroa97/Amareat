using System;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.Devices;
using Amareat.Models.API.Responses.Devices;

namespace Amareat.Services.Api.Interfaces
{
    public interface IDevicesService
    {
        Task<bool> SaveDevice(SaveDevice device, CancellationToken cancellationToken);

        Task<DeviceListWithRoom> GetDevicesByBuilding(string status, string idBuilding, CancellationToken cancellationToken);

        Task<Device> GetDevice(string id, CancellationToken cancellationToken);

        Task<DeviceList> GetDevices(string status, CancellationToken cancellationToken);

        Task<bool> EditDevice(EditDevice device, CancellationToken cancellationToken);
    }
}
