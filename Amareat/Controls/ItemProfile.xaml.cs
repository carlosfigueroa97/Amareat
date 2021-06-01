using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Amareat.Controls
{
    public partial class ItemProfile : ContentView
    {
        public ItemProfile()
        {
            InitializeComponent();
        }

        public static BindableProperty TextLabelProperty = BindableProperty.Create(
            nameof(TextLabel), typeof(string), typeof(ItemProfile), null,
            propertyChanged: OnTextLabel);

        public static BindableProperty TextFrameProperty = BindableProperty.Create(
            nameof(TextFrame), typeof(string), typeof(ItemProfile), null,
            propertyChanged: OnTextFrame);

        public string TextLabel
        {
            get => (string)GetValue(TextLabelProperty);
            set => SetValue(TextLabelProperty, value);
        }

        public string TextFrame
        {
            get => (string)GetValue(TextFrameProperty);
            set => SetValue(TextFrameProperty, value);
        }

        private static void OnTextLabel(BindableObject bindable, object oldVal, object newVal)
        {
            var contentView = bindable as ContentView;
            var stackLayout = contentView.Content as StackLayout;
            var label = stackLayout.Children[0] as Label;
            label.Text = newVal.ToString();
        }

        private static void OnTextFrame(BindableObject bindable, object oldVal, object newVal)
        {
            var contentView = bindable as ContentView;
            var stackLayout = contentView.Content as StackLayout;
            var frame = stackLayout.Children[1] as Frame;
            var label = frame.Content as Label;
            label.Text = newVal.ToString();
        }
    }
}
