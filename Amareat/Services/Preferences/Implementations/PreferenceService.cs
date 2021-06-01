using System;
using Amareat.Services.Preferences.Interfaces;
using Newtonsoft.Json;

namespace Amareat.Services.Preferences.Implementations
{
    public class PreferenceService : IPreferenceService
    {
        public PreferenceService()
        {
        }

        public bool UserHasCompletedEnrollment
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(UserHasCompletedEnrollment), false);
            set => Xamarin.Essentials.Preferences.Set(nameof(UserHasCompletedEnrollment), value);
        }

        public bool IsUserLoggedIn
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(IsUserLoggedIn), false);
            set => Xamarin.Essentials.Preferences.Set(nameof(IsUserLoggedIn), value);
        }

        public bool IsFirstTimer
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(IsFirstTimer), false);
            set => Xamarin.Essentials.Preferences.Set(nameof(IsFirstTimer), value);
        }

        public bool IsAdmin
        {
            get => Xamarin.Essentials.Preferences.Get(nameof(IsAdmin), false);
            set => Xamarin.Essentials.Preferences.Set(nameof(IsAdmin), value);
        }

        public object GetPreference(string key)
        {
            object value = null;
            var serializedValue = Xamarin.Essentials.Preferences.Get(key, null);

            if (serializedValue != null)
            {
                value = JsonConvert.DeserializeObject<object>(serializedValue);
            }

            return value;
        }

        public void SavePreference(string key, object value)
        {
            Xamarin.Essentials.Preferences.Set(key, JsonConvert.SerializeObject(value));
        }

        public void ResetProperties()
        {
            IsUserLoggedIn = false;
            UserHasCompletedEnrollment = false;
            IsFirstTimer = false;
        }
    }
}
