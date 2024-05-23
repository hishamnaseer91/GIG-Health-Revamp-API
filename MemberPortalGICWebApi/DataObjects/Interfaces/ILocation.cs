using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface ILocation
    {
        IList<LocationInfo> getLatLongFromDB(string Culture);

    }
}
