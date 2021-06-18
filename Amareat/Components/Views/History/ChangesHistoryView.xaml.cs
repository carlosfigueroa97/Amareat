using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Amareat.Components.Views.History
{
    public partial class ChangesHistoryView : ContentPage
    {
        ChangesHistoryViewModel _vm =>
            BindingContext as ChangesHistoryViewModel;

        public ChangesHistoryView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            _vm.GetHistoryDataCommand.Execute(null);
            base.OnAppearing();
        }
    }
}
