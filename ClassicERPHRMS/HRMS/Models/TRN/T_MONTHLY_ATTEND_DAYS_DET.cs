using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_MONTHLY_ATTEND_DAYS_DET
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_MONTHCODE { get; set; }
        public DateTime? FROM_DATE { get; set; } = DateTime.Now;
        public DateTime? TO_DATE { get; set; } = DateTime.Now;
        public string PAY_TYPE { get; set; } = "P";
        public string FK_EMPCODE { get; set; }
        public string FK_ATTEND_CODE { get; set; }
        public decimal DAYS { get; set; }
        public decimal HOURS { get; set; }
        public decimal MINS { get; set; }
        public string ADJ_MONTH { get; set; }
        public string OT_TYPE { get; set; }
        public string PTYPE { get; set; } = "S";
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime USER_ENTDT_NEW { get; set; } = System.DateTime.Now;
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
        //public string DESCRIPTION { get; set; }

        public string _description { get; set; }
        public string AttendCode { get; set; }
        public string Description { get; set; }
        public string LeaveAlloted { get; set; }
        public string ReaminingLeave { get; set; }
        public string TotalLeaveDays { get; set; }
        public string TotalLeaveHours { get; set; }
        public string TotalLeaveMin { get; set; }
    }

    public class ATTEND_DAYS_DET
    {
        public int ID { get; set; }
        public string PK_ATTEND_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string UNIT { get; set; }
        public string FK_MONTHCODE { get; set; }
        public decimal DAYS { get; set; }
        public decimal HOURS { get; set; }
        public decimal MINS { get; set; }
        public string FK_EMPCODE { get; set; }
    }

    public class MonthlyAttend
    {
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string SHIFT { get; set; }

        [JsonProperty("PRESENT-FULL")]
        public decimal PRESENTFULL { get; set; }
        public decimal? WEEKLY_OFF { get; set; }

        [JsonProperty("LOP DAYS")]
        public decimal? LOPDAYS { get; set; }
        public decimal? WITHOUT_PAY { get; set; }
        public decimal? PAID_HOLIDAY { get; set; }
        public decimal? TOTAL_PAID_DAYS { get; set; }
        public decimal? EL_LEAVE { get; set; }
        public decimal? CL_LEAVE { get; set; }
        public decimal? SL_LEAVE { get; set; }
        public decimal? OT { get; set; }
        public decimal? ADV { get; set; }
        public decimal? ITAX { get; set; }
        public decimal? oth_ded { get; set; }
        public decimal? COMP_OFF { get; set; }
        public decimal? EXTRA_DAYS { get; set; }
        public decimal? LESS_HOUR_WORKED { get; set; }
    }
}