using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.Rooms;
using Amareat.Models.API.Responses.Rooms;

namespace Amareat.Services.Api.Interfaces
{
    public interface IRoomsService
    {
        Task<bool> SaveRoom(SaveRoom room, CancellationToken cancellationToken);

        Task<Room> GetRoom(string id, CancellationToken cancellationToken);

        Task<RoomList> GetRooms(string status, CancellationToken cancellationToken);

        Task<bool> EditRoom(EditRoom room, CancellationToken cancellationToken);
    }
}
