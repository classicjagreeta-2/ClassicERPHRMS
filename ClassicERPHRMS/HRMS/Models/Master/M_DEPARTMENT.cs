using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_DEPARTMENT
    {
        public int id { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_DEPT_CODE { get; set; }
        public string reF_CODE { get; set; }
        public string cosT_CENTRE { get; set; }
        public string description { get; set; }
          public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public string useR_NAME_NEW { get; set; }
        public DateTime? useR_ENTDT_NEW { get; set; }
       public string oS_USER { get; set; } = "Laptop";
       public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(); 
        public string dtag { get; set; }

        public string ptype { get; set; }
        
    }
    public class M_DEPARTMENT_Dto
    {
        public M_DEPARTMENT m_department { get; set; }
    }
}