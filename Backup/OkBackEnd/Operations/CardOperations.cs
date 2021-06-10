using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using NefMobile.SecurityServices;
using System.Text;
using EpaycoSdk;
using EpaycoSdk.Models;
using EpaycoSdk.Models.Charge;
using EpaycoSdk.Utils;
using System.Configuration;

namespace OkBackEnd
{
    public class CardOperations
    {

        static string epayco_customer = ConfigurationManager.AppSettings["EPAYCO_CUSTOMER"];
        static string epayco_key = ConfigurationManager.AppSettings["EPAYCO_KEY"];
        static string epayco_public = ConfigurationManager.AppSettings["EPAYCO_PUBLIC"];
        static string epayco_private = ConfigurationManager.AppSettings["EPAYCO_PRIVATE"];

        static Epayco objEpayco = new EpaycoSdk.Epayco(epayco_public, epayco_private, "ES", true);

        static NefMobile.SecurityServices.StringServices objCrypt = new NefMobile.SecurityServices.StringServices();

        public static SaveCardResponse SaveNewCard(SaveCardRequest request)
        {
            SaveCardResponse result = new SaveCardResponse();

            try
            {
                UserResponse objUser = UserOperations.GetUserByID(request.user_id);

                //string o = objCrypt.Set("4575623182290326;2025;12;123;DIEGO;ARIZALA;VISA", Constants.sKey);
                //request.token = o;
                string CardString = objCrypt.Get(request.token, Constants.sKey);
                string[] CardParams = CardString.Split(';');

                string sCardNumber = objCrypt.Get(CardParams[0], Constants.sKey).Replace("-","");
                string sCardExpirationYear = objCrypt.Get(CardParams[1], Constants.sKey);
                string sCardExpirationMonth = objCrypt.Get(CardParams[2], Constants.sKey);
                string sCardCVV = objCrypt.Get(CardParams[3], Constants.sKey);
                string sName = objCrypt.Get(CardParams[4], Constants.sKey);
                string sLastName = objCrypt.Get(CardParams[5], Constants.sKey);
                string sHolder = objCrypt.Get(CardParams[6], Constants.sKey);

                TokenModel token = objEpayco.CreateToken(sCardNumber.Replace("-",""), //cardNumber
                                                         sCardExpirationYear, //expYear
                                                         sCardExpirationMonth, //expMonth
                                                         sCardCVV //cvc
                                                         );
                if (token.status == false)
                    throw new Exception(token.data.description);

                CustomerCreateModel customer = objEpayco.CustomerCreate(
                      token.id, //string
                      sName, //string
                      sLastName, //string
                      objUser.UserEmail, //string 
                      true, //boolean
                      "Bogota", //string 
                      "Calle 64F # 24-70", //string
                      objUser.UserPhone, //string
                      objUser.UserPhone //string
                );

                if (customer.status == false)
                    throw new Exception(customer.data.description);

                string sCols = "person_id, token, customer_id, card_token, card_holder";
                StringBuilder sbParams = new StringBuilder();
                sbParams.Append("'" + request.user_id + "', ");
                sbParams.Append("'" + request.token + "', ");
                sbParams.Append("'" + customer.data.customerId + "', ");
                sbParams.Append("'" + token.id + "', ");
                sbParams.Append("'" + sHolder.Substring(0,1) + "'");
                bool bResult = DataOperations.Insert("ok_credit_card", sCols, sbParams.ToString());
                if (!bResult)
                    throw new Exception("Error saving card");

                //ChargeModel VerifyPayment = objEpayco.ChargeCreate(
                //    token.id,
                //    request.user_id,
                //    "CC",
                //    objUser.DocNum,
                //    objUser.FirstName,
                //    objUser.LastName,
                //    objUser.UserEmail,
                //    "OK-PRE-2021" + request.user_id,
                //    "OK_VERIFY_CARD_PAYMENT",
                //    "5000",
                //    "0",
                //    "5000",
                //    "COP",
                //    "1",
                //    "Calle 64F # 24-70",
                //    objUser.UserPhone,
                //    objUser.UserPhone,
                //    "https://backend.oktechnologiesapps.com",
                //    "https://backend.oktechnologiesapps.com",
                //    "34.205.216.4",
                //    objUser.UserId,
                //    objUser.DocType,
                //    objUser.DocNum,
                //    "extra4",
                //    "extra5",
                //    "extra6",
                //    "extra7",
                //    "extra8",
                //    "extra9",
                //    "extra10");


                result.cardNumber = CardParams[0];
                result.ResponseCode = "0";
                result.ResponseMessage = "CARD SAVED SUCCESSFULLY";
                result.user_id = request.user_id;

                return result;
            }
            catch (Exception ex)
            {
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                result.user_id = request.user_id;
                result.cardNumber = "0";
                return result;
            }
        }

