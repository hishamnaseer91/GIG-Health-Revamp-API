using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.Search
{
    public class SearchDAL : DBGenerics, ISearch
    {

        public IList<DDLProviderType> DALProviderTypeDDL(string Culture)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                query = @"SELECT A.PROVIDER_TYPE_ID as Value , A.PROVIDER_TYPE_DESCIPTION_EN as Name from VWMNI_PROVIDER_TYPE A ORDER BY A.PROVIDER_TYPE_DESCIPTION_EN";
            else
                //query = @"SELECT A.PROVIDER_TYPE_ID as Value , A.PROVIDER_TYPE_DESCIPTION_AR as Name from VWMNI_PROVIDER_TYPE A ORDER BY A.PROVIDER_TYPE_DESCIPTION_AR";
                query = @"SELECT A.PROVIDER_TYPE_ID as Value , A.PROVIDER_TYPE_DESCIPTION_AR as Name from VWMNI_PROVIDER_TYPE A WHERE A.PROVIDER_TYPE_ID NOT IN ('H','I','L','P','T')
                          UNION
                          SELECT B.PROVIDER_TYPE_ID AS Value, B.PROVIDER_TYPE_EDESC AS NAME FROM mednext.PVTYPE B WHERE B.PROVIDER_TYPE_ID IN ('H','I','L','P','T')
                          ORDER BY Name";
            //            @"Select A.PROVIDER_TYPE_ID as Value , A.PROVIDER_TYPE_DESCRIPTION as Name from MEDNEXT.RPLPROVIDERTYPE a 
            //            where ( A.PROVIDER_TYPE_ID<>'A' and   A.PROVIDER_TYPE_ID<>'B'and  A.PROVIDER_TYPE_ID<>'C' 
            //            and  A.PROVIDER_TYPE_ID<>'V' and  A.PROVIDER_TYPE_ID<>'R') "
            return Db.ExecuteList<DDLProviderType>(query);
        }

        public IList<DDLPlans> DALPlanListDDL(string culture)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
                query = @"SELECT PLAN_ID, PLAN_DESC_EN  AS PLAN_DESC FROM TBL_PLANS";
            else
                query = @"SELECT PLAN_ID, PLAN_DESC_AR AS PLAN_DESC FROM TBL_PLANS";
            return Db.ExecuteList<DDLPlans>(query);
        }

        public IList<DDLLocationTypes> DALLocationTypeDDL(string culture)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
                query = @"select id, type_name_en as type_name from location_types";
            else
                query = @"select id, type_name_ar as type_name from location_types";
            return Db.ExecuteList<DDLLocationTypes>(query);
        }

        public IList<DDLNetwork> DALNetworkListDDL(string culture, int planid)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
                query = @"SELECT C.NETWORK_ID, C.POLICY_DESC_EN AS POLICY_DESC FROM TBL_PLANS A 
                          JOIN TBL_PLANS_NETWORK_MAPPING B ON B.PLAN_ID = A.PLAN_ID
                          JOIN NETWORK_POLICY_MAPPING C on C.NETWORK_ID = B.NET_ID
                          WHERE A.PLAN_ID = " + planid;
            else
                query = @"SELECT C.NETWORK_ID, C.POLICY_DESC_AR AS POLICY_DESC FROM TBL_PLANS A 
                          JOIN TBL_PLANS_NETWORK_MAPPING B ON B.PLAN_ID = A.PLAN_ID
                          JOIN NETWORK_POLICY_MAPPING C on C.NETWORK_ID = B.NET_ID
                          WHERE A.PLAN_ID = " + planid;
            return Db.ExecuteList<DDLNetwork>(query);
        }

        
        public int isDiscountOptionApplicable()
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT ENABLE_AFYA_DISCOUNT from GLOBAL_CONFIG WHERE ROWNUM = 1 ORDER BY ID DESC";

            return db.ExecuteScalarInt32(query);
        }

        public string getCardPrintDate(string CivilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"select to_date(B.FAX, 'DD/MM/YYYY') FAX from MEDNEXT.RPLMEMBER a, Mednext.RPLMEMBERADDRESS b where  
                             A.MEMBER_NUMBER=B.MEMBER_NUMBER 
                             and A.POLICY_NUMBER=(SELECT ACTIVE_AFYA_POLICY_NO from VWGLOBAL_CONFIG) 
                             and A.NATIONAL_IDENTITY = :NATIONAL_IDENTITY ";

            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":NATIONAL_IDENTITY", CivilID));
        }

        public string ClaimRegisterSmsVisibility(string CivilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT  B.POLICY_NO  from MEMBER_USERS B WHERE  
                             B.POLICY_NO =(SELECT ACTIVE_AFYA_POLICY_NO from VWGLOBAL_CONFIG) 
                             and B.CIVIL_ID = :NATIONAL_IDENTITY";

            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":NATIONAL_IDENTITY", CivilID));
        }

        public string ClaimRegisterSmsVisibility(string CivilID, string PolicyNo)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT  B.POLICY_NO  from MEMBER_USERS B WHERE  
                             B.POLICY_NO =(SELECT ACTIVE_AFYA_POLICY_NO from VWGLOBAL_CONFIG) 
                             and B.CIVIL_ID = :NATIONAL_IDENTITY and B.POLICY_NO = :POLICY_NO";

            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":NATIONAL_IDENTITY", CivilID), ParamBuilder.Par(":POLICY_NO", PolicyNo));
        }

        public string ClaimRegisterEnable(string CivilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"select  A.CIVIL_ID from SMS_EXCLUDE_MEMBER A WHERE A.CIVIL_ID = :CIVIL_ID";

            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":CIVIL_ID", CivilID));
        }

        public int UpdateExcludeMemeber(string CivilID, bool isSmsApplicable)
        {
            var result = -1;
            var deleteCivilID = @"delete from SMS_EXCLUDE_MEMBER a where A.CIVIL_ID = :CIVIL_ID";
            var deleteCivilIDresult = ExecuteNonQuery(deleteCivilID, ParamBuilder.Par(":CIVIL_ID", CivilID));

            if (!isSmsApplicable)
            {
                DBGenerics db = new DBGenerics();
                var query = @"INSERT INTO SMS_EXCLUDE_MEMBER a ( A.CIVIL_ID, A.CREATED_AT) 
                            VALUES (:CIVIL_ID ,SYSDATE)";
                result = ExecuteNonQuery(query, ParamBuilder.Par(":CIVIL_ID", CivilID));
                return result;

            }
            else
            {
                return 0;
            }

            //return result;
            
        }


        public string getAfyaOfferExpiry()
        {
            DBGenerics db = new DBGenerics();
            string query = @"select * from (select AFYA_OFFER_EXPIRY  from GLOBAL_CONFIG  ORDER BY ID DESC ) WHERE ROWNUM = 1";

            return db.ExecuteScalarNvarchar(query);
        }

        public IList<DDLNetworkByCivilId> DALNetworkListDDLbyCivilIDInToken(string culture, string civilID)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
            {
                #region Old Query

                //                query = @"SELECT DISTINCT
                //  POLICIES.POLICY_HOLDER,
                //  POLICIES.POLICY_EFFECTIVE_DATE,
                //  MEMBERS.EXPIRY_DATE,
                //  MEMBERS.DELETION_DATE,
                //  MEMBERS.MEMBER_NUMBER,
                //  MEMBERS.POLICY_NUMBER,
                //  RPLPROVIDERNETWORK.NETWORK_ID
                //FROM
                //  mednext.RPLMEMBER MEMBERS
                //  JOIN mednext.RPLMEMBERMODIFICATIONHISTORY MEMBERS_HISTORY ON
                //    MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
                //    AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
                //    AND MEMBERS_HISTORY.MODIFICATION_EFFECTIVE_DATE = (
                //      SELECT MAX(J.MODIFICATION_EFFECTIVE_DATE)
                //      FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                //      WHERE
                //        J.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
                //        AND J.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
                //        AND J.EVENT_NUMBER = (
                //          SELECT MAX(J2.EVENT_NUMBER)
                //          FROM mednext.RPLMEMBERMODIFICATIONHISTORY J2
                //          WHERE J2.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
                //            AND J2.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
                //        )
                //        AND J.MODIFICATION_EFFECTIVE_DATE <= SYSDATE
                //    )
                //  JOIN mednext.RPLPOLICYMODIFICATIONHISTORY POLICIES ON
                //    MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
                //    AND POLICIES.MODIFICATION_EFFECTIVE_DATE = (
                //      SELECT MAX(B.MODIFICATION_EFFECTIVE_DATE)
                //      FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                //      WHERE
                //        B.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
                //        AND B.EVENT_NUMBER = (
                //          SELECT MAX(B2.EVENT_NUMBER)
                //          FROM mednext.RPLPOLICYMODIFICATIONHISTORY B2
                //          WHERE B2.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
                //        )
                //        AND B.MODIFICATION_EFFECTIVE_DATE <= SYSDATE
                //    )
                //  JOIN mednext.RPLPROVIDERNETWORK ON
                //    RPLPROVIDERNETWORK.NETWORK_ID = (
                //      SELECT pkg.DEFAULT_NETWORK_ID
                //      FROM mednext.RPLPACKAGE pkg
                //      WHERE
                //        pkg.INSURANCE_COMPANY_NUMBER = MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER
                //        AND pkg.PACKAGE_NUMBER = MEMBERS_HISTORY.PACKAGE_NUMBER
                //    )
                //WHERE
                //  (MEMBERS.POLICY_NUMBER, MEMBERS.MEMBER_NUMBER) IN (
                //    SELECT X.POLICY_NUMBER, X.MEMBER_NUMBER
                //    FROM mednext.RPLMEMBER X
                //    WHERE X.NATIONAL_IDENTITY = :CivilId
                //  )
                //  AND MEMBERS.EXPIRY_DATE = (
                //    SELECT MAX(EXPIRY_DATE)
                //    FROM mednext.RPLMEMBER
                //    WHERE
                //      (POLICY_NUMBER, MEMBER_NUMBER) IN (
                //        SELECT X.POLICY_NUMBER, X.MEMBER_NUMBER
                //        FROM mednext.RPLMEMBER X
                //        WHERE X.NATIONAL_IDENTITY = :CivilId
                //      )
                //  )";
                #endregion

                query = @"WITH MEMBERS AS (
  SELECT E.*
  FROM mednext.RPLMEMBER E
  WHERE E.NATIONAL_IDENTITY = :CivilId
),
MEMBERS_HISTORY AS (
  SELECT J.*,
         MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
         MAX(J.EVENT_NUMBER) OVER (PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
  FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
  JOIN MEMBERS G ON J.POLICY_NUMBER = G.POLICY_NUMBER AND J.MEMBER_NUMBER = G.MEMBER_NUMBER
  WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
    AND NVL(J.EXPIRY_DATE, J.DELETION_DATE) >= sysdate
),
POLICIES AS (
  SELECT B.*,
         MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
         MAX(B.EVENT_NUMBER) OVER (PARTITION BY B.POLICY_NUMBER) AS MAX_EVENT
  FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
  JOIN MEMBERS M ON B.POLICY_NUMBER = M.POLICY_NUMBER
  WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
)
SELECT DISTINCT POLICIES.POLICY_HOLDER,
                POLICIES.POLICY_EFFECTIVE_DATE,
                MEMBERS.EXPIRY_DATE,
                MEMBERS.DELETION_DATE,
                MEMBERS.MEMBER_NUMBER,
                MEMBERS.POLICY_NUMBER,
                RPLPROVIDERNETWORK.NETWORK_ID
FROM MEMBERS
JOIN MEMBERS_HISTORY ON MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
                     AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
JOIN mednext.RPLPACKAGE pkg ON MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = pkg.INSURANCE_COMPANY_NUMBER
                           AND MEMBERS_HISTORY.PACKAGE_NUMBER = pkg.PACKAGE_NUMBER
JOIN POLICIES ON MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
JOIN mednext.RPLPROVIDERNETWORK ON RPLPROVIDERNETWORK.NETWORK_ID = pkg.DEFAULT_NETWORK_ID
WHERE POLICIES.CANCELLATION_DATE IS NULL
  AND MEMBERS.DELETION_DATE IS NULL
  -- AND RPLPROVIDERNETWORK.PROVIDER_ID = :CurrentProvideriD";
            }
            else
            {
                query = @"WITH MEMBERS AS (
  SELECT E.*
  FROM mednext.RPLMEMBER E
  WHERE E.NATIONAL_IDENTITY = :CivilId
),
MEMBERS_HISTORY AS (
  SELECT J.*,
         MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
         MAX(J.EVENT_NUMBER) OVER (PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
  FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
  JOIN MEMBERS G ON J.POLICY_NUMBER = G.POLICY_NUMBER AND J.MEMBER_NUMBER = G.MEMBER_NUMBER
  WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
    AND NVL(J.EXPIRY_DATE, J.DELETION_DATE) >= sysdate
),
POLICIES AS (
  SELECT B.*,
         MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
         MAX(B.EVENT_NUMBER) OVER (PARTITION BY B.POLICY_NUMBER) AS MAX_EVENT
  FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
  JOIN MEMBERS M ON B.POLICY_NUMBER = M.POLICY_NUMBER
  WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
)
SELECT DISTINCT POLICIES.POLICY_HOLDER,
                POLICIES.POLICY_EFFECTIVE_DATE,
                MEMBERS.EXPIRY_DATE,
                MEMBERS.DELETION_DATE,
                MEMBERS.MEMBER_NUMBER,
                MEMBERS.POLICY_NUMBER,
                RPLPROVIDERNETWORK.NETWORK_ID
FROM MEMBERS
JOIN MEMBERS_HISTORY ON MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
                     AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
JOIN mednext.RPLPACKAGE pkg ON MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = pkg.INSURANCE_COMPANY_NUMBER
                           AND MEMBERS_HISTORY.PACKAGE_NUMBER = pkg.PACKAGE_NUMBER
JOIN POLICIES ON MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
JOIN mednext.RPLPROVIDERNETWORK ON RPLPROVIDERNETWORK.NETWORK_ID = pkg.DEFAULT_NETWORK_ID
WHERE POLICIES.CANCELLATION_DATE IS NULL
  AND MEMBERS.DELETION_DATE IS NULL
  -- AND RPLPROVIDERNETWORK.PROVIDER_ID = :CurrentProvideriD";
            }
            return Db.ExecuteList<DDLNetworkByCivilId>(query, ParamBuilder.Par(":CivilId", civilID));
        }
        public IList<DDLNetworkByCivilId> DALNetworkListDDLbyCivilID(string culture, string civilID)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
            {
                query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                             WHERE 1=1
                          AND (E.POLICY_NUMBER,E.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM  mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = :CivilId)             
                               ),
                               MEMBERS_HISTORY AS (SELECT I.*
                                      FROM (SELECT J.*,
                                                   MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                                   MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                              FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                             WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                               AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                                     ) I
                             WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                               AND I.EVENT_NUMBER                      = MAX_EVENT
                               AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate),
                POLICIES AS (SELECT C.*
                               FROM (SELECT B.*,
                                            MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                            MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                                       FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                                      WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                        AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                              WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                                AND C.EVENT_NUMBER                = C.MAX_EVENT)  
           SELECT distinct POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,
                  MEMBERS.EXPIRY_DATE,
                  MEMBERS.DELETION_DATE,
                  MEMBERS.MEMBER_NUMBER,
                        MEMBERS.POLICY_NUMBER,
                         RPLPROVIDERNETWORK.NETWORK_ID,
                         NETWORK_POLICY_MAPPING.POLICY_DESC_EN || ' (' || MEMBERS.POLICY_NUMBER || ')' AS POLICY_DESC
                        -- RPLPROVIDERNETWORK.NETWORK_DESCRIPTION
             FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,
                  POLICIES,  mednext.RPLPROVIDERNETWORK, TBL_PLANS_NETWORK_MAPPING , NETWORK_POLICY_MAPPING
           WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
            AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
             AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
             AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER
             AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER
             AND RPLPROVIDERNETWORK.NETWORK_ID             = pkg.DEFAULT_NETWORK_ID
             AND NETWORK_POLICY_MAPPING.NETWORK_ID         = RPLPROVIDERNETWORK.NETWORK_ID
             AND TBL_PLANS_NETWORK_MAPPING.NET_ID          = NETWORK_POLICY_MAPPING.NETWORK_ID ";
            
            }
            else
            {
                query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                             WHERE 1=1
                          AND (E.POLICY_NUMBER,E.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM  mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = :CivilId)             
                               ),
                               MEMBERS_HISTORY AS (SELECT I.*
                                      FROM (SELECT J.*,
                                                   MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                                   MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                              FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                             WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                               AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                                     ) I
                             WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                               AND I.EVENT_NUMBER                      = MAX_EVENT
                               AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate),
                POLICIES AS (SELECT C.*
                               FROM (SELECT B.*,
                                            MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                            MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                                       FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                                      WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                        AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                              WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                                AND C.EVENT_NUMBER                = C.MAX_EVENT)  
           SELECT distinct POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,
                  MEMBERS.EXPIRY_DATE,
                  MEMBERS.DELETION_DATE,
                  MEMBERS.MEMBER_NUMBER,
                        MEMBERS.POLICY_NUMBER,
                         RPLPROVIDERNETWORK.NETWORK_ID,
                         NETWORK_POLICY_MAPPING.POLICY_DESC_AR || ' (' || MEMBERS.POLICY_NUMBER || ')' AS POLICY_DESC
                        -- RPLPROVIDERNETWORK.NETWORK_DESCRIPTION
             FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,
                  POLICIES,  mednext.RPLPROVIDERNETWORK, TBL_PLANS_NETWORK_MAPPING , NETWORK_POLICY_MAPPING
           WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
            AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
             AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
             AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER
             AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER
             AND RPLPROVIDERNETWORK.NETWORK_ID             = pkg.DEFAULT_NETWORK_ID
             AND NETWORK_POLICY_MAPPING.NETWORK_ID         = RPLPROVIDERNETWORK.NETWORK_ID
            AND TBL_PLANS_NETWORK_MAPPING.NET_ID          = NETWORK_POLICY_MAPPING.NETWORK_ID ";
            }
            return Db.ExecuteList<DDLNetworkByCivilId>(query, ParamBuilder.Par(":CivilId", civilID));
        }

        public IList<DDLProviders> DALProviderDDL(string Culture, string ProviderType)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (string.IsNullOrEmpty(ProviderType))
            {
                //                string Query = @"Select A.PROVIDER_ID as Value, A.PROVIDER_NAME as Name  from MEDNEXT.RPLPROVIDER a where ( A.PROVIDER_TYPE_ID<>'A' and   A.PROVIDER_TYPE_ID<>'B'and  A.PROVIDER_TYPE_ID<>'C' 
                //            and  A.PROVIDER_TYPE_ID<>'V' and  A.PROVIDER_TYPE_ID<>'R')";
                if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                    query = @"SELECT A.PROVIDER_ID as Value, A.PROVIDER_NAME_EN as Name  from VWMNI_PROVIDER A";
                else
                    query = @"SELECT A.PROVIDER_ID as Value, A.PROVIDER_NAME_AR as Name  from VWMNI_PROVIDER A";
                return Db.ExecuteList<DDLProviders>(query);
            }
            else
            {
                //string Query = "Select A.PROVIDER_ID as Value, A.PROVIDER_NAME as Name  from MEDNEXT.RPLPROVIDER a where A.PROVIDER_TYPE_ID=:pt_id";
                if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                    query = @"SELECT A.PROVIDER_ID as Value, A.PROVIDER_NAME_EN as Name  from VWMNI_PROVIDER A where A.PROVIDER_TYPE_ID =:pt_id";
                else
                    query = @"SELECT A.PROVIDER_ID as Value, A.PROVIDER_NAME_AR as Name  from VWMNI_PROVIDER A where A.PROVIDER_TYPE_ID =:pt_id";
                return Db.ExecuteList<DDLProviders>(query, ParamBuilder.Par(":pt_id", ProviderType));
            }
        }


        public IList<DDLSpecialty> DALSpecialtyDDL(string Culture)
        {
            DBGenerics Db = new DBGenerics();
            //string Query = "Select SPECIALITY_ID as Value,SPECIALITY_SHORT_DESCRIPTION as Name from MEDNEXT.RPLSPECIALITY";
            string query = string.Empty;
            if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                query = @"SELECT sp.SPECIALITY_ID Value
                        ,sp.SPECIALITY_DESCIPTION_EN Name
                    FROM VWMNI_SPECIALITY sp
                ORDER BY sp.SPECIALITY_DESCIPTION_EN";
            else
                query = @"SELECT sp.SPECIALITY_ID Value
                        ,sp.SPECIALITY_DESCIPTION_AR Name
                    FROM VWMNI_SPECIALITY sp
                ORDER BY sp.SPECIALITY_DESCIPTION_AR";
            //                query = @"SELECT sp.SPECIALITY_ID AS Value,
            //       sp.SPECIALITY_DESCIPTION_EN AS Name
            //FROM VWMNI_SPECIALITY sp
            //WHERE SPECIALITY_ID NOT IN ('NEP','U','PRDN','PSDN','ODN','GDN','EDN')

            //UNION

            //SELECT SPECIALITY_ID AS Value, 
            //       SPECIALITY_LDESC AS Name
            //FROM mednext.VT_PVSPECIALITY
            //WHERE SPECIALITY_ID IN ('NEP','U','PRDN','PSDN','ODN','GDN','EDN')

            //ORDER BY Name 
            //";
            //            else
            //                query = @" SELECT sp.SPECIALITY_ID AS Value,
            //       sp.SPECIALITY_DESCIPTION_AR AS Name
            //FROM VWMNI_SPECIALITY sp
            //WHERE SPECIALITY_ID NOT IN ('NEP','U','PRDN','PSDN','ODN','GDN','EDN')

            //UNION

            //SELECT SPECIALITY_ID AS Value, 
            //       SPECIALITY_EDESC AS Name
            //FROM mednext.VT_PVSPECIALITY
            //WHERE SPECIALITY_ID IN ('NEP','U','PRDN','PSDN','ODN','GDN','EDN')

            //ORDER BY Name 
            //";
            return Db.ExecuteList<DDLSpecialty>(query);
        }

        public IList<DDLRegion> DALRegionDDL(string Culture)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                query = @"SELECT REGION_ID as Value, REGION_DESCRIPTION_EN as Name FROM VWMNI_REGION";
            else
                query = @"SELECT REGION_ID as Value, REGION_DESCRIPTION_AR as Name FROM VWMNI_REGION";
            //if (Culture == "en") { Query = "SELECT REGION_ID as Value, REGION_DESCRIPTION as Name FROM mednext.RPLREGION"; }
            //else { Query = "SELECT a.REGION_DESCRIPTION_ARABIC as Name, a.REGION_ID as Value FROM GMONLINE.RPLREGIONExtension a"; }
            return Db.ExecuteList<DDLRegion>(query);
        }

        public IList<DDLRegionArea> DALAreaDDL(string Culture, int Region)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (Region.Equals(0))
            {
                //string Query = "SELECT DISTRICT_ID as Value, LOCAL_DESCRIPTION as Name FROM MEDNEXT.DISTRICT";
                if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                    query = @"SELECT DISTRICT_ID as Value, DISTRICT_DESCRIPTION_EN as Name FROM VWMNI_DISTRICT";
                else
                    query = @"SELECT DISTRICT_ID as Value, DISTRICT_DESCRIPTION_AR as Name FROM VWMNI_DISTRICT";
                return Db.ExecuteList<DDLRegionArea>(query);
            }
            else
            {
                //string Query = "SELECT DISTRICT_ID as Value, LOCAL_DESCRIPTION as Name FROM MEDNEXT.DISTRICT WHERE PARENT_ID=:parent_id";
                if (!String.IsNullOrEmpty(Culture) && Culture.ToLower() == "en")
                    query = @"SELECT DISTRICT_ID as Value, DISTRICT_DESCRIPTION_EN as Name FROM VWMNI_DISTRICT WHERE PARENT_ID=:parent_id";
                else
                    query = @"SELECT DISTRICT_ID as Value, DISTRICT_DESCRIPTION_AR as Name FROM VWMNI_DISTRICT WHERE PARENT_ID=:parent_id";
                return Db.ExecuteList<DDLRegionArea>(query, ParamBuilder.Par(":parent_id", Region));
            }
        }

        public long GetNetworkId(string civilId)
        {
            DBGenerics db = new DBGenerics();
            string query = "select NETWORK_ID from member_users where CIVIL_ID=:civilid";
            return db.ExecuteScalarInt64(query, ParamBuilder.Par(":civilid", civilId));
        }


        public GlobalConfigMapping getAfyaActivePolicyNumber()
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT * from GLOBAL_CONFIG WHERE ROWNUM = 1 ORDER BY ID DESC ";

            return db.ExecuteSingle<GlobalConfigMapping>(query);
        }

        public AddsMapping getActiveAddsDetils(int policyNo)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT * from ADS WHERE ROWNUM = 1 and  sysdate <= EXPIRY_DATE and POLICY_NUMBER = :POLNO  ORDER BY AD_ID DESC ";

            return db.ExecuteSingle<AddsMapping>(query, ParamBuilder.Par(":POLNO", policyNo));
        }

        public AppVersion getAppVersionDetils()
        {
            
            DBGenerics db = new DBGenerics();
            string query = @"SELECT * from TBL_APP_VERSION";

            return db.ExecuteSingle<AppVersion>(query);
        
        }

        public DiscountCouponMapping getDiscountCouponDetils(string CivilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"select CIVIL_ID, to_char( CARD_PRINT_DATE, 'dd/MM/YYYY') CARD_PRINT_DATE, to_char( COUPN_EXPIRY_DATE, 'dd/MM/YYYY') COUPN_EXPIRY_DATE, to_char( REDEEMED_DATE, 'dd/MM/YYYY') REDEEMED_DATE, COUPN_STATUS, REDEEMED_POLICY_TYPE, REDEEMED_POLICY_NO  from DISCOUNT_COUPON a where rownum =1 and  A.CIVIL_ID = :CIVIL_ID order by created_date desc";
                                        
            return db.ExecuteSingle<DiscountCouponMapping>(query, ParamBuilder.Par(":CIVIL_ID", Convert.ToInt64(CivilID)));
        }                                  
                                            
        public string GetCoveregeName(string networkID)
        {                             
            DBGenerics db = new DBGenerics();
            string query = "select ATTACHMENT from coveragebooklets WHERE NETWORKID = :NETWORKID";
            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":NETWORKID", Convert.ToInt32(networkID)));
        }
        //        #region "SearchQuery"

        //        public IList<SearchResult> DALSearchResult(SearchModel Model, string networkId)
        //        {

        //            DBGenerics db = new DBGenerics();
        //            //string query = @"select * from VWNWSEARCH a where A.PROVIDER_TYPE_ID=:providertypeid and A.NETWORK_ID=:networkId and A.REGION_ID=:regionid";
        //            //return db.ExecuteList<SearchResult>(query, ParamBuilder. Par(":providertypeid", Model.ProviderType), ParamBuilder.Par(":networkId", Model.NetworkId), 
        //            //    ParamBuilder.Par(":regionid", Model.Region));
        //            string query = string.Empty;
        //            //            query = @"SELECT pa.PROVIDER_ID
        //            //      ,pa.MAIN_PROVIDER_ID
        //            //      ,pa.LOCATION_TYPE_ID
        //            //      ,pa.LOCATION_TYPE_DESCRIPTION
        //            //      ,pa.LOCATION_NAME
        //            //      ,pa.ADDRESS
        //            //      ,pa.COUNTRY_ID
        //            //      ,pa.COUNTRY_DESCRIPTION
        //            //      ,pa.REGION_ID
        //            //      ,pa.REGION_DESCRIPTION_EN
        //            //      ,pa.REGION_DESCRIPTION_AR
        //            //      ,pa.DISTRICT_ID
        //            //      ,pa.DISTRICT_DESCRIPTION_EN
        //            //      ,pa.DISTRICT_DESCRIPTION_AR
        //            //      ,pa.CITY_ID
        //            //      ,pa.CITY_DESCRIPTION
        //            //      ,pa.PHONE_NUMBER1
        //            //      ,pa.PHONE_NUMBER2
        //            //      ,pa.FAX
        //            //      ,pa.EMAIL_ADDRESS1
        //            //      ,pa.WEB_SITE
        //            //      ,pa.LONGTITUDE
        //            //      ,pa.LATITUDE
        //            //      ,pn.PROVIDER_ID
        //            //      ,pn.PROVIDER_TYPE_ID
        //            //      ,pn.MAIN_PROVIDER_ID
        //            //      ,pn.NETWORK_ID
        //            //      ,pn.NETWORK_DESCRIPTION
        //            //      ,pn.NETWORK_TYPE_ID
        //            //      ,pn.NETWORK_START_DATE
        //            //      ,pn.NETWORK_STOP_DATE
        //            //      ,pr.PROVIDER_NAME_AR
        //            //      ,pr.PROVIDER_NAME_EN
        //            //      ,PROVIDER_TYPE_DESCIPTION_EN
        //            //      ,PROVIDER_TYPE_DESCIPTION_AR";
        //            //            if(Model.Specialty != null)
        //            //            {
        //            //                query += @" FROM VWMNI_PROVIDER_ADDRESS pa
        //            //      ,VWMNI_PROVIDER_NETWORK pn
        //            //      ,VWMNI_PROVIDER pr
        //            //      ,VWMNI_PROVIDER_SPECIALITY pc
        //            // WHERE pn.PROVIDER_ID = pa.PROVIDER_ID AND pn.PROVIDER_ID = pr.PROVIDER_ID AND PR.PROVIDER_ID = PC.PROVIDER_ID
        //            //   AND pn.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
        //            //   AND pn.NETWORK_ID = :p_NETWORK_ID
        //            //   AND PC.SPECIALITY_ID= :p_SPECIALITY_ID";
        //            //                return db.ExecuteList<SearchResult>(query, ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType), ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId), ParamBuilder.Par(":p_SPECIALITY_ID", Model.Specialty));
        //            //            }
        //            //            else
        //            //            {
        //            //                query += @" FROM VWMNI_PROVIDER_ADDRESS pa
        //            //      ,VWMNI_PROVIDER_NETWORK pn
        //            //      ,VWMNI_PROVIDER pr
        //            // WHERE pn.PROVIDER_ID = pa.PROVIDER_ID AND pn.PROVIDER_ID = pr.PROVIDER_ID
        //            //   AND pn.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
        //            //   AND pn.NETWORK_ID = :p_NETWORK_ID";
        //            //            return db.ExecuteList<SearchResult>(query, ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType), ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId));
        //            //            }
        //            query = @"SELECT  PR.PROVIDER_ID, PR.MAIN_PROVIDER_NAME_AR, PR.MAIN_PROVIDER_NAME_EN, PAM.LOCATION_NAME_AR PROVIDER_NAME_AR, PAM.LOCATION_NAME_EN PROVIDER_NAME_EN  , PR.PROVIDER_TYPE_DESCIPTION_EN, PR.PROVIDER_TYPE_DESCIPTION_AR
        //, PAM.ADDRESS, PAM.LATITUDE, PAM.LONGTITUDE, PAM.PHONE_NUMBER1,
        // PAM.REGION_DESCRIPTION_EN, PAM.REGION_DESCRIPTION_AR, PAM.DISTRICT_DESCRIPTION_EN, PAM.DISTRICT_DESCRIPTION_AR
        //  FROM VWMNI_PROVIDER pr, VWMNI_PROVIDER_ADDRESS PAM
        // WHERE pr.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
        // and PR.PROVIDER_ID= PAM.PROVIDER_ID
        //   AND (:p_PROVIDER_ID IS NULL OR pr.PROVIDER_ID = :p_PROVIDER_ID)
        //   AND (:p_REGION_ID IS NULL OR PAM.REGION_ID = :p_REGION_ID)
        //   AND (:p_DISTRICT_ID IS NULL OR PAM.DISTRICT_ID = :p_DISTRICT_ID)
        //   AND EXISTS
        //         (SELECT NULL
        //            FROM VWMNI_PROVIDER_NETWORK pn
        //           WHERE pn.NETWORK_ID = :p_NETWORK_ID
        //             AND pn.PROVIDER_ID = pr.PROVIDER_ID)
        //   AND (:p_PROVIDER_NAME_EN IS NULL     OR UPPER (pr.PROVIDER_NAME_EN) LIKE
        //          UPPER (   '%'
        //                 || :p_PROVIDER_NAME_EN
        //                 || '%'))
        //   AND (:p_PROVIDER_NAME_AR IS NULL
        //     OR pr.PROVIDER_NAME_AR LIKE
        //             '%'
        //          || :p_PROVIDER_NAME_AR
        //          || '%')
        //   AND (:p_SPECIALITY_ID IS NULL
        //     OR EXISTS
        //          (SELECT NULL
        //             FROM VWMNI_PROVIDER_SPECIALITY ps
        //            WHERE ps.PROVIDER_ID = pr.PROVIDER_ID
        //              AND ps.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
        //              AND ps.SPECIALITY_ID = :p_SPECIALITY_ID))";
        //            bool? isEng = null;
        //            string searchTemp = null;
        //            string searchText = null;
        //            if (Model.FreeTextBox != null)
        //            {
        //                searchText = Model.FreeTextBox;
        //                searchTemp = Model.FreeTextBox.Trim().Replace(" ", "");
        //                if (Regex.IsMatch(searchTemp, "^[a-zA-Z0-9.]*$"))
        //                    isEng = true;
        //                else
        //                    isEng = false;
        //            }
        //            return db.ExecuteList<SearchResult>(query,
        //                ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType),
        //                ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId),
        //                ParamBuilder.Par(":p_SPECIALITY_ID", Model.Specialty != null ? Model.Specialty : DBNull.Value.ToString()),
        //                ParamBuilder.Par(":p_REGION_ID", Model.Region != null ? Model.Region : DBNull.Value.ToString()),
        //                ParamBuilder.Par(":p_DISTRICT_ID", Model.Area != null ? Model.Area : DBNull.Value.ToString()),
        //                ParamBuilder.Par(":p_PROVIDER_ID", Model.Providers != null ? Model.Providers : DBNull.Value.ToString()),
        //                ParamBuilder.Par(":p_PROVIDER_NAME_EN", isEng != null && isEng == true ? searchText : DBNull.Value.ToString()),
        //                ParamBuilder.Par(":p_PROVIDER_NAME_AR", isEng != null && isEng == false ? searchText : DBNull.Value.ToString()));
        //        }
        //        #endregion


        #region "SearchQuery"

        public IList<SearchResult> DALSearchResult(SearchModel Model, string networkId)
        {

            DBGenerics db = new DBGenerics();
            //string query = @"select * from VWNWSEARCH a where A.PROVIDER_TYPE_ID=:providertypeid and A.NETWORK_ID=:networkId and A.REGION_ID=:regionid";
            //return db.ExecuteList<SearchResult>(query, ParamBuilder. Par(":providertypeid", Model.ProviderType), ParamBuilder.Par(":networkId", Model.NetworkId), 
            //    ParamBuilder.Par(":regionid", Model.Region));
            string query = string.Empty;
            //            query = @"SELECT pa.PROVIDER_ID
            //      ,pa.MAIN_PROVIDER_ID
            //      ,pa.LOCATION_TYPE_ID
            //      ,pa.LOCATION_TYPE_DESCRIPTION
            //      ,pa.LOCATION_NAME
            //      ,pa.ADDRESS
            //      ,pa.COUNTRY_ID
            //      ,pa.COUNTRY_DESCRIPTION
            //      ,pa.REGION_ID
            //      ,pa.REGION_DESCRIPTION_EN
            //      ,pa.REGION_DESCRIPTION_AR
            //      ,pa.DISTRICT_ID
            //      ,pa.DISTRICT_DESCRIPTION_EN
            //      ,pa.DISTRICT_DESCRIPTION_AR
            //      ,pa.CITY_ID
            //      ,pa.CITY_DESCRIPTION
            //      ,pa.PHONE_NUMBER1
            //      ,pa.PHONE_NUMBER2
            //      ,pa.FAX
            //      ,pa.EMAIL_ADDRESS1
            //      ,pa.WEB_SITE
            //      ,pa.LONGTITUDE
            //      ,pa.LATITUDE
            //      ,pn.PROVIDER_ID
            //      ,pn.PROVIDER_TYPE_ID
            //      ,pn.MAIN_PROVIDER_ID
            //      ,pn.NETWORK_ID
            //      ,pn.NETWORK_DESCRIPTION
            //      ,pn.NETWORK_TYPE_ID
            //      ,pn.NETWORK_START_DATE
            //      ,pn.NETWORK_STOP_DATE
            //      ,pr.PROVIDER_NAME_AR
            //      ,pr.PROVIDER_NAME_EN
            //      ,PROVIDER_TYPE_DESCIPTION_EN
            //      ,PROVIDER_TYPE_DESCIPTION_AR";
            //            if(Model.Specialty != null)
            //            {
            //                query += @" FROM VWMNI_PROVIDER_ADDRESS pa
            //      ,VWMNI_PROVIDER_NETWORK pn
            //      ,VWMNI_PROVIDER pr
            //      ,VWMNI_PROVIDER_SPECIALITY pc
            // WHERE pn.PROVIDER_ID = pa.PROVIDER_ID AND pn.PROVIDER_ID = pr.PROVIDER_ID AND PR.PROVIDER_ID = PC.PROVIDER_ID
            //   AND pn.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
            //   AND pn.NETWORK_ID = :p_NETWORK_ID
            //   AND PC.SPECIALITY_ID= :p_SPECIALITY_ID";
            //                return db.ExecuteList<SearchResult>(query, ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType), ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId), ParamBuilder.Par(":p_SPECIALITY_ID", Model.Specialty));
            //            }
            //            else
            //            {
            //                query += @" FROM VWMNI_PROVIDER_ADDRESS pa
            //      ,VWMNI_PROVIDER_NETWORK pn
            //      ,VWMNI_PROVIDER pr
            // WHERE pn.PROVIDER_ID = pa.PROVIDER_ID AND pn.PROVIDER_ID = pr.PROVIDER_ID
            //   AND pn.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
            //   AND pn.NETWORK_ID = :p_NETWORK_ID";
            //            return db.ExecuteList<SearchResult>(query, ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType), ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId));
            //            }

            if (Model.ProviderType == "D" || Model.ProviderType == "E")
            {
                query = @"SELECT  PR.PROVIDER_ID, PR.PROVIDER_NAME_EN, PR.PROVIDER_NAME_AR, PR.MAIN_PROVIDER_NAME_AR, PR.MAIN_PROVIDER_NAME_EN, PAM.LOCATION_NAME_AR, PAM.LOCATION_NAME_EN, PR.PROVIDER_TYPE_DESCIPTION_EN || ' - ' || PR.MAIN_PROVIDER_NAME_EN as PROVIDER_TYPE_DESCIPTION_EN, PR.PROVIDER_TYPE_DESCIPTION_AR || ' - ' || PR.MAIN_PROVIDER_NAME_AR  as PROVIDER_TYPE_DESCIPTION_AR , PAM.ADDRESS, PAM.LATITUDE, PAM.LONGTITUDE, PAM.PHONE_NUMBER1,
 PAM.REGION_DESCRIPTION_EN, PAM.REGION_DESCRIPTION_AR, PAM.DISTRICT_DESCRIPTION_EN, PAM.DISTRICT_DESCRIPTION_AR
  FROM VWMNI_PROVIDER pr, VWMNI_PROVIDER_ADDRESS PAM
 WHERE pr.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
 and PR.PROVIDER_ID= PAM.PROVIDER_ID
   AND (:p_PROVIDER_ID IS NULL OR pr.PROVIDER_ID = :p_PROVIDER_ID)
   AND (:p_REGION_ID IS NULL OR PAM.REGION_ID = :p_REGION_ID)
   AND (:p_DISTRICT_ID IS NULL OR PAM.DISTRICT_ID = :p_DISTRICT_ID)
   AND EXISTS
         (SELECT NULL
            FROM VWMNI_PROVIDER_NETWORK pn
           WHERE pn.NETWORK_ID = :p_NETWORK_ID
             AND pn.PROVIDER_ID = pr.PROVIDER_ID)
   AND (:p_PROVIDER_NAME_EN IS NULL     OR UPPER (pr.PROVIDER_NAME_EN) LIKE
          UPPER (   '%'
                 || :p_PROVIDER_NAME_EN
                 || '%'))
   AND (:p_PROVIDER_NAME_AR IS NULL
     OR pr.PROVIDER_NAME_AR LIKE
             '%'
          || :p_PROVIDER_NAME_AR
          || '%')
   AND (:p_SPECIALITY_ID IS NULL
     OR EXISTS
          (SELECT NULL
             FROM VWMNI_PROVIDER_SPECIALITY ps
            WHERE ps.PROVIDER_ID = pr.PROVIDER_ID
              AND ps.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
              AND ps.SPECIALITY_ID = :p_SPECIALITY_ID))";
            }

            else
            {
                query = @"SELECT  PR.PROVIDER_ID, PR.PROVIDER_NAME_EN, PR.PROVIDER_NAME_AR, PR.MAIN_PROVIDER_NAME_AR, PR.MAIN_PROVIDER_NAME_EN, PAM.LOCATION_NAME_AR, PAM.LOCATION_NAME_EN, PR.PROVIDER_TYPE_DESCIPTION_EN, PR.PROVIDER_TYPE_DESCIPTION_AR
, PAM.ADDRESS, PAM.LATITUDE, PAM.LONGTITUDE, PAM.PHONE_NUMBER1,
 PAM.REGION_DESCRIPTION_EN, PAM.REGION_DESCRIPTION_AR, PAM.DISTRICT_DESCRIPTION_EN, PAM.DISTRICT_DESCRIPTION_AR
  FROM VWMNI_PROVIDER pr, VWMNI_PROVIDER_ADDRESS PAM
 WHERE pr.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
 and PR.PROVIDER_ID= PAM.PROVIDER_ID
   AND (:p_PROVIDER_ID IS NULL OR pr.PROVIDER_ID = :p_PROVIDER_ID)
   AND (:p_REGION_ID IS NULL OR PAM.REGION_ID = :p_REGION_ID)
   AND (:p_DISTRICT_ID IS NULL OR PAM.DISTRICT_ID = :p_DISTRICT_ID)
   AND EXISTS
         (SELECT NULL
            FROM VWMNI_PROVIDER_NETWORK pn
           WHERE pn.NETWORK_ID = :p_NETWORK_ID
             AND pn.PROVIDER_ID = pr.PROVIDER_ID)
   AND (:p_PROVIDER_NAME_EN IS NULL     OR UPPER (pr.PROVIDER_NAME_EN) LIKE
          UPPER (   '%'
                 || :p_PROVIDER_NAME_EN
                 || '%'))
   AND (:p_PROVIDER_NAME_AR IS NULL
     OR pr.PROVIDER_NAME_AR LIKE
             '%'
          || :p_PROVIDER_NAME_AR
          || '%')
   AND (:p_SPECIALITY_ID IS NULL
     OR EXISTS
          (SELECT NULL
             FROM VWMNI_PROVIDER_SPECIALITY ps
            WHERE ps.PROVIDER_ID = pr.PROVIDER_ID
              AND ps.PROVIDER_TYPE_ID = :p_PROVIDER_TYPE_ID
              AND ps.SPECIALITY_ID = :p_SPECIALITY_ID))";
            }

            bool? isEng = null;
            string searchTemp = null;
            string searchText = null;
            if (Model.FreeTextBox != null)
            {
                searchText = Model.FreeTextBox;
                searchTemp = Model.FreeTextBox.Trim().Replace(" ", "");
                if (Regex.IsMatch(searchTemp, "^[a-zA-Z0-9.]*$"))
                    isEng = true;
                else
                    isEng = false;
            }
            return db.ExecuteList<SearchResult>(query,
                ParamBuilder.Par(":p_PROVIDER_TYPE_ID", Model.ProviderType),
                ParamBuilder.Par(":p_NETWORK_ID", Model.NetworkId),
                ParamBuilder.Par(":p_SPECIALITY_ID", Model.Specialty != null ? Model.Specialty : DBNull.Value.ToString()),
                ParamBuilder.Par(":p_REGION_ID", Model.Region != null ? Model.Region : DBNull.Value.ToString()),
                ParamBuilder.Par(":p_DISTRICT_ID", Model.Area != null ? Model.Area : DBNull.Value.ToString()),
                ParamBuilder.Par(":p_PROVIDER_ID", Model.Providers != null ? Model.Providers : DBNull.Value.ToString()),
                ParamBuilder.Par(":p_PROVIDER_NAME_EN", isEng != null && isEng == true ? searchText : DBNull.Value.ToString()),
                ParamBuilder.Par(":p_PROVIDER_NAME_AR", isEng != null && isEng == false ? searchText : DBNull.Value.ToString()));
        }
        #endregion


        public IList<BenefitsMappingList> GetBenefitsMappingList()
        {
            DBGenerics db = new DBGenerics();
            string query = "select * from BENEFITS_MAPPING";
            return db.ExecuteList<BenefitsMappingList>(query);
        }

        public IList<DDLNetworkCardMapping> BIZNetworkListDDLCardPickup(string culture, int planid)
        {
            DBGenerics Db = new DBGenerics();
            string query = string.Empty;
            if (!String.IsNullOrEmpty(culture) && (culture.ToLower() == "en" || culture.ToLower() == "en-us"))
                query = @"SELECT PLAN_ID, POLICY_NO, POLICY_DESC_ENG AS POLICY_DESC, SHOW_POLICY_LIST, LOCATION_TYPE_ID, STOP_DATE, DEFAULT_SELECTION
                          from CARD_PICKUP_LOCATION_MAPPING A WHERE A.PLAN_ID = " + planid;
            else
                query = @"SELECT PLAN_ID, POLICY_NO, POLICY_DESC_AR AS POLICY_DESC, SHOW_POLICY_LIST, LOCATION_TYPE_ID, STOP_DATE,DEFAULT_SELECTION
                          from CARD_PICKUP_LOCATION_MAPPING A WHERE A.PLAN_ID = " + planid;
            return Db.ExecuteList<DDLNetworkCardMapping>(query);
        }
    }



}
