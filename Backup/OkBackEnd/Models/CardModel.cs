using System;
namespace OkBackEnd
{
    public class SaveCardRequest
    {
        public string user_id { get; set; }
        public string token { get; set; }
    }

    public class CardObject
    {
        public string CardHolder { get; set; }
        public string CardHolderId { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public string CardCVV2 { get; set; }
    }

    public class SaveCardResponse : GenericResponse
    {
        public string user_id { get; set; }
        public string cardNumber { get; set; }
    }

    public class DeleteCardRequest
    {
        public string card_id { get; set; }
    }

    public class DeleteCardResponse : GenericResponse
    {
        public string card_id { get; set; }
    }

    public class PaymentRequest
    {
        public string Card_id { get; set; }
        public string User_id { get; set; }
        public string Amount { get; set; }
        public string Description { get; set; }
        public string ServiceId { get; set; }
    }

    public class PaymentResponse : GenericResponse
    {
        public string PaymentResult { get; set; }
        public string Authorization { get; set; }

    }

    public class GetCardRequest
    {
        public string user_id { get; set; }
    }

    public class Card
    {
        public string card_id { get; set; }
        public string card_holder { get; set; }
        public string token { get; set; }
    }

    public class GetCardResponse : GenericResponse
    {
        public string user_id { get; set; }
        public Card[] cards { get; set; }
    }
}
