using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class EMPLOYEE_IMPORT_EXCEL
    {
        public string UNIT { get; set; }
        public string LOCATION { get; set; }
        public string EMPCODE { get; set; }
        public string TITLE { get; set; }
        public string FNAME { get; set; }
        public string MNAME { get; set; }
        public string LNAME { get; set; }
        public string PFNO { get; set; }
        public string ESINO { get; set; }
        public string PANCARDNO { get; set; }
        public string PASSPORTNO { get; set; }
        public DateTime? PASSPORTISSUEDON { get; set; }
        public DateTime? P_RENEWALDATE { get; set; }
        public DateTime? DATEOFBIRTH { get; set; }
        public DateTime? DATEOFJOINING { get; set; }
        public DateTime? DATEOFLEAVING { get; set; }
        public string LEFT_SERVICE { get; set; }
        public DateTime? TRANSFERRED { get; set; }
        public string TRANSFERREDTO { get; set; }
        public string BLOODGROUP { get; set; }
        public string PERM_HOUSENO { get; set; }
        public string PERM_STREETNAME1 { get; set; }
        public string PERM_STREETNAME2 { get; set; }
        public string PERM_CITY { get; set; }
        public int? PERM_PINCODE { get; set; }
        public string PERM_POSTOFF { get; set; }
        public string PERM_POLICEST { get; set; }
        public int? PERM_RESTEL1 { get; set; }
        public int? PERM_RESTEL2 { get; set; }
        public int? PERM_MOBILENO { get; set; }
        public string TEMP_HOUSENO { get; set; }
        public string TEMP_STREETNAME1 { get; set; }
        public string TEMP_STREETNAME2 { get; set; }
        public string TEMP_CITY { get; set; }
        public int? TEMP_PINCODE { get; set; }
        public string TEMP_POSTOFF { get; set; }
        public string TEMP_POLICEST { get; set; }
        public int? TEMP_RESTEL1 { get; set; }
        public int? TEMP_RESTEL2 { get; set; }
        public int? TEMP_MOBILENO { get; set; }
        public string EMAIL_ID { get; set; }
        public string SHORT_NAME { get; set; }
        public string PASPRT_ISD_FRM { get; set; }
        public string CHQ_CSH { get; set; }
        public string IFSC_CODE { get; set; }
        public string SWIFT_CODE { get; set; }
        public string IBAN_NO { get; set; }
        public string AC_NO { get; set; }
        public string BRANCH { get; set; }
        public string BANK_NAME { get; set; }
        public string SEX { get; set; }
        public string LNAME1 { get; set; }
        public string MNAME1 { get; set; }
        public string FNAME1 { get; set; }
        public string TITLE1 { get; set; }
        public string COUNTRY { get; set; }
        public string COUNTRY2 { get; set; }
        public string GENERAL_SHIFT { get; set; }
        public string STAFF_WORKER { get; set; }
        public string TNNO { get; set; }
        public string PFD { get; set; }
        public string SBI_LOAN_AC { get; set; }
        public string RGB_LOAN_AC { get; set; }
        public string SBI_EDU_LOAN_AC { get; set; }
        public string PENSION_NO { get; set; }
        public string ACODE { get; set; }
        public string REF_CODE { get; set; }
        public string COST_CENTRE { get; set; }
        public string SACODE { get; set; }
        public string SSUBCODE { get; set; }
        public string INCENTIVE { get; set; }
        public DateTime? DOJ_EPS { get; set; }
        public DateTime? DOJ_EPF { get; set; }
        public DateTime? DOL_EPS { get; set; }
        public DateTime? DOL_EPF { get; set; }
        public DateTime? DOG { get; set; }
        public string TRUST_PF { get; set; }
        public string RELATIONSHIP { get; set; }
        public string REASON_LV { get; set; }
        public bool? BONUS { get; set; }
        public string UAN { get; set; }
        public string STATE { get; set; }
        public string STATE_CODE { get; set; }
        public string CATE_CODE { get; set; }
        public string CATE_CODE1 { get; set; }
        public string LEG_CODE { get; set; }
        public string ACTIVE_TAG { get; set; }
        public string DEDT_PF { get; set; }
        public string DEDT_ESI { get; set; }
        public string AADHAR_NO { get; set; }
        public string VOTER_ID { get; set; }
        public DateTime? EDATE { get; set; }
        public string DEPT_CODE { get; set; }
        public string SUB_DEPT_CODE { get; set; }
        public string DESIG { get; set; }
        public string DESG_CODE { get; set; }
        public string GRADE_CODE { get; set; }
        public string GROUP_CODE { get; set; }
        public decimal? BASIC { get; set; }
        public decimal? DA_RS { get; set; }
        public decimal? DA_PERCENT { get; set; }
        public decimal? HRA_RS { get; set; }
        public decimal? HRA_PERCENT { get; set; }
        public decimal? VDA_RS { get; set; }
        public decimal? VDA_PERCENT { get; set; }
        public decimal? PF { get; set; }
        public decimal? EPF { get; set; }
        public decimal? FPF { get; set; }
        public decimal? ESI_RS { get; set; }
        public decimal? ESI_PERCENT { get; set; }
        public decimal? CONVEYANCE_RS { get; set; }
        public decimal? CONVEYANCE_PERCENT { get; set; }
        public decimal? ESI_LIMIT { get; set; }
        public decimal? STATU_PF_LIMIT { get; set; }
        public string ESI_PER_EMPLR { get; set; }
        public string PF_SAL_LIMIT_EMPLR { get; set; }
        public string ESI_MANUALLY { get; set; }
        public string DA_FORMULA { get; set; }
        public string HRA_FORMULA { get; set; }
        public string VDA_FORMULA { get; set; }
        public string PTAX_MANUALLY { get; set; }
        public string MAXPTAX { get; set; }
        public string SA_FUND_PER { get; set; }
        public string SA_FUND_AMT { get; set; }


    }
    public class MissingData
    {
        public string EMPCODE { get; set; }
        public List<string> MissingFields { get; set; }        
        public List<string> DuplicateFields { get; set; }        
    }

}