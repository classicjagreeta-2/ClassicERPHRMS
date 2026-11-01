using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_ATTEND_DAYS_TYPE
	{
        public int id { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_ATTEND_CODE { get; set; }
        public string description { get; set; }
        public bool withouT_PAY { get; set; }
        public bool extrA_DAYS { get; set; }
        public bool leave { get; set; }
        public bool overtime { get; set; }
        public int? ordeR_TAG { get; set; }
        public bool onlY_DAYS { get; set; }
        public string shorT_NAME { get; set; }
        public string oT_TYPE { get; set; }
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
        public string defaulT_CODE { get; set; }
        public bool weeklY_OFF { get; set; }
        public bool holiday { get; set; }

        public string ptype { get; set; }
        [Required]
        public string attendencetype { get; set; }

        //public bool _withouTPAY { get; set; }
        //public bool _extrADAYS { get; set; }
        //public bool _leave { get; set; }
        //public bool _overtime { get; set; }
        //public bool _weeklYOFF { get; set; }
        //public bool _holiday { get; set; }

    }
    public class M_ATTEND_DAYS_TYPE_Dto
    {
        public M_ATTEND_DAYS_TYPE m_attenddaystype { get; set; }
    }
}