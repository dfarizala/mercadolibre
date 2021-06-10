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
    [RoutePrefix("api/card")]
    public class CardController : ApiController
    {

        [HttpPost]
        [Route("savecard")]
        public IHttpActionResult SaveCard(SaveCardRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = CardOperations.SaveNewCard(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        [Route("getcards")]
        public IHttpActionResult GetCards(GetCardRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = CardOperations.GetCards(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        [Route("newpayment")]
        public IHttpActionResult GeneratePay(PaymentRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = CardOperations.GeneratePay(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        [Route("deletecard")]
        public IHttpActionResult DeleteCard(DeleteCardRequest request)
        {
            if (request == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            try
            {
                var result = CardOperations.DeleteCard(request);
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