        public static PaymentResponse GeneratePay(PaymentRequest request)
        {
            PaymentResponse result = new PaymentResponse();
            DateTime sDate = System.DateTime.Now;
            string sTransactionID = Guid.NewGuid().ToString();

            try
            {
                UserResponse objUser = UserOperations.GetUserByID(request.User_id);

                string oSQL = "SELECT * FROM ok_credit_card WHERE id = "
                                + request.Card_id + " AND person_id = " + request.Card_id;

                DataSet ds = DataOperations.SelectSimple(oSQL);
                if (ds == null || ds.Tables[0].Rows.Count == 0) throw new Exception("ERROR GETTING CARD");

                string sCardToken = ds.Tables[0].Rows[0][4].ToString();
                string sUserToken = ds.Tables[0].Rows[0][3].ToString();

                ChargeModel VerifyPayment = objEpayco.ChargeCreate(
                    sCardToken,
                    sUserToken,
                    "CC",
                    objUser.DocNum,
                    objUser.FirstName,
                    objUser.LastName,
                    objUser.UserEmail,
                    "OK-PAY-2021" + request.User_id + "" + request.ServiceId,
                    request.Description,
                    request.Amount,
                    "0",
                    request.Amount,
                    "COP",
                    "1",
                    "Calle 64F # 24-70",
                    objUser.UserPhone,
                    objUser.UserPhone,
                    "https://backend.oktechnologiesapps.com",
                    "https://backend.oktechnologiesapps.com",
                    "34.205.216.4",
                    objUser.UserId,
                    objUser.DocType,
                    objUser.DocNum,
                    request.ServiceId,
                    sTransactionID,
                    "extra6",
                    "extra7",
                    "extra8",
                    "extra9",
                    "extra10");

                if (VerifyPayment.data.respuesta != "OK")
                    throw new Exception(VerifyPayment.data.respuesta);

                string sTranCols = "service_id, payment_method, user_id, amount, description, \"authorization\", date_created, time_created, transaction_id";
                StringBuilder sTranParam = new StringBuilder();
                sTranParam.Append("" + request.ServiceId + ", ");
                sTranParam.Append("'1', ");
                sTranParam.Append("" + request.User_id + ", ");
                sTranParam.Append("" + request.Amount + ", ");
                sTranParam.Append("'" + request.Description + "', ");
                sTranParam.Append("'" + VerifyPayment.data.autorizacion + "', ");
                sTranParam.Append("'" + sDate.ToString("yyyy-MM-dd") + "', ");
                sTranParam.Append("'" + sDate.ToString("HH:mm:ss") + "', ");
                sTranParam.Append("'" + sTransactionID + "', ");

                bool bInsertTran = DataOperations.Insert("ok_sales", sTranCols, sTranParam.ToString());

                result.Authorization = VerifyPayment.data.autorizacion;
                result.PaymentResult = "SUCCESS";
                result.ResponseCode = "0";
                result.ResponseMessage = "PAYMENT SUCCESSFULL";
                return result;

            }
            catch (Exception ex)
            {
                result.Authorization = "";
                result.PaymentResult = "FAILED";
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }

        public static GetCardResponse GetCards(GetCardRequest request)
        {
            GetCardResponse result = new GetCardResponse();
            try
            {
                string sSQL = "SELECT * FROM ok_credit_card WHERE person_id = " + request.user_id;
                DataSet ds = DataOperations.SelectSimple(sSQL);
                if (ds == null || ds.Tables[0].Rows.Count == 0) throw new Exception("NO CREDIT CARD AVAIABLE");

                result.user_id = request.user_id;

                List<Card> oList = new List<Card>();

                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    Card obj = new Card();
                    obj.card_id = dr[0].ToString();
                    string cardChain = objCrypt.Get(dr[2].ToString(), Constants.sKey);
                    string[] cardObject = cardChain.Split(';');
                    string sCardNumber = objCrypt.Get(cardObject[0], Constants.sKey);
                    string[] cardTuplet = sCardNumber.Split('-');
                    obj.token = "****** " + cardTuplet[cardTuplet.Length - 1];
                    obj.card_holder = dr[5].ToString();
                    oList.Add(obj);
                }

                if(oList.Count > 0)
                {
                    result.cards = oList.ToArray();
                    result.ResponseCode = "0";
                    result.ResponseMessage = "SUCCESSFULL";
                    return result;
                }
                else
                {
                    result.cards = null;
                    result.ResponseCode = "0";
                    result.ResponseMessage = "SUCCESSFULL";
                    return result;

                }
            }
            catch(Exception ex)
            {
                result.cards = null;
                result.user_id = null;
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }

        public static DeleteCardResponse DeleteCard(DeleteCardRequest request)
        {
            DeleteCardResponse result = new DeleteCardResponse();

            try
            {
                bool bResult = DataOperations.Delete("ok_credit_card", "id = " + request.card_id);

                if (!bResult) throw new Exception("ERROR DELETING CARD");

                result.card_id = request.card_id;
                result.ResponseCode = "0";
                result.ResponseMessage = "SUCCESSFULL";
                return result;

            }
            catch (Exception ex)
            {
                result.card_id = "";
                result.ResponseCode = "999";
                result.ResponseMessage = ex.Message;
                return result;
            }
        }
    }
}
