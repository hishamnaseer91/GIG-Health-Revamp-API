using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class SMTPModel : IDataMapper
    {

        public string SMTPID { get; set; }

        public string SMTPHost { get; set; }

        public int SMTPPort { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string EmailForm { get; set; }

        public string Subject { get; set; }

        public int ModifiedBy { get; set; }

        public DateTime ModifiedDate { get; set; }


        public void MapProperties(DbDataReader dr)
        {
            SMTPHost = dr.GetString("SMTP_HOST");
            SMTPPort = dr.GetInt32("SMTP_PORT");
            UserName = dr.GetString("USERNAME");
            Password = dr.GetString("PASSWORD");
            EmailForm = dr.GetString("EMAIL_FROM");


        }
    }
}