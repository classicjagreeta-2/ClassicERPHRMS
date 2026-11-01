using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_EMP_DEDUCT_DET
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_EMPCODE { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime EFF_DATE { get; set; }
        public string FK_DEDUCT_CODE { get; set; }
        public decimal DEDUCT_RS { get; set; }
        public decimal? DEDUCT_PERCENT { get; set; }
        public decimal? DEDUCT_LIMIT { get; set; }

        public bool INCL_PF { get; set; } = false;
        public bool INCL_ESI { get; set; } = false;
        public bool INCL_PTAX { get; set; } = false;
        public bool INCL_PITAX { get; set; } = false;
        public bool ACTIVE_STATUS { get; set; } = false;

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? DDATE { get; set; }
        public string FK_CURR_CODE { get; set; }
        public decimal? NUMBER_INST { get; set; }
        public decimal? ADVANCE_AMOUNT { get; set; }
        public decimal? TOTAL_DEDUCTED { get; set; }
        public decimal? TOTAL_INST_RECD { get; set; }
        public string ADVANCE_CLOSED { get; set; }
        public string CLOSEDBY { get; set; }
        public DateTime? CLOSED_ON { get; set; }
        public decimal? CLOSED_AMOUNT { get; set; }
        public string FK_MONTHCODE { get; set; }
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
        public string DOCNO { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? DOCDT { get; set; }
        public string DeductionDesc { get; set; }
        public string PTYPE { get; set; } = "S";
    }
    
    public class M_EMP_DEDUCT_DET_Dto
    {
        public M_EMP_DEDUCT_DET m_empdeductdet { get; set; }
    }
}
