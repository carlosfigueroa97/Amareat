using System;
using System.Net;

namespace Amareat.Exceptions
{
    public class ApiErrorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ApiMessageResponse { get; set; }

        public string ReasonPhrase { get; set; }

        public ApiErrorException()
        {
        }
    }
}
