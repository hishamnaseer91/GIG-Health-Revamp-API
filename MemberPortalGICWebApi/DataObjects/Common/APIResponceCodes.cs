using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class APIResponceCodes
    {
        public string Code { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DescriptionArabic { get; set; }
        public Object Data { get; set; }
        public string Result { get; set; }

        public string ResultAR { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class Coverege
    {
        public string FileName { get; set; }
        public string Base64 { get; set; }

        
    }
    public class GenericModel
    {
        public string name         { get; set;}  
        public string arabicName   { get; set;}
        public string civilId { get; set; }

        public string medicalNo { get; set; }

        public string policyNo { get; set; }
    }
  
}