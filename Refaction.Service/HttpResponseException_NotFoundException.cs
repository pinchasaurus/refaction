using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Refaction.Service
{
    public class HttpResponseException_NotFoundException : HttpResponseException
    {
        public HttpResponseException_NotFoundException()
            : base(System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}