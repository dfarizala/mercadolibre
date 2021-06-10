using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkBackEnd
{
    public class GenericResponse
    {
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
    }

    public class ExistRequest
    {
        public string Phone { get; set; }
    }

    public class ExistResponse : GenericResponse
    {
        public string PHone { get; set; }
        public string Exist { get; set; }
        public string Code { get; set; }
    }

    public class RegisterRequest
    {
        public string Phone { get; set; }
        public string DocType { get; set; }
        public string DocNumber { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserType { get; set; }
        public string Passwd { get; set; }
        public string LoginType { get; set; }
    }

    public class RegisterResponse : GenericResponse
    {
        public string Phone { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string ReferCode { get; set; }
    }

    public class DocType
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Abreviation { get; set; }

    }

    public class DocTypeResponse : GenericResponse
    {
        public DocType[] DocType { get; set; }
    }
}
