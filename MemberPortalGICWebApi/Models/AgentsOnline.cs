using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class AgentsOnline : IDataMapper
    {
        public string Aname { get; set; }
        public string AnameV { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            Aname = dr.GetString("PROVIDER_NAME");
            AnameV = dr.GetString("PROVIDER_ID");
        }
    }
}