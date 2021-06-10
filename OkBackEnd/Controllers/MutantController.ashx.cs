using System;
using System.Net;
using System.Web.UI;
using System.Web.Http;
using System.Net.Http;
using System.Collections.Generic;
using Mercadolibre.Models;

namespace Mercadolibre.Controllers
{
    [AllowAnonymous]
    //[RoutePrefix("/xmen")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("stats")]
        public IHttpActionResult EchoPing()
        {
            StatsResponse result = Operation.GetStats();
            return Ok(result);
        }

        [HttpPost]
        [Route("mutant")]
        public IHttpActionResult isMutant(DnaRequest dna)
        {
            if (dna == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            bool isMutant = Operation.isMutant(dna.dna);
            string sChain = "";
            int iMutant = 0;
            if (isMutant) iMutant = 1;

            foreach(string s in dna.dna)
            {
                sChain = sChain + " " + s;
            }
            string sSQL = "INSERT INTO xmen_dna (dna_chain, is_mutant) VALUES ('" + sChain + "', " + iMutant + ")";
            bool bResult = DataOperations.Insert(sSQL);

            if (isMutant)
            {
                return Ok(isMutant.ToString());
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
        }
    }
}