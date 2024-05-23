using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    interface IComplaints
    {
        int AddNewComplaint(AddComplaintModel model);
        IList<Categories> GetAllCategories(CategoryListModel model);

        string GetMemberID(string civilId);

        IList<SMTPModel> GetSMTPSetting();

    }
}
