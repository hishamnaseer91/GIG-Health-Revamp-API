using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class Categories : IDataMapper
    {
        public int CategoryId { get; set; }
        //public string CategoryEnglish { get; set; }
        //public string CategoryArabic { get; set; }

        public string Category { get; set; }
        public bool Status { get; set; }
        public string SortBy { get; set; }
        public string FieldExtra
        {
            get; set;
        }

        public void MapProperties(DbDataReader dr)
        {
            CategoryId = dr.GetInt32("CATEGORYID");
            //CategoryEnglish = dr.GetString("CATEGORYENGLISH");
            //CategoryArabic = dr.GetString("CATEGORYARABIC");
            Category = dr.GetString("CATEGORY");
            Status = dr.GetBooleanExtra("STATUS");
            SortBy = dr.GetString("SORTBY");
          
        }
    }
}