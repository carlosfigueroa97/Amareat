using System;
using System.Net;

namespace Amareat.Exceptions
{
    public class ApiErrorException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }

        public ApiErrorException()
        {
        }
    }
}
