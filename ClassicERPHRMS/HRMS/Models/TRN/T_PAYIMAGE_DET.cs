using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
	public class T_PAYIMAGE_DET
	{
	 public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
		public string UNIT { get; set; }
		public int ID { get; set; }
		public string FK_EMPCODE { get; set; }
		public string FK_MONTHCODE { get; set; }
		public DateTime? FROM_PERIOD { get; set; }
		public DateTime? TO_PERIOD { get; set; }
		public string PAY_TYPE { get; set; }
		public string FK_ALLOW_CODE { get; set; }
		public decimal ALLOW_AMT { get; set; }
		public string FK_DEDUCT_CODE { get; set; }
		public decimal? DEDUCT_AMT { get; set; }
		public decimal? FK_ADV_CODE { get; set; }
		public decimal? ADV_AMT { get; set; }
		public decimal? ARR_CODE { get; set; }
		public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;
		public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
		public string USER_NAME_NEW { get; set; }
		public DateTime? USER_ENTDT_NEW { get; set; }
		 public string OS_USER { get; set; }= "Laptop";
		  public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
		public string DTAG { get; set; }
		public int FK_HEAD_ID { get; set; }
	}
}