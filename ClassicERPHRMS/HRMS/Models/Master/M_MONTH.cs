using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_MONTH
    {
        public string unit { get; set; } = "";
        public int id { get; set; }
        public string pK_MONTHCODE { get; set; }
        public int wkno { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public int nuM_DAYS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime froM_PERIOD { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime tO_PERIOD { get; set; }
        public decimal? weeK_OFF { get; set; }
        public decimal? paiD_HOLIDAY { get; set; }
        public string locK_CALCULATE_PAYROLL { get; set; }
        public string ptype { get; set; }
          public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public string useR_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? useR_ENTDT_NEW { get; set; }
       public string oS_USER { get; set; } = "Laptop";
       public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string dtag { get; set; }

        public string MonthName { get; set; }
    }

    public class M_montyh_dto
    {
        public M_MONTH m_month { get; set; }
    }
}