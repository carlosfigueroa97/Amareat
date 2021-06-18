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
        public const string SecretKey = "uTOS0URkGMpLimGghRmIoprcUNBPqWuo";
        public const string IV = "C!sV!BjLO28nA4Iz";
        public const string Algorithm  = "aes256";

#if DEBUG
        public const string ApiUrl = "https://amareat-dev.herokuapp.com/";
#else
        public const string ApiUrl = "";
#endif

        public const string CheckInternetUrl = "https://google.com";

        public const string ApiVersion = "V1";
        public const string ApiRoute = "/app/api/" + ApiVersion + "/";
        public const string Users = ApiRoute + "users/";
        public const string Buildings = ApiRoute + "buildings/";
        public const string Rooms = ApiRoute + "rooms/";
        public const string Devices = ApiRoute + "devices/";
        public const string TypeDevices = ApiRoute + "typeDevices/";
        public const string History = ApiRoute + "history/";


        // Preference key
        public const string IdUser = "idUser";

        #endregion
    }
}
