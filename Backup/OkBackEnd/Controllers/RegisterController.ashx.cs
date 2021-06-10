using System;
using System.Net;
using System.Web.UI;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using OkBackEnd.Models;
using Newtonsoft.Json;
using OkBackEnd;

namespace OkBackEnd.Controllers
{
    [Authorize]
    [RoutePrefix("api/register")]
    public class RegisterController : ApiController
    {

        [HttpPost]
        [Route("getexistphone")]
        public IHttpActionResult GetExistPhone(ExistRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = RegisterOperations.GetExist(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        [HttpGet]
        [Route("getdoctype")]
        public IHttpActionResult GetDocType()
        {
            try
            {
                var result = RegisterOperations.GetDocumentType();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("registeruser")]
        public IHttpActionResult RegisterUser(RegisterRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = RegisterOperations.RegisterUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
