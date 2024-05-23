using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects.DBView
{
    public class DbViewDAL: DBGenerics
    {
        public IList<DbModel> GetAllusers()
        {
            DBGenerics db = new DBGenerics();
            var query = @"select * from member_users";
            return db.ExecuteList<DbModel>(query);
        }


    }
}