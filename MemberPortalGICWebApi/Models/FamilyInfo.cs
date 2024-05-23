using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class FamilyInfo : IDataMapper
    {
        public string CIVIL_ID { get; set; }
        public DateTime DOB { get; set; }
        public int FaamilyID { get; set; }
        public string gender { get; set; }
        public string GUID { get; set; }
        public List<MemberAllClaims> MAllCalims { get; set; }
        public long Member_id { get; set; }
        public string Name { get; set; }
        public long PolicyNumber { get; set; }
        public string PrincIpleCID { get; set; }
        public string PrincipleMember { get; set; }
        public string Relation { get; set; }

        public void MapProperties(DbDataReader dr)
        {
            Member_id = dr["MEMBER_NUMBER"] != DBNull.Value ? Convert.ToInt64(dr["MEMBER_NUMBER"]) : default(long);
            DOB = dr["DATE_OF_BIRTH"] != DBNull.Value ? Convert.ToDateTime(dr["DATE_OF_BIRTH"]) : default(DateTime);
            gender = dr["SEX_DESCRIPTION"] != DBNull.Value ? Convert.ToString(dr["SEX_DESCRIPTION"]) : default(string);
            Name = dr["NAME"] != DBNull.Value ? Convert.ToString(dr["NAME"]) : default(string);
            PolicyNumber = dr["POLICY_NUMBER"] != DBNull.Value ? Convert.ToInt64(dr["POLICY_NUMBER"]) : default(long);
            FaamilyID = dr["PRINCIPAL_NUMBER"] != DBNull.Value ? Convert.ToInt32(dr["PRINCIPAL_NUMBER"]) : default(int);
            PrincipleMember = dr["PRINCIPAL_FLAG"] != DBNull.Value ? Convert.ToString(dr["PRINCIPAL_FLAG"]) : default(string);
            Relation = dr["RELATION_DESCRIPTION"] != DBNull.Value ? Convert.ToString(dr["RELATION_DESCRIPTION"]) : default(string);
            PrincIpleCID = dr["NATIONAL_IDENTITY"] != DBNull.Value ? Convert.ToString(dr["NATIONAL_IDENTITY"]) : default(string);
            CIVIL_ID = dr["NATIONAL_IDENTITY"] != DBNull.Value ? Convert.ToString(dr["NATIONAL_IDENTITY"]) : default(string);
            // Created_By = dr["Created"] != DBNull.Value ? Convert.ToString(dr["PHONE_NUMBER1"]) : default(string)
            GUID = Guid.NewGuid().ToString();
        }
    }
}