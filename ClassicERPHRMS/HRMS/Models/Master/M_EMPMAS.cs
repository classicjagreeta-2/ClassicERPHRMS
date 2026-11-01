using HRMS.Models.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
	public class M_EMPMAS
	{        
        public int id { get; set; }
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string unit { get; set; } = "";
        public string pK_EMPCODE { get; set; }
        public string title { get; set; }
        public string fname { get; set; }
        public string mname { get; set; }
        public string lname { get; set; }
        public string pfno { get; set; }
        public string esino { get; set; }
        public string pancardno { get; set; }
        public string passportno { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? passportissuedon { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? p_RENEWALDATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? dateofbirth { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? dateofjoining { get; set; }
        public DateTime? dateofleaving { get; set; }
        public bool lefT_SERVICE { get; set; }
        public DateTime? transferred { get; set; }
        public string transferredto { get; set; }
        public string bloodgroup { get; set; }
        public string perM_HOUSENO { get; set; }
        public string perM_STREETNAME1 { get; set; }
        public string perM_STREETNAME2 { get; set; }
        public string perM_CITY { get; set; }
        public int? perM_PINCODE { get; set; } = 0;
        public string perM_POSTOFF { get; set; }
        public string perM_POLICEST { get; set; }
        public int? perM_RESTEL1 { get; set; }
        public int? perM_RESTEL2 { get; set; }
        public int? perM_MOBILENO { get; set; }
        public string temP_HOUSENO { get; set; }
        public string temP_STREETNAME1 { get; set; }
        public string temP_STREETNAME2 { get; set; }
        public string temP_CITY { get; set; }
        public int? temP_PINCODE { get; set; }
        public string temP_POSTOFF { get; set; }
        public string temP_POLICEST { get; set; }
        public int? temP_RESTEL1 { get; set; }
        public int? temP_RESTEL2 { get; set; }
        public int? temP_MOBILENO { get; set; }
        public string emaiL_ID { get; set; }
        public string shorT_NAME { get; set; }
        public string pasprT_ISD_FRM { get; set; }
        public string chQ_CSH { get; set; }
        public string svngS_CRNT { get; set; }
        public string aC_NO { get; set; }
        public string branch { get; set; }
        public string banK_NAME { get; set; }
        public string sex { get; set; }
        public string fK_SHIFT_CODE { get; set; }
        public string tnno { get; set; }
        public string pensioN_NO { get; set; }
        public string acode { get; set; }
        public string reF_CODE { get; set; }
        public string sacode { get; set; }
        public string ssubcode { get; set; }
        public DateTime? doJ_EPS { get; set; }
        public DateTime? doJ_EPF { get; set; }
        public DateTime? doL_EPS { get; set; }
        public DateTime? doL_EPF { get; set; }
        public DateTime? dog { get; set; }
        public string fK_REASON_CODE { get; set; }
        public string uan { get; set; }
        public string fK_STATE_CODE { get; set; }
        public bool activE_TAG { get; set; }
        public string aadhaR_NO { get; set; }
        public string leG_CODE { get; set; }
        public string voteR_ID { get; set; }
        public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public string useR_NAME_NEW { get; set; }
        public DateTime? useR_ENTDT_NEW { get; set; }
        public string oS_USER { get; set; } = "Laptop";
        public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string dtag { get; set; }
        public string namE_IN_VALID_DOC { get; set; }
        public string fK_COUNTRY_CODE_PERM { get; set; }
        public string fK_COUNTRY_CODE_TEMP { get; set; }
        public string ifsC_CODE { get; set; }
        public string swifT_CODE { get; set; }
        public string ibaN_NO { get; set; }
        public string whatsapP_NO { get; set; }
        public string emP_IMAGE_PATH { get; set; }
        public string g_FNAME { get; set; }
        public string g_MNAME { get; set; }
        public string g_LNAME { get; set; }
        public string g_TITLE { get; set; }
        public string emeR_CONCT_NAME { get; set; }
        public string emeR_CONCT_MOBILE { get; set; }
        public string emeR_CONCT_RELATION { get; set; }
        public string fK_COUNTRY_CODE_EMP { get; set; }
        public string fK_COUNTRY_CODE_BANK { get; set; }
        public string ptype { get; set; }

    }
public class M_EMPMAS_Dto
    {
    public M_EMPMAS m_empmas { get; set; }
    public List<M_EMPMAS> m_empmas_list { get; set; }
}


public class M_EMPMASTER
	{
		public int ID { get; set; }
		public string PK_EMPCODE { get; set; }
		public string TITLE { get; set; }
		public string FNAME { get; set; }
		public string MNAME { get; set; }
		public string LNAME { get; set; }
	}

   
}
	
