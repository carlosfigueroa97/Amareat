using System;
using System.Threading.Tasks;
using Amareat.Services.SecureStorage.Interfaces;

namespace Amareat.Services.SecureStorage.Implementations
{
    public class SecureStorage : ISecureStorage
    {
        public SecureStorage()
        {
        }

        public Task<string> GetValue(string key)
        {
            return Xamarin.Essentials.SecureStorage.GetAsync(key);
        }

        public async Task SetValue(string key, string value)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(key, value);
        }

        public void DeleteValue(string key)
        {
            Xamarin.Essentials.SecureStorage.Remove(key);
        }

        public void ResetAllProperties()
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
        }
    }
}
