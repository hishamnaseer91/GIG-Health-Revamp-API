using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;

namespace MemberPortalGICWebApi.Models
{
    public class CertificateRequest
    {
        public string PolicyNo { get; set; }
        public DateTime PrintDate { get; set; }
        public  string CivilId { get; set; }
        public string MemberNo { get; set; }
        public string MemberName { get; set; }
        public string PassportNo { get; set; }
        public string Email { get; set; }
    }
    public class AfyaTravelCert : IDataMapper
    {
        public long ID { get; set; }
        public DateTime CERT_PRINT_DATE { get; set; }
        public string MEMBER_NAME { get; set; }
        public string PASSPORT_NO { get; set; }
        public string MEMBER_NO { get; set; }
        public string CIVIL_ID { get; set; }
        public string POLICY_NO { get; set; }
        public string CERT_FILE { get; set; }
        public string CERT_HASH { get; set; }
        public string SOURCE { get; set; }
        public string DOB { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            ID = dr.GetInt64("ID");
            CERT_PRINT_DATE = dr.GetDateTime("CERT_PRINT_DATE");
            MEMBER_NAME = dr.GetString("MEMBER_NAME");
            PASSPORT_NO = dr.GetString("PASSPORT_NO");
            MEMBER_NO = dr.GetString("MEMBER_NO");
            CIVIL_ID = dr.GetString("CIVIL_ID");
            POLICY_NO = dr.GetString("POLICY_NO");
            CERT_FILE = dr.GetString("CERT_FILE");
            CERT_HASH = dr.GetString("CERT_HASH");
            SOURCE = dr.GetString("SOURCE");
        }
    }
}