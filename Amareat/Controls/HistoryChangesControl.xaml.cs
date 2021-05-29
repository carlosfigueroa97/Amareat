using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Amareat.Controls
{
    public partial class HistoryChangesControl : ContentView
    {
        #region Public Properties

        public string DeviceNameText
        {
            get
            {
                return base.GetValue(DeviceNameTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(DeviceNameTextProperty, value);
            }
        }

        public string StatusText
        {
            get
            {
                return base.GetValue(StatusTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(StatusTextProperty, value);
            }
        }

        public string UserText
        {
            get
            {
                return base.GetValue(UserTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(UserTextProperty, value);
            }
        }

        public string DateText
        {
            get
            {
                return base.GetValue(DateTextProperty)?.ToString();
            }

            set
            {
                base.SetValue(DateTextProperty, value);
            }
        }

        #endregion

        public HistoryChangesControl()
        {
            InitializeComponent();
        }

        #region Public Methods

        public static readonly BindableProperty DeviceNameTextProperty =
            BindableProperty.Create(
                nameof(DeviceNameText),
                typeof(string),
                typeof(HistoryChangesControl),
                propertyChanged: DeviceNamePropertyChanged);

        public static readonly BindableProperty StatusTextProperty =
            BindableProperty.Create(
                nameof(StatusText),
                typeof(string),
                typeof(HistoryChangesControl),
                propertyChanged: StatusPropertyChanged);

        public static readonly BindableProperty UserTextProperty =
            BindableProperty.Create(
                nameof(UserText),
                typeof(string),
                typeof(HistoryChangesControl),
                propertyChanged: UserPropertyChanged);


        public static readonly BindableProperty DateTextProperty =
           BindableProperty.Create(
               nameof(DateText),
               typeof(string),
               typeof(HistoryChangesControl),
               propertyChanged: DatePropertyChanged);


        #endregion

        #region Private Methods

        private static void DeviceNamePropertyChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            var control = (HistoryChangesControl)bindable;
            control.DeviceNameLbl.Text = newValue?.ToString();
        }

        private static void StatusPropertyChanged(BindableObject bindable,
            object oldValue, object newValue)
        {
            var control = (HistoryChangesControl)bindable;
            control.StatusLbl.Text = newValue?.ToString();
        }

        private static void UserPropertyChanged(BindableObject bindable,
                    object oldValue, object newValue)
        {
            var control = (HistoryChangesControl)bindable;
            control.UserLbl.Text = newValue?.ToString();
        }

        private static void DatePropertyChanged(BindableObject bindable,
                            object oldValue, object newValue)
        {
            var control = (HistoryChangesControl)bindable;
            control.DateLbl.Text = newValue?.ToString();
        }

        #endregion
    }
}
