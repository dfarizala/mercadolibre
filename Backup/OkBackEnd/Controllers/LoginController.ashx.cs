using System;
using System.Net;
using System.Web.UI;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using OkBackEnd.Models;

namespace OkBackEnd.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            bool isCredentialValid = Login.VerifyAuth(login.AppID); //OdesyApi.Operations.BasicOperations.LoginApp(login.AppVersion,

            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.AppID);
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}