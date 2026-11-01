using HRMS.Models.TRN;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_SHIFT
    {
    public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public int id { get; set; }
        public string pK_SHIFT_CODE { get; set; }
        public string description { get; set; }
        public string shifT_START_FROM { get; set; }
        public string luncH_START_FROM { get; set; }
        public string shifT_END_AT { get; set; }
        public string luncH_END_AT { get; set; }
        public string totaL_HOURS { get; set; }
        public string totaL_LUNCH_HOURS { get; set; }
        public string shifT_GRACE_PERIOD { get; set; }
        public string luncH_GRACE_PERIOD { get; set; }
        public string saT_END_AT { get; set; }
        public string generaL_SHIFT { get; set; }
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
        public string ptype { get; set; }

        public string dtag { get; set; }

    }

    public class MONTHLY_ATTENDANCE_ENTRY
    {
        public string UNIT { get; set; }
        public string MONTHCODE { get; set; }
        public string EMPCODE { get; set; }
        public List<SHIFT_DET> SHIFT_DET { get; set; }
        public List<ATTEND_DAYS_DET> ATTEND_DAYS_DET { get; set; }
    }
    public class SHIFT_DET
    {
        public int ID { get; set; }
        public string PK_SHIFT_CODE { get; set; }
        public string DESCRIPTION { get; set; }
        public string UNIT { get; set; }
        public string FK_MONTHCODE { get; set; }
        public decimal DAYS { get; set; }
        public decimal HOURS { get; set; }
        public decimal MINS { get; set; }
        public string FK_EMPCODE { get; set; }
        //public List<T_MONTHLY_ATTEND_DAYS_DET> T_MONTHLY_ATTEND_DAYS_DET { get; set; }
    }
    public class M_SHIFT_DETAILS : M_SHIFT
    {
        public M_SHIFT m_SHIFT { get; set; }
        public string UNIT_DESC { get; set; }
    }

    public class M_SHIFT_Dto 
    {
        public M_SHIFT m_shift { get; set; }
        
    }
}