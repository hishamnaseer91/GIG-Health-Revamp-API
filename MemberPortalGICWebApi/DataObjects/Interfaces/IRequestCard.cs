using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
    public interface IRequestCard
    {
        bool IsActiveMember(string civilID);

        string IsValidMemberbyPolicyNumber(CardModel model);
        int AddCardRequest(CardModel model);

        BindCardModel ExistingCardInfo(CardModel model);

        MemberInfo GetName(string civilID);

        BindCardModel ExistingCardInfoFromMEDICALCARDRECORD(CardModel model);

        int GetPrintedCardsCount(CardModel model);
        long gETMemberbNumber(CardModel model);
    }
}
