using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.ClaimsDAL
{
    public class ClaimsDAL : DBGenerics, IClaims
    {
        public IList<AgentsOnline> GetAllProviders()
        {
            DBGenerics db = new DBGenerics();
            var query = @"Select a.PROVIDER_NAME,a.PROVIDER_ID from MEDNEXT.RPLPROVIDER a where A.PROVIDER_TYPE_ID<>'D' ";
            return db.ExecuteList<AgentsOnline>(query);
        }

        public IList<AgentRequests> GetpendningAndRejectedEntries(GetClaimRequestModel Filter)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH AUTHORISATIONS as (SELECT * 
                          FROM mednext.RPLAUTHORISATION RPLA ";

            //Query commented 
            //if (!string.IsNullOrEmpty(Filter.Agentname))
            //{
            //    query += "    WHERE RPLA.AUTHORISATION_NUMBER  in  ( SELECT BB.AUTHORISATION_NUMBER FROM CLM_ASSIGMENT_HISTORY_NEW vv ,REQUEST_BACK bb WHERE  VV.REQUEST_ID=BB.REQUEST_ID and  vv.CLM_ASSIGN_TO = '" + Filter.Agentname + "'  and  VV.CLM_ASSIGNMENT_ID =(select max(bv.CLM_ASSIGNMENT_ID) from CLM_ASSIGMENT_HISTORY_NEW bv where BB.REQUEST_ID=  Bv.REQUEST_ID) )  ";
            //    //query += @" WHERE AUTHORISATION_NUMBER in ( SELECT BB.AUTHORISATION_NUMBER FROM CLM_ASSIGMENT_HISTORY_NEW vv ,REQUEST_BACK bb WHERE  VV.REQUEST_ID=BB.REQUEST_ID and  vv.CLM_ASSIGN_TO = '" + Filter.Agentname + "' ) ";
            //}
            //else
            //{
            //    query += @" WHERE RPLA.AUTHORISATION_NUMBER  in ( SELECT BB.AUTHORISATION_NUMBER FROM CLM_ASSIGMENT_HISTORY_NEW vv ,REQUEST_BACK bb WHERE  VV.REQUEST_ID=BB.REQUEST_ID ) ";
            //}
            query += @" WHERE RPLA.AUTHORISATION_NUMBER  in ( SELECT BB.AUTHORISATION_NUMBER FROM CLM_ASSIGMENT_HISTORY_NEW vv ,REQUEST_BACK bb WHERE  VV.REQUEST_ID=BB.REQUEST_ID ) ";

            //if (!string.IsNullOrEmpty(Filter.PlanNumber))
            //{
            //    query += @"       AND EXISTS ( SELECT 1 FROM MEDNEXT.RPLINVOICE        F
            //                                  , MEDNEXT.RPLINVOICELINE    G
            //                              WHERE F.INVOICE_NUMBER = G.INVOICE_NUMBER
            //                                AND F.INVOICE_NUMBER =RPLA.PRE_INVOICE_NUMBER
            //                                AND G.PLAN_NUMBER    = " + Filter.PlanNumber + " )   ";
            //}

            if (!string.IsNullOrEmpty(Filter.IncidentType))
            {
                if (Filter.IncidentType == "4")
                {
                    query += @"   and INCIDENT_NUMBER in( Select INCIDENT_NUMBER from MEDNEXT.RPLINCIDENT a where A.INCIDENT_NUMBER=INCIDENT_NUMBER and A.INCIDENT_TYPE_ID=02 and A.OUTPATIENT_CATEGORY_ID='CHOP')  ";
                }
                if (Filter.IncidentType == "3")
                {
                    query += @"   and INCIDENT_NUMBER in( Select INCIDENT_NUMBER from MEDNEXT.RPLINCIDENT a where A.INCIDENT_NUMBER=INCIDENT_NUMBER and A.INCIDENT_TYPE_ID=02 and A.OUTPATIENT_CATEGORY_ID='DE')  ";
                }
                if (Filter.IncidentType == "2")
                {
                    query += @"  and INCIDENT_NUMBER in( Select INCIDENT_NUMBER from MEDNEXT.RPLINCIDENT a where A.INCIDENT_NUMBER=INCIDENT_NUMBER and A.INCIDENT_TYPE_ID=02 and A.OUTPATIENT_CATEGORY_ID is null ) ";

                }
                if (Filter.IncidentType == "1")
                {
                    query += @"   and INCIDENT_NUMBER in( Select INCIDENT_NUMBER from MEDNEXT.RPLINCIDENT a where A.INCIDENT_NUMBER=INCIDENT_NUMBER and A.INCIDENT_TYPE_ID=01)  ";
                }

            }
            //if (!string.IsNullOrEmpty(Filter.AuthorizationNo))
            //{
            //    query += @"  and RPLA.AUTHORISATION_NUMBER= " + Filter.AuthorizationNo;
            //}
            //if (!string.IsNullOrEmpty(Filter.IncidentNo))
            //{
            //    query += @"  and RPLA.INCIDENT_NUMBER= " + Filter.IncidentNo;
            //}
            //if (!string.IsNullOrEmpty(Filter.PolicyNo))
            //{
            //    query += @"  and RPLA.POLICY_NUMBER= " + Filter.PolicyNo;
            //}
            //if (!string.IsNullOrEmpty(Filter.MemberID))
            //{
            //    query += @"  and RPLA.MEMBER_NUMBER= " + Filter.MemberID;
            //}

            if (Filter.AuthorizeStatus == null)
            {
                query += @"  AND (RPLA.AUTHORISATION_STATUS_ID = 'A')";
            }

            //if (Filter.AuthorizeStatus == "1")
            //{
            //    query += @"  AND (RPLA.AUTHORISATION_STATUS_ID = 'A' OR RPLA.AUTHORISATION_STATUS_ID = 'R')";
            //}
            //if (Filter.AuthorizeStatus == "2")
            //{
            //    query += @"AND (RPLA.AUTHORISATION_STATUS_ID = 'A' )";
            //}
            //if (Filter.AuthorizeStatus == "3")
            //{
            //    query += @"AND ( RPLA.AUTHORISATION_STATUS_ID = 'R')";
            //}
            if (!string.IsNullOrEmpty(Filter.CivilId))
            {
                query += @"   AND (POLICY_NUMBER,MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY ='" + Filter.CivilId + "')";
            }
            if (Filter.ToDate2 != null && Filter.FromDate2 != null)
            {
                query += @"and AUTHORISATION_EFFECTIVE_DATE >=TO_DATE(nvl('" + Filter.FromDate2 + "','10/09/2015'), 'dd/MM/yyyy') AND   AUTHORISATION_EFFECTIVE_DATE<= TO_DATE(nvl('" + Filter.ToDate2 + @"', sysdate),'dd/MM/yyyy')";
            }
            else if (Filter.ToDate2 != null)
            {
                query += @"and   AUTHORISATION_EFFECTIVE_DATE<= TO_DATE(nvl('" + Filter.ToDate2 + @"', sysdate),'dd/MM/yyyy')";
            }
            else if (Filter.FromDate2 != null)
            {
                query += @"and AUTHORISATION_EFFECTIVE_DATE >=TO_DATE(nvl('" + Filter.FromDate2 + "','10/09/2015'), 'dd/MM/yyyy') ";
            }

            //if (!string.IsNullOrEmpty(Filter.ICD_10_Code))
            //{
            //    query += @"   AND  RPLA.AUTHORISATION_NUMBER in (  Select AA.AUTHORISATION_NUMBER FROM MEDNEXT.RPLINCIDENTDIAGNOSIS DA, MEDNEXT.RPLAUTHORISATION AA where AA.INCIDENT_NUMBER=DA.INCIDENT_NUMBER and DA.DIAGNOSIS_ID='" + Filter.ICD_10_Code + "')  ";
            //}
            query += @"
       
                     ),
     POLICIES AS (SELECT K.*
                    FROM (SELECT DISTINCT L.*,
                                 MAX(L.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY L.POLICY_NUMBER,L.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(L.EVENT_NUMBER)                OVER (PARTITION BY L.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY L
                           WHERE L.POLICY_NUMBER in (SELECT POLICY_NUMBER FROM AUTHORISATIONS)
                             AND L.MODIFICATION_EFFECTIVE_DATE <= sysdate) K
                   WHERE K.MODIFICATION_EFFECTIVE_DATE = K.MAX_DATE
                     AND K.EVENT_NUMBER                = K.MAX_EVENT
                     )
SELECT 
       B.AUTHORISATION_NUMBER                               AS REQUEST_ID,
       B.INCIDENT_NUMBER                                    AS CLAIM_NUMBER,
       B.POLICY_NUMBER,
       A.POLICY_HOLDER                                      AS COMPANY,
       B.REQUESTER_PROVIDER_ID                              AS HOSPITAL_CODE,
       D.PROVIDER_NAME                                      AS HOSPITAL_NAME,
       B.MEMBER_NUMBER                                      AS MEMBER_ID,
       TO_CHAR(E.MODIFICATION_DATE,'DD/MM/YYYY HH24:mm:ss') AS CLAIM_REGISTER_TIME,
       (SELECT SUM(NVL(REQUESTED_AMOUNT,0)) 
          FROM mednext.RPLAUTHORISATIONPROCEDURE F
          WHERE F.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER) AS ESTIMATED_AMOUNT,
    (SELECT SUM(NVL(F.SYSTEM_ESTIMATED_COST_AMOUNT,0)) 
          FROM mednext.RPLAUTHORISATIONPROCEDURE F
          WHERE F.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER) AS SYSTEM_ESTIMATED_COST_AMOUNT,
       B.AUTHORISATION_STATUS_ID                                 AS STATUS_CODE,
      BK.MDPR_STATUS                                AS STATUS,
       (select VALUE
              from mednext.RPLMEMBERCUSTOMFIELD dd
             where dd.POLICY_NUMBER = B.POLICY_NUMBER
               and dd.MEMBER_NUMBER = B.MEMBER_NUMBER
               and dd.FIELD_ID      = 'VIP'
              ) AS VIP,
(SELECT SPECIALITY_ID
          FROM (SELECT G.SPECIALITY_ID
                      ,RANK() OVER(PARTITION BY decode (G.primary_speciality_indicator,'Y',1,2) ORDER BY b.rowid)
                  FROM MEDNEXT.RPLPROVIDERSPECIALITY G
                 WHERE 1=1
                   AND G.PROVIDER_ID = C.PROVIDER_ID
                   AND rownum = 1) ) SPECIALITY_ID,
       BK.RBY,
       fd.NATIONAL_IDENTITY,
       BK.REQUEST_ID as ARID ,
BK.REJECTION_REASON_ID ,
BK.REJECTION_REASON
,BK.NOTES_REPORT
 ,VV.CLM_ASSIGN_DT as CLM_TIME  
,BK.MEDNEXT_STATUS_DT
,VV.CLM_ASSIGN_TO

  
FROM   POLICIES                     A,
       AUTHORISATIONS               B,
       mednext.RPLINCIDENT                  C,
       mednext.RPLPROVIDER                  D,
       mednext.RPLAUTHORISATIONMODHISTORY   E,
       REQUEST_BACK BK,
       mednext.RPLMEMBER fd
       , CLM_ASSIGMENT_HISTORY_NEW vv
WHERE  A.POLICY_NUMBER           = B.POLICY_NUMBER

  and BK.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER
  and BK.CLM_NO  = B.INCIDENT_NUMBER  
  and B.POLICY_NUMBER=fd.POLICY_NUMBER
 and BK.REQUEST_ID= VV.REQUEST_ID
  and VV.CLM_ASSIGNMENT_ID=(select max(v.CLM_ASSIGNMENT_ID) from CLM_ASSIGMENT_HISTORY_NEW v where BK.REQUEST_ID= v.REQUEST_ID)
  and B.MEMBER_NUMBER= fd.MEMBER_NUMBER
  AND  B.INCIDENT_NUMBER         = C.INCIDENT_NUMBER
  AND  B.REQUESTER_PROVIDER_ID   = D.PROVIDER_ID";
            if (!string.IsNullOrEmpty(Filter.Providerid))
            {
                query += "   and D.PROVIDER_sID='" + Filter.Providerid + "'";
            }
            query += @"  AND  B.AUTHORISATION_NUMBER    = E.AUTHORISATION_NUMBER
  AND  E.ACTION_TYPE_ID          = '10'";
            return db.ExecuteList<AgentRequests>(query);
        }

    
        public IList<AuthorizationServices> GETAuthorizationServicesWithSystemAmount(ProcedureloadModel model)
        {

            DBGenerics db = new DBGenerics();
            var query = @"Select b.*,c.SYSTEM_ESTIMATED_COST_AMOUNT from REQUEST_DETAIL b,MEDNEXT.RPLAUTHORISATIONPROCEDURE c where  b.IS_DELETED= 0 and B.REQUESTID=:RequestId2  and C.PROCEDURE_NUMBER=B.MDPROCEDURE_NUMBER";
            return db.ExecuteList<AuthorizationServices>(query, ParamBuilder.Par(":RequestId2", model.RequestId));
            
            
        }

        //New Query for all claims
        public IList<ClaimsModel> DALClaimsUser(string Status, string policyNumber, string membernumber , string Culture)
        {
            DBGenerics db = new DBGenerics();
            #region old Query
            //            var Query = @"SELECT AUTH.POLICY_NUMBER,
            //AUTH.AUTHORISATION_NUMBER,
            //         AUTH.MEMBER_NUMBER,
            //         BK.CR_DT as CREATION_DATE,
            //         SUM (GROSS_CLAIMED_AMOUNT) NET_PAYABLE_AMOUNT,
            //         CUSTOMER_NUMBER,
            //         POLICY_HOLDER,
            //         PROVID.PROVIDER_NAME,
            //         AUTH.INCIDENT_NUMBER,
            //         NOTE,
            //         AUTH.AUTHORISATION_NUMBER,
            //         (SELECT SPECIALITY_ID
            //          FROM (SELECT G.SPECIALITY_ID
            //                      ,RANK() OVER(PARTITION BY decode (G.primary_speciality_indicator,'Y',1,2) ORDER BY AUTH.rowid)
            //                  FROM MEDNEXT.RPLPROVIDERSPECIALITY G
            //                 WHERE 1=1
            //                   AND G.PROVIDER_ID = C.PROVIDER_ID
            //                   AND rownum = 1) ) SPECIALITY_ID
            // ,AUTH.AUTHORISATION_STATUS_DESCR
            //  ,BK.MEDNEXT_STATUS_DT,BK.REQUEST_ID
            //  , (SELECT SUM(NVL(REQUESTED_AMOUNT,0)) 
            //          FROM mednext.RPLAUTHORISATIONPROCEDURE F
            //          WHERE F.AUTHORISATION_NUMBER = AUTH.AUTHORISATION_NUMBER) AS ESTIMATED_AMOUNT
            //    FROM mednext.RPLAUTHORISATION AUTH
            //         INNER JOIN mednext.RPLINVOICE RPLIVICE
            //            ON     (AUTH.POLICY_NUMBER = RPLIVICE.POLICY_NUMBER)
            //               AND (AUTH.MEMBER_NUMBER = RPLIVICE.MEMBER_NUMBER)

            //         INNER JOIN mednext.RPLPROVIDER PROVID
            //            ON (REQUESTER_PROVIDER_ID = PROVID.PROVIDER_ID)

            //         INNER JOIN mednext.RPLPOLICYMODIFICATIONHISTORY PROVIDHIS
            //            ON (AUTH.POLICY_NUMBER =
            //                   PROVIDHIS.POLICY_NUMBER)

            //         LEFT JOIN mednext.RPLHOSPITALISATIONNOTE HOSPNOTE
            //            ON (AUTH.INCIDENT_NUMBER =
            //                   HOSPNOTE.INCIDENT_NUMBER)

            //         INNER JOIN mednext.RPLINVOICELINE INCOICELINE
            //            ON     (AUTH.AUTHORISATION_NUMBER =
            //                       INCOICELINE.AUTHORISATION_NUMBER)
            //                AND (RPLIVICE.INVOICE_NUMBER = INCOICELINE.INVOICE_NUMBER), MEDNEXT.RPLINCIDENT C,
            //GMONLINE.REQUEST_BACK              BK
            //   WHERE     AUTH.POLICY_NUMBER =:policynumber
            //             AND AUTH.MEMBER_NUMBER = :member_number
            //             -- AUTH.National_Identity=:ID            
            //and  NET_PAYABLE_AMOUNT>0 
            //  AND (AUTH.AUTHORISATION_STATUS_ID = 'A' OR AUTH.AUTHORISATION_STATUS_ID = 'R')  AND (AUTH.AUTHORISATION_STATUS_ID = 'A' )  AND  AUTH.INCIDENT_NUMBER         = C.INCIDENT_NUMBER
            //AND BK.AUTHORISATION_NUMBER = AUTH.AUTHORISATION_NUMBER
            //            AND BK.CLM_NO  = C.INCIDENT_NUMBER
            //GROUP BY BK.CR_DT,
            //         AUTH.POLICY_NUMBER,
            //         AUTH.MEMBER_NUMBER,

            //         PROVID.PROVIDER_NAME,
            //           AUTH.rowid,
            //         AUTH.INCIDENT_NUMBER,
            //         CUSTOMER_NUMBER,
            //         POLICY_HOLDER,
            //         NOTE,
            //         AUTH.AUTHORISATION_NUMBER,
            //         SPECIALITY_ID,
            //        AUTHORISATION_STATUS_DESCR, C.PROVIDER_ID, BK.MEDNEXT_STATUS_DT,BK.REQUEST_ID";
            #endregion

            string Query = string.Empty;
            if (!String.IsNullOrEmpty(Culture) && (Culture.ToLower() == "en" || Culture.ToLower() == "en-us"))
            {
                #region NEw Query
                Query = @"WITH AUTHORISATIONS as (SELECT * 
                          FROM mednext.RPLAUTHORISATION RPLA  
                          WHERE 
                          RPLA.POLICY_NUMBER= :policynumber  and RPLA.MEMBER_NUMBER= :member_number  AND (RPLA.AUTHORISATION_STATUS_ID = 'A')
                     )
SELECT 
       B.AUTHORISATION_NUMBER                               AS REQUEST_ID,
       B.INCIDENT_NUMBER                                    AS CLAIM_NUMBER,
       B.POLICY_NUMBER,
       B.REQUESTER_PROVIDER_ID                              AS HOSPITAL_CODE,
       F.PROVIDER_NAME_EN                                      AS HOSPITAL_NAME,
       B.MEMBER_NUMBER                                      AS MEMBER_ID,
       E.MODIFICATION_DATE  AS CLAIM_REGISTER_TIME,
     --  TO_CHAR(E.MODIFICATION_DATE,'DD/MM/YYYY') AS CLAIM_REGISTER_TIME,
     to_char((SELECT SUM(NVL(REQUESTED_AMOUNT,0)) 
          FROM mednext.RPLAUTHORISATIONPROCEDURE F
          WHERE F.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER and F.PROCEDURE_STATUS_ID = 'A'), '9,999.999')   AS ESTIMATED_AMOUNT,
         B.AUTHORISATION_STATUS_ID                                 AS STATUS_CODE,
      BK.MDPR_STATUS                                AS AUTHORISATION_STATUS_DESCR,
     (SELECT SPECIALITY_ID
          FROM (SELECT G.SPECIALITY_ID
                      ,RANK() OVER(PARTITION BY decode (G.primary_speciality_indicator,'Y',1,2) ORDER BY b.rowid)
                  FROM MEDNEXT.RPLPROVIDERSPECIALITY G
                 WHERE 1=1
                   AND G.PROVIDER_ID = C.PROVIDER_ID
                   AND rownum = 1) ) SPECIALITY_ID,
       BK.RBY,
       fd.NATIONAL_IDENTITY,
BK.MEDNEXT_STATUS_DT
FROM   AUTHORISATIONS               B,
       mednext.RPLINCIDENT                  C,
       mednext.RPLPROVIDER                  D,
       mednext.RPLAUTHORISATIONMODHISTORY   E,
       VWMNI_PROVIDER F,
       REQUEST_BACK BK,
       mednext.RPLMEMBER fd
WHERE  BK.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER
  and BK.CLM_NO  = B.INCIDENT_NUMBER  
  and B.POLICY_NUMBER=fd.POLICY_NUMBER
  and B.MEMBER_NUMBER= fd.MEMBER_NUMBER
  AND  B.INCIDENT_NUMBER         = C.INCIDENT_NUMBER
  AND  B.REQUESTER_PROVIDER_ID   = D.PROVIDER_ID 
  AND D.PROVIDER_ID = F.PROVIDER_ID
AND  B.AUTHORISATION_NUMBER    = E.AUTHORISATION_NUMBER
 AND  E.ACTION_TYPE_ID          in ( '10') order by CLAIM_REGISTER_TIME desc";
                #endregion
            }
            else
            {
                #region NEw Query
                Query = @"WITH AUTHORISATIONS as (SELECT * 
                          FROM mednext.RPLAUTHORISATION RPLA  
                          WHERE 
                          RPLA.POLICY_NUMBER= :policynumber  and RPLA.MEMBER_NUMBER= :member_number  AND (RPLA.AUTHORISATION_STATUS_ID = 'A')
                     )
SELECT 
       B.AUTHORISATION_NUMBER                               AS REQUEST_ID,
       B.INCIDENT_NUMBER                                    AS CLAIM_NUMBER,
       B.POLICY_NUMBER,
       B.REQUESTER_PROVIDER_ID                              AS HOSPITAL_CODE,
       F.PROVIDER_NAME_AR                                      AS HOSPITAL_NAME,
       B.MEMBER_NUMBER                                      AS MEMBER_ID,
       E.MODIFICATION_DATE  AS CLAIM_REGISTER_TIME,
     --  TO_CHAR(E.MODIFICATION_DATE,'DD/MM/YYYY') AS CLAIM_REGISTER_TIME,
       to_char((SELECT SUM(NVL(REQUESTED_AMOUNT,0)) 
          FROM mednext.RPLAUTHORISATIONPROCEDURE F
          WHERE F.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER), '9,999.999')   AS ESTIMATED_AMOUNT,
         B.AUTHORISATION_STATUS_ID                                 AS STATUS_CODE,
      BK.MDPR_STATUS                                AS AUTHORISATION_STATUS_DESCR,
     (SELECT SPECIALITY_ID
          FROM (SELECT G.SPECIALITY_ID
                      ,RANK() OVER(PARTITION BY decode (G.primary_speciality_indicator,'Y',1,2) ORDER BY b.rowid)
                  FROM MEDNEXT.RPLPROVIDERSPECIALITY G
                 WHERE 1=1
                   AND G.PROVIDER_ID = C.PROVIDER_ID
                   AND rownum = 1) ) SPECIALITY_ID,
       BK.RBY,
       fd.NATIONAL_IDENTITY,
BK.MEDNEXT_STATUS_DT
FROM   AUTHORISATIONS               B,
       mednext.RPLINCIDENT                  C,
       mednext.RPLPROVIDER                  D,
       mednext.RPLAUTHORISATIONMODHISTORY   E,
       VWMNI_PROVIDER F,
       REQUEST_BACK BK,
       mednext.RPLMEMBER fd
WHERE  BK.AUTHORISATION_NUMBER = B.AUTHORISATION_NUMBER
  and BK.CLM_NO  = B.INCIDENT_NUMBER  
  and B.POLICY_NUMBER=fd.POLICY_NUMBER
  and B.MEMBER_NUMBER= fd.MEMBER_NUMBER
  AND  B.INCIDENT_NUMBER         = C.INCIDENT_NUMBER
  AND  B.REQUESTER_PROVIDER_ID   = D.PROVIDER_ID 
  AND D.PROVIDER_ID = F.PROVIDER_ID
AND  B.AUTHORISATION_NUMBER    = E.AUTHORISATION_NUMBER
 AND  E.ACTION_TYPE_ID          in ( '10') order by CLAIM_REGISTER_TIME desc";
                #endregion
            }
            return db.ExecuteList<ClaimsModel>(Query, ParamBuilder.Par(":policynumber", policyNumber), ParamBuilder.Par(":member_number", membernumber));

        }
    }

}