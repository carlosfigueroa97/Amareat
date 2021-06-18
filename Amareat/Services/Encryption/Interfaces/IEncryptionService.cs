using System;
using System.Threading.Tasks;

namespace Amareat.Services.Encryption.Interfaces
{
    public interface IEncryptionService
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}
