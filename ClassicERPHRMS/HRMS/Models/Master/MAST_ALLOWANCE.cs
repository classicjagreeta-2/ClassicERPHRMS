using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class MAST_ALLOWANCE
	{
	 public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];

		public string UNIT { get; set; }

		public int ID { get; set; }

		public string ALL_CODE { get; set; }

		public string DESCRIPTION { get; set; }

		public string SHORT_NAME { get; set; }

		public string CALCULATION_MODE { get; set; }

		public string TRANSACTION_MASTER {  get; set; }

		public string INCL_PF {  get; set; }

		public string INCL_ESI { get; set; }

		public string INCL_PTAX { get; set; }

		public string INCL_TAXABLE { get; set; }

		public string INCLUDE_VOUCHER { get ; set; }

		public string VOUCHER_DEBIT { get; set; }

		public string EMP_WELFARE {  get; set; }

		public string VOUCHER_OT_AC { get;set; }

		public string VOUCHER_HEAD { get; set; }

		public string INCL_OT {  get; set; }


	}
}