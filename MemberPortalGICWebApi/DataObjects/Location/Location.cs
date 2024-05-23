using MemberPortalGICWebApi.DataObjects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MemberPortalGICWebApi.Models;

namespace MemberPortalGICWebApi.DataObjects.Location
{
    public class Location : DBGenerics, ILocation
    {
        public IList<LocationInfo> getLatLongFromDB(string Culture)
        {
            DBGenerics Db = new DBGenerics();
            if (!String.IsNullOrEmpty(Culture) && (Culture.ToLower() == "ar"))
            {

                string query = @"SELECT B.REGION_DESCRIPTION_AR as REGION_DESCRIPTION, C.DISTRICT_DESCRIPTION_AR AS FOREIGN_DESCRIPTION, D.TYPE_NAME_AR as TYPE_NAME_EN, A.LOCATION_CONTACT,  A.LOCATION_DESC_AR AS LocationName, A.LOCATION_ADRESS,A.LOCATION_LAT as Latitude , 
                             A.LOCATION_LON AS Longitude , A.LOCATION_REGION, A.LOCATION_AREA, LOCATION_TYPE 
                             FROM DISTRIBUTED_LOCATIONS A, VWMNI_REGION B , VWMNI_DISTRICT C, location_types D
                             WHERE A.LOCATION_REGION = B.REGION_ID
                             AND A.LOCATION_AREA = C.DISTRICT_ID
                             AND A.LOCATION_TYPE = D.ID
                             AND A.LOCATION_STATUS = 1";


                return Db.ExecuteList<LocationInfo>(query);
            }
            else
            {

                string query = @"SELECT B.REGION_DESCRIPTION, C.FOREIGN_DESCRIPTION, D.TYPE_NAME_EN, A.LOCATION_CONTACT,  A.LOCATION_DESC_EN AS LocationName , A.LOCATION_ADRESS  ,A.LOCATION_LAT as Latitude , 
                             A.LOCATION_LON AS Longitude , A.LOCATION_REGION, A.LOCATION_AREA, LOCATION_TYPE 
                             FROM DISTRIBUTED_LOCATIONS A, mednext.RPLREGION B , mednext.DISTRICT C, location_types D
                             WHERE A.LOCATION_REGION = B.REGION_ID
                             AND A.LOCATION_AREA = C.DISTRICT_ID
                             AND A.LOCATION_TYPE = D.ID
                             AND A.LOCATION_STATUS = 1";
                return Db.ExecuteList<LocationInfo>(query);
            }
        }
    }
}