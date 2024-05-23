using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects
{
    public interface ILoginUser
    {
        int CheckUserExistence(UserModel Model);

        int UserIsActiveOrNot(long id);

        MappingUserModel BIZUserModel(string id);

        ActivePolicyModels GetActivePolicyCivilId(string civilID);

        int UpdatePolicyNumber(ActivePolicyModels model);
    }
}
