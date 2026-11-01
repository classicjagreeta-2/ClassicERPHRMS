using HRMS.Models.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_LEAVE_APPLICATION
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }

        public string DOCNO { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]

        public DateTime APPLY_DATE { get; set; }

        public string FK_EMPCODE { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FROM_DATE {  get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime TO_DATE { get; set; }


        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime LEAVE_DATE { get ; set; }

        public string FK_ATTEND_CODE { get; set; }

        public string FK_REASON_CODE { get; set; }

        public string APPROVED {  get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? APP_DATE { get; set; } // Approved Date

        public string APP_USER { get; set; } //User who approved 

        public string APP_TERMINAL_ID { get; set; }

        public string APP_OS_USER { get; set; }

        public string USER_NAME { get; set; }= Utility.SessionHelper.GetUser().getUserName;
        public DateTime USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime? USER_ENTDT_NEW { get; set; }
         public string OS_USER { get; set; }= "Laptop";
          public string TERMINAL_ID { get; set; } =  System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; }

        public decimal? SESSION {  get; set; }
        public decimal? NO_OF_DAYS {  get; set; }
        public string REMARKS {  get; set; }
        public string PTYPE { get; set; } = "S";

        public string EmpName { get; set; }
        public string Reason { get; set; }
        public string Attend { get; set; }

    }
    public class T_Leave_App : IEquatable<T_Leave_App>
    {
        public string FK_EMPCODE { get; set; }

        public string REMARKS { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FROM_DATE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime TO_DATE { get; set; }
        public string FK_REASON_CODE { get; set; }
        public string FK_ATTEND_CODE { get; set; }
        public decimal? NO_OF_DAYS { get; set; }
        public string EmpName { get; set; }
        public string Reason { get; set; }
        public string Attend { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]

        public DateTime? APPLY_DATE { get; set; }
        public string APPROVED { get; set; }
        //public string color { get; set; }
        //public bool Equals(T_Leave_App other)
        //{
        //    if (other == null) return false;
        //    return FK_EMPCODE == other.FK_EMPCODE
        //        && APPLY_DATE == other.APPLY_DATE
        //        && REMARKS == other.REMARKS
        //        && FROM_DATE == other.FROM_DATE
        //        && TO_DATE == other.TO_DATE
        //        && FK_REASON_CODE == other.FK_REASON_CODE
        //        && FK_ATTEND_CODE == other.FK_ATTEND_CODE
        //        && NO_OF_DAYS == other.NO_OF_DAYS
        //        && EmpName == other.EmpName
        //        && Reason == other.Reason
        //        && Attend == other.Attend
        //        && APPROVED == other.APPROVED;
        //}
        public bool Equals(T_Leave_App other)
        {
            if (ReferenceEquals(other, null)) return false;
            return FK_EMPCODE == other.FK_EMPCODE
                && APPLY_DATE == other.APPLY_DATE
                && REMARKS == other.REMARKS
                && FROM_DATE == other.FROM_DATE
                && TO_DATE == other.TO_DATE
                && FK_REASON_CODE == other.FK_REASON_CODE
                && FK_ATTEND_CODE == other.FK_ATTEND_CODE
                && NO_OF_DAYS == other.NO_OF_DAYS
                && EmpName == other.EmpName
                && Reason == other.Reason
                && Attend == other.Attend
                && APPROVED == other.APPROVED;
        }

        //public override int GetHashCode()
        //{
        //    return FK_EMPCODE.GetHashCode() ^ APPLY_DATE.GetHashCode() ^ REMARKS.GetHashCode() ^ FROM_DATE.GetHashCode() ^ TO_DATE.GetHashCode() ^ FK_REASON_CODE.GetHashCode() ^ FK_ATTEND_CODE.GetHashCode() ^ NO_OF_DAYS.GetHashCode() ^ EmpName.GetHashCode() ^ Reason.GetHashCode() ^ Attend.GetHashCode() ^ APPROVED.GetHashCode();
        //}
        public override int GetHashCode()
        {
            return 0; // FK_EMPCODE.GetHashCode() ^ APPLY_DATE.GetHashCode() ^ REMARKS.GetHashCode() ^ FROM_DATE.GetHashCode() ^ TO_DATE.GetHashCode() ^ FK_REASON_CODE.GetHashCode() ^ FK_ATTEND_CODE.GetHashCode() ^ NO_OF_DAYS.GetHashCode() ^ EmpName.GetHashCode() ^ Reason.GetHashCode() ^ Attend.GetHashCode() ^ APPROVED.GetHashCode();
        }
        public class T_LEAVE_APPLICATION_Dto
        {
            //public List<T_LEAVE_APPLICATION> t_leaveapplication { get; set; }
            public T_LEAVE_APPLICATION t_leaveapplication { get; set; }
        }


        public class MyViewModel
        {
            public string[] SelectedIds { get; set; }
            public IEnumerable<M_UNIT> Items { get; set; }
        }




    }
}