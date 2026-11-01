using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HRMS.Models.Master
{
    public class M_LOCATION
    {
        public string Loca { get; set; } = (string)HttpContext.Current.Session["loca"];

        public string unit { get; set; }

        public int ID { get; set; }

        public string PK_LOCATION_CODE { get; set; }

        public string DESCRIPTION { get; set; }

        public string ADD1 { get; set; }


        public string ADD2 { get; set; }

        public string CITY { get; set; }

        public string PIN { get; set; }


        public int FK_STATE_CODE { get; set; }

        public string STATE_CODE { get; set; }

        public string EMAIL_ID { get; set; }

        public string PHONE { get; set; }

        public string MOBILE { get; set; }

        public string user_name { get; set; }

        public DateTime? user_entdt { get; set; }

        public string user_name_new { get; set; }

        public DateTime? user_entdt_new { get; set; }

        public string os_user { get; set; }

        public string terminal_id { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

        public string dtag { get; set; }




    }
}