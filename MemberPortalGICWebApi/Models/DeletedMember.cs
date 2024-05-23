using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class DeletedMemberHistory
    {
        public decimal ID { get; set; }
        public string CIVIL_ID { get; set; }
        public string POLICY_NO { get; set; }
        public string MEMBER_ID { get; set; }
        public string MEDICAL_INSURANCE_CARD { get; set; }
        public string MOBILE_NO { get; set; }
        public string PRIMARY_EMAIL { get; set; }
        public DateTime CREATION_DATE { get; set; }
        public DateTime DELETED_DATE { get; set; }
        public string DELETED_BY { get; set; }
        public string DELETION_REMARKS { get; set; }
        public string NETWORK_ID { get; set; }
        public string DEVICE_ID { get; set; }
        public int AFYA { get; set; }
        public string PRINCIPALMEMBER { get; set; }
        public string RELATIONSHIP_DESCRIPTION { get; set; }

    }
}