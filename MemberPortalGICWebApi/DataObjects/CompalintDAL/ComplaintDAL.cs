using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.CompalintDAL
{
    public class ComplaintDAL : DBGenerics, IComplaints
    {
        public string GetMemberID(string civilId)
        {
            DBGenerics db = new DBGenerics();
            var query = "select Member_ID from member_users  where civil_id = :civilId";
            var result = db.ExecuteScalarNvarchar(query, ParamBuilder.Par(":civilId", civilId));
            return result;
        }

        public IList<Categories> GetAllCategories(CategoryListModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = "";
            if (model.Culture == "ar")
            {
                query = "Select CATEGORYID, CATEGORYARABIC as CATEGORY, STATUS, SORTBY from Categories";
            }

            else
            {
                   query = "Select CATEGORYID, CATEGORYENGLISH as CATEGORY, STATUS, SORTBY from Categories";
            }
            return db.ExecuteList<Categories>(query);
        }
        public int AddNewComplaint(AddComplaintModel model)
        {
            DBGenerics db = new DBGenerics();
            var query = @"INSERT INTO COMPLAINT a ( A.SUBJECT, A.DESCRIPTION,A.USERID,A.CREATIONDATE,A.CategoryName,A.CIVILID) 
                            VALUES (:subject ,:description,:userId,:creationdate,:categoryname,:civilId)
                            RETURNING A.COMPLAINTID INTO :my_id_param";
            var result = ExecuteNonQueryOracle(query, ":my_id_param", ParamBuilder.Par(":subject", model.Subject), ParamBuilder.Par(":description", model.Description), ParamBuilder.Par(":userId", model.CivilId), ParamBuilder.Par(":CREATIONDATE", model.CreationDate), ParamBuilder.Par(":categoryname", model.CategoryName), ParamBuilder.Par(":civilId", model.CivilId), ParamBuilder.OutPar(":my_id_param", 0));
            return result;

        }

        public IList<SMTPModel> GetSMTPSetting()
        {
            DBGenerics db = new DBGenerics();
            //  var Query = @"select NOTIFICATION_ID , NOTIFICATION_DATE , NOTIFICATION_SUBJECT,  NOTIFICATION_ATTACHMENT from Gic_notification where notification_type = '4'  ORDER BY NOTIFICATION_DATE DESC";
            var query = "SELECT * FROM SMTP_SETTING";
            return db.ExecuteList<SMTPModel>(query);
        }
    }
}