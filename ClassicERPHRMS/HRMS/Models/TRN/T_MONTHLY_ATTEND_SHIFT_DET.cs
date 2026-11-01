using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_MONTHLY_ATTEND_SHIFT_DET
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_MONTHCODE { get; set; }
        public DateTime FROM_DATE { get; set; } = DateTime.Now;
        public DateTime TO_DATE { get; set; } = DateTime.Now;
        public string PAY_TYPE { get; set; } = "P";
        public string FK_EMPCODE { get; set; }
        public string FK_SHIFT_CODE { get; set; }
        public decimal DAYS { get; set; }
        public decimal HOURS { get; set; }
        public decimal MINS { get; set; }
        public string PTYPE { get; set; } = "S";
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime USER_ENTDT_NEW { get; set; } = System.DateTime.Now;
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
    }
}