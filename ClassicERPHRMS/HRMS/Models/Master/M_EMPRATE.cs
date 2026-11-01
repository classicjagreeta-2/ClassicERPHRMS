using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_EMPRATE
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        public string FK_EMPCODE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? EDATE { get; set; }
        public decimal? BASIC { get; set; }
        public decimal? DA_RS { get; set; } = 0m;
        public decimal? DA_PERCENT { get; set; } = 0m;
        public decimal? HRA_RS { get; set; }
        public decimal? HRA_PERCENT { get; set; }
        public decimal? VDA_RS { get; set; }
        public decimal? VDA_PERCENT { get; set; }
        public decimal? PF { get; set; }
        public decimal? EPF { get; set; }
        public decimal? FPF { get; set; }
        public decimal? ESI_RS { get; set; }
        public decimal? ESI_PERCENT { get; set; }
        public decimal? CONVEYANCE_RS { get; set; }
        public decimal? CONVEYANCE_PERCENT { get; set; }
        public decimal? ESI_LIMIT { get; set; }
        public string STAFF_WORKER { get; set; }
        public decimal? STATU_PF_LIMIT { get; set; }
        public decimal? ESI_PER_EMPLR { get; set; }
        public decimal? PF_SAL_LIMIT_EMPLR { get; set; }
        public string ESI_MANUALLY { get; set; }
        public string DA_FORMULA { get; set; }
        public string HRA_FORMULA { get; set; }
        public string VDA_FORMULA { get; set; }
        public string PTAX_MANUALLY { get; set; }
        public decimal? MAXPTAX { get; set; }
        public decimal? SA_FUND_PER { get; set; }
        public decimal? SA_FUND_AMT { get; set; }
        public string ALLOW_ARR { get; set; }
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
        public string PTYPE { get; set; } = "S";
        public string FK_CURR_CODE { get; set; }
    }
    public class m_emp_Dto
    {
        public M_EMPRATE m_emprate { get; set; }
        public List<M_EMPRATE> m_emprate_List { get; set; }
    }

}