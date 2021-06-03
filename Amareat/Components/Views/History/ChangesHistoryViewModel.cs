using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amareat.Components.Base;
using HistoryApi = Amareat.Models.API.Responses.History;
using Amareat.Services.Api.Interfaces;
using Amareat.Services.Crash.Interfaces;
using MvvmHelpers.Commands;

namespace Amareat.Components.Views.History
{
    public class ChangesHistoryViewModel : BaseVm
    {
        #region Properties & Commands

        #region Private Properties

        private readonly ICrashReporting _crashReporting;
        private readonly IHistoryService _historyService;

        private CancellationToken cancellationToken =
            new CancellationTokenSource().Token;

        private bool _isEmpty;

        #endregion

        #region Public Properties

        public Command GetHistoryDataCommand { get; set; }

        public List<HistoryApi.History> HistoryListData { get; set; }

        public bool IsEmpty
        {
            get => _isEmpty;
            set => SetProperty(ref _isEmpty, value);
        }

        #endregion

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
                IsBusy = true;
                IsEmpty = true;

                var apiResponse =
                    await _historyService.GetHistory(cancellationToken);

                if (apiResponse?.Data?.Count != 0)
                {
                    HistoryListData = apiResponse.Data;
                    IsEmpty = false;
                }

                OnPropertyChanged(nameof(HistoryListData));
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
    }
}
