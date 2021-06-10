using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NefMobile.SecurityServices;
using BCrypt.Net;

namespace OkBackEnd
{
    public class Login
    {
        public static bool VerifyAuth(string appSecret)
        {
            try
            {
                StringServices Crypt = new StringServices();

                string sKey = Crypt.Get(appSecret, Constants.sKey);
                if (sKey == null || sKey == "") return false;

                bool isCorrect = BCrypt.Net.BCrypt.Verify(sKey, Constants.sAppId);
                return isCorrect;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
        
    }
}
