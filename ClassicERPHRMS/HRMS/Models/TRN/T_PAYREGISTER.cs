
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_PAYREGISTER
    {
        //public int ID { get; set; }
        public string FK_EMPCODE { get; set; }
        public string FK_MONTHCODE { get; set; }
        public string UNIT { get; set; }
        public string NAME { get; set; }
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string TITLE { get; set; }
        public string FNAME { get; set; }
        public string MNAME { get; set; }
        public string LNAME { get; set; }
        public string SEX { get; set; }
        public string BANK_NAME { get; set; }
        public string AC_NO { get; set; }
        public string SWIFT_CODE { get; set; }
        public string IBAN_NO { get; set; }
        public string IFSC_CODE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? DATEOFBIRTH { get; set; }
        public DateTime? DATEOFJOINING { get; set; }
        public string UAN { get; set; }
        public string ESINO { get; set; }
        public string DESCRIPTION { get; set; }
        public string DESG_DESCN { get; set; }
        //public string EMP_TYPE_DESCN { get; set; }
        public string COST_CODE { get; set; }
        public string COST_DESCN { get; set; }

        public string DESCN { get; set; } //ADDED FOR COSTCENTER DESCRIPTION 
        public string PAY_TYPE { get; set; }
        public decimal BASIC_PAY { get; set; }
        public decimal DA_PAY { get; set; }
        public decimal HRA_PAY { get; set; }
        public decimal VDA_PAY { get; set; }
        public decimal CONV_PAY { get; set; }
        public decimal ACT_BASIC_PAY { get; set; }
        public decimal ACT_DA_PAY { get; set; }
        public decimal ACT_VDA_PAY { get; set; }
        public decimal ACT_CONV_PAY { get; set; }
        public decimal ACT_HRA_PAY { get; set; }
        public decimal INC_DAY { get; set; }
        public decimal INC_AMT { get; set; }
        public decimal PREV_MONTH_ADJ { get; set; }
        public decimal CURR_MONTH_ADJ { get; set; }
        public decimal EARNED_WAGES { get; set; }
        public decimal TOTAL_ALLOW_AMT { get; set; }
        public decimal GROSS_PAY { get; set; }
        public decimal PF { get; set; }
        public decimal ESI { get; set; }
        public decimal PTAX_AMT { get; set; }
        public decimal INCOMETAX_AMT { get; set; }
        public decimal OTHER_DED_AMT { get; set; }
        public decimal LWF { get; set; }
        public decimal TOTAL_DED_AMT { get; set; }
        public decimal NET_SAL_PAID { get; set; }
        public decimal NET_PAY { get; set; }
        public decimal DAYS_PRESENT { get; set; }
        public decimal ABSENT { get; set; }
        public decimal PAID_HOLIDAY { get; set; }
        public decimal PAID_LEAVE { get; set; }
        public decimal C_LEAVE { get; set; }
        public decimal WEEK_LEAVE { get; set; }
        public decimal ARREAR_DAY { get; set; }
        public string ARREAR_MONTH { get; set; }
        public string STAFF_WORKER { get; set; }
        public string LOGO_PATH { get; set; }
        public DateTime? FROM_PERIOD { get; set; }
        public DateTime? TO_PERIOD { get; set; }

        //new model 
         public string  TotalDays           { get; set; } 
         public string  LeaveEncashDays   { get; set; }
         public string  LeavewithoutPay   { get; set; }
         public string  LeavewithPay      { get; set; }
         public string  Extradays          { get; set; }
         public string  LateDays           { get; set; }
         public string  CA                  { get; set; }
         public string  BasicpayHRACATotal   { get; set; }
         public string  ArrearPay           { get; set; }
         public string  OtherAllowance      { get; set; }
         public string Incentive { get; set; }
    }
 
}


                                   