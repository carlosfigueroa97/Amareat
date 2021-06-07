using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Amareat.Models.Wrappers
{
    public class AddMainMenuWrapper
    {
        public string ItemText { get; set; }

        public Color BackgrounStackColor { get; set; }

        public Style ItemStyle { get; set; }

        public Func<Task> InvokeView { get; set; }
    }
}
