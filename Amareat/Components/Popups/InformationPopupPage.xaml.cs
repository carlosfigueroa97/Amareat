using System;
using System.Linq;
using System.Windows.Input;
using Amareat.Components.Popups.Base;
using Amareat.Helpers;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace Amareat.Components.Popups
{
    public partial class InformationPopupPage : BasePopupPage
    {
        #region Properties & Commands

        ICommand CommandAfter { get; set; }

        public new static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(InformationPopupPage),
            "Some Title Here", propertyChanged: (bindable, oldVal, newVal) =>
            {
                var view = (InformationPopupPage)bindable;
                view.TheTitle.Text = (string)newVal;
            });

        public new string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static BindableProperty BodyProperty = BindableProperty.Create(nameof(Body), typeof(string), typeof(InformationPopupPage),
            "Lorem ipsum", propertyChanged: (bindable, oldVal, newVal) =>
            {
                var view = (InformationPopupPage)bindable;
                view.TheBody.Text = (string)newVal;
            });

        public string Body
        {
            get
            {
                return (string)GetValue(BodyProperty);
            }
            set
            {
                SetValue(BodyProperty, value);
            }
        }

        public static BindableProperty OkButtonTextProperty = BindableProperty.Create(nameof(OkButtonText), typeof(string), typeof(InformationPopupPage),
            ConstantGlobal.OK, propertyChanged: (bindable, oldVal, newVal) =>
            {
                var view = (InformationPopupPage)bindable;
                view.TheOkButton.Text = (string)newVal;
            });

        public string OkButtonText
        {
            get
            {
                return (string)GetValue(OkButtonTextProperty);
            }
            set
            {
                SetValue(OkButtonTextProperty, value);
            }
        }

        public static BindableProperty MainCommandProperty = BindableProperty.Create(nameof(MainCommand), typeof(Command), typeof(InformationPopupPage),
        null, propertyChanged: (bindable, oldVal, newVal) =>
        {
            var view = (InformationPopupPage)bindable;
            view.TheOkButton.Command = (ICommand)newVal;
        });

        public ICommand MainCommand
        {
            get
            {
                return (ICommand)GetValue(MainCommandProperty);
            }
            set
            {
                SetValue(MainCommandProperty, value);
            }
        }

        public static BindableProperty InflateViewProperty = BindableProperty.Create(nameof(InflateView),
            typeof(Grid), typeof(InformationPopupPage), null, propertyChanged: (bindable, oldVal, newVal) =>
            {
                InformationPopupPage okCancelView = (InformationPopupPage)bindable;
                Grid newLayout = (Grid)newVal;

                Grid gridLayout = okCancelView.InflatedView;
                okCancelView.InflatedView.IsVisible = true;
                gridLayout.Children.Clear();
                gridLayout.Children.Add(newLayout);
            });

        public Grid InflateView
        {
            get
            {
                return (Grid)GetValue(InflateViewProperty);
            }
            set
            {
                SetValue(InflateViewProperty, value);
            }
        }

        #endregion

        public InformationPopupPage(string title, string message, string cancel = "OK", ICommand commandAfter = null)
        {
            InitializeComponent();

            InflatedView.IsVisible = false;
            Title = title;
            Body = message;
            CommandAfter = commandAfter;
        }

        void TheButtonClicked(object sender, EventArgs e)
        {
            if (PopupNavigation.Instance.PopupStack.Any())
            {
                PopupNavigation.Instance.PopAsync();
                if (CommandAfter != null)
                {
                    CommandAfter.Execute(null);
                }
            }
        }
    }
}
