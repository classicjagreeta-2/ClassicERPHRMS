using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_LEAVE_OP_DECLARE
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_EMPCODE { get; set; }
        public string FK_ATTEND_CODE { get; set; }
        public decimal OPENING { get; set; }
        public decimal ALLOTED { get; set; }
        public string AUTO_CALC { get; set; }
        public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime? USER_ENTDT_NEW { get; set; }
         public string OS_USER { get; set; }= "Laptop";
          public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }

    }
}