using Xamarin.Forms;

namespace Amareat.Components.Views.Rooms.Home
{
    public partial class RoomListView : ContentPage
    {
        RoomListViewModel Vm => BindingContext as RoomListViewModel;

        public RoomListView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            //Vm.GetDataCommand.Execute(null);
            Vm.ConnectSocketCommand.Execute(null);
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            Vm.DisconnectSocketCommand.Execute(null);
            base.OnDisappearing();
        }
    }
}
