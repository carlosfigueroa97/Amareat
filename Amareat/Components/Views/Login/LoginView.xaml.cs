using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Amareat.Components.Views.Login
{
    public partial class LoginView : ContentPage
    {
        LoginViewModel Vm => BindingContext as LoginViewModel;

        public LoginView()
        {
            InitializeComponent();
            user.TextEntryChanged += UserEntryTextChanged;
            password.TextEntryChanged += PasswordEntryTextChanged;
        }

        void UserEntryTextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            Vm.User = entry.Text;
        }

        void PasswordEntryTextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            Vm.Password = entry.Text;
        }
    }
}
