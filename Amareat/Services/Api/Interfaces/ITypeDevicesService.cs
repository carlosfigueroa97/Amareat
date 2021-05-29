using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.TypeDevices;
using Amareat.Models.API.Responses.TypeDevices;

namespace Amareat.Services.Api.Interfaces
{
    public interface ITypeDevicesService
    {
        Task<bool> SaveTypeDevice(SaveTypeDevice typeDevice, CancellationToken cancellationToken);

        Task<TypeDeviceList> GetTypeDevices(string status, CancellationToken cancellationToken);
    }
}
