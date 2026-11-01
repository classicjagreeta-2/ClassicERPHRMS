using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace HRMS.Models.TRN
{
    public class T_GRATUITY
    {
        public virtual string LOCA { get; set; } = Utility.SessionHelper.GetUser().Loca;
        public virtual string UNIT { get; set; }
        public virtual int ID { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public virtual DateTime EFF_DATE { get; set; }
        public virtual string FK_UNIT_CODE { get; set; }
        public virtual string FK_MONTHCODE { get; set; }
        public virtual string FK_EMPCODE { get; set; }
        public virtual string FK_CAT_CODE { get; set; }
        public virtual string FK_EMP_TYPE { get; set; }
        public virtual string FK_DEPT_CODE { get; set; }
        public virtual string FK_SUB_DEPT_CODE { get; set; }
        public virtual string FK_GRADE_CODE { get; set; }
        public virtual string FK_DESG_CODE { get; set; }
        public virtual string FK_COST_CODE { get; set; }
        public virtual string FK_CURR_CODE { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public virtual DateTime DATEOFBIRTH { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public virtual DateTime DATEOFJOINING { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyy-MM-dd}")]
        public virtual DateTime? DATEOFLEAVING { get; set; }
        public virtual decimal LAST_BASIC { get; set; }
        public virtual decimal LAST_DA { get; set; }
        public virtual decimal LAST_VDA { get; set; }
        public virtual decimal SERVICE_YEAR { get; set; }
        public virtual decimal GRATUITY_DAYS { get; set; }
        public virtual decimal GRATUITY_RATE { get; set; }
        public virtual decimal GRATUITY_AMOUNT { get; set; }
        public virtual string GRATUITY_TYPE { get; set; }
        public virtual string USER_NAME { get; set; }
        public virtual DateTime USER_ENTDT { get; set; }
        public virtual string USER_NAME_NEW { get; set; }
        public virtual DateTime? USER_ENTDT_NEW { get; set; }
        public virtual string OS_USER { get; set; }
        public virtual string TERMINAL_ID { get; set; }
        public virtual string DTAG { get; set; }
    }
}