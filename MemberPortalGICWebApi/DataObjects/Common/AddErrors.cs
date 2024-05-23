using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class AddErrors : DBGenerics
    {
        public int InsertErrorLog(Logs Log)
        {
            DBGenerics db = new DBGenerics();
            Log.ErrorExp = Log.ErrorExp.Replace("'", "''");
            Log.ErrorCode = Log.ErrorCode.Replace("'", "''");
            Log.ErorDesc = Log.ErorDesc.Replace("'", "''");
            string Query = @"INSERT INTO ERRORLOGS ( ECODE, EDESC,  EDT, EXPEC,F1) VALUES (:ECODESds ,:EDESCVsd ,sysdate ,:ESCPECTs, 'E'   )";
            return db.ExecuteScalarInt32(Query, ParamBuilder.Par(":ECODESds", Log.ErrorCode), ParamBuilder.Par(":EDESCVsd", Log.ErorDesc), ParamBuilder.Par(":ESCPECTs", Log.ErrorExp));

        }
        public int InsertSuccessLog(Logs Log)
        {
            DBGenerics db = new DBGenerics();
            Log.ErrorExp = Log.ErrorExp.Replace("'", "''");
            Log.ErrorCode = Log.ErrorCode.Replace("'", "''");
            Log.ErorDesc = Log.ErorDesc.Replace("'", "''");
            string Query = @"INSERT INTO ERRORLOGS ( ECODE, EDESC,  EDT, EXPEC,F1) VALUES (:ECODESds ,:EDESCVsd ,sysdate ,:ESCPECTs, 'S'   )";
            return db.ExecuteScalarInt32(Query, ParamBuilder.Par(":ECODESds", Log.ErrorCode), ParamBuilder.Par(":EDESCVsd", Log.ErorDesc), ParamBuilder.Par(":ESCPECTs", Log.ErrorExp));

        }


    }
}