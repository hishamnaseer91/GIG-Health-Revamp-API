using Dapper;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace MemberPortalGICWebApi.DataObjects.TravelDAL
{
    public class TravelDAL : DBGenerics, ITravel
    {
        private readonly string _connectionString;
        public TravelDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ToString();
        }
        public AfyaTravelCert GetCertDetails(string memberNo, string policyNo)
        {
            AfyaTravelCert obj = new AfyaTravelCert();
            try
            {
                DBGenerics db = new DBGenerics();
                string query = @"select * from afya_travel_cert a where a.MEMBER_NO = :MEMBER_NO and a.POLICY_NO = :POLICY_NO";
                obj = db.ExecuteSingle<AfyaTravelCert>(query, ParamBuilder.Par(":MEMBER_NO", memberNo), ParamBuilder.Par(":POLICY_NO", policyNo));
            }
            catch (Exception ex)
            {
                return null;
            }
            return obj;
        }
        public int SaveCertDetails(AfyaTravelCert model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"insert into afya_travel_cert (MEMBER_NAME,PASSPORT_NO,MEMBER_NO,CIVIL_ID,POLICY_NO,CERT_FILE,CERT_HASH,SOURCE)
                        values (:MEMBER_NAME,:PASSPORT_NO,:MEMBER_NO,:CIVIL_ID,:POLICY_NO,:CERT_FILE,:CERT_HASH,:SOURCE) RETURNING ID INTO :my_id_param";
            try
            {
                var result = ExecuteNonQueryOracle(query, ":my_id_param",
                                   ParamBuilder.Par(":MEMBER_NAME", model.MEMBER_NAME),
                                   ParamBuilder.Par(":PASSPORT_NO", model.PASSPORT_NO),
                                   ParamBuilder.Par(":MEMBER_NO", model.MEMBER_NO),
                                   ParamBuilder.Par(":CIVIL_ID", model.CIVIL_ID),
                                   ParamBuilder.Par(":POLICY_NO", model.POLICY_NO),
                                   ParamBuilder.Par(":CERT_FILE", model.CERT_FILE),
                                   ParamBuilder.Par(":CERT_HASH", model.CERT_HASH),
                                   ParamBuilder.Par(":SOURCE", model.SOURCE),
                                   ParamBuilder.OutPar(":my_id_param", 0));
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public MemberInfoCert GetMemberInfo(string memberNo)
        {

            DBGenerics db = new DBGenerics();
            string query = "select DATE_OF_BIRTH from MEDNEXT.RPLMEMBER where MEMBER_NUMBER = :memberNo";
            return db.ExecuteSingle<MemberInfoCert>(query, ParamBuilder.Par(":memberNo", memberNo));
        }
    }
}