using System;
using System.Net;

namespace Amareat.Exceptions
{
    public class RefreshTokenException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ApiMessageResponse { get; set; }

        public string ReasonPhrase { get; set; }

        public RefreshTokenException()
        {
        }
    }
}
