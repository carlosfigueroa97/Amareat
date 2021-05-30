using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using HistoryApi = Amareat.Models.API.Responses.History;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using Amareat.Services.Localization.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Views.History
{
    public class ChangesHistoryViewModel : BaseVm
    {
        #region Properties & Commands

        private readonly ICrashReporting _crashReporting;
        private readonly IHistoryService _historyService;

        private CancellationToken cancellationToken =
            new CancellationTokenSource().Token;

        public Command GetHistoryDataCommand { get; set; }

        public List<HistoryApi.History> HistoryListData { get; set; }
        /*public string DeviceName { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }*/

        #endregion

        public ChangesHistoryViewModel(ICrashReporting crashReporting,
            IHistoryService historyService)
        {
            _crashReporting = crashReporting;
            _historyService = historyService;

            GetHistoryDataCommand = new Command(async () =>
                await ExecuteGetHistoryDataCommand());
        }


        #region Methods

        async Task ExecuteGetHistoryDataCommand()
        {
            try
            {
                var apiResponse =
                    await _historyService.GetHistory(cancellationToken);

                if (apiResponse != null)
                {
                    HistoryListData = apiResponse.Data;
                }

                OnPropertyChanged(nameof(HistoryListData));
            }
            catch (Exception ex)
            {
                _crashReporting.TrackError(ex);
            }
        }

        #endregion
    }
}
