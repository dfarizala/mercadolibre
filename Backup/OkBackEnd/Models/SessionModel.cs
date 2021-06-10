using System;

namespace OkBackEnd
{
    public class UserRequest
    {
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserPwd { get; set; }
        public string UserType { get; set; }
        public string UserLogin { get; set; }
    }

    public class UserResponse : GenericResponse
    {
        public string UserId { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string DeviceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoginType { get; set; }
        public string ReferCode { get; set; }
        public string DocType { get; set; }
        public string DocNum { get; set; }
    }
}
