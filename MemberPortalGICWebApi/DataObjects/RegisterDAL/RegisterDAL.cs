using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.RegisterDAL
{
    public class RegisterDAL : DBGenerics, IRegister
    {

        public MemberInfo GetName(string civilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT A.FIRST_NAME || ' ' || A.MIDDLE_NAME || ' ' ||A.LAST_NAME as Name , B.EMAIL_ADDRESS1
                             FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                             WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid
                             and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                             order by A.EXPIRY_DATE DESC";

            return db.ExecuteSingle<MemberInfo>(query, ParamBuilder.Par(":nid", civilID));
        }

        public bool IsMemberExist(string civilId)
        {
            bool isExist;
            DBGenerics db = new DBGenerics();
            var query = "select CIVIL_ID  from member_users where CIVIL_ID = :civilId";
            string result = db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":civilId", civilId));

            if (result == "-1")
            { isExist = false; }
            else
            { isExist = true; }

            return isExist;
        }

        public bool IsRegistrationComplete(string civilId)
        {
            bool isRegistrationComplete;
            DBGenerics db = new DBGenerics();

            int regComplete = 0;
            int isFirstLogin = 1;
            var query = "select CIVIL_ID  from member_users where CIVIL_ID = :civilId and registration_complete =:isRegistration and IS_FIRST_LOGIN = :isFirstLogin";
            string result = db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":civilId", civilId), ParamBuilder.Par(":isRegistration", regComplete), ParamBuilder.Par(":isFirstLogin", isFirstLogin));

            if (result == "-1")
            { isRegistrationComplete = false; }
            else
            { isRegistrationComplete = true; }
            return isRegistrationComplete;
        }

        public AddMemberUserModel GetMemberUserDetails(string civilId, string memberID)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1=1
               AND (E.POLICY_NUMBER,E.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM  mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = :NantionalCivilId)
             
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
SELECT POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,

       MEMBERS.EXPIRY_DATE,
       MEMBERS.DELETION_DATE,
       MEMBERS.MEMBER_NUMBER,
 MEMBERS.PRINCIPAL_FLAG,
MEMBERS.RELATION_DESCRIPTION,
             MEMBERS.POLICY_NUMBER,
               MEMBERS.First_NAme || ' ' || MEMBERS.Middle_NAME || ' ' || MEMBERS.last_name as Name ,
             pkg.DEFAULT_NETWORK_ID as NETWORK_ID
       
  FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,
       POLICIES
WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
 AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER
  
  order by POLICIES.POLICY_NUMBER asc
  ";
            //Remaing add memberID
            return db.ExecuteSingle<AddMemberUserModel>(query, ParamBuilder.Par(":NantionalCivilId", civilId));
        }


        public int UpdateRegisterMemberComplete(RegisterMemberCompleteModel model)
        {

            DBGenerics db = new DBGenerics();

            //  var query = "select Civil_ID from member_users where civil_id =:civilId and Medical_Insurance_Card=:medicalCardNo  and Passwprd =:password";
            var query = "Update member_users set IS_FIRST_LOGIN = '0' , REGISTRATION_COMPLETE = '1' where civil_id =:civilId   and Password =:password   RETURNING ID INTO :my_id_param";

            var result = db.ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":civilId", model.CivilId), ParamBuilder.Par(":password", model.Password), ParamBuilder.OutPar(":my_id_param", 0));
            return result;

        }
        public MemberUser GetMemberProfileDetails(string Id)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E, member_users
                  WHERE 1 = 1


                   AND E.POLICY_NUMBER = MEMBER_USERS.POLICY_NO
                AND E.MEMBER_NUMBER = MEMBER_USERS.MEMBER_ID
                and member_users.id =:id
                    ),
     MEMBERS_HISTORY AS (SELECT I.*
                           FROM(SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND(J.POLICY_NUMBER, J.MEMBER_NUMBER) IN(SELECT G.POLICY_NUMBER, MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE = MAX_DATE
                    AND I.EVENT_NUMBER = MAX_EVENT
                    AND NVL(I.EXPIRY_DATE, I.DELETION_DATE) >= sysdate),
     POLICIES AS (SELECT C.*
                    FROM(SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER(PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN(SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER = C.MAX_EVENT)                     
SELECT RPLNETWORK.NETWORK_DESCRIPTION,
MEMBERS.MEMBER_NUMBER,
         MEMBERS.FIRST_NAME || '  ' ||
       MEMBERS.MIDDLE_NAME || '  ' ||
       MEMBERS.LAST_NAME NAME,
        MEMBERS.INTERNATIONAL_FIRST_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_LAST_NAME INTERNATIONAL_NAME,
        (select Value
          from mednext.RPLMEMBERCUSTOMFIELD CUST
         where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
           and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
           and CUST.FIELD_ID = 'VIP'
          and CUST.VALUE = 'Y') VIP_FLAG,
       MEMBERS_HISTORY.MEMBER_EFFECTIVE_DATE,
       MEMBERS_HISTORY.EXPIRY_DATE MEMBEREXPIRY,
       MEMBERS_HISTORY.DELETION_DATE   ,
       MEMBERS.NATIONAL_IDENTITY,
       POLICIES.POLICY_HOLDER,
       POLICIES.POLICY_NUMBER,
       POLICIES.POLICY_EFFECTIVE_DATE,
       POLICIES.EXPIRY_DATE POLICYEXPIRY,
       POLICIES.CANCELLATION_DATE POLICY_CANCELLATION_DATE,
       RPLPACKAGE.INSURANCE_COMPANY_NUMBER,
       RPLPACKAGE.DEFAULT_NETWORK_ID,
       RPLMEMBERADDRESS.PHONE_NUMBER1,
       RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
       MEMBERS.CLASS_ID,
       MEMBERS.RELATION_ID,
       MEMBERS.EFFECTIVE_DATE,
       MEMBERS.SEX_ID GENDER_ID,
       MEMBERS.DATE_OF_BIRTH, 
       MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR
  FROM MEMBERS,
       MEMBERS_HISTORY,
       POLICIES,
       mednext.RPLPACKAGE,
       mednext.RPLNETWORK,
       mednext.RPLMEMBERADDRESS

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
  AND MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS.MEMBER_NUMBER = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER = RPLPACKAGE.PACKAGE_NUMBER
  AND RPLNETWORK.NETWORK_ID = RPLPACKAGE.DEFAULT_NETWORK_ID
           ";
            return db.ExecuteSingle<MemberUser>(query, ParamBuilder.Par(":id", Id));

        }
        public int AddNewMember(AddMemberUserModel model)
        {
            DBGenerics db = new DBGenerics();

            // CivilId = inputmodel.CivilId,
            //PolicyNo = result.PolicyNo,
            //Member_Id = inputmodel.MedicalInsuranceCardNumber,
            //MedicalInsuranceCardNumber = inputmodel.MedicalInsuranceCardNumber,
            //Mobile = inputmodel.PhoneNumber,
            //CreationDate = DateTime.Now,
            //Password = EncryptedPassword

            var query = @"INSERT INTO member_users a ( A.CIVIL_ID, A.POLICY_NO,A.MEMBER_ID,A.MEDICAL_INSURANCE_CARD, A.MOBILE_NO,A.PASSWORD,A.CREATION_DATE,A.NETWORK_ID,A.DEVICE_ID,A.USER_AGENT,A.PrincipalMember,A.RELATIONSHIP_DESCRIPTION) 
                                             
                                              VALUES (:civilId ,:policyNo,:memberId,:micard,:mobile,:password,SYSDATE,:netowrkid,:deviceId,:useragent,:PrincipalMember,:RELATIONSHIP_DESCRIPTION) RETURNING A.Id INTO :my_id_param";

            var result = ExecuteNonQueryOracle(query, ":my_id_param",
                                    ParamBuilder.Par(":civilId", model.CivilId),
                                    ParamBuilder.Par(":policyNo", model.PolicyNo),
                                    ParamBuilder.Par(":memberId", model.Member_Id),
                                    ParamBuilder.Par(":micard", model.MedicalInsuranceCardNumber),
                                    ParamBuilder.Par(":mobile", model.Mobile),
                                    ParamBuilder.Par(":password", model.Password),
                                    ParamBuilder.Par(":netowrkid", model.Network_Id),
                                    ParamBuilder.Par(":deviceId", model.client_id),
                                    ParamBuilder.Par(":useragent", model.client_secret),
                                    ParamBuilder.Par(":PrincipalMember", model.PrincipalMember.ToUpper()),
                                    ParamBuilder.Par(":RELATIONSHIP_DESCRIPTION", model.RelationshipDescription),
                                    ParamBuilder.OutPar(":my_id_param", 0));
            return result;
        }

        public ForgotPassoword ForgotPassword(ResetPasswordModel model)
        {
            DBGenerics db = new DBGenerics();

            var query = "select CIVIL_ID, MEDICAL_INSURANCE_CARD , Password , mobile_no from member_users where Civil_id =:civilId and Is_Active =1 ";

            var result = db.ExecuteSingle<ForgotPassoword>(query, ParamBuilder.Par(":civilId", model.CivilId));
            return result;
        }

        public string LastFourDigits(string civilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT B.PHONE_NUMBER1
                              FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                              WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid 
                              and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                              order by A.EXPIRY_DATE DESC";
            return db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":nid", civilID));
        }

        public string IsValidUser(RegisterMemberModel model)
        {
            DBGenerics db = new DBGenerics();
            
            // string query = "select Id from medical_card_record where NATIONAL_IDENTITY = :nid and MEM_ID = :mid and MOBILE_NUMBER = :mob  order by ID desc";
            string result = "";
            string query = @"SELECT A.NATIONAL_IDENTITY
                             FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                             WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid
                             and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                             order by A.EXPIRY_DATE DESC";

            if (db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilId)) > 0)

            {
               // query = "select Id from medical_card_record where NATIONAL_IDENTITY = :nid and MEM_ID = :mid  order by ID desc";

                //if (db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilId), ParamBuilder.Par(":mid", model.MedicalInsuranceCardNumber)) > 0)

               // {
                    query = @"SELECT B.PHONE_NUMBER1
                              FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                              WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid and B.PHONE_NUMBER1 = :mob
                              and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                              order by A.EXPIRY_DATE DESC";
                //query = "select Id from medical_card_record where NATIONAL_IDENTITY = :nid and MEM_ID = :mid and MOBILE_NUMBER = :mob order by ID desc";
                var phnNum = db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilId), ParamBuilder.Par(":mob", model.PhoneNumber));
                    if (phnNum > 0)

                    {
                        return result = "success";

                    }

                    else
                    {
                        return result = "InvalidPhoneNo";

                    }
                //}

              //  else
                //{
               //     return result = "InvalidMemberId";

               // }
            }

            else
            {
                return result = "InvalidCivilId";

            }
        }



        public int UpdatePassword(string CivilId, string Password)
        {

            DBGenerics db = new DBGenerics();

            var query = "Update member_users set PASSWORD = :password,ISFIRSTPASWORDCHANGED = 0 where civil_id =:civilId RETURNING Id INTO :my_id_param";

            var result = db.ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":civilId", CivilId), ParamBuilder.Par(":password", Password), ParamBuilder.OutPar(":my_id_param", 0));
            return result;

        }

        public int AddSMS(SmsModel model)
        {
            var connection = new OracleConnection(DBHelper.ConnectionStringSMS);
            try
            {
                connection.Open();
                OracleCommand cmd = connection.CreateCommand();
                cmd.CommandText = @"INSERT INTO SMSUSER.GM_ONLINE_SMS@GICPROD (OS_MOBILE_NO, OS_TEXT, OS_CLAIM_NO, OS_DATE, OS_LANG_FLAG) VALUES (:OS_MOBILE_NO, :OS_TEXT, :OS_CLAIM_NO, sysdate, :OS_LANG_FLAG)";
                cmd.Parameters.Add(":OS_MOBILE_NO", model.OS_MOBILE_NO);
                cmd.Parameters.Add(":OS_TEXT", model.OS_TEXT);
                cmd.Parameters.Add(":OS_CLAIM_NO", model.OS_CLAIM_NO);
                cmd.Parameters.Add(":OS_LANG_FLAG", model.OS_LANG_FLAG);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { connection.Close(); }
        }

        public MemberNameModel GetMemberName(string civilId) {
            DBGenerics db = new DBGenerics();
            var Query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E, member_users
                  WHERE 1 = 1


                   AND E.POLICY_NUMBER = MEMBER_USERS.POLICY_NO
                AND E.MEMBER_NUMBER = MEMBER_USERS.MEMBER_ID
                and member_users.civil_id =:id
                    ),
     MEMBERS_HISTORY AS (SELECT I.*
                           FROM(SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER, J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER(PARTITION BY J.POLICY_NUMBER, J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1 = 1
                                --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND(J.POLICY_NUMBER, J.MEMBER_NUMBER) IN(SELECT G.POLICY_NUMBER, MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE = MAX_DATE
                    AND I.EVENT_NUMBER = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE, I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM(SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER(PARTITION BY B.POLICY_NUMBER, B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER(PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1 = 1
                            -- and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN(SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER = C.MAX_EVENT)                     
SELECT RPLNETWORK.NETWORK_DESCRIPTION,
MEMBERS.MEMBER_NUMBER,
         MEMBERS.FIRST_NAME || '  ' ||
       MEMBERS.MIDDLE_NAME || '  ' ||
       MEMBERS.LAST_NAME NAME,
        MEMBERS.INTERNATIONAL_FIRST_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_MIDDLE_NAME || '  ' ||
       MEMBERS.INTERNATIONAL_LAST_NAME INTERNATIONAL_NAME,
        (select Value
          from mednext.RPLMEMBERCUSTOMFIELD CUST
         where CUST.POLICY_NUMBER = MEMBERS.POLICY_NUMBER
           and CUST.MEMBER_NUMBER = MEMBERS.MEMBER_NUMBER
           and CUST.FIELD_ID = 'VIP'
          and CUST.VALUE = 'Y') VIP_FLAG,
       MEMBERS_HISTORY.MEMBER_EFFECTIVE_DATE,
       MEMBERS_HISTORY.EXPIRY_DATE MEMBEREXPIRY,
       MEMBERS_HISTORY.DELETION_DATE   ,
       MEMBERS.NATIONAL_IDENTITY,
       POLICIES.POLICY_HOLDER,
       POLICIES.POLICY_NUMBER,
       POLICIES.POLICY_EFFECTIVE_DATE,
       POLICIES.EXPIRY_DATE POLICYEXPIRY,
       POLICIES.CANCELLATION_DATE POLICY_CANCELLATION_DATE,
       RPLPACKAGE.INSURANCE_COMPANY_NUMBER,
       RPLPACKAGE.DEFAULT_NETWORK_ID,
       RPLMEMBERADDRESS.PHONE_NUMBER1,
       RPLMEMBERADDRESS.ADDRESS_TYPE_ID,
       MEMBERS.CLASS_ID,
       MEMBERS.RELATION_ID,
       MEMBERS.EFFECTIVE_DATE,
       MEMBERS.SEX_ID GENDER_ID,
       MEMBERS.DATE_OF_BIRTH, 
       MEMBERS.PRINCIPAL_FLAG PRINCIPALINDICATOR,
   RPLMEMBERADDRESS.ADDRESS,
       RPLMEMBERADDRESS.REGION_ID,
       RPLMEMBERADDRESS.DISTRICT_ID
  FROM MEMBERS,
       MEMBERS_HISTORY,
       POLICIES,
       mednext.RPLPACKAGE,
       mednext.RPLNETWORK,
       mednext.RPLMEMBERADDRESS

WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
  AND MEMBERS.POLICY_NUMBER = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS.MEMBER_NUMBER = RPLMEMBERADDRESS.MEMBER_NUMBER(+)
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER = RPLPACKAGE.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER = RPLPACKAGE.PACKAGE_NUMBER
  AND RPLNETWORK.NETWORK_ID = RPLPACKAGE.DEFAULT_NETWORK_ID";
            return ExecuteSingle<MemberNameModel>(Query, ParamBuilder.Par(":id", civilId));

        } 
         public ActivePolicyModel GetActivePolicyCivilId(string civilID)
        {
            DBGenerics db = new DBGenerics();
            var query = @"WITH MEMBERS AS (SELECT E.*
                   FROM mednext.RPLMEMBER E
                  WHERE 1=1
               AND (E.POLICY_NUMBER,E.MEMBER_NUMBER) IN (SELECT X.POLICY_NUMBER,X.MEMBER_NUMBER FROM  mednext.RPLMEMBER X WHERE X.NATIONAL_IDENTITY = :NantionalCivilId)
             
                    ),
                    MEMBERS_HISTORY AS (SELECT I.*
                           FROM (SELECT J.*,
                                        MAX(J.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER,J.EVENT_NUMBER) AS MAX_DATE,
                                        MAX(J.EVENT_NUMBER)                OVER (PARTITION BY J.POLICY_NUMBER,J.MEMBER_NUMBER) AS MAX_EVENT
                                   FROM mednext.RPLMEMBERMODIFICATIONHISTORY J
                                  WHERE 1=1 
                                 --and J.MODIFICATION_EFFECTIVE_DATE <= sysdate
                                    AND (J.POLICY_NUMBER,J.MEMBER_NUMBER) IN (SELECT G.POLICY_NUMBER,MEMBER_NUMBER FROM MEMBERS G)
                          ) I
                  WHERE I.MODIFICATION_EFFECTIVE_DATE       = MAX_DATE
                    AND I.EVENT_NUMBER                      = MAX_EVENT
                    --AND NVL(I.EXPIRY_DATE,I.DELETION_DATE) >= sysdate
),
     POLICIES AS (SELECT C.*
                    FROM (SELECT B.*,
                                 MAX(B.MODIFICATION_EFFECTIVE_DATE) OVER (PARTITION BY B.POLICY_NUMBER,B.EVENT_NUMBER) AS MAX_DATE,
                                 MAX(B.EVENT_NUMBER)                OVER (PARTITION BY B.POLICY_NUMBER)                AS MAX_EVENT
                            FROM mednext.RPLPOLICYMODIFICATIONHISTORY B
                           WHERE 1=1 
                                --and B.MODIFICATION_EFFECTIVE_DATE <= sysdate
                             AND B.POLICY_NUMBER IN (SELECT POLICY_NUMBER FROM MEMBERS)) C
                   WHERE C.MODIFICATION_EFFECTIVE_DATE = C.MAX_DATE
                     AND C.EVENT_NUMBER                = C.MAX_EVENT)  
SELECT POLICIES.POLICY_HOLDER,POLICIES.POLICY_EFFECTIVE_DATE,

       MEMBERS.EXPIRY_DATE,
       MEMBERS.DELETION_DATE,
       MEMBERS.MEMBER_NUMBER,
             MEMBERS.POLICY_NUMBER,
            MEMBERS.First_NAme || ' ' || MEMBERS.Middle_NAME || ' ' || MEMBERS.last_name as Name ,
             pkg.DEFAULT_NETWORK_ID as NETWORK_ID
       
  FROM MEMBERS,MEMBERS_HISTORY,  mednext.RPLPACKAGE pkg,
       POLICIES
WHERE MEMBERS.POLICY_NUMBER = POLICIES.POLICY_NUMBER
 AND MEMBERS.POLICY_NUMBER                     = MEMBERS_HISTORY.POLICY_NUMBER
  AND MEMBERS.MEMBER_NUMBER                     = MEMBERS_HISTORY.MEMBER_NUMBER
  AND MEMBERS_HISTORY.INSURANCE_COMPANY_NUMBER  = pkg.INSURANCE_COMPANY_NUMBER
  AND MEMBERS_HISTORY.PACKAGE_NUMBER            = pkg.PACKAGE_NUMBER
   and trunc(sysdate) between POLICIES.POLICY_EFFECTIVE_DATE and MEMBERS.EXPIRY_DATE
  order by POLICIES.POLICY_NUMBER asc";
            return db.ExecuteSingle<ActivePolicyModel>(query, ParamBuilder.Par(":NantionalCivilId", civilID));
        }
    }
}