using System;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Models.API.Requests.History;
using Amareat.Models.API.Responses.History;

namespace Amareat.Services.Api.Interfaces
{
    public interface IHistoryService
    {
        Task<bool> SaveHistory(SaveHistory history, CancellationToken cancellationToken);

        Task<HistoryList> GetHistory(CancellationToken cancellationToken);
    }
}
