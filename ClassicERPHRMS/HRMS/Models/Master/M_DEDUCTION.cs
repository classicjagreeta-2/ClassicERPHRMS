using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_DEDUCTION
	{
        public int id { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_DEDUCT_CODE { get; set; }
        public string deducT_TYPE { get; set; }
        public string description { get; set; }
        public string shorT_NAME { get; set; }
        public string fK_CALC_MODE { get; set; }
        public bool incL_PF { get; set; }
        public bool incL_ESI { get; set; }
        public bool incL_PTAX { get; set; }
        public bool incL_TAXABLE { get; set; }
        public string includE_VOUCHER { get; set; }
        public string voucheR_CREDIT { get; set; }
        public string suspencE_AC { get; set; }
        public string sundrY_CREDITOR { get; set; }
        public string voucheR_ADV_AC { get; set; }
        public string voucheR_HEAD { get; set; }
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
        public bool bool_incL_PF { get; set; }
        public bool bool_incL_ESI { get; set; }
        public bool bool_incL_PTAX { get; set; }
        public bool bool_incL_TAXABLE { get; set; }

        public string ptype { get; set; }
     
       
    }
    public class M_DEDUCTION_Dto
    {
        public M_DEDUCTION m_deduction { get; set; }
    }
}