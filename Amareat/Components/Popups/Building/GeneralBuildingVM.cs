using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Amareat.Components.Base;
using Amareat.Models.API.Requests.Rooms;

namespace Amareat.Components.Popups.Building
{
    public class GeneralBuildingVM : BaseVm
    {
        #region Properties

        public static ObservableCollection<SimpleRoom> RoomsToSaveList 
        {
            get;
            set;
        }

        #endregion

        public GeneralBuildingVM()
        {
            if(RoomsToSaveList is null)
                RoomsToSaveList = new ObservableCollection<SimpleRoom>();
        }
    }
}
