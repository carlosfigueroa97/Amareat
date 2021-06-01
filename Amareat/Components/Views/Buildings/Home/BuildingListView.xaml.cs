using Xamarin.Forms;

namespace Amareat.Components.Views.Buildings.Home
{
    public partial class BuildingListView : ContentPage
    {
        private BuildingListViewModel Vm => BindingContext as BuildingListViewModel;

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
