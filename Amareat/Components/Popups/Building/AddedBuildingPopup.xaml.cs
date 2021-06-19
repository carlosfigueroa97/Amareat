using System;
using obj = System.Object;
using System.Collections.Generic;
using Amareat.Components.Popups.Base;
using Xamarin.Forms;
using txtEvent = Xamarin.Forms.TextChangedEventArgs;

namespace Amareat.Components.Popups.Building
{
    public partial class AddedBuildingPopup : BasePopupPage
    {
        AddedBuildingViewModel vm => BindingContext as AddedBuildingViewModel;

        public AddedBuildingPopup()
        {
            InitializeComponent();
            building.TextEntryChanged += BuildingEntryChanged;
        }

        protected override void OnDisappearing()
        {
            building.TextEntryChanged -= BuildingEntryChanged;
            base.OnDisappearing();
        }

        #region Methods

        void BuildingEntryChanged(obj sender, txtEvent e) 
        { 
            var entry = sender as Entry;
            vm.BuildingName = entry.Text;
        }

        #endregion
    }
}
