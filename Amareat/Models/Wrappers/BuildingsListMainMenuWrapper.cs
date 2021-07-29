using System.Collections.ObjectModel;
using Amareat.Models.API.Responses.Buildings;

namespace Amareat.Models.Wrappers
{
    public static class BuildingsListMainMenuWrapper
    {
        
        public static ObservableCollection<Building> BuildingList
        {
            get;
            set;
        }
    }
}
