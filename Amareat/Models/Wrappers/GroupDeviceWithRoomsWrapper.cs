using System;
using System.Collections.Generic;
using Amareat.Models.API.Responses.Devices;
using Amareat.Models.API.Responses.Rooms;

namespace Amareat.Models.Wrappers
{
    public class GroupDeviceWithRoomsWrapper : List<Devices>
    {
        public Room Room { get; set; }

        public GroupDeviceWithRoomsWrapper(Room room, List<Devices> devices) : base(devices)
        {
            Room = room;
        }
    }
}
