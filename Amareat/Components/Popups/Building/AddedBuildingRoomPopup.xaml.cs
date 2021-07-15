using System;
using obj = System.Object;
using System.Collections.Generic;
using Amareat.Components.Popups.Base;
using Xamarin.Forms;
using txtEvent = Xamarin.Forms.TextChangedEventArgs;

namespace Amareat.Components.Popups.Building
{
    public partial class AddedBuildingRoomPopup : BasePopupPage
    {
        AddedBuildingRoomViewModel vm => 
            BindingContext as AddedBuildingRoomViewModel;

        public AddedBuildingRoomPopup()
        {
            InitializeComponent();
            room.TextEntryChanged += RoomEntryChanged;
        }

        protected override void OnDisappearing()
        {
            room.TextEntryChanged -= RoomEntryChanged;
            base.OnDisappearing();
        }

        #region Methods

        void RoomEntryChanged(obj sender, txtEvent e)
        {
            var entry = sender as Entry;
            vm.RoomName = entry.Text;
        }

        #endregion
    }
}
