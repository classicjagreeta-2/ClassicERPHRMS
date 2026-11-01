using HRMS.Models.Master;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HRMS.Models.TRN
{
    public class T_DAILY_ATTEND
    {
        public string LOCA { get; set; } = (string)HttpContext.Current.Session["loca"];
        public string UNIT { get; set; }
        public int ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public DateTime? EDATE { get; set; }
        public string FK_SHIFT_CODE { get; set; }
        public string FK_EMPCODE { get; set; }
        public string FK_ATTEND_CODE { get; set; }
        public string IN_TIME { get; set; }
        public string OUT_TIME { get; set; }
        public decimal? TOT_HOURS_WORKED { get; set; }
        public decimal? LATE_TIME { get; set; }
        public decimal? ATTEND { get; set; }
        public decimal? LEAVE { get; set; }
        public decimal? HOLIDAY { get; set; }
        public decimal? ABSENT { get; set; }
        public decimal? WEEKLY_OFF { get; set; }
        public string REMARKS { get; set; } = "";
        public string USER_NAME { get; set; } = Utility.SessionHelper.GetUser().getUserName;
        public DateTime? USER_ENTDT { get; set; } = System.DateTime.Now;
        public string USER_NAME_NEW { get; set; }
        public DateTime? USER_ENTDT_NEW { get; set; }
        public string OS_USER { get; set; } = "Laptop";
        public string TERMINAL_ID { get; set; } = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        public string DTAG { get; set; } = "";
        public string FK_MONTHCODE { get; set; }
        public string PTYPE { get; set; } = "S";
        public string Attends { get; set; }
        public string reason { get; set; }
        public string _time { get; set; }
        public string _name { get; set; }
        public string _empCode { get; set; }
    }
    public class T_DAILY_ATTEND_Dto
    {
        public List<T_DAILY_ATTEND> t_dailyattend { get; set; }
    }

    public class Daily_Attend_ExcelUpload
    {
        [JsonProperty("Employee ID")]
        public string EmployeeID { get; set; }

        [JsonProperty("First Name")]
        public string FirstName { get; set; }
        public string Department { get; set; }
        public string Date { get; set; }
        public string Timetable { get; set; }
        public string Duration { get; set; }

        [JsonProperty("Clock In")]
        public string ClockIn { get; set; }

        [JsonProperty("Clock Out")]
        public string ClockOut { get; set; }

        [JsonProperty("Actual WT")]
        public string ActualWT { get; set; }

        [JsonProperty("Total OT")]
        public string TotalOT { get; set; }

        [JsonProperty("Normal OT")]
        public string NormalOT { get; set; }

        [JsonProperty("Week Off OT")]
        public string WeekOffOT { get; set; }

        [JsonProperty("Holiday OT")]
        public string HolidayOT { get; set; }

        [JsonProperty("Total WT")]
        public string TotalWT { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}