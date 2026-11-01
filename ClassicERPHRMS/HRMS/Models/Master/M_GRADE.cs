using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_GRADE
    {
        public int id { get; set; }
    public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_GRADE_CODE { get; set; }
        public string description { get; set; }
        public decimal? starT_BASIC { get; set; }
        public decimal? enD_BASIC { get; set; }
        public decimal? incR_RATE { get; set; }
        public decimal? nO_OF_YEAR { get; set; }
        public string user_name { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? user_entdt { get; set; } = System.DateTime.Now;
        public string user_name_new { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? user_entdt_new { get; set; }
        public string os_user { get; set; }
        public string terminal_id { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string dtag { get; set; }

        public string ptype { get; set; }
    }
    public class M_GRADE_Dto
    {
        public M_GRADE m_grade { get; set; }
    }
}