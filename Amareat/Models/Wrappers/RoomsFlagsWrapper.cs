using System;
using Amareat.Components.Base;

namespace Amareat.Models.Wrappers
{
    public class RoomsFlagsWrapper : BaseVm
    {
        public RoomsFlagsWrapper(bool isListViewVisible, bool isLabelVisible)
        {
            IsListViewVisible = isListViewVisible;
            IsLabelVisible = isLabelVisible;
        }

        private bool _isListViewVisible;
        public bool IsListViewVisible {
            get => _isListViewVisible;
            set => SetProperty(ref _isListViewVisible, value);
        }

        private bool _isLabelVisible;
        public bool IsLabelVisible
        {
            get => _isLabelVisible;
            set => SetProperty(ref _isLabelVisible, value);
        }
    }
}
