using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.Family
{
    public class FamilyDAL : DBGenerics, IFamily
    {
        #region ActivePolicies

        public IList<Policy> GetPolicybyCivilId(string civilId)
        {
            DBGenerics db = new DBGenerics();
            string query = @"WITH MEMBERS AS (SELECT E.*

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

MEMBERS_HISTORY.PACKAGE_NUMBER,

       MEMBERS.MEMBER_NUMBER,

             MEMBERS.POLICY_NUMBER,

              RPLPROVIDERNETWORK.NETWORK_ID

  FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,

       POLICIES,  mednext.RPLPROVIDERNETWORK

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER

AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER

  AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER

  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER

  AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER

  AND RPLPROVIDERNETWORK.NETWORK_ID             = pkg.DEFAULT_NETWORK_ID
  
  and POLICIES.CANCELLATION_DATE is NULL
  and MEMBERS.DELETION_DATE is NULL

  --AND RPLPROVIDERNETWORK.PROVIDER_ID            = :CurrentProvideriD";
                         return db.ExecuteList<Policy>(query, ParamBuilder.Par(":CivilId", civilId));
          
        }

        #endregion

        #region Family Details

        public IList<FamilyInfo> GetFamilyDetails(long memberNumber, long policyNumber)
        {
            DBGenerics db = new DBGenerics();
            #region
            var query = @"   SELECT MEMBER.MEMBER_NUMBER,
                     FIRST_NAME|| '  '||
                     MIDDLE_NAME|| '  '||
                     LAST_NAME NAME,
                     NATIONAL_IDENTITY,
                     DATE_OF_BIRTH,
                     MEMHIST.PRINCIPAL_FLAG,
                     MEMHIST.PRINCIPAL_NUMBER,
                     MEMHIST.SEX_DESCRIPTION,
                     MEMHIST.RELATION_DESCRIPTION,
                     MAX (EVENT_NUMBER),
                     MEMHIST.POLICY_NUMBER
                FROM mednext.RPLMEMBER MEMBER
                     INNER JOIN mednext.RPLMEMBERMODIFICATIONHISTORY MEMHIST
                        ON     (MEMBER.INSURANCE_COMPANY_NUMBER =
                                   MEMHIST.INSURANCE_COMPANY_NUMBER)
                           AND (MEMBER.MEMBER_NUMBER =
                                   MEMHIST.MEMBER_NUMBER)
                           AND (MEMBER.POLICY_NUMBER =
                                   MEMHIST.POLICY_NUMBER)
                WHERE MEMHIST.PRINCIPAL_NUMBER = :memberid and MEMHIST.POLICY_NUMBER=:policynumber
            --AND NVL(MEMBER.EXPIRY_DATE,MEMBER.DELETION_DATE) >= TO_CHAR (SYSDATE, 'DD-MON-YY')
            --and MEMHIST.MODIFICATION_EFFECTIVE_DATE <= TO_CHAR (SYSDATE, 'DD-MON-YY')
            GROUP BY MEMBER.MEMBER_NUMBER,
                     FIRST_NAME,
                     MIDDLE_NAME,
                     LAST_NAME,
                     NATIONAL_IDENTITY,
                     DATE_OF_BIRTH,
                     MEMHIST.PRINCIPAL_FLAG,
                     MEMHIST.PRINCIPAL_NUMBER,
                     MEMHIST.SEX_DESCRIPTION,
                     MEMHIST.RELATION_DESCRIPTION,
                     MEMHIST.POLICY_NUMBER
            		 ";
            #endregion
            return db.ExecuteList<FamilyInfo>(query, ParamBuilder.Par(":policynumber", policyNumber), ParamBuilder.Par(":memberid", memberNumber));
        }

        #endregion
    }
}