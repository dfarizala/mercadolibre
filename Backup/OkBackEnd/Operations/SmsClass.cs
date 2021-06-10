using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Net.Configuration;
using System.Configuration;
using System.Web;

namespace OkBackEnd
{
    public class SmsService
    {

        public static bool Send(string number, string strMessage)
        {
            try
            {
                string sUrl = "https://api.hablame.co/sms/envio/";
                Random generator = new Random();
                bool r = false;

                string sPost = "api=huccq3Le9JS3ZeGJPMHk5KqqU3lS9G&cliente=10012553&numero=" + number + "&sms=" + strMessage + "";

                var httpRequest = (HttpWebRequest)WebRequest.Create(sUrl);
                httpRequest.Method = "POST";
                httpRequest.ContentType = "application/x-www-form-urlencoded";
                using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
                {
                    streamWriter.Write(sPost);
                    streamWriter.Flush();
                }
                var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
                r = true;
                return r;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
