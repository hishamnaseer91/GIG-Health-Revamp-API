using Dapper;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class GeneralDAL
    {
        private readonly string _connectionString;
        public GeneralDAL()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ToString(); ;
        }

        public GeneralConfigration GetConfigration()
        {
            GeneralConfigration _objList = new GeneralConfigration();
            try
            {
                //log.Error("DB Error UnderWrittingDepartmentDAL >>GetGlobeMedStagingEndorsementData model> ");
                using (var connection = new OracleConnection(_connectionString))
                {
                    //3 -> Sync In Progress
                    var query = "Select* from(Select TO_CHAR(B.CREATED_AT, 'DD-MM-YYYY HH:MI:SS AM') LAST_UPDATED, B.* from GLOBAL_CONFIG B order by id desc) where rownum = 1  ";


                    DynamicParameters dbParams = new DynamicParameters();

                    _objList = connection.Query<GeneralConfigration>(query, commandType: CommandType.Text, param: dbParams).FirstOrDefault();
                }
            }

            catch (Exception ex)
            {

                // log.Error("DB Error UnderwrittingDepartmentDAL >> GetInProgressEndorsement model> " + ex);
            }
            return _objList;

        }

    }
}