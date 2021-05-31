using System;
using System.Collections.Generic;
using Amareat.Components.Views.Home;
using Xamarin.Forms;

namespace Amareat.Components.Views.Buildings.Home
{
    public partial class BuildingListView : ContentPage
    {
        private HomeViewModel Vm => BindingContext as HomeViewModel;

        public BuildingListView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Vm.GetBuildingsCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
