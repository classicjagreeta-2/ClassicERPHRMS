using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_CATEGORY
    {
        public int id { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_CATE_CODE { get; set; }
        public string catE_DESCN { get; set; }
        public string user_name { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? user_entdt { get; set; } = System.DateTime.Now;
        public string user_name_new { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? user_entdt_new { get; set; }
        public string os_user { get; set; } = "Laptop";
        public string terminal_id { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string dtag { get; set; }
        public string PTYPE { get; set; }
    }
    public class M_CATEGORY_Dto
    {
        public M_CATEGORY m_category { get; set; }
    }
}