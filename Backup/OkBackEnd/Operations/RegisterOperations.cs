using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NefMobile.SecurityServices;
using System.Text;

namespace OkBackEnd
{
    public class RegisterOperations
    {

        static readonly Random _random = new Random();

        public static ExistResponse GetExist(ExistRequest request)
        {
            ExistResponse result = new ExistResponse();

            try
            {
                string sSql = "SELECT COUNT(*) FROM ok_persons WHERE \"Phone\" = '" + request.Phone + "'";
                int iCount = 0;
                DataSet ds = DataOperations.SelectSimple(sSql);
                if (ds == null || ds.Tables[0].Rows.Count == 0) throw new DataException("Database or table error");
                int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out iCount);
                if(iCount == 0)
                {
                    int iCode1 = _random.Next(100, 999);
                    int iCode2 = _random.Next(100, 999);
                    string sCodeSeparated = iCode1.ToString() + "-" + iCode2.ToString();
                    string sCodeTotal = iCode1.ToString() + "" + iCode2.ToString();

                    StringServices Crypt = new StringServices();
                    string sCode = Crypt.Set(sCodeTotal, Constants.sKey);
                    string sMsg = "Tu número de verificacion para OK es: " + sCodeSeparated;
                    bool bMsg = SmsService.Send(request.Phone, sMsg);
                    if (!bMsg) throw new ApplicationException("No message send");

                    result.Exist = "0";
                    result.PHone = request.Phone;
                    result.Code = sCode;
                    result.ResponseCode = "0";
                    result.ResponseMessage = "Successfull";
                }
                else
                {
                    result.Exist = "1";
                    result.PHone = request.Phone;
                    result.Code = "0";
                    result.ResponseCode = "0";
                    result.ResponseMessage = "Successfull";
                }

                return result;

            }
            catch(Exception ex)
            {
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }

        public static DocType[] GetDocumentType()
        {
            List<DocType> result = new List<DocType>();
            try
            {
                string sSQL = "SELECT * FROM ok_doctype";
                DataSet ds = DataOperations.SelectSimple(sSQL);
                if (ds == null || ds.Tables[0].Rows.Count == 0) throw new DataException();
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    DocType obj = new DocType();
                    obj.Id = dr[0].ToString();
                    obj.Type = dr[1].ToString();
                    obj.Abreviation = dr[2].ToString();

                    result.Add(obj);
                }

                return result.ToArray();
            }
            catch(Exception ex)
            {
                return null;
            }           
        }

        public static RegisterResponse RegisterUser(RegisterRequest request)
        {
            RegisterResponse result = new RegisterResponse();

            try
            {
                string sDate = System.DateTime.Now.ToString("yyyy-MM-dd");
                string sRefer = GenerateRefer();
                StringBuilder sbParams = new StringBuilder();
                string sColums = "\"Phone\", device_id, doc_number, doc_type, email, first_name, last_name, passwd, refer_code, type, join_date, login_type";
                sbParams.Append("'" + request.Phone + "',");
                sbParams.Append("'',");
                sbParams.Append("'" + request.DocNumber + "',");
                sbParams.Append("" + request.DocType + ",");
                sbParams.Append("'" + request.Email + "',");
                sbParams.Append("'" + request.FirstName + "',");
                sbParams.Append("'" + request.LastName + "',");
                sbParams.Append("'" + request.Passwd + "',");
                sbParams.Append("'" + sRefer + "',");
                sbParams.Append("" + request.UserType + ",");
                sbParams.Append("'" + sDate + "',");
                sbParams.Append("'" + request.LoginType + "'");

                bool bInsert = DataOperations.Insert("ok_persons", sColums, sbParams.ToString());
                if (!bInsert) throw new DataException("Error saving record");

                DataSet dId = DataOperations.SelectSimple("SELECT MAX(Id) FROM public.ok_persons");
                if (dId == null || dId.Tables[0].Rows.Count == 0) throw new DataException("Obtain ID fail");

                string sId = dId.Tables[0].Rows[0][0].ToString();

                result.Email = request.Email;
                result.Phone = request.Phone;
                result.ReferCode = sRefer;
                result.UserId = sId;
                result.ResponseCode = "0";
                result.ResponseMessage = "Successfull";

            }
            catch (Exception ex)
            {
                result.Email = request.Email;
                result.Phone = request.Phone;
                result.ReferCode = "";
                result.UserId = "";
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
            }

            return result;
        }

        public static string GenerateRefer()
        {
            int longitud = 6;
            const string alfabeto = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder token = new StringBuilder();
            Random rnd = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int indice = rnd.Next(alfabeto.Length);
                token.Append(alfabeto[indice]);
            }
            return token.ToString();
        }

    }
}
