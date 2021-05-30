using System;
using Amareat.Components.Base;

namespace Amareat.Components.Views.History
{
    public class ChangesHistoryViewModel : BaseVm
    {
        #region Properties & Commands

        public string DeviceName { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        public DateTime Date { get; set; }

        #endregion
        public ChangesHistoryViewModel()
        {
        }
    }
}
