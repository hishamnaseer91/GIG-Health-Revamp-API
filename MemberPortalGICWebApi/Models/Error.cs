using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class Error
    {
        public string ErrorName { get; set; }
        public string ErrorDesc { get; set; }
        public string ErrorDescArabic { get; set; }
    }

    public class RegisterCompleteUserResponce {
        public string UID { get; set; }
        public string MemberId { get; set; }
        public string UserName { get; set; }
        public string UserNameArabic { get; set; }
        public string MedicalInsuranceCard { get; set;}

    }

    public class AlreadyRegisteredDesciptionResponce
    {

        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionArabic { get; set; }
    }

    public class UserExistResponce {
        public string Code { get; set; }
        public string Description { get; set; }
        public string DescriptionArabic { get; set; }
    }

    public class ErrorLogs_DB {
        public string ErrorCode { get; set; }
        public string ErorDesc { get; set; }

        public string ErrorExp { get; set; }
        public string TypeError { get; set; }
    }
}
