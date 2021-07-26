using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Errors
{

    public class RestException : Exception
    {

        public HttpStatusCode Code { get; }
        public string Error { get; }
        public bool Success { get; }

        public RestException(HttpStatusCode code, string error = null,bool success=false)
        {
            Code = code;
            Success = success;
            Error = error;
        }

        


        // Main from reactivities course

        //public RestException(HttpStatusCode code, object errors = null)
        //{
        //    Code = code;
        //    Errors = errors;
        //}

        //public HttpStatusCode Code { get; }
        //public object Errors { get; }
    }
}
