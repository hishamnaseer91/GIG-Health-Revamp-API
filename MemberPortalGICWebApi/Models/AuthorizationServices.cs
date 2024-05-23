using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class AuthorizationServices : IDataMapper
    {
        [ScaffoldColumn(false)]
        public long REQUEST_DETAIL_ID { get; set; }


        [ScaffoldColumn(false)]
        public string AUTHORIZATION_NO { get; set; }
        [Required(ErrorMessage = "Select the Service Code")]
        public string HospitalServiceCode { get; set; }

        [Required(ErrorMessage = "Select the Service Code And Descption Will Populate Auto")]
        public string HSCDescription { get; set; }

        public string CodeFullSERVICE { get; set; }
        public string CLM_NO { get; set; }

        public bool ARF { get; set; }//Approval Required Flag

        public DateTime Service_Date { get; set; }
        [Required(ErrorMessage = "Enter the Quantity or Days")]
        [Range(typeof(decimal), "0.01", "100000.00", ErrorMessage = "enter decimal value")]
        //[DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal Days_Qty { get; set; }
        [Required(ErrorMessage = "Enter the Price")]
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        [Range(typeof(decimal), "0.01", "100000.00", ErrorMessage = "enter decimal value")]
        public decimal UnitPrice { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]

        public decimal ClaimAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal InsuredAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal CoPaymet { get; set; }
        [DisplayFormat(DataFormatString = "{0:#,##0.000#}")]
        public decimal ExeceedingLimit { get; set; }
        public string Notes { get; set; }
        public string PROCEDURE_ID { get; set; }
        public decimal MdPROCEDURE_Number { get; set; }
        public string MdPR_STATUS { get; set; }
        public string flag { get; set; }
        [ScaffoldColumn(false)]
        public string PROCDURE_TYPE_ID { get; set; }

        public string ISLOV { get; set; }

        [ScaffoldColumn(false)]
        public string CREATED_BY { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Pick a Service date")]

        public DateTime CREATED_DATE { get; set; }

        [ScaffoldColumn(false)]
        public int ISACTIVE { get; set; }

        [ScaffoldColumn(false)]
        public int STATUS { get; set; }

        [ScaffoldColumn(false)]
        public string NEWIDREQUEST { get; set; }
        public string ATYPE { get; set; }
        //[Required(ErrorMessage = "Select the Teeth")]
        public string TEETH { get; set; }
        public string Rejection_Desc { get; set; }
        public int? RejectionId { get; set; }
        public string REPORT_NOTES { get; set; }


        public void MapProperties(DbDataReader dr)
        {
            NEWIDREQUEST = dr.GetString("REQUESTID");
            AUTHORIZATION_NO = dr.GetString("AUTHORIZATION_NO");
            HospitalServiceCode = dr.GetString("SERVICE_CODE");
            Service_Date = dr.GetDateTime("CREATED_DATE");
            HSCDescription = dr.GetString("SERVICE_DESCRIPTION");
            CodeFullSERVICE = dr.GetString("SERVICE_DESCRIPTION");
            ISLOV = dr.GetString("ISLOV");
            Notes = dr.GetString("NOTES");
            ARF = dr.GetBooleanExtra("STATUS");
            InsuredAmount = dr.GetDecimal("SYSTEM_ESTIMATED_COST_AMOUNT");
            ClaimAmount = dr.GetDecimal("CLAIM_AMOUNT");
            Days_Qty = dr.GetDecimal("DAYS");
            CREATED_BY = dr.GetString("CREATED_BY");
            CREATED_DATE = dr.GetDateTime("CREATED_DATE");
            REQUEST_DETAIL_ID = dr.GetInt64("REQUEST_DETAIL_ID");
            PROCDURE_TYPE_ID = dr.GetString("PROCDURE_TYPE_ID");
            UnitPrice = dr.GetDecimal("UNIT_PRICE");
            PROCEDURE_ID = dr.GetString("PROCEDURE_ID");
            MdPR_STATUS = dr.GetString("MdPR_STATUS");
            MdPROCEDURE_Number = dr.GetDecimal("MdPROCEDURE_Number");
            CoPaymet = dr.GetDecimal("COPAYMENT");
            ExeceedingLimit = dr.GetDecimal("EXECEDLIMIT");
            ATYPE = dr.GetString("ATYPE");
            TEETH = dr.GetString("TEETH");
            Rejection_Desc = dr.GetString("REJECTION_REASON");
            RejectionId = dr.GetInt32Nullable("REJECTION_REASON_ID");
            REPORT_NOTES = dr.GetString("NOTES_REPORT");

        }
    }
}