using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models
{
    public class Common
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string OS_USER { get; set; } = "Laptop"; 
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
    }
}