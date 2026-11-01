using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using HRMS.Models.Master;

namespace HRMS.Models.Master
{
	public class M_SUB_DEPT
    {
    public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public object unit { get; set; } = "";
        public int id { get; set; }
        public string pK_SUB_DEPT { get; set; }
        public string fK_DEPT_CODE { get; set; }
        public string suB_DEPT_DESCN { get; set; }
        public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime useR_ENTDT { get; set; }
        public object useR_NAME_NEW { get; set; }
        public object useR_ENTDT_NEW { get; set; }
       public string oS_USER { get; set; } = "Laptop";
       public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public object dtag { get; set; }
        public string ptype { get; set; }
    }

    public class SUB_DEPT_DETAILS : M_SUB_DEPT
    {
        public M_SUB_DEPT m_SUB_DEPT { get; set; }
        public string DEPRT_DES { get; set; }

    }
    public class  M_SUB_DEPT_dto
    {
        public M_SUB_DEPT m_subdept { get; set; }
       

    }
}