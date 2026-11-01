using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HRMS.Models.Master;
using HRMS.Models.TRN;

namespace HRMS_VIEWMODEL24
{
    public class Payroll_Common
    {
        public class ModuleDailyAttendance
        {
            public bool BY_DataRange { get; set; }
            public bool BY_MonthRange { get; set; }
            public bool BY_SingleFYYear { get; set; }
            public bool BY_SingleMonth { get; set; }
            public string EmpCode { get; set; }
            public string MonthCode { get; set; }

        }
        public class Payroll_T_LEAVE_OP_DECLARE //: //SearchModelExtra
        {
            public string EmpCode { get; set; }
            public string AttendCode { get; set; }

        }
        public class Payroll_T_DailyAttendance 
        {

            public List<T_DAILY_ATTEND> t_DAILY_ATTENDs { get; set; }
            public string EmpCode { get; set; }


        }
        public class Payroll_T_LeaveApplication
        {
            public List<T_LEAVE_APPLICATION> t_LEAVE_APPLICATION { get; set; }
        }
        public class Payroll_M_Holiday
        {
            public List<M_HOLIDAY> m_HOLIDAYs { get; set; }
        }
        public class Payroll_T_Attend
        {
            public List<T_ATTEND> t_Attend { get; set; }
        }
        public class USER_SCHEMA_LINK
        {
            public virtual int ID { get; set; }
            public virtual string LOCA { get; set; }
            public virtual string UNIT { get; set; }
            public virtual string USER_ORA { get; set; }
            public virtual string USR_ID { get; set; }
            public virtual string MODULE_NAME { get; set; }
            public virtual string USER_ENT { get; set; }
            public virtual DateTime ENTDT { get; set; }
            public virtual string TIME_ORA { get; set; }
            public virtual string USER_NEW { get; set; }
            public virtual DateTime ENTDT_NEW { get; set; }
            public virtual string TIME_NEW { get; set; }
            public virtual string DTAG { get; set; }
            public virtual string MACHINE_NO { get; set; }
            public virtual int API_ALLOW { get; set; }
        }
        public class USER_SCHEMA_LINK_Dto
        {
            public USER_SCHEMA_LINK uSER_SCHEMA_LINK { get; set; }
        }
    }
}