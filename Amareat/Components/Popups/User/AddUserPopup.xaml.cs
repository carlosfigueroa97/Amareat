using System;
using System.Collections.Generic;
using Amareat.Components.Popups.Base;
using Xamarin.Forms;

namespace Amareat.Components.Popups.User
{
    public partial class AddUserPopup : BasePopupPage
    {
        AddUserViewModel vm => BindingContext as AddUserViewModel;

        public AddUserPopup()
        {
            InitializeComponent();
            user.TextEntryChanged += UserEntryChanged;
            password.TextEntryChanged += PasswordEntryChanged;
            email.TextEntryChanged += EmailEntryChanged;
            isAdmin.Toggled += IsAdminChanged;
        }


        protected override void OnDisappearing()
        {
            user.TextEntryChanged -= UserEntryChanged;
            password.TextEntryChanged -= PasswordEntryChanged;
            email.TextEntryChanged -= EmailEntryChanged;
            isAdmin.Toggled -= IsAdminChanged;
            base.OnDisappearing();
        }

        #region Methods
        void UserEntryChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            vm.User = entry.Text;
        }

        void PasswordEntryChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            vm.Password = entry.Text;
        }

        void EmailEntryChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            vm.Email = entry.Text;
        }

        void IsAdminChanged(System.Object sender, ToggledEventArgs e)
        {
            var toggle = sender as Switch;
            vm.IsAdmin = toggle.IsToggled;
        }

        #endregion
    }
}
