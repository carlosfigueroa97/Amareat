using Amareat.Droid.Renderers;
using Amareat.Renderers;
using Android.Content.Res;
using Android.Graphics;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(EntryBottomLine), typeof(EntryBottomLineDroid))]
namespace Amareat.Droid.Renderers
{
    public class EntryBottomLineDroid : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control == null || e.NewElement == null) return;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
                Control.BackgroundTintList = ColorStateList.ValueOf(Android.Graphics.Color.Black);
            else
                Control.Background.SetColorFilter(Android.Graphics.Color.Black, PorterDuff.Mode.SrcAtop);
        }
    }
}
