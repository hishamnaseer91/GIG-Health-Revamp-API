using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.RequestCard
{
    public class RequestCard : DBGenerics, IRequestCard
    {
        public bool IsActiveMember(string civilID)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT A.NATIONAL_IDENTITY
                             FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                             WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid
                             and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                             order by A.EXPIRY_DATE DESC";

            if (db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", civilID)) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

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

        public int AddCardRequest(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"INSERT INTO TBL_REQUEST_CARD 
                          (CIVIL_ID ,MOBILE_NUMBER, PREFERRED_TIME, REQUESTED_DATE, DEVICE_ID, REGION, AREA, BLOCK, STREET_NO, BUILDING_NO, FLOOR_NO, 
                          IS_CARD_DELIVERY_REQUEST, IS_CARD_REPRINT_REQUEST)                                     
                          VALUES (:CIVIL_ID ,:MOBILE_NUMBER,:PREFERRED_TIME,SYSDATE,:DEVICE_ID,:REGION,:AREA,:BLOCK,:STREET_NO,:BUILDING_NO,:FLOOR_NO,
                          :IS_CARD_DELIVERY_REQUEST, :IS_CARD_REPRINT_REQUEST)
                          RETURNING REQUEST_CARD_ID INTO :my_id_param";

            var result = ExecuteNonQueryOracle(query, ":my_id_param",
                                    ParamBuilder.Par(":CIVIL_ID", model.CivilID),
                                    ParamBuilder.Par(":MOBILE_NUMBER", model.PhoneNumber),
                                    ParamBuilder.Par(":PREFERRED_TIME", model.PreferredTime),
                                    ParamBuilder.Par(":DEVICE_ID", model.DeviceID),
                                    ParamBuilder.Par(":REGION", string.IsNullOrEmpty(model.Region) ? "" : model.Region),
                                    ParamBuilder.Par(":AREA", string.IsNullOrEmpty(model.Area) ? "" : model.Area),
                                    ParamBuilder.Par(":BLOCK", string.IsNullOrEmpty(model.Block) ? "" : model.Block),
                                    ParamBuilder.Par(":STREET_NO", string.IsNullOrEmpty(model.StreetNo) ? "" : model.StreetNo),
                                    ParamBuilder.Par(":BUILDING_NO", string.IsNullOrEmpty(model.BuildingNo) ? "" : model.BuildingNo),
                                    ParamBuilder.Par(":FLOOR_NO", string.IsNullOrEmpty(model.FloorNo) ? "" : model.FloorNo),
                                    ParamBuilder.Par(":IS_CARD_DELIVERY_REQUEST", model.isCardDileveryRequest),
                                    ParamBuilder.Par(":IS_CARD_REPRINT_REQUEST", model.isCardRePrintingRequest),
                                    ParamBuilder.OutPar(":my_id_param", 0));
            return result;
        }

        public string IsValidMemberbyPolicyNumber(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT A.NATIONAL_IDENTITY
                             FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                             WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid and A.POLICY_NUMBER = :pol
                             and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                             order by A.EXPIRY_DATE DESC";

            if (db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilID), ParamBuilder.Par(":pol", model.PolicyNumber)) > 0)
            {

                query = @"SELECT B.PHONE_NUMBER1
                              FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                              WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid and A.POLICY_NUMBER = :pol and B.PHONE_NUMBER1 = :mob
                              and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE and rownum = 1
                              order by A.EXPIRY_DATE DESC";

                if (db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilID),
                    ParamBuilder.Par(":pol", model.PolicyNumber), ParamBuilder.Par(":mob", model.PhoneNumber)) > 0)

                {
                    return  "Success";
                }

                else
                {
                    return "Invalid Phone Number";

                }
            }
            else
            {
                return "Invalid Civil ID";
            }
        }

        public BindCardModel ExistingCardInfo(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"SELECT C.REGION, C.AREA, C.BLOCK, C.STREET_NO, C.BUILDING_NO, C.FLOOR_NO
                          FROM TBL_REQUEST_CARD C
                          WHERE C.CIVIL_ID = :nid";
         
            return db.ExecuteSingle<BindCardModel>(query, ParamBuilder.Par(":nid", model.CivilID));
        }

        public long gETMemberbNumber(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            string query = @"SELECT A.MEMBER_NUMBER
                             FROM MEDNEXT.RPLMEMBERADDRESS B, MEDNEXT.RPLMEMBER A
                             WHERE B.MEMBER_NUMBER = A.MEMBER_NUMBER  and A.NATIONAL_IDENTITY = :nid and A.POLICY_NUMBER = :pol
                             and sysdate between A.EFFECTIVE_DATE AND A.EXPIRY_DATE 
                             order by A.EXPIRY_DATE DESC";

            return db.ExecuteScalarInt64(query, ParamBuilder.Par(":nid", model.CivilID), ParamBuilder.Par(":pol", model.PolicyNumber));
        }

        public BindCardModel ExistingCardInfoFromMEDICALCARDRECORD(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"SELECT C.GOVERNORATE AS REGION, C.AREA, C.BLOCK, C.STREET_NO, C.BUILDING_NO, C.FLOOR_NO
                              FROM MEDICAL_CARD_RECORD C
                              where C.NATIONAL_IDENTITY = :nid";

            return db.ExecuteSingle<BindCardModel>(query, ParamBuilder.Par(":nid", model.CivilID));
        }

        public int GetPrintedCardsCount(CardModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"select Count(*) as Count from MEDICAL_CARD_RECORD where POL_ID = :pol  and MEM_ID = :mol and IS_CARD_PRINTED = 1";

            return db.ExecuteScalarInt32(query, ParamBuilder.Par(":pol", model.PolicyNumber), ParamBuilder.Par(":mol", model.MemberNumber));
        }
    }
}