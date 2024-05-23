using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi
{
    public class OracleParameterbuilder : IParamBuilder
    {

        public DbParameter Par(string pname, object pval)
        {
            return new OracleParameter(pname, pval);
        }
        public DbParameter OutPar(string pname, object val)
        {

            return new OracleParameter(pname, val) { Direction = ParameterDirection.Output };
        }

        public DbParameter ParNVarChar(string pname, string pval, int size)
        {

            OracleParameter p = new OracleParameter(pname, OracleType.NVarChar, size);
            p.Value = pval;
            return p;
        }
        public DbParameter ParNVarCharMax(string pname, string pval)
        {
            OracleParameter p = new OracleParameter(pname, OracleType.NVarChar, -1);
            p.Value = pval;
            return p;
        }



    }


}