using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NefMobile.SecurityServices;
using System.Text;

namespace OkBackEnd
{
    public class UserOperations
    {
        public static UserResponse GetUser(UserRequest request)
        {
            UserResponse result = new UserResponse();

            try
            {
                string sSql = "";
                if (request.UserLogin == null || request.UserLogin == "") throw new ArgumentException("No login type");
                if(request.UserLogin == "PH")
                {
                    if (request.UserPhone == null || request.UserPhone == "") throw new ArgumentException("No phone number");
                    sSql = "SELECT * FROM ok_persons WHERE \"Phone\" = '" + request.UserPhone + "' AND status = 1 AND type = " + request.UserType;
                }
                else
                {
                    if (request.UserType == null || request.UserType == "") throw new ArgumentException("No email");
                    sSql = "SELECT * FROM ok_persons WHERE email = '" + request.UserEmail + "' AND status = 1 AND type = " + request.UserType;
                }

                DataSet ds = DataOperations.SelectSimple(sSql);
                if (ds == null) throw new DataException("Database error");
                if (ds.Tables[0].Rows.Count == 0) throw new EvaluateException("User does not exist");

                StringServices Crypto = new StringServices();

                string sDbPwdCrypt = ds.Tables[0].Rows[0][8].ToString();
                string sDbPwd = Crypto.Get(sDbPwdCrypt, Constants.sKey);
                string sPwdCrypt = request.UserPwd;
                string sPwd = Crypto.Get(sPwdCrypt, Constants.sKey);
                if (request.UserLogin == "PH")
                {
                    if (sPwd != sDbPwd) throw new EvaluateException("Password incorrect");
                }

                result.DeviceId = ds.Tables[0].Rows[0][1].ToString();
                result.FirstName = ds.Tables[0].Rows[0][5].ToString();
                result.LastName = ds.Tables[0].Rows[0][7].ToString();
                result.LoginType = ds.Tables[0].Rows[0][12].ToString();
                result.ReferCode = ds.Tables[0].Rows[0][9].ToString();
                result.UserEmail = ds.Tables[0].Rows[0][4].ToString();
                result.UserId = ds.Tables[0].Rows[0][6].ToString();
                result.UserPhone = ds.Tables[0].Rows[0][0].ToString();
                result.DocType = ds.Tables[0].Rows[0][3].ToString();
                result.DocNum = ds.Tables[0].Rows[0][2].ToString();

                result.ResponseCode = "0";
                result.ResponseMessage = "Successfull";
                return result;
            }
            catch(Exception ex)
            {
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }

        public static UserResponse GetUserByID(string id)
        {
            UserResponse result = new UserResponse();

            try
            {
                string sSql = "";
                sSql = "SELECT * FROM ok_persons WHERE id = '" + id + "' AND status = 1";

                DataSet ds = DataOperations.SelectSimple(sSql);
                if (ds == null) throw new DataException("Database error");
                if (ds.Tables[0].Rows.Count == 0) throw new EvaluateException("User does not exist");

                result.DeviceId = ds.Tables[0].Rows[0][1].ToString();
                result.FirstName = ds.Tables[0].Rows[0][5].ToString();
                result.LastName = ds.Tables[0].Rows[0][7].ToString();
                result.LoginType = ds.Tables[0].Rows[0][12].ToString();
                result.ReferCode = ds.Tables[0].Rows[0][9].ToString();
                result.UserEmail = ds.Tables[0].Rows[0][4].ToString();
                result.UserId = ds.Tables[0].Rows[0][6].ToString();
                result.UserPhone = ds.Tables[0].Rows[0][0].ToString();
                result.DocType = ds.Tables[0].Rows[0][3].ToString();
                result.DocNum = ds.Tables[0].Rows[0][2].ToString();

                result.ResponseCode = "0";
                result.ResponseMessage = "Successfull";
                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }

    }
}
