using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.DataObjects
{
    public class DBHelper
    {
        public static String ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Default"].ToString(); }
        }
        public static String ConnectionStringSMS
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionStringSMS"].ToString(); }
        }
    }
}