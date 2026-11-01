using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_CURRENCY
    {
        public int ID { get; set; }
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; } = "";
        public string PK_CURR_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; }= "Laptop"; 
        public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }
        public string SYMBOL { get; set; }
        public string INTERNATIONAL_FMT { get; set; }
        public  string SUBUNIT { get; set; }
        public  string SHT_DESCN { get; set; }
        public string ptype { get; set; }
    }
    public class M_CURRENCY_Dto
    {
        public M_CURRENCY m_currency { get; set; }
    }
}