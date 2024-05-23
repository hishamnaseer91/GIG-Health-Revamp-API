using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class GeneralConfigration
    {
        public int ID { get; set; }

        public DateTime AfyaOfferExpiry { get; set; }
        public int ACTIVE_AFYA_POLICY_NO { get; set; }  //Afya 2
        public int ACTIVE_AFYA_3_POLICY_NO { get; set; }
        public bool ENABLE_AFYA_POLICY { get; set; }

        public bool ENABLE_AFYA_DISCOUNT { get; set; }

        public int ENABLE_AFYA_POLICY_NO { get; set; }
    }
}