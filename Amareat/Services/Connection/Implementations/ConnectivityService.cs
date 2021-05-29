using System;
using System.Net;
using Amareat.Helpers;
using Amareat.Services.Connection.Interfaces;
using Xamarin.Essentials;

namespace Amareat.Services.Connection.Implementations
{
    public class ConnectivityService : IConnectivityService
    {
        public ConnectivityService()
        {
        }

        public bool UserHasInternetConnection => Connectivity.NetworkAccess == NetworkAccess.Internet ? true : false;

        public bool CheckInternetAccess()
        {
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(ConstantGlobal.CheckInternetUrl);

                iNetRequest.Timeout = 5000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                iNetResponse.Close();

                return true;
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public bool CheckInternetAndConnectionAccess()
        {
            try
            {
                if (UserHasInternetConnection)
                {
                    if (CheckInternetAccess())
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }
    }
}
