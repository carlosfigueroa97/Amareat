using System;
namespace Amareat.Helpers
{
    public static class ConstantGlobal
    {
        #region Public Properties

        public const string ResourcesPath = "Amareat.Localization.AppResources";
        public const string AppCenteriOSToken = "";
        public const string AppCenterAndroidToken = "";
        public const string OK = "OK";
        public const string Authorization = "Authorization";
        public const string Bearer = "Bearer";

#if DEBUG
        public const string ApiUrl = "https://amareat-dev.herokuapp.com/";
#else
        public const string ApiUrl = "";
#endif

        public const string CheckInternetUrl = "https://google.com";

        public const string ApiVersion = "V1";
        public const string ApiRoute = "/app/api/" + ApiVersion + "/";
        public const string Users = "users/";
        public const string Buildings = "buildings/";
        public const string Rooms = "rooms/";
        public const string Devices = "devices/";
        public const string TypeDevices = "typeDevices/";
        public const string History = "history/";

        #endregion
    }
}
