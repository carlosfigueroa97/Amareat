using System;
using Amareat.Utils.Extensions;
using Xamarin.Forms;

namespace Amareat.Helpers
{
    public static class Styles
    {
        public static Style NormalPopupLabel = (Style)ResourceExtensions.GetResourceValue(nameof(NormalPopupLabel));
        public static Style CancelPopupLabel = (Style)ResourceExtensions.GetResourceValue(nameof(CancelPopupLabel));
    }
}
