using System;
using System.Collections.ObjectModel;
using Amareat.Models.API.Requests.Rooms;

namespace Amareat.Models.Wrappers
{
    public static class RoomsToAddWrapper
    {
        public static ObservableCollection<SimpleRoom> RoomsToAddList 
        { 
            get; 
            
            set; 
        }
    }
}
