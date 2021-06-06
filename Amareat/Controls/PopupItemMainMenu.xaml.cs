using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Amareat.Controls
{
    public partial class PopupItemMainMenu : ContentView
    {
        #region Public Properties

        public string ItemText
        {
            get
            {
                return (string)GetValue(SetItemTextProperty);
            }

            set
            {
                SetValue(SetItemTextProperty, value);
            }
        }

        public Style ItemStyle
        {
            get
            {
                return (Style)GetValue(SetItemStyleProperty);
            }

            set
            {
                SetValue(SetItemStyleProperty, value);
            }
        }

        public Color BackgroundStackColor
        {
            get
            {
                return (Color)GetValue(SetBackgroundColorProperty);
            }

            set
            {
                SetValue(SetBackgroundColorProperty, value);
            }
        }

        #endregion

        public PopupItemMainMenu()
        {
            InitializeComponent();
        }

        #region Public Methods

        public static readonly BindableProperty SetItemTextProperty =
            BindableProperty.Create(
                nameof(ItemText),
                typeof(string),
                typeof(PopupItemMainMenu),
                propertyChanged: ItemTextPropertyChanged);

        public static readonly BindableProperty SetItemStyleProperty =
            BindableProperty.Create(
                nameof(ItemStyle),
                typeof(Style),
                typeof(PopupItemMainMenu),
                propertyChanged: ItemStyleChanged);

        public static readonly BindableProperty SetBackgroundColorProperty =
            BindableProperty.Create(
                nameof(BackgroundStackColor),
                typeof(Color),
                typeof(PopupItemMainMenu),
                propertyChanged: BackgroundColorChanged);

        #endregion

        #region Private Methods

        private static void ItemTextPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (PopupItemMainMenu)bindable;
            control.ItemTextLbl.Text = newValue.ToString();
        }

        private static void ItemStyleChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var newStyle = (Style)newValue;
            var control = (PopupItemMainMenu)bindable;
            control.ItemTextLbl.Style = newStyle;
        }

        private static void BackgroundColorChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var newColor = (Color)newValue;
            var control = (PopupItemMainMenu)bindable;
            control.BackgroundStack.BackgroundColor = newColor;
        }

        #endregion
    }
}
