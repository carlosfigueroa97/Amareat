using System.Collections.ObjectModel;
using Amareat.Models.API.Requests.Rooms;

namespace Amareat.Models.Wrappers
{
    public static class RoomsToAddWrapper
    {
        public static ObservableCollection<SimpleRoom> RoomsToSaveList
        {
            get;
            set;
        }

        public static RoomsFlagsWrapper RoomsFlagsWrapper
        {
            get;
            set;
        }
    }
}
