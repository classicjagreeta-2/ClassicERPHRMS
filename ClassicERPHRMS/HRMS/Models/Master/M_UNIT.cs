using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_UNIT
    {
        public string loca { get; set; } = (string)HttpContext.Current.Session["loca"];
        public int id { get; set; }

        public string code { get; set; }
        public string scode { get; set; }
        public string dire { get; set; }
        public string name { get; set; }
        public string coname { get; set; }
        public string adD1 { get; set; }
        public string adD2 { get; set; }
        public string cstno { get; set; }
        public DateTime? cstdt { get; set; }
        public string lstno { get; set; }
        public DateTime? lstdt { get; set; }
        public string eccno { get; set; }
        public string range { get; set; }
        public string div { get; set; }
        public string ceregdno { get; set; }
        public string plano { get; set; }
        public string unsrtempt { get; set; }
        public string staxcd { get; set; }
        public string simb { get; set; }
        public string autotrns { get; set; }
        public string autotrnsf { get; set; }
        public string autobl { get; set; }
        public string frt { get; set; }
        public string disc { get; set; }
        public string vat { get; set; }
        public string multI_NO { get; set; }
        public string sloc { get; set; }
        public string planT_CODE { get; set; }
        public string cR_MANG { get; set; }
        public string routE_JUMP { get; set; }
        public string t_UNIT { get; set; }
        public string maiN_UNIT { get; set; }
        public string exps { get; set; }
        public string c_FREE_DS { get; set; }
        public string mR_F_ENT { get; set; }
        public string pU_F_ENT { get; set; }
        public string sL_ORDER { get; set; }
        public string sL_ORDER_APP { get; set; }
        public string depO_SCHEMA_NAME { get; set; }
        public string depO_UNIT1 { get; set; }
        public string depO_UNIT2 { get; set; }
        public string region { get; set; }
        public string proD_EMPTY_CHECK { get; set; }
        public string mfG_UNIT { get; set; }
        public string ciN_NO { get; set; }
        public string reG_ADD1 { get; set; }
        public string reG_ADD2 { get; set; }
        public string reG_ADD3 { get; set; }
        public string reG_CITY { get; set; }
        public string reG_PIN { get; set; }

        //[Required(ErrorMessage = "Email Id is required.")]
        //[RegularExpression("/^[^\\]+[^\\]+\\.[^\\]+$/", ErrorMessage = "Invalid Email Id.")]
        public string emaiL_ID { get; set; }
        public string website { get; set; }
        public string state { get; set; }
        public string gsT_NO { get; set; }
        public string paN_NO { get; set; }
        public string inV_FMT { get; set; }
        public string fssaI_NO { get; set; }
        public int? nO_OF_COPY { get; set; }
        public string activE_TAG { get; set; }
        public string city { get; set; }
        //[Required(ErrorMessage = "Pin Code is required.")]
        [RegularExpression("^[1-9]{1}[0-9]{2}\\s{0,1}[0-9]{3}$", ErrorMessage = "Invalid pin code.")]
        public string pin { get; set; }

        //[Required(ErrorMessage = "fax Number is required.")]
        [RegularExpression("^([0-10]{11})$", ErrorMessage = "Invalid fax Number.")]
        public string fax { get; set; }

        //[Required(ErrorMessage = "Mobile Number is required.")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string tel { get; set; }
        public string commitionerate { get; set; }
        public decimal? claiM_DISC_PER { get; set; }
        public string logO_PATH { get; set; }
        public DateTime? useR_ENTDT { get; set; } = System.DateTime.Now;
        public string useR_NAME_NEW { get; set; }
        public DateTime? useR_ENTDT_NEW { get; set; }
        public string useR_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public string oS_USER { get; set; }
        public string terminaL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string dtag { get; set; }

        public string unit { get; set; }
        public string ptype { get; set; }
    }
    public class M_UNIT_DTO
    {
        public M_UNIT m_unit { get; set; }
    }


}