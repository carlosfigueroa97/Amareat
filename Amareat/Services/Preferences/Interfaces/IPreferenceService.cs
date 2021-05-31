using System;
namespace Amareat.Services.Preferences.Interfaces
{
    public interface IPreferenceService
    {
        bool IsUserLoggedIn { get; set; }

        bool IsFirstTimer { get; set; }

        bool IsAdmin { get; set; }

        void SavePreference(string key, object value);

        object GetPreference(string key);

        void ResetProperties();
    }
}
