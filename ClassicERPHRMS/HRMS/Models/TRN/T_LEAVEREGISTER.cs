using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_LEAVEREGISTER
    {
        public string FK_EMPCODE { get; set; }
        public string FK_MONTHCODE { get; set; }
        public DateTime FROM_PERIOD { get; set; }
        public DateTime TO_PERIOD { get; set; }
        public string NAME { get; set; }
        //public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string TITLE { get; set; }
        public string FNAME { get; set; }
        public string MNAME { get; set; }
        public string LNAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string FK_ATTEND_CODE { get; set; }
        public string LEAVE_DESC { get; set; }
        public string DESG_DESCN { get; set; }
        public string FK_COST_CODE { get; set; }
        public string DESCN { get; set; }
        public decimal OPENING { get; set; }
        public decimal ALLOTED { get; set; }
        public decimal DAYS { get; set; }
        public decimal HOURS { get; set; }
        public decimal MINS { get; set; }
        public string LOGO_PATH { get; set; }
    }
 
}