using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class LocationInfo : IDataMapper
    {

        public string GoogleLocationName { get; set; }
        public string LocationName { get; set; }
        public string LocationAddress { get; set; }
        public string LocationTypeID { get; set; }
        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public double Distance { get; set; }

        public string CalulatedDistance {get; set;}

        public string RegionID { get; set; }

        public string AreaID { get; set; }

        public string RegionName { get; set; }

        public string AreaName { get; set; }

        public string LocationTypeName { get; set; } 

        public string LocationContactNo { get; set; }

        public double Duration { get; set; }

        public string CalculatedDuration { get; set; }
        public void MapProperties(DbDataReader dr)
        {
            LocationName = dr.GetString("LocationName");
            LocationAddress = dr.GetString("LOCATION_ADRESS");
            Latitude = dr.GetString("Latitude");
            Longitude = dr.GetString("Longitude");
            RegionID = dr.GetInt32("LOCATION_REGION").ToString();
            AreaID = dr.GetInt32("LOCATION_AREA").ToString();
            LocationTypeID = dr.GetInt32("LOCATION_TYPE").ToString();
            RegionName = dr.GetString("REGION_DESCRIPTION");
            AreaName = dr.GetString("FOREIGN_DESCRIPTION");
            LocationTypeName = dr.GetString("TYPE_NAME_EN");
            LocationContactNo = dr.GetString("LOCATION_CONTACT");
        }
    }
}