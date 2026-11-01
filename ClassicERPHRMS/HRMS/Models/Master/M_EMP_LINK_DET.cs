using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_EMP_LINK_DET
	{
	 public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
		public string UNIT { get; set; }
		public int ID { get; set; }
		[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
		public DateTime? EFF_DATE { get; set; }
		public string FK_UNIT_CODE { get; set; }
		public string FK_EMPCODE { get; set; }
		public string FK_CAT_CODE { get; set; }
		public string FK_EMP_TYPE { get; set; }
		public string FK_DEPT_CODE { get; set; }
		public string FK_SUB_DEPT_CODE { get; set; }
		public string FK_GRADE_CODE { get; set; }
		public string FK_DESG_CODE { get; set; }
		public string FK_COST_CODE { get; set; }
		public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;

		public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
		public string USER_NAME_NEW { get; set; }

		public DateTime? USER_ENTDT_NEW { get; set; }
		public string OS_USER { get; set; }= "Laptop";
		public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
		public string DTAG { get; set; }

        public string PTYPE { get; set; }

        public bool APPLY_PF { get; set; } = false;
        public bool APPLY_ESI { get; set; } = false;
        public bool APPLY_BONUS { get; set; } = false;
        public bool APPLY_INCENTIVE { get; set; } = false;
        public bool APPLY_ITAX { get; set; } = false;
        public bool APPLY_PTAX { get; set; } = false;
        public bool APPLY_GRATUITY { get; set; } = false;
        public bool APPLY_FC { get; set; } = false;
        public bool APPLY_FC_ON_OT { get; set; } = false;
        public bool APPLY_VPF { get; set; } = false;
    }
    public class M_EMP_LINK_DET_Dto
    {
        public M_EMP_LINK_DET m_emplinkdet { get; set; }
        public List<M_EMP_LINK_DET> m_emplinkdet_list { get; set; }
    }
}