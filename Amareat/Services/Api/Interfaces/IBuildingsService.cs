using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.Buildings;
using Amareat.Models.API.Responses.Buildings;

namespace Amareat.Services.Api.Interfaces
{
    public interface IBuildingsService
    {
        Task<BuildingData> SaveBuilding(SaveBuilding building, CancellationToken cancellationToken);

        Task<BuildingList> GetBuildings(string status, CancellationToken cancellationToken);

        Task<Building> GetBuilding(string id, CancellationToken cancellationToken);

        Task<bool> EditBuilding(EditBuilding editBuilding, CancellationToken cancellationToken);

        Task<BuildingList> SearchBuilding(string searchValue, CancellationToken cancellationToken);
    }
}
