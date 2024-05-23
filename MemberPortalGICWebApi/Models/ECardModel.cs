using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MemberPortalGICWebApi.Models
{
    public class ECardModel
    {
        public ECardLabels CardLabels { get; set; }
        public ECardValue CardValues { get; set; }

    }
    public class ECardLabels
    {
        public string Deductable = "Deductible";
        public string Waiting_p = "Waiting P.";
        public string Mem_no = "Mem. No";
        public string Exclusion = "Exclusion";
        public string Policyno = "Policy No";
        public string Co_pay = "Co Pay";
        public string ExpiryDate = "Expiry";
        public string Enrolment = "Eff. Date";
        public string Mem_name = "Mem. Name";
        public string Dob = "DOB";
        public string Cidno = "C.I.D No";
        public string Plan = "Plan Type";
        public string PolicyHolder = "Pol. Holder";
        public string Maternity = "Maternity";
        public string CardNo = "Card No";
    }
    public class ECardValue
    {
        public string PolicyNo { get; set; }
        public string ProductDescription { get; set; }
        public string CardToShow { get; set; }
        //public string Co_Pay { get; set; }
        private string _Co_Pay { get; set; }
        public string Co_Pay
        {
            get { return _Co_Pay ?? string.Empty; }
            set { _Co_Pay = value; }
        }
        public string ExpiryDate { get; set; }
        public string Enrollment { get; set; }
        public string ProductCode { get; set; }
        public string Mem_Name { get; set; }
        public string Dob { get; set; }
        public string CidNo { get; set; }
        public string Plan { get; set; }
        public string PolicyHolder { get; set; }
        public string Maternity { get; set; }
        private string _CardNo { get; set; }
        public string CardNo
        {
            get { return _CardNo ?? string.Empty; }
            set { _CardNo = value; }
        }
        private string _Chronic_Waiting_P { get; set; }
        public string Chronic_Waiting_P
        {
            get { return _Chronic_Waiting_P ?? string.Empty; }
            set { _Chronic_Waiting_P = value; }
        }
        private string _Wellness_Waiting_P { get; set; }
        public string Wellness_Waiting_P
        {
            get { return _Wellness_Waiting_P ?? string.Empty; }
            set { _Wellness_Waiting_P = value; }
        }
        private string _MaternityIn_Waiting_P { get; set; }
        public string MaternityIn_Waiting_P
        {
            get { return _MaternityIn_Waiting_P ?? string.Empty; }
            set { _MaternityIn_Waiting_P = value; }
        }
        private string _MaternityOut_Waiting_P { get; set; }
        public string MaternityOut_Waiting_P
        {
            get { return _MaternityOut_Waiting_P ?? string.Empty; }
            set { _MaternityOut_Waiting_P = value; }
        }
        private string _Dental_Waiting_P { get; set; }
        public string Dental_Waiting_P
        {
            get { return _Dental_Waiting_P ?? string.Empty; }
            set { _Dental_Waiting_P = value; }
        }
        private string _Deductable;
        public string Deductable
        {
            get { return _Deductable ?? string.Empty; }
            set { _Deductable = value; }
        }
        private string _Waiting_P;
        public string Waiting_P
        {
            get { return _Waiting_P ?? string.Empty; }
            set { _Waiting_P = value; }
        }
        public string Mem_No { get; set; }
        private string _Exclusion;
        public string Exclusion
        {
            get { return _Exclusion ?? string.Empty; }
            set { _Exclusion = value; }
        }
        private string _CoPayIn;
        public string CoPayIn
        {
            get { return _CoPayIn ?? string.Empty; }
            set { _CoPayIn = value; }
        }
        private string _CoPayOut;
        public string CoPayOut
        {
            get { return _CoPayOut ?? string.Empty; }
            set { _CoPayOut = value; }
        }
        private string _CoPayDT;
        public string CoPayDT
        {
            get { return _CoPayDT ?? string.Empty; }
            set { _CoPayDT = value; }
        }
        private string _MaternityLimit;
        public string MaternityLimit
        {
            get { return _MaternityLimit ?? string.Empty; }
            set { _MaternityLimit = value; }
        }
        private string _PackageNumber;
        public string PackageNumber
        {
            get { return _PackageNumber ?? string.Empty; }
            set { _PackageNumber = value; }
        }
        public bool ShowCoPayDental { get; set; }
    }
    public static class ECardTypes
    {
        public static readonly string Default = "DEFAULT";
        public static readonly string Fay = "FAY";
        public static readonly string FayPlus = "FAY PLUS";
        public static readonly string FayDual = "FAY DUAL";
        public static readonly string FayRegional = "FAY REGIONAL";
        public static readonly string FayAXA = "FAY AXA";
        public static readonly string FayInternational = "FAY-INTERNATIONAL";
        public static readonly string FayGlobal = "FAY-GLOBAL";
        public static readonly string FayKidsCare = "FAY KIDS CARE";
        public static readonly string Asfar = "ASFAR";
        public static readonly string PGH = "PGH";
        public static readonly string Heston = "HESTON";
        public static readonly string Equate = "EQUATE";
        public static readonly string Afya = "AFYA";
        public static readonly string KNPC = "KNPC";
        public static readonly string Reaya = "REAYA";
    }
}