using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_ENUM_PARA
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];

        public string UNIT { get; set; }

        public int ID { get; set; }

        public string DESCN { get; set; }

        public string DESCN1 { get; set; }

        public string DESCN2 { get; set; }

        public int EnmCode { get; set; }

        public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;

        
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;

        public string USER_NAME_NEW { get; set; }

        public DateTime? USER_ENTDT_NEW { get; set; }

         public string OS_USER { get; set; }= "Laptop"; 

          public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

        public string DTAG { get; set; }
    }
}