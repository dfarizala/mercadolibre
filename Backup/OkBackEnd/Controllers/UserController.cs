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
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("getuser")]
        public IHttpActionResult GetExistPhone(UserRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = UserOperations.GetUser(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

    }
}
