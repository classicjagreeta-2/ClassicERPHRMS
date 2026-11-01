using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_PAYIMAGE
    {
        public string UNIT { get; set; } 
        public int ID { get; set; }
        public string FK_EMPCODE { get; set; } = "";
        public string FK_MONTHCODE { get; set; } = "";
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime FROM_PERIOD { get; set; } = DateTime.Now;
        public DateTime TO_PERIOD { get; set; }
        public string PAY_TYPE { get; set; }
        public decimal DAYS_PRESENT { get; set; }
        public decimal PAID_HOLIDAY { get; set; }
        public decimal MONTHDAYS { get; set; }
        public decimal PFSAL { get; set; }
        public decimal PF { get; set; }
        public decimal FPF { get; set; }
        public decimal EPF { get; set; }
        public decimal ESISAL { get; set; }
        public decimal ESI { get; set; }
        public decimal BASIC_PAY { get; set; }
        public decimal DA_PAY { get; set; }
        public decimal HRA_PAY { get; set; }
        public decimal VDA_PAY { get; set; }
        public decimal NET_PAY { get; set; }
        public decimal CONV_PAY { get; set; }
        public decimal UNPAID_LEAVE { get; set; }
        public decimal TOT_HR_FOR_OT { get; set; }
        public decimal OT_PAY { get; set; }
        public decimal PREV_MONTH_ADJ { get; set; }
        public decimal CURR_MONTH_ADJ { get; set; }
        public decimal NET_SAL_PAID { get; set; }
        public decimal PTAX_SAL { get; set; }
        public decimal PTAX_AMT { get; set; }
        public decimal WEEK_LEAVE { get; set; }
        public decimal SUSPEND_SAL { get; set; }
        public decimal PAID_LEAVE { get; set; }
        public decimal ABSENT { get; set; }
        public decimal EXTRA_DAYS { get; set; }
        public decimal ACT_BASIC_PAY { get; set; }
        public decimal ACT_DA_PAY { get; set; }
        public decimal ACT_HRA_PAY { get; set; }
        public decimal ACT_VDA_PAY { get; set; }
        public decimal ACT_CONV_PAY { get; set; }
        public decimal EARNED_WAGES { get; set; }
        public decimal TOT_PAYABLE_DAYS { get; set; }
        public decimal VOLNT_PF { get; set; }
        public decimal STATU_PF { get; set; }
        public decimal ESI_EMPLR { get; set; }
        public decimal EPF_EMPLR_ACT { get; set; }
        public decimal FPF_EMPLR_ACT { get; set; }
        public decimal PF_SAL_EMPLR { get; set; }
        public decimal INCOMETAX_AMT { get; set; }
        public decimal ACT_ESI_SAL { get; set; }
        public decimal PF_MAX_LIMIT_EMPLR { get; set; }
        public decimal EXTRA_DAYS_AMT { get; set; }
        public decimal NOT_IN_WORK { get; set; }
        public decimal RATE_OF_PAY { get; set; }
        public string STAFF_WORKER { get; set; }
        public string ESI_EXEMPTED { get; set; }
        public decimal ARREAR_DAY { get; set; }
        public string ARREAR_MONTH { get; set; }
        public decimal ARR_CODE { get; set; }
        public decimal EXGRATIA { get; set; }
        public decimal LESS_HOUR_AMT { get; set; }
        public bool DEDT_PF { get; set; }
        public bool DEDT_ESI { get; set; }
        public decimal TOTAL_ALLOW_AMT { get; set; }
        public decimal OTHER_DED_AMT { get; set; }
        public decimal TOTAL_DED_AMT { get; set; }
        public decimal GROSS_PAY { get; set; }
        public decimal FULL_DAY { get; set; }
        public decimal HALF_DAY { get; set; }
        public decimal UNPAID_HOLIDAY { get; set; }
        public decimal SICK_LEAVE { get; set; }
        public decimal UNPAID_SICK { get; set; }
        public decimal MATERNITY_LEAVE { get; set; }
        public decimal UNPAID_MATERNITY { get; set; }
        public decimal OTHER_LEAVE { get; set; }
        public decimal UNPAID_OTHER { get; set; }
        public string PFNO { get; set; }
        public string DISBURSE { get; set; }
        public string MW { get; set; }
        public decimal DEPEN_A { get; set; }
        public decimal DEPEN_C { get; set; }
        public decimal DEPEN_M { get; set; }
        public string CENSUSNO { get; set; }
        public string DEDT_RAT { get; set; }
        public string PENSION { get; set; }
        public decimal W_LEAVE { get; set; }
        public decimal ATTN_AMT { get; set; }
        public decimal SL_AMT { get; set; }
        public decimal EL_AMT { get; set; }
        public decimal GL_AMT { get; set; }
        public decimal OL_AMT { get; set; }
        public decimal ML_AMT { get; set; }
        public decimal WL_AMT { get; set; }
        public decimal ELP_AMT { get; set; }
        public decimal FCONC { get; set; }
        public decimal RAT_AMT { get; set; }
        public decimal ADV_AMT { get; set; }
        public decimal LWF { get; set; }
        public string RAT_CODE { get; set; }
        public decimal SARD_COMM { get; set; }
        public decimal VAR_DED { get; set; }
        public decimal REV_AMT { get; set; }
        public decimal C_LEAVE { get; set; }
        public decimal BONUS_AMT { get; set; }
        public decimal POP_AMT { get; set; }
        public decimal BONUS_RATE { get; set; }
        public decimal NIRIK_AMT { get; set; }
        public decimal PRORAT_AMT { get; set; }
        public decimal RAT_ESTQTY { get; set; }
        public string ECD { get; set; }
        public decimal INC_DAY { get; set; }
        public decimal INC_AMT { get; set; }
        public decimal NET_ELPAMT { get; set; }
        public decimal MILL_AMT { get; set; }
        public string DVSN_CODE { get; set; }
        public string DEK_CODE { get; set; }
        public string SARD_CODE { get; set; }
        public string DOCYEAR { get; set; }
        public decimal ELP_PF { get; set; }
        public decimal ELP_EPF { get; set; }
        public decimal ELP_FPF { get; set; }
        public decimal ELP_PF_GR_AMT { get; set; }
        public decimal PF_FOODC { get; set; }
        public decimal SPOUSE { get; set; }
        public decimal EL_DAY { get; set; }
        public decimal BON_DAY { get; set; }
        public decimal BON_WAGE { get; set; }
        public string PAY_MODE { get; set; }
        public decimal FCOMP { get; set; }
        public decimal INTERIM_RATE { get; set; }
        public decimal INTERIM_AMT { get; set; }
        public decimal EXG_RATE { get; set; }
        public decimal PF_RATE { get; set; }
        public decimal EPF_RATE { get; set; }
        public decimal FPF_RATE { get; set; }
        public string PTYPE { get; set; }
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; } = "";
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime USER_ENTDT_NEW { get; set; } =System.DateTime.Now;
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; } = "";

       

       
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class PaySlipList
    {
        public string EMPCODE { get; set; }
        public string MONTHCODE { get; set; }
        public string UNIT { get; set; }
        public string UNITCODE { get; set; }
        public string TITLE { get; set; }
        public string FNAME { get; set; }
        public string MNAME { get; set; }
        public string LNAME { get; set; }
        public string SEX { get; set; }
        public string BANK_NAME { get; set; }
        public string AC_NO { get; set; }
        public string IBAN_NO { get; set; }
        public DateTime? DATEOFBIRTH { get; set; }
        public DateTime? DATEOFJOINING { get; set; }
        public string UAN { get; set; }
        public string ESINO { get; set; }
        public string IFSC_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESG_DESCN { get; set; }
        public string COST_CODE { get; set; }
        public string DESCN { get; set; }
        public double BASIC_PAY { get; set; }
        public double ACT_BASIC_PAY { get; set; }
        public double DA_PAY { get; set; }
        public double ACT_DA_PAY { get; set; }
        public double VDA_PAY { get; set; }
        public double ACT_VDA_PAY { get; set; }
        public double CONV_PAY { get; set; }
        public double ACT_CONV_PAY { get; set; }
        public double PREV_MONTH_ADJ { get; set; }
        public double INC_DAY { get; set; }
        public double INC_AMT { get; set; }
        public double EARNED_WAGES { get; set; }
        public double TOTAL_ALLOW_AMT { get; set; }
        public double GROSS_PAY { get; set; }
        public double PF { get; set; }
        public double ESI { get; set; }
        public double PTAX_AMT { get; set; }
        public double INCOMETAX_AMT { get; set; }
        public double OTHER_DED_AMT { get; set; }
        public double LWF { get; set; }
        public double TOTAL_DED_AMT { get; set; }
        public double CURR_MONTH_ADJ { get; set; }
        public double NET_SAL_PAID { get; set; }
        public double NET_PAY { get; set; }
        public double DAYS_PRESENT { get; set; }
        public double ABSENT { get; set; }
        public double PAID_HOLIDAY { get; set; }
        public double PAID_LEAVE { get; set; }
        public double C_LEAVE { get; set; }
        public double WEEK_LEAVE { get; set; }
        public string LOGO_PATH { get; set; }
    }

    public class payimagedittocopy
    {
        public string EMPCODE { get; set; }
        public string MONTHCODE { get; set; }
        public string UNIT { get; set; }
        public string UNITCODE { get; set; }
        public string TITLE { get; set; }
        public string FNAME { get; set; }
        public string MNAME { get; set; }
        public string LNAME { get; set; }
        public string SEX { get; set; }
        public string BANK_NAME { get; set; }
        public string AC_NO { get; set; }
        public string IBAN_NO { get; set; }
        public DateTime DATEOFBIRTH { get; set; }
        public DateTime DATEOFJOINING { get; set; }
        public string UAN { get; set; }
        public string ESINO { get; set; }
        public string IFSC_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESG_DESCN { get; set; }
        public string COST_CODE { get; set; }
        public string CDESCN { get; set; }
        public string BASIC_PAY { get; set; }
        public string ACT_BASIC_PAY { get; set; }
        public string DA_PAY { get; set; }
        public string ACT_DA_PAY { get; set; }
        public string VDA_PAY { get; set; }
        public string ACT_VDA_PAY { get; set; }
        public string CONV_PAY { get; set; }
        public string ACT_CONV_PAY { get; set; }
        public string PREV_MONTH_ADJ { get; set; }
        public string INC_DAY { get; set; }
        public string INC_AMT { get; set; }
        public string EARNED_WAGES { get; set; }
        public string TOTAL_ALLOW_AMT { get; set; }
        public string GROSS_PAY { get; set; }
        public string PF { get; set; }
        public string ESI { get; set; }
        public string PTAX_AMT { get; set; }
        public string INCOMETAX_AMT { get; set; }
        public string OTHER_DED_AMT { get; set; }
        public string LWF { get; set; }
        public string TOTAL_DED_AMT { get; set; }
        public string CURR_MONTH_ADJ { get; set; }
        public string NET_SAL_PAID { get; set; }
        public string NET_PAY { get; set; }
        public string DAYS_PRESENT { get; set; }
        public string ABSENT { get; set; }
        public string PAID_HOLIDAY { get; set; }
        public string PAID_LEAVE { get; set; }
        public string C_LEAVE { get; set; }
        public string WEEK_LEAVE { get; set; }
        public string LOGO_PATH { get; set; }
        public string DEDUCT_DESC { get; set; }
        public string DEDUCT_CODE { get; set; }
        public string ACT_HRA_PAY { get; set; }
        public string HRA_PAY { get; set; }
    }


}