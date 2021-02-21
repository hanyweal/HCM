using HCMBLL;
using HCMBLL.Enums;
using HCMServices.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HCMServices.ExtensionMethods;

namespace HCMServices.Controllers
{
    //[CustomBasicAuthentication]
    public class CountriesController : ApiController
    {
        // 1
        [AllowAnonymous]
        [HttpGet, Route("api/Countries/GetCountries")]
        public HttpResponseMessage GetCountries()
        {
            return Request.CreateResponse(HttpStatusCode.OK, new CountriesBLL().GetCountries());
        }

    }
}
