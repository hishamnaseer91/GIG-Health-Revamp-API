using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class Notifications : IDataMapper
    {
       
            public int NotificationId { get; set; }

            public DateTime NotificationDate { get; set; }
           
            public string NotificationSubject { get; set; }

          //  public string NotificationAttachment { get; set; }

            public void MapProperties(DbDataReader dr)
            {
                NotificationId = dr.GetInt32("NOTIFICATION_ID");
                NotificationDate = dr.GetDateTime("NOTIFICATION_DATE");
                NotificationSubject = dr.GetString("NOTIFICATION_SUBJECT");
               // NotificationAttachment = dr.GetString("NOTIFICATION_ATTACHMENT");
            }
        }
    }
