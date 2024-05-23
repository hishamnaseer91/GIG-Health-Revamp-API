using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class SmsModel
    {
        public string OS_SYS_ID { get; set; }
        public string OS_MOBILE_NO { get; set; }
        public string OS_TEXT { get; set; }
        public string OS_CLAIM_NO { get; set; }
        public string OS_DATE { get; set; }
        public string OS_LANG_FLAG { get; set; }
    }
}