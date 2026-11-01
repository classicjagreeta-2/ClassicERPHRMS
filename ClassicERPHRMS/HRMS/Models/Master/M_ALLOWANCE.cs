using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_ALLOWANCE
	{
        public int id { get; set; }

      public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];

        public string unit { get; set; }
        public string pK_alL_CODE { get; set; }
        public string description { get; set; }
        public string shorT_NAME { get; set; }
        public string fK_CALC_MODE { get; set; }
        public object transactioN_MASTER { get; set; }
        public bool incL_PF { get; set; }
        public bool incL_ESI { get; set; }
        public bool incL_PTAX { get; set; }
        public bool incL_TAXABLE { get; set; }
        public bool includE_VOUCHER { get; set; }
        public bool voucheR_DEBIT { get; set; }
        public bool emP_WELFARE { get; set; }
        public bool voucheR_OT_AC { get; set; }
        public string voucheR_HEAD { get; set; }
        public bool incL_OT { get; set; }
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
        public bool incL_GROSS { get; set; }
        public bool bool_INCL_PF { get; set; }
        public bool bool_INCL_ESI { get; set; }
        public bool bool_INCL_PTAX { get; set; }
        public bool bool_INCL_TAXABLE { get; set; }
        public bool bool_INCL_OT { get; set; }

        public string ptype { get; set; }
    }
    public class M_ALLOWANCE_Dto
    {
        public M_ALLOWANCE m_allowance { get; set; }
    }
}