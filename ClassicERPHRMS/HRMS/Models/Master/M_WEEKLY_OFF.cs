using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_WEEKLY_OFF
    {
    public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public int id { get; set; }
        public string fK_EMPCODE { get; set; }
        public string fK_CAT_CODE { get; set; }
        public string fK_EMP_TYPE { get; set; }
        public string fK_DEPT_CODE { get; set; }
        public string ofF_DAY1 { get; set; }
        public string ofF_DAY2 { get; set; }
        public string fK_ATTEND_CODE { get; set; }
          public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public string useR_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? useR_ENTDT_NEW { get; set; }
       public string oS_USER { get; set; } = "Laptop";
       public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString(); 
        public string dtag { get; set; }
        public string ptype { get; set; }
    }
    public class M_WEEKLY_OFF_Dto
    {
        public M_WEEKLY_OFF m_weeklyoff { get; set; }
    }
}