using System;
namespace Amareat.Services.Connection.Interfaces
{
    public interface IConnectivityService
    {
        bool UserHasInternetConnection { get; }

        bool CheckInternetAccess();

        bool CheckInternetAndConnectionAccess();
    }
}
