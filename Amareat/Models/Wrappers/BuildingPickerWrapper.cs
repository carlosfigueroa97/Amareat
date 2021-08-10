using System.Collections.Generic;
using Amareat.Models.API.Responses.Buildings;

namespace Amareat.Models.Wrappers
{
	public class BuildingPickerWrapper
	{
		public List<Building> Data { get; set; }

		public bool IsCalledFromAddPopup { get; set; }
	}
}
