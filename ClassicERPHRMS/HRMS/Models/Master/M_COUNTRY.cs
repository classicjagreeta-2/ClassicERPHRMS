using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_COUNTRY
    {
        public int id { get; set; }
    public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public object unit { get; set; } = "";
        public string pK_COUNTRY_CODE { get; set; }
        public string description { get; set; }
          public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public object useR_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? useR_ENTDT_NEW { get; set; }
       public string oS_USER { get; set; } = "Laptop";
       public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public object dtag { get; set; }

        public object ptype { get; set; }
    }
    public class M_COUNTRY_Dto
    {
        public M_COUNTRY m_country { get; set; }
    }
}