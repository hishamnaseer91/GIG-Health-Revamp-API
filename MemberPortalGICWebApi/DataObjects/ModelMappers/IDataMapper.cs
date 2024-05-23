using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi
{
    public interface IDataMapper
    {
        void MapProperties(DbDataReader dr);
    }
}
