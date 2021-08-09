using System;
using Amareat.Services.Localization.Interfaces;
using Amareat.Utils.ServiceLocator;

namespace Amareat.Helpers
{
    public static class Resources
    {
        private static ILocalizationService _localizationService =>
            ServiceLocatorProvider
            .Instance
            .GetService<ILocalizationService>();

        public static string OK = GetResources("OK");
        public static string Error = GetResources("Error");
        public static string Warning = GetResources("Warning");
        public static string SignIn = GetResources("SignIn");
        public static string User = GetResources("User");
        public static string Password = GetResources("Password");
        public static string InsertYourUser = GetResources("InsertYourUser");
        public static string InsertYourPassword = GetResources("InsertYourPassword");
        public static string ForgotYourPassword = GetResources("ForgotYourPassword");
        public static string PleaseContactAdministrator = GetResources("PleaseContactAdministrator");
        public static string WrongCredentials = GetResources("WrongCredentials");
        public static string CheckYourDataWell = GetResources("CheckYourDataWell");
        public static string AnUnexpectedErrorHasOcurred = GetResources("AnUnexpectedErrorHasOcurred");
        public static string Active = GetResources("Active");
        public static string Inactive = GetResources("Inactive");
        public static string On = GetResources("On");
        public static string Off = GetResources("Off");
        public static string UserNotSaved = GetResources("UserNotSaved");
        public static string UserSaved = GetResources("UserSaved");
        public static string AddUser = GetResources("AddUser");
        public static string AddBuilding = GetResources("AddBuilding");
        public static string AddRoom = GetResources("AddRoom");
        public static string AddDevice = GetResources("AddDevice");
        public static string Cancel = GetResources("Cancel");
        public static string ShortPasswordError = GetResources("ShortPasswordError");
        public static string InvalidEmailError = GetResources("InvalidEmailError");
        public static string BuildingNotSaved = GetResources("BuildingNotSaved");
        public static string BuildingSaved = GetResources("BuildingSaved");
        public static string BuildingNameEmpty = GetResources("BuildingNameEmpty");
        public static string NoBuildingFound = GetResources("NoBuildingFound");
        public static string RoomNameEmpty = GetResources("RoomNameEmpty");

        private static string GetResources(string key)
        {
            return _localizationService.GetResource(key);
        }
    }
}
