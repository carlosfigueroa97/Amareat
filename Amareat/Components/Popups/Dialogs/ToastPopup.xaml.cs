using System;
using System.Collections.Generic;
using Amareat.Components.Popups.Base;
using Xamarin.Forms;

namespace Amareat.Components.Popups.Dialogs
{
    public partial class ToastPopup : BasePopupPage
    {
        #region Properties

        public string TextMessage
        {
            get => (string)GetValue(TextMessageProperty);
            set => SetValue(TextMessageProperty, value);
        }

        #endregion

        public ToastPopup(string textMessage)
        {
            InitializeComponent();
            TextMessage = textMessage;
        }

        public static BindableProperty TextMessageProperty
            = BindableProperty.Create(
                nameof(TextMessage),
                typeof(string),
                typeof(ToastPopup),
                "Ups, algo no salió como se esperaba",
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    var control = (ToastPopup)bindable;
                    control.textLbl.Text = newValue.ToString();
                });
    }
}
