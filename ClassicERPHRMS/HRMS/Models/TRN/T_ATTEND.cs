using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models.Master;

namespace HRMS.Models.TRN
{
    public class T_ATTEND
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public DateTime EDATE { get; set; }
        public string FK_EMPCODE { get; set; }
        //public string FK_ATTEND_CODE { get; set; }
        public decimal ATTEND { get; set; }
        public decimal LEAVE { get; set; }
        public decimal HOLIDAY { get; set; }
        public decimal ABSENT { get; set; }
        public decimal WEEKLY_OFF { get; set; }
        public decimal LATE_TIME { get; set; }
        public string REMARKS { get; set; }
        public string FK_MONTHCODE { get; set; }
        public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime USER_ENTDT_NEW { get; set; }
         public string OS_USER { get; set; }= "Laptop"; 
          public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
    }
}