using System;
using System.Threading.Tasks;

namespace Amareat.Services.SecureStorage.Interfaces
{
    public interface ISecureStorage
    {
        Task SetValue(string key, string value);

        Task<string> GetValue(string key);

        void DeleteValue(string key);

        void ResetAllProperties();
    }
}
