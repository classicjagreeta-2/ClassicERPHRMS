using HRMS.Models.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_MONTHLY_ATTEND
    {
        public string LOCA { get; set; }
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_MONTHCODE { get; set; }
        public DateTime FROM_DATE { get; set; } = System.DateTime.Now;
        public DateTime TO_DATE { get; set; } = DateTime.Now;
        public string PAY_TYPE { get; set; }
        public string FK_EMPCODE { get; set; }
        public decimal? PRESENT { get; set; }
        public decimal? PAID_HOLIDAY { get; set; }
        public decimal? UNPAID_LEAVE { get; set; }
        public decimal? WEEK_LEAVE { get; set; }
        public decimal? PAID_LEAVE { get; set; }
        public decimal? ABSENT { get; set; }
        public decimal? EXTRA_DAYS { get; set; }
        public decimal? TOTAL_DAYS { get; set; }
        public decimal? LESS_HOUR_WORKED { get; set; }
        public decimal? ARREAR_DAY { get; set; }
        public string ARREAR_MONTH { get; set; }
        public decimal? LEAVE_ENCASH { get; set; }
        public decimal? LATE_DAY { get; set; }
        public string PTYPE { get; set; }
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }

        public class T_MONTHLY_ATTEND_Dto
        {
            public List<T_MONTHLY_ATTEND> t_monthly_attend { get; set; }
            public List<T_MONTHLY_ATTEND_DAYS_DET> t_monthly_attend_days { get; set; }
            public List<T_MONTHLY_ATTEND_SHIFT_DET> t_monthly_attend_shft { get; set; }

        }

    }
}