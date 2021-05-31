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

        private static string GetResources(string key)
        {
            return _localizationService.GetResource(key);
        }
    }
}
