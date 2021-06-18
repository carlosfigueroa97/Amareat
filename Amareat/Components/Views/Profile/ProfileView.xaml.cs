using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Amareat.Components.Views.Profile
{
    public partial class ProfileView : ContentPage
    {

        ProfileViewModel Vm => BindingContext as ProfileViewModel;

        public ProfileView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Vm.GetUserProfileCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
