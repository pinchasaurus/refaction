using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Refaction.Service
{
    public class HttpResponseException_NotFoundException : HttpResponseException
    {
        // the long name of this class was chosen because
        // it is important that future developers know that this is an HttpResponseException
        // (Web API handles HttpResponseException differently from other exceptions)

        public HttpResponseException_NotFoundException()
            : base(System.Net.HttpStatusCode.NotFound)
        {
        }
    }
}