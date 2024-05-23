using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi
{
    public interface IParamBuilder
    {
        DbParameter Par(string pname, object pval);
        DbParameter OutPar(string pname, object val);
        DbParameter ParNVarChar(string pname, string pval, int size);
        DbParameter ParNVarCharMax(string pname, string pval);
   
    }
}
