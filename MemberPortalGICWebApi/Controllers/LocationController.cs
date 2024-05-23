using MemberPortalGICWebApi.DataObjects;
using MemberPortalGICWebApi.DataObjects.Interfaces;
using MemberPortalGICWebApi.DataObjects.Location;
using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace MemberPortalGICWebApi.Controllers
{
    [RoutePrefix("api/Location")]
    public class LocationController : ApiController
    {
        ILocation obj = new Location();

        [HttpPost]
        [Route("NearestLocation")]
        public IHttpActionResult getDistance(LocationFinderAttributes inputmodel)
        {
            if (inputmodel == null)
            {
                inputmodel = new LocationFinderAttributes();
                inputmodel.Culture = "en";
            }

            var LatLongFromDB = obj.getLatLongFromDB(inputmodel.Culture);

            List<LocationInfo> list = new List<LocationInfo>();

            if (LatLongFromDB != null)
            {
                var KMEngAr = inputmodel.Culture.ToLower() == "en" ? " KM" : " كم ";

                if (string.IsNullOrEmpty(inputmodel.Lat) && string.IsNullOrEmpty(inputmodel.Long) &&
                            string.IsNullOrEmpty(inputmodel.RegionID) && string.IsNullOrEmpty(inputmodel.AreaID))
                {
                    var getLatLongByAreaRegionFromDB = new List<LocationInfo>();
                    if (!string.IsNullOrEmpty(inputmodel.LocationTypeID))
                    {
                        getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.LocationTypeID == inputmodel.LocationTypeID).ToList();
                    }
                    else
                    {
                        getLatLongByAreaRegionFromDB = LatLongFromDB.ToList();
                    }

                    foreach (var loc in getLatLongByAreaRegionFromDB)
                    {
                        LocationInfo obj = new LocationInfo();
                        obj.LocationName = loc.LocationName;
                        obj.Latitude = loc.Latitude;
                        obj.Longitude = loc.Longitude;
                        obj.CalculatedDuration = "";
                        obj.CalulatedDistance = "";
                        obj.RegionName = loc.RegionName;
                        obj.AreaName = loc.AreaName;
                        obj.LocationTypeName = loc.LocationTypeName;
                        obj.LocationContactNo = loc.LocationContactNo;
                        list.Add(obj);
                    }
                    APIResponceCodes code = new APIResponceCodes()
                    {
                        Code = "CD-05",
                        Type = "Get/Model",
                        Data = list,
                        Description = "Record Found",

                    };
                    return Ok(code);
                }
                else
                {
                    if (!string.IsNullOrEmpty(inputmodel.Lat) && !string.IsNullOrEmpty(inputmodel.Long) &&
                            !string.IsNullOrEmpty(inputmodel.RegionID) && !string.IsNullOrEmpty(inputmodel.AreaID))
                    {
                        var getLatLongByAreaRegionFromDB = new List<LocationInfo>();
                        if (!string.IsNullOrEmpty(inputmodel.LocationTypeID))
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.RegionID == inputmodel.RegionID &&
                                  x.AreaID == inputmodel.AreaID && x.LocationTypeID == inputmodel.LocationTypeID).ToList();
                        }
                        else
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.RegionID == inputmodel.RegionID &&
                                 x.AreaID == inputmodel.AreaID).ToList();
                        }


                        inputmodel.list = getLatLongByAreaRegionFromDB;


                        if (inputmodel.list.Count() > 0)
                        {
                            foreach (var loc in inputmodel.list)
                            {
                                if (!string.IsNullOrEmpty(inputmodel.Lat) && !string.IsNullOrEmpty(inputmodel.Long) &&
                                !string.IsNullOrEmpty(loc.Latitude) && !string.IsNullOrEmpty(loc.Longitude))
                                {
                                    var sCoord = new GeoCoordinate(Convert.ToDouble(inputmodel.Lat), Convert.ToDouble(inputmodel.Long));
                                    var eCoord = new GeoCoordinate(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
                                    var distance = Math.Round(sCoord.GetDistanceTo(eCoord) / 1000, 2);
                                    loc.Distance = distance;
                                    if (inputmodel.Culture.ToLower() == "en")
                                    {
                                        loc.CalulatedDistance = distance.ToString() + KMEngAr;
                                    }
                                    else
                                    {
                                        loc.CalulatedDistance = distance.ToString() + " " + "كم";
                                    }
                                }
                                else
                                {
                                    loc.CalulatedDistance = "0";
                                }
                            }
                            var SortedListByKM = inputmodel.list.OrderBy(x => x.Distance).ToList();
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-05",
                                Type = "Get/Model",
                                Data = SortedListByKM,
                                Description = "Nearest Locations",

                            };
                            return Ok(code);
                        }
                        else
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "Get/Model",
                                Description = "No Record Found",
                                DescriptionArabic = "لم يتم العثور على سجل"

                            };
                            return Ok(code);
                        }

                    }
                    else if (!string.IsNullOrEmpty(inputmodel.RegionID) && !string.IsNullOrEmpty(inputmodel.AreaID) &&
                        string.IsNullOrEmpty(inputmodel.Lat) && string.IsNullOrEmpty(inputmodel.Long))
                    {
                        var getLatLongByAreaRegionFromDB = new List<LocationInfo>();
                        if (!string.IsNullOrEmpty(inputmodel.LocationTypeID))
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.RegionID == inputmodel.RegionID &&
                                  x.AreaID == inputmodel.AreaID && x.LocationTypeID == inputmodel.LocationTypeID).ToList();
                        }
                        else
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.RegionID == inputmodel.RegionID &&
                                 x.AreaID == inputmodel.AreaID).ToList();
                        }

                        if (getLatLongByAreaRegionFromDB.Count() > 0)
                        {
                            foreach (var loc in getLatLongByAreaRegionFromDB)
                            {
                                LocationInfo obj = new LocationInfo();
                                obj.LocationName = loc.LocationName;
                                obj.Latitude = loc.Latitude;
                                obj.Longitude = loc.Longitude;
                                obj.CalculatedDuration = "";
                                obj.CalulatedDistance = "";
                                obj.RegionName = loc.RegionName;
                                obj.AreaName = loc.AreaName;
                                obj.LocationTypeName = loc.LocationTypeName;
                                obj.LocationContactNo = loc.LocationContactNo;
                                list.Add(obj);
                            }
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-05",
                                Type = "Get/Model",
                                Data = list,
                                Description = "Record Found",


                            };
                            return Ok(code);
                        }
                        else
                        {
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "Get/Model",
                                Description = "No Record Found",
                                DescriptionArabic = "لم يتم العثور على سجل"
                            };
                            return Ok(code);
                        }
                    }
                    //CHECK
                    else if (string.IsNullOrEmpty(inputmodel.RegionID) && string.IsNullOrEmpty(inputmodel.AreaID)
                        && !string.IsNullOrEmpty(inputmodel.Lat) && !string.IsNullOrEmpty(inputmodel.Long))
                    {
                        var getLatLongByAreaRegionFromDB = new List<LocationInfo>();
                        if (!string.IsNullOrEmpty(inputmodel.LocationTypeID))
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.Where(x => x.LocationTypeID == inputmodel.LocationTypeID).ToList();
                        }
                        else
                        {
                            getLatLongByAreaRegionFromDB = LatLongFromDB.ToList();
                        }

                        inputmodel.list = getLatLongByAreaRegionFromDB;

                        if (inputmodel.list.Count() > 0)
                        {

                            foreach (var loc in inputmodel.list)
                            {

                                if (!string.IsNullOrEmpty(inputmodel.Lat) && !string.IsNullOrEmpty(inputmodel.Long) &&
                                    !string.IsNullOrEmpty(loc.Latitude) && !string.IsNullOrEmpty(loc.Longitude))
                                {
                                    var sCoord = new GeoCoordinate(Convert.ToDouble(inputmodel.Lat), Convert.ToDouble(inputmodel.Long));
                                    var eCoord = new GeoCoordinate(Convert.ToDouble(loc.Latitude), Convert.ToDouble(loc.Longitude));
                                    var distance = Math.Round(sCoord.GetDistanceTo(eCoord) / 1000, 2);
                                    loc.Distance = distance;
                                    if (inputmodel.Culture.ToLower() == "en")
                                    {
                                        loc.CalulatedDistance = distance.ToString() + KMEngAr;
                                    }
                                    else
                                    {
                                        loc.CalulatedDistance = distance.ToString() + " " + "كم";
                                    }
                                }
                                else
                                {
                                    loc.CalulatedDistance = "0";
                                }

                            }
                            var SortedListByKM = inputmodel.list.OrderBy(x => x.Distance).ToList();
                            APIResponceCodes code = new APIResponceCodes()
                            {
                                Code = "CD-05",
                                Type = "Get/Model",
                                Data = SortedListByKM,
                                Description = "Nearest Locations",

                            };
                            return Ok(code);
                        }
                        else
                        {
                            APIResponceCodes codes = new APIResponceCodes()
                            {
                                Code = "CD-06",
                                Type = "Get/Model",
                                Description = "No Record Found",
                                DescriptionArabic = "لم يتم العثور على سجل"
                            };
                            return Ok(codes);
                        }
                    }
                    else
                    {
                        APIResponceCodes code = new APIResponceCodes()
                        {
                            Code = "CD-06",
                            Type = "Get/Model",
                            Description = "No Record Found",
                            DescriptionArabic = "لم يتم العثور على سجل"
                        };
                        return Ok(code);
                    }
                }
            }

            else
            {
                APIResponceCodes code = new APIResponceCodes()
                {
                    Code = "CD-06",
                    Type = "Get/Model",
                    Description = "No Record Found",
                    DescriptionArabic = "لم يتم العثور على سجل"
                };
                return Ok(code);
            }
        }

        public LocationInfo GetDrivingDistanceInMiles(string origin, string destination)
        {
            LocationInfo obj = new LocationInfo();
            string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&mode=driving&sensor=false&language=en-EN&units=imperial&key=AIzaSyBbGPFaV-_UmZXhF5LEMfI26JfH3_8LPy8";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);
            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                if (xmldoc.GetElementsByTagName("status")[1].ChildNodes[0].InnerText != "NOT_FOUND")
                {
                    XmlNodeList duration = xmldoc.GetElementsByTagName("duration");
                    XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                    obj.Distance = Convert.ToDouble(distance[0].ChildNodes[1].InnerText.Replace(" mi", ""));
                    obj.CalculatedDuration = duration[0].ChildNodes[1].InnerText;
                    return obj;
                }


            }
            obj.Distance = 0;
            return obj;
        }

        /// <summary> 
        /// Get Location based on Latitude and Longitude.
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        public string GetLocation(double latitude, double longitude)
        {
            string url = "https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=false&key=AIzaSyBbGPFaV-_UmZXhF5LEMfI26JfH3_8LPy8";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader sreader = new StreamReader(dataStream);
            string responsereader = sreader.ReadToEnd();
            response.Close();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(responsereader);
            if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
            {
                XmlNodeList location = xmldoc.GetElementsByTagName("distance");
                return xmldoc.GetElementsByTagName("formatted_address")[0].ChildNodes[0].InnerText;
            }

            return "";
        }

        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }

            return dist;
        }
    }
}
