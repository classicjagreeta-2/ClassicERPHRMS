using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_DEDUCT_CALC_MODE
    {
        public int id { get; set; }
       public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; }
        public string pK_CALC_MODE { get; set; }
        public string description { get; set; }
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
    public class M_DEDUCT_CALC_MODE_DETAILS : M_DEDUCT_CALC_MODE
    {
        public M_DEDUCT_CALC_MODE m_DEDUCT_CALC_MODE { get; set; }
        public string UNIT_DESC { get; set; }
    }
    public class M_DEDUCT_CALC_MODE_Dto
    {
        public M_DEDUCT_CALC_MODE m_deductcalcmode { get; set; }
    }
}