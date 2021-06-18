using Amareat.iOS.Renderers;
using Amareat.Renderers;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EntryBottomLine), typeof(EntryBottomLineiOS))]
namespace Amareat.iOS.Renderers
{
    public class EntryBottomLineiOS : EntryRenderer
    {
        private CALayer _line;

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            _line = null;

            if (Control == null || e.NewElement == null)
                return;

            Control.BorderStyle = UITextBorderStyle.None;

            _line = new CALayer
            {
                BorderColor = UIColor.FromRGB(174, 174, 174).CGColor,
                BackgroundColor = UIColor.FromRGB(174, 174, 174).CGColor,
                Frame = new CGRect(0, Frame.Height / 2, 350, 1.5f)
            };

            Control.Layer.AddSublayer(_line);
        }
    }
}
