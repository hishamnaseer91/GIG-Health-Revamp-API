using MemberPortalGICWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemberPortalGICWebApi.DataObjects.Interfaces
{
   public interface ISearch
    {

        IList<DDLProviderType> DALProviderTypeDDL(string Culture);

        IList<DDLPlans> DALPlanListDDL(string culture);

        IList<DDLLocationTypes> DALLocationTypeDDL(string culture);

        IList<DDLNetwork> DALNetworkListDDL(string culture, int planid);
        IList<DDLNetworkByCivilId> DALNetworkListDDLbyCivilID(string culture, string civilID);

        IList<DDLNetworkByCivilId> DALNetworkListDDLbyCivilIDInToken(string culture, string civilID);

        IList<DDLProviders> DALProviderDDL(string Culture, string ProviderType);
        IList<DDLSpecialty> DALSpecialtyDDL(string Culture);

        IList<DDLRegion> DALRegionDDL(string Culture);
        IList<DDLRegionArea> DALAreaDDL(string Culture, int Region);
        long GetNetworkId(string civilId);

        string GetCoveregeName(string networkID);

        int isDiscountOptionApplicable();

        GlobalConfigMapping getAfyaActivePolicyNumber();

        string getCardPrintDate(string CivilID);

        string getAfyaOfferExpiry();

        AddsMapping getActiveAddsDetils(int policyNo);

        AppVersion getAppVersionDetils();

        DiscountCouponMapping getDiscountCouponDetils(string CivilID);

        IList<SearchResult> DALSearchResult(SearchModel Model, string networkId);

        IList<BenefitsMappingList> GetBenefitsMappingList();

        IList<DDLNetworkCardMapping> BIZNetworkListDDLCardPickup(string culture, int planid);

        string ClaimRegisterSmsVisibility(string CivilID);

        string ClaimRegisterSmsVisibility(string CivilID, string PolicyNo);

        string ClaimRegisterEnable(string CivilID);

        int UpdateExcludeMemeber(string CivilID, bool isSmsApplicable);
    }
}
