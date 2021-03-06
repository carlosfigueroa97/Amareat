using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Amareat.Controls
{
    public partial class TitleAndEntry : ContentView
    {
        public TitleAndEntry()
        {
            InitializeComponent();
        }

        public static BindableProperty TextLabelProperty = BindableProperty.Create(
            nameof(TextLabelProperty), typeof(string), typeof(TitleAndEntry), null,
            propertyChanged: OnTitleLabel);

        public static BindableProperty PlaceholderEntryProperty = BindableProperty.Create(
            nameof(PlaceholderEntry), typeof(string), typeof(TitleAndEntry), null,
            propertyChanged: OnPlaceholderEntry);

        public static BindableProperty TextEntryProperty = BindableProperty.Create(
            nameof(TextEntry), typeof(string), typeof(TitleAndEntry), null,
            BindingMode.TwoWay, propertyChanged: OnTextEntry);

        public static BindableProperty IsPasswordProperty = BindableProperty.Create(
            nameof(IsPassword), typeof(bool), typeof(TitleAndEntry), null,
            propertyChanged: OnIsPassword);

        public static BindableProperty TextEntryChangedProperty = BindableProperty.Create(
            nameof(TextEntryChanged), typeof(EventHandler<TextChangedEventArgs>), typeof(TitleAndEntry), null,
            propertyChanged: OnTextEntryChanged);

        public static BindableProperty ErrorMessageProperty = BindableProperty.Create(
            nameof(ErrorMessage), typeof(string), typeof(TitleAndEntry), null,
            propertyChanged: OnErrorMessage);

        public static BindableProperty IsEntryWrongProperty = BindableProperty.Create(
            nameof(IsEntryWrong), typeof(bool), typeof(TitleAndEntry),
            propertyChanged: OnWrongEntry);

        public string TextLabel
        {
            get
            {
                return (string)GetValue(TextLabelProperty);
            }
            set
            {
                SetValue(TextLabelProperty, value);
            }
        }

        public string PlaceholderEntry
        {
            get
            {
                return (string)GetValue(PlaceholderEntryProperty);
            }
            set
            {
                SetValue(PlaceholderEntryProperty, value);
            }
        }

        public string TextEntry
        {
            get
            {
                return (string)GetValue(TextEntryProperty);
            }
            set
            {
                SetValue(TextEntryProperty, value);
            }
        }

        public bool IsPassword
        {
            get
            {
                return (bool)GetValue(IsPasswordProperty);
            }
            set
            {
                SetValue(IsPasswordProperty, value);
            }
        }

        public string ErrorMessage
        {
            get
            {
                return (string)GetValue(ErrorMessageProperty);
            }

            set
            {
                SetValue(ErrorMessageProperty, value);
            }
        }

        public bool IsEntryWrong
        {
            get
            {
                return (bool)GetValue(IsEntryWrongProperty);
            }

            set
            {
                SetValue(IsEntryWrongProperty, value);
            }
        }

        public EventHandler<TextChangedEventArgs> TextEntryChanged
        {
            get
            {
                return (EventHandler<TextChangedEventArgs>)GetValue(TextEntryChangedProperty);
            }
            set
            {
                SetValue(TextEntryChangedProperty, value);
            }
        }

        private static void OnTitleLabel(BindableObject bindable, object oldVal, object newVal)
        {
            var stackLayout = GetStackLayout(bindable);
            var label = stackLayout.Children[0] as Label;
            label.Text = newVal.ToString();
        }

        private static void OnPlaceholderEntry(BindableObject bindable, object oldVal, object newVal)
        {
            var stackLayout = GetStackLayout(bindable);
            var entry = stackLayout.Children[1] as Entry;
            entry.Placeholder = newVal.ToString();
        }

        private static void OnTextEntry(BindableObject bindable, object oldVal, object newVal)
        {
            var stackLayout = GetStackLayout(bindable);
            var entry = stackLayout.Children[1] as Entry;
            entry.Text = newVal.ToString();
        }

        private static void OnTextEntryChanged(BindableObject bindable, object oldVal, object newVal)
        {
            var stackLayout = GetStackLayout(bindable);
            var entry = stackLayout.Children[1] as Entry;
            entry.TextChanged += (EventHandler<TextChangedEventArgs>)newVal;
        }

        private static void OnIsPassword(BindableObject bindable, object oldVal, object newVal)
        {
            var stackLayout = GetStackLayout(bindable);
            var entry = stackLayout.Children[1] as Entry;
            entry.IsPassword = (bool)newVal;
        }

        private static StackLayout GetStackLayout(BindableObject bindable)
        {
            var contentView = bindable as ContentView;
            return contentView.Content as StackLayout;
        }

        private static void OnErrorMessage(BindableObject bindable, object oldVal, object newVal)
        {
            var control = (TitleAndEntry)bindable;
            control.ErrorMessageLbl.Text = newVal.ToString();
        }

        private static void OnWrongEntry(BindableObject bindable, object oldVal, object newVal)
        {
            var control = (TitleAndEntry)bindable;
            control.ErrorMessageLbl.IsVisible = (bool)newVal;
        }
    }
}
