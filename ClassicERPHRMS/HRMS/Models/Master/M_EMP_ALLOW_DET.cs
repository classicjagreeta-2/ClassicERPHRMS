using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_EMP_ALLOW_DET
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_EMPCODE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime EFF_DATE { get; set; } 

        public string _allowanceDesc;
        public string FK_ALL_CODE { get; set; }

        public decimal ALL_RS { get; set; }
        public decimal? ALL_PERCENT { get; set; }
        public decimal? ALL_LIMIT { get; set; }
        public bool INCL_PF { get; set; } = false;
        public bool INCL_ESI { get; set; } = false;
        public bool INCL_PTAX { get; set; } = false;
        public bool INCOME_TAXABLE { get; set; } = false;
        public bool ACTIVE_STATUS { get; set; } = false;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? DDATE { get; set; }
        public bool INCL_OT { get; set; } = false;
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
        public string FK_CURR_CODE { get; set; }
        public bool INCL_GROSS { get; set; } = false;

        public string PTYPE { get; set; } = "S";
    }
    public class M_EMP_ALLOW_DET_Dto
    {
        public M_EMP_ALLOW_DET m_empallowdet { get; set; }
    }
}