
using HRMS.Models;
using HRMS.Models.Master;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;
using HRMS.Utility;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using HRMS.Models.TRN;
using static HRMS.Models.TRN.T_MONTHLY_ATTEND;
using System.Drawing;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using static iTextSharp.text.pdf.AcroFields;
using Newtonsoft.Json.Linq;

namespace HRMS.Controllers.Import
{
    public class ImportController : Controller
    {
        // GET: Import
        public ActionResult Employee()
        {
            return View();          
        }

        [HttpPost]
        public async Task<ActionResult> Employee(HttpPostedFileBase fileUpload)
        {
            List<EMPLOYEE_IMPORT_EXCEL> employee_import_excel = new List<EMPLOYEE_IMPORT_EXCEL>();
            if (ModelState.IsValid)
            {
                var request = Request.Files["FileUpload"];

                if (fileUpload == null)
                {
                    ModelState.AddModelError("FileUpload2", "Please Select a file ");
                }
                else
                {
                    ModelState.Clear(); 
                    var dt = ImportExcelOrCSV();
                    var collection = JsonConvert.DeserializeObject<List<EMPLOYEE_IMPORT_EXCEL>>(dt);
                    var dunit = collection.Select(x => x.UNIT);                  
                    var filterCollection = collection
                      .Where(b => !string.IsNullOrEmpty(b.EMPCODE))
                      .Select(b => new { b.EMPCODE, b.GENERAL_SHIFT, b.STAFF_WORKER, b.COST_CENTRE, b.STATE_CODE, b.DESIG, b.DEPT_CODE, b.CATE_CODE, b.GRADE_CODE }).ToList();
                    var M_SHIFT = await Services.GetAsync<List<M_SHIFT>>("/api/Shift/GetAll");
                    var M_EMP_TYPE = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
                    var M_COST_CENTRE = await Services.GetAsync<List<M_COST_CENTRE>>("/api/CostCentre/GetAll");
                    var M_STATE = await Services.GetAsync<List<M_STATE>>("/api/State/GetAll");
                    var M_DESIGNATION = await Services.GetAsync<List<M_DESIGNATION>>("/api/Designation/GetAll");
                    var M_DEPARTMENT = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
                    var M_CATEGORY = await Services.GetAsync<List<M_CATEGORY>>("/api/Category/GetAll");
                    var M_GRADE = await Services.GetAsync<List<M_GRADE>>("/api/Grade/GetAll");
                    var EmpCodeList = filterCollection.Select(x => x.EMPCODE).ToList();
                    var empCodeArray = string.Join(",", EmpCodeList);
                    var duplicateEmpCode = await Services.GetAsync<List<M_EMPMAS>>(string.Format("/api/Employee/CheckAllEmpCode?EmpCode={0}", empCodeArray));
                    var missingShifts = filterCollection
                        .Where(fc => !M_SHIFT.Data.Any(ds => ds.pK_SHIFT_CODE.Contains(fc.GENERAL_SHIFT)))
                        .Select(fc => new { fc.EMPCODE, fc.GENERAL_SHIFT })
                        .ToList();

                    var missingEmpTypes = filterCollection
                        .Where(fc => !M_EMP_TYPE.Data.Any(det => det.pK_CODE.Contains(fc.STAFF_WORKER)))
                        .Select(fc => new { fc.EMPCODE, fc.STAFF_WORKER })
                        .ToList();

                    var missingCostCentres = filterCollection
                        .Where(fc => !M_COST_CENTRE.Data.Any(dcc => dcc.pK_CODE.Contains(fc.COST_CENTRE)))
                        .Select(fc => new { fc.EMPCODE, fc.COST_CENTRE })
                        .ToList();

                    var missingStates = filterCollection
                        .Where(fc => !M_STATE.Data.Any(ds => ds.pK_STATE_CODE.Contains(fc.STATE_CODE)))
                        .Select(fc => new { fc.EMPCODE, fc.STATE_CODE })
                        .ToList();

                    var missingDepts = filterCollection
                        .Where(fc => !M_DEPARTMENT.Data.Any(dd => dd.pK_DEPT_CODE.Contains(fc.DEPT_CODE)))
                        .Select(fc => new { fc.EMPCODE, fc.DEPT_CODE })
                        .ToList();

                    var missingGrades = filterCollection
                        .Where(fc => !M_GRADE.Data.Any(dg => dg.pK_GRADE_CODE.Contains(fc.GRADE_CODE)))
                        .Select(fc => new { fc.EMPCODE, fc.GRADE_CODE })
                        .ToList();

                    var missingData = filterCollection
                        .Select(fc => new MissingData
                        {
                            EMPCODE = fc.EMPCODE,
                            MissingFields = new List<string>(),
                            DuplicateFields = new List<string>()
                        })
                        .ToList();

                    missingData.ForEach(md =>
                        {
                            if (duplicateEmpCode.Data.Any(ms => ms.pK_EMPCODE == md.EMPCODE)) md.DuplicateFields.Add("EMPLOYEE CODE : "+ duplicateEmpCode.Data.Where(ms => ms.pK_EMPCODE == md.EMPCODE).Select(x=> x.pK_EMPCODE).FirstOrDefault());
                            if (missingShifts.Any(ms => ms.EMPCODE == md.EMPCODE)) md.MissingFields.Add("GENERAL_SHIFT : "+ missingShifts.Where(ms => ms.EMPCODE == md.EMPCODE).Select(x=>x.GENERAL_SHIFT).FirstOrDefault());
                            if (missingEmpTypes.Any(me => me.EMPCODE == md.EMPCODE)) md.MissingFields.Add("STAFF_WORKER : "+ missingEmpTypes.Where(ms => ms.EMPCODE == md.EMPCODE).Select(x => x.STAFF_WORKER).FirstOrDefault());
                            if (missingCostCentres.Any(mc => mc.EMPCODE == md.EMPCODE)) md.MissingFields.Add("COST_CENTRE : "+ missingCostCentres.Where(ms => ms.EMPCODE == md.EMPCODE).Select(x=>x.COST_CENTRE).FirstOrDefault());
                            if (missingStates.Any(ms => ms.EMPCODE == md.EMPCODE)) md.MissingFields.Add("STATE_CODE : "+ missingStates.Where(ms => ms.EMPCODE == md.EMPCODE).Select(c=>c.STATE_CODE).FirstOrDefault());
                            if (missingDepts.Any(mdpt => mdpt.EMPCODE == md.EMPCODE)) md.MissingFields.Add("DEPT_CODE : " + missingDepts.Where(ms => ms.EMPCODE == md.EMPCODE).Select(x => x.DEPT_CODE).FirstOrDefault());
                            if (missingGrades.Any(mg => mg.EMPCODE == md.EMPCODE)) md.MissingFields.Add("GRADE_CODE : " + missingGrades.Where(ms => ms.EMPCODE == md.EMPCODE).Select(x => x.GRADE_CODE).FirstOrDefault());
                        });

                    ViewBag.MissingData = null;
                   
                    
                    //if (missingData != null)
                   
                    if (missingData.Any(md => md.MissingFields.Any() || md.DuplicateFields.Any())) 
                    {
                        ViewBag.MissingData = missingData;
                        TempData["toastrError"] = "Mismatch data found against Empcode. Please check the below list."; 
                        return View(); 
                    }
                    //var M_ENUM_PARA = await Services.GetAsync<List<M_ENUM_PARA>>("/api/EmpType/GetAll");
                    List<M_EMPMAS> employee = new List<M_EMPMAS>();
                    List<M_EMP_LINK_DET> empLink = new List<M_EMP_LINK_DET>();

                    List<M_EMPRATE> empRate = new List<M_EMPRATE>();

                    foreach (var item in collection)
                    {
                        if (item.EMPCODE != null)
                        {
                            M_EMPMAS emp = new M_EMPMAS
                            {
                                pK_EMPCODE = item.EMPCODE,
                                unit = item.LOCATION,
                                title = item.TITLE,
                                fname = item.FNAME,
                                mname = item.MNAME,
                                lname = item.LNAME,
                                pfno = item.PFNO,
                                esino = item.ESINO,
                                pancardno = item.PANCARDNO,
                                passportno = item.PASSPORTNO,
                                passportissuedon = item.PASSPORTISSUEDON,
                                p_RENEWALDATE = item.P_RENEWALDATE,
                                dateofbirth = item.DATEOFBIRTH,
                                dateofjoining = item.DATEOFJOINING,
                                dateofleaving = item.DATEOFLEAVING,
                                lefT_SERVICE = item.LEFT_SERVICE == "Y" ? true : false,
                                transferred = item.TRANSFERRED,
                                transferredto = item.TRANSFERREDTO,
                                bloodgroup = item.BLOODGROUP,
                                perM_HOUSENO = item.PERM_HOUSENO,
                                perM_STREETNAME1 = item.PERM_STREETNAME1,
                                perM_STREETNAME2 = item.PERM_STREETNAME2,
                                perM_CITY = item.PERM_CITY,
                                perM_PINCODE = item.PERM_PINCODE,
                                perM_POSTOFF = item.PERM_POSTOFF,
                                perM_POLICEST = item.PERM_POLICEST,
                                perM_RESTEL1 = item.PERM_RESTEL1,
                                perM_RESTEL2 = item.PERM_RESTEL2,
                                perM_MOBILENO = item.PERM_MOBILENO,
                                temP_HOUSENO = item.TEMP_HOUSENO,
                                temP_STREETNAME1 = item.TEMP_STREETNAME1,
                                temP_STREETNAME2 = item.TEMP_STREETNAME2,
                                temP_CITY = item.TEMP_CITY,
                                temP_PINCODE = item.TEMP_PINCODE,
                                temP_POSTOFF = item.TEMP_POSTOFF,
                                temP_POLICEST = item.TEMP_POLICEST,
                                temP_RESTEL1 = item.TEMP_RESTEL1,
                                temP_RESTEL2 = item.TEMP_RESTEL2,
                                temP_MOBILENO = item.TEMP_MOBILENO,
                                emaiL_ID = item.EMAIL_ID,
                                shorT_NAME = item.SHORT_NAME,
                                pasprT_ISD_FRM = item.PASPRT_ISD_FRM,
                                chQ_CSH = item.CHQ_CSH,
                                aC_NO = item.AC_NO,
                                branch = item.BRANCH,
                                banK_NAME = item.BANK_NAME,
                                sex = item.SEX,
                                fK_SHIFT_CODE = item.SWIFT_CODE,
                                tnno = item.TNNO,
                                pensioN_NO = item.PENSION_NO,
                                acode = item.ACODE,
                                reF_CODE = item.REF_CODE,
                                sacode = item.SACODE,
                                ssubcode = item.SSUBCODE,
                                doJ_EPS = item.DOJ_EPS,
                                doJ_EPF = item.DOJ_EPF,
                                doL_EPS = item.DOL_EPS,
                                doL_EPF = item.DOL_EPF,
                                dog = item.DOG,
                                fK_REASON_CODE = item.REASON_LV,
                                uan = item.UAN,
                                fK_STATE_CODE = item.STATE_CODE,
                                activE_TAG = item.ACTIVE_TAG == "Y" ? true : false,
                                aadhaR_NO = item.AADHAR_NO,
                                leG_CODE = item.LEG_CODE,
                                voteR_ID = item.VOTER_ID,
                                ifsC_CODE = item.IFSC_CODE,
                                swifT_CODE = item.SWIFT_CODE,
                                ibaN_NO = item.IBAN_NO,
                                g_FNAME = item.FNAME,
                                g_MNAME = item.MNAME,
                                g_LNAME = item.LNAME,
                                g_TITLE = item.TITLE,
                                emeR_CONCT_RELATION = item.RELATIONSHIP,
                                ptype = item.STAFF_WORKER,


                            };
                            employee.Add(emp);

                            M_EMP_LINK_DET TABLE2 = new M_EMP_LINK_DET
                            {
                                UNIT = item.LOCATION,
                                FK_UNIT_CODE = item.LOCATION,
                                FK_EMPCODE = item.EMPCODE,
                                FK_COST_CODE = item.COST_CENTRE,
                                APPLY_BONUS = item.BONUS ?? false,
                                APPLY_ESI = item.DEDT_ESI == "Y" ? true : false,
                                APPLY_PF = item.DEDT_PF == "Y" ? true : false,
                                EFF_DATE = item.EDATE,
                                FK_DEPT_CODE = item.DEPT_CODE,
                                FK_SUB_DEPT_CODE = item.SUB_DEPT_CODE,
                                FK_DESG_CODE = item.DESG_CODE,
                                FK_CAT_CODE = item.CATE_CODE1,
                                FK_GRADE_CODE = item.GRADE_CODE,
                                FK_EMP_TYPE = item.STAFF_WORKER,
                                PTYPE = "S",
                                
                            };
                            empLink.Add(TABLE2);
                            M_EMPRATE TABLE3 = new M_EMPRATE
                            {

                                UNIT = item.LOCATION,
                                FK_EMPCODE = item.EMPCODE,
                                EDATE = item.EDATE,
                                BASIC = item.BASIC != null ? item.BASIC : 0m,
                                DA_RS = item.DA_RS  != null ? item.DA_RS :0m,
                                DA_PERCENT = item.DA_PERCENT != null ? item.DA_PERCENT:0m,
                                HRA_RS = item.HRA_RS != null ? item.HRA_RS : 0m,
                                HRA_PERCENT = item.HRA_PERCENT != null ? item.HRA_PERCENT : 0m,
                                VDA_RS = item.VDA_RS != null ? item.VDA_RS : 0,
                                VDA_PERCENT = item.VDA_PERCENT != null ? item.VDA_PERCENT : 0m,
                                PF = item.PF != null ? item.PF : 0m,
                                EPF = item.EPF != null ? item.EPF : 0m,
                                FPF = item.FPF != null ? item.FPF : 0m,
                                ESI_RS = item.ESI_RS != null ? item.ESI_RS : 0m,
                                ESI_PERCENT = item.ESI_PERCENT != null ? item.ESI_PERCENT : 0m,
                                CONVEYANCE_PERCENT = item.CONVEYANCE_PERCENT != null ? item.CONVEYANCE_PERCENT : 0m,
                                CONVEYANCE_RS = item.CONVEYANCE_RS != null ? item.CONVEYANCE_RS : 0m,
                                ESI_LIMIT = item.ESI_LIMIT != null ? item.ESI_LIMIT : 0m,
                                STAFF_WORKER = item.STAFF_WORKER,
                                STATU_PF_LIMIT = item.STATU_PF_LIMIT,



                            };
                            empRate.Add(TABLE3);
                        }
                    }
                    M_EMPMAS_Dto _EMPMAS_Dto = new M_EMPMAS_Dto();
                    _EMPMAS_Dto.m_empmas_list = employee;
                    var empResult = await Services.PostAsync<List<M_EMPMAS_Dto>>("/api/Employee/SaveListAll", _EMPMAS_Dto); //need to make list employee save api

                    M_EMP_LINK_DET_Dto m_EMP_LINK_ = new M_EMP_LINK_DET_Dto();
                    m_EMP_LINK_.m_emplinkdet_list = empLink;
                    var empLinkResult = await Services.PostAsync<List<M_EMP_LINK_DET_Dto>>("/api/EmployeeLinkDet/SaveListAll", m_EMP_LINK_);//need to make list empLinkDet save api

                    m_emp_Dto m_Emp_Dto = new m_emp_Dto();
                    m_Emp_Dto.m_emprate_List = empRate;
                    var empRateResult = await Services.PostAsync<List<m_emp_Dto>>("/api/EmployeeRate/SaveListAll", m_Emp_Dto);//need to make api for list empRate save
                    if (empResult.IsSuccessStatusCode == true && empLinkResult.IsSuccessStatusCode == true && empRateResult.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted for Employee master,LinkMaster & Rate Master ";                        
                    }
                    else
                    {
                        TempData["toastrError"] = "Employee master: " + empResult.StatusCode + " LinkMaster: " + empLinkResult.StatusCode + " Rate Master: " + empRateResult.StatusCode;
                    }
                }
            }

            return View();
        }

        [ActionName("AttendanceMonthly")]
        public async Task<ActionResult> AttendanceMonthly(string Unit = "")
        {
            var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");

            List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            if (Unit != null)
            {
                var result1 = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetByUnit?unit={0}", Lst.FirstOrDefault().Value));
                List<ModelMaster> monthList = result1.Data.Select(x => new ModelMaster
                {
                    Value = x.pK_MONTHCODE,
                    Text = x.pK_MONTHCODE + "-" + DateTime.ParseExact(x.pK_MONTHCODE, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM")
                }).ToList();
                ViewBag.Month = monthList;
            }
            else
            {
                ViewBag.Month = null;
            }
            return View();
        }

        [ActionName("AttendanceMonthly")]
        [HttpPost]
        public async Task<ActionResult> AttendanceMonthly(FormCollection formCollection, HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                var unit = formCollection["UNIT"];
                var monthcode = formCollection["MONTHCODE"];
                if (string.IsNullOrEmpty(monthcode))
                {
                    monthcode = "202409";
                }
                var request = Request.Files["FileUpload"];

                var result1 = await Services.GetAsync<M_MONTH>("/api/Month/GetByMonthCode?search=" + monthcode);

                var asas = (dynamic)null;
                if (string.IsNullOrEmpty(unit) && string.IsNullOrEmpty(monthcode) && request == null)
                {
                    ModelState.AddModelError("Unit", "Please select a UNIT");
                    ModelState.AddModelError("Monthcode", "Please select a MONTH code");
                    ModelState.AddModelError("FileUpload3", "Please Select a file ");
                }
                else
                {
                    asas = ImportExcelOrCSV();
                }
                List<MonthlyAttend> list = new List<MonthlyAttend>();
                list = JsonConvert.DeserializeObject<List<MonthlyAttend>>(asas);
                List<T_MONTHLY_ATTEND> _ATTENDs = new List<T_MONTHLY_ATTEND>();
                List<T_MONTHLY_ATTEND_DAYS_DET> monthly_att = new List<T_MONTHLY_ATTEND_DAYS_DET>();
                List<T_MONTHLY_ATTEND_SHIFT_DET> monthly_shift = new List<T_MONTHLY_ATTEND_SHIFT_DET>();
                foreach (var item in list)
                {
                    T_MONTHLY_ATTEND att = new T_MONTHLY_ATTEND
                    {
                        ID = 0,
                        LOCA = "A",
                        UNIT = unit,
                        FK_MONTHCODE = monthcode,
                        FROM_DATE = result1.Data.froM_PERIOD,
                        TO_DATE = result1.Data.tO_PERIOD,
                        FK_EMPCODE = item.EMP_CODE,
                        PRESENT = item.PRESENTFULL,
                        PAID_HOLIDAY = item.PAID_HOLIDAY ?? 0,
                        UNPAID_LEAVE = item.WITHOUT_PAY ?? 0,
                        WEEK_LEAVE = item.WEEKLY_OFF ?? 0,
                        PAID_LEAVE = item.EL_LEAVE ?? 0,
                        ABSENT = item.LOPDAYS ?? 0,
                        EXTRA_DAYS = item.EXTRA_DAYS ?? 0,
                        TOTAL_DAYS = item.PRESENTFULL + item.PAID_HOLIDAY ?? 0 + item.WITHOUT_PAY ?? 0 + item.WEEKLY_OFF ?? 0 + item.EL_LEAVE ?? 0,
                        LESS_HOUR_WORKED = item.LESS_HOUR_WORKED ?? 0,
                        ARREAR_DAY = 0,
                        ARREAR_MONTH = "",
                        LATE_DAY = 0,
                        LEAVE_ENCASH = 0,
                        PTYPE = "S",
                        PAY_TYPE = "P",
                        USER_NAME = "TEST",
                        USER_ENTDT = DateTime.Now,
                        USER_NAME_NEW = "CORETEAM",
                        USER_ENTDT_NEW = DateTime.Now,
                        OS_USER = "O",
                        TERMINAL_ID = "CLASSIC",
                        DTAG = ""
                    };
                    _ATTENDs.Add(att);
                    T_MONTHLY_ATTEND_DAYS_DET _ATTEND_DAYS_DET = new T_MONTHLY_ATTEND_DAYS_DET
                    {
                        LOCA = "A",
                        UNIT = unit,
                        FK_EMPCODE = item.EMP_CODE,
                        FK_ATTEND_CODE = "P",
                        FK_MONTHCODE = monthcode,
                        FROM_DATE = result1.Data.froM_PERIOD,
                        TO_DATE = result1.Data.tO_PERIOD,
                        DAYS = Math.Floor(item.PRESENTFULL),
                        HOURS = (item.PRESENTFULL - Math.Floor(item.PRESENTFULL)) * 8,    //0m
                        MINS = 0,
                        OT_TYPE = "",
                        PTYPE = "S",
                        PAY_TYPE = "P",
                        ADJ_MONTH = "",
                        USER_NAME = "TEST",
                        USER_ENTDT = DateTime.Now,
                        USER_NAME_NEW = "CORETEAM",
                        USER_ENTDT_NEW = DateTime.Now,
                        OS_USER = "O",
                        TERMINAL_ID = "CLASSIC",
                        DTAG = ""
                    };
                    monthly_att.Add(_ATTEND_DAYS_DET);

                    if (item.OT > 0)
                    {
                        _ATTEND_DAYS_DET = new T_MONTHLY_ATTEND_DAYS_DET
                        {
                            LOCA = "A",
                            UNIT = unit,
                            FK_EMPCODE = item.EMP_CODE,
                            FK_ATTEND_CODE = "IN",
                            FK_MONTHCODE = monthcode,
                            FROM_DATE = result1.Data.froM_PERIOD,
                            TO_DATE = result1.Data.tO_PERIOD,
                            DAYS = Math.Floor(item.OT ?? 0),
                            HOURS = (item.OT ?? 0 - Math.Floor(item.OT ?? 0)) * 8,    //0m
                            MINS = 0,
                            OT_TYPE = "",
                            PTYPE = "S",
                            PAY_TYPE = "P",
                            USER_NAME = "TEST",
                            USER_ENTDT = DateTime.Now,
                            USER_NAME_NEW = "CORETEAM",
                            USER_ENTDT_NEW = DateTime.Now,
                            OS_USER = "O",
                            TERMINAL_ID = "CLASSIC",
                            DTAG = ""
                        };
                        monthly_att.Add(_ATTEND_DAYS_DET);
                    }

                    if (item.WEEKLY_OFF > 0)
                    {
                        _ATTEND_DAYS_DET = new T_MONTHLY_ATTEND_DAYS_DET
                        {
                            LOCA = "A",
                            UNIT = unit,
                            FK_EMPCODE = item.EMP_CODE,
                            FK_ATTEND_CODE = "W",
                            FK_MONTHCODE = monthcode,
                            FROM_DATE = result1.Data.froM_PERIOD,
                            TO_DATE = result1.Data.tO_PERIOD,
                            DAYS = item.WEEKLY_OFF ?? 0,
                            HOURS = 0m,
                            MINS = 0,
                            OT_TYPE = "",
                            PTYPE = "S",
                            PAY_TYPE = "P",
                            USER_NAME = "TEST",
                            USER_ENTDT = DateTime.Now,
                            USER_NAME_NEW = "CORETEAM",
                            USER_ENTDT_NEW = DateTime.Now,
                            OS_USER = "O",
                            TERMINAL_ID = "CLASSIC",
                            DTAG = ""
                        };
                        monthly_att.Add(_ATTEND_DAYS_DET);
                    }
                    if (item.PAID_HOLIDAY > 0)
                    {
                        _ATTEND_DAYS_DET = new T_MONTHLY_ATTEND_DAYS_DET
                        {
                            LOCA = "A",
                            UNIT = unit,
                            FK_EMPCODE = item.EMP_CODE,
                            FK_ATTEND_CODE = "PH",
                            FK_MONTHCODE = monthcode,
                            FROM_DATE = result1.Data.froM_PERIOD,
                            TO_DATE = result1.Data.tO_PERIOD,
                            DAYS = Math.Floor(item.PAID_HOLIDAY ?? 0),
                            HOURS = (item.PAID_HOLIDAY ?? 0 - Math.Floor(item.PAID_HOLIDAY ?? 0)) * 8,    //0m
                            MINS = 0,
                            OT_TYPE = "",
                            PTYPE = "S",
                            PAY_TYPE = "P",
                            USER_NAME = "TEST",
                            USER_ENTDT = DateTime.Now,
                            USER_NAME_NEW = "CORETEAM",
                            USER_ENTDT_NEW = DateTime.Now,
                            OS_USER = "O",
                            TERMINAL_ID = "CLASSIC",
                            DTAG = ""
                        };
                        monthly_att.Add(_ATTEND_DAYS_DET);
                    }
                    if (item.EL_LEAVE > 0)
                    {
                        _ATTEND_DAYS_DET = new T_MONTHLY_ATTEND_DAYS_DET
                        {
                            LOCA = "A",
                            UNIT = unit,
                            FK_EMPCODE = item.EMP_CODE,
                            FK_ATTEND_CODE = "LD",
                            FK_MONTHCODE = monthcode,
                            FROM_DATE = result1.Data.froM_PERIOD,
                            TO_DATE = result1.Data.tO_PERIOD,
                            DAYS = Math.Floor(item.EL_LEAVE ?? 0),
                            HOURS = (item.EL_LEAVE ?? 0 - Math.Floor(item.EL_LEAVE ?? 0)) * 8,    //0m
                            MINS = 0,
                            OT_TYPE = "",
                            PTYPE = "S",
                            PAY_TYPE = "P",
                            USER_NAME = "TEST",
                            USER_ENTDT = DateTime.Now,
                            USER_NAME_NEW = "CORETEAM",
                            USER_ENTDT_NEW = DateTime.Now,
                            OS_USER = "O",
                            TERMINAL_ID = "CLASSIC",
                            DTAG = ""
                        };
                        monthly_att.Add(_ATTEND_DAYS_DET);
                    }
                    T_MONTHLY_ATTEND_SHIFT_DET _ATTEND_SHIFT_DET = new T_MONTHLY_ATTEND_SHIFT_DET
                    {
                        LOCA = "A",
                        UNIT = unit,
                        FK_EMPCODE = item.EMP_CODE,
                        FK_SHIFT_CODE = item.SHIFT,
                        FK_MONTHCODE = monthcode,
                        FROM_DATE = result1.Data.froM_PERIOD,
                        TO_DATE = result1.Data.tO_PERIOD,
                        DAYS = Math.Floor(item.PRESENTFULL),
                        HOURS = (item.PRESENTFULL - Math.Floor(item.PRESENTFULL)) * 8,
                        MINS = 0,
                        PTYPE = "S",
                        PAY_TYPE = "P",
                        USER_NAME = "TEST",
                        USER_ENTDT = DateTime.Now,
                        USER_NAME_NEW = "CORETEAM",
                        USER_ENTDT_NEW = DateTime.Now,
                        OS_USER = "O",
                        TERMINAL_ID = "CLASSIC",
                        DTAG = ""
                    };
                    monthly_shift.Add(_ATTEND_SHIFT_DET);
                }

                T_MONTHLY_ATTEND_Dto _Dto = new T_MONTHLY_ATTEND_Dto();
                _Dto.t_monthly_attend = (_ATTENDs);
                _Dto.t_monthly_attend_days = (monthly_att);
                _Dto.t_monthly_attend_shft = (monthly_shift);

                var responce = await Services.PostAsync<List<object>>("/api/Attend/MonthlyAttendanceUploadNSave", _Dto);
                if (responce.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = responce.Message;
                }
            }
            return View();
        }

        public async Task<ActionResult> AttendanceDaily()
        {
            var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AttendanceDaily(FormCollection formCollection)
        {
            var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;

            //var dt = ImportExcelOrCSV();
            if (ModelState.IsValid)
            {
                var unit = formCollection["UNIT"];
                var DATE = formCollection["EDATE"];
                var Option = formCollection["Option"];
                var request = Request.Files["fileUpload"];
                var monthCode = DateTime.Parse(DATE).ToString("yyyy") + DateTime.Parse(DATE).ToString("MM");

                if (string.IsNullOrEmpty(unit) && string.IsNullOrEmpty(DATE) && string.IsNullOrEmpty(Option) && request == null)
                {
                    ModelState.AddModelError("Unit", "Please select a UNIT");
                    ModelState.AddModelError("Date", "Please select a DATE ");
                    ModelState.AddModelError("option", "Please select ONE Option ");
                    ModelState.AddModelError("fileUpload", "Please Select a file ");
                }
                else
                {
                    var dt = ImportExcelOrCSV();
                    var collection1 = JsonConvert.DeserializeObject<List<object>>(dt);
                    var collection = JsonConvert.DeserializeObject<List<Daily_Attend_ExcelUpload>>(dt);

                    var legacycode = collection.DistinctBy(e => e.EmployeeID).Select(e => new Daily_Attend_ExcelUpload { EmployeeID = e.EmployeeID }).ToList();
                    var getEmpCode = await Services.GetAsync<List<M_EMPMAS>>("/api/Employee/GetAllEmpCode");

                    //var ss = getEmpCode.Data.Where(c => !legacycode.Any(cc => c.leG_CODE == cc.EmployeeID)).ToList();
                    var ssreverse = legacycode.Where(c => !getEmpCode.Data.Any(cc => c.EmployeeID == cc.leG_CODE)).ToList();
                    //if (!ssreverse.Any())
                    //{
                    List<T_DAILY_ATTEND> dailyAttendList = new List<T_DAILY_ATTEND>();
                    foreach (var item in collection)
                    {
                        if (item.EmployeeID != null)
                        {
                            T_DAILY_ATTEND da = new T_DAILY_ATTEND
                            {
                                UNIT = unit,
                                FK_EMPCODE = item.EmployeeID,
                                EDATE = DateTime.Parse(item.Date),
                                FK_SHIFT_CODE = item.Timetable.Substring(0, 1),
                                TOT_HOURS_WORKED = 0m, // format coming datetime format in String form "8:30" but db has decimal field
                                IN_TIME = item.ClockIn,
                                OUT_TIME = item.ClockOut,
                                LATE_TIME = 0,
                                ATTEND = 0,
                                HOLIDAY = 0,
                                LEAVE = 0,
                                ABSENT = 0,
                                REMARKS = "",
                                WEEKLY_OFF = 0,
                                FK_MONTHCODE = DateTime.Parse(item.Date).ToString("yyyy") + DateTime.Parse(item.Date).ToString("MM"),
                                FK_ATTEND_CODE = ExtractBracketValue(item.Status),
                            };
                            dailyAttendList.Add(da);
                        }
                    }
                    foreach (var item in dailyAttendList)
                    {
                        if (item.FK_ATTEND_CODE == "P")
                        {
                            item.ATTEND = 1;
                            //LATE_TIME = Convert.ToDecimal(employee.LATE_TIME);
                        }
                        else if (item.FK_ATTEND_CODE == "HF")
                        {
                            item.ATTEND = (Decimal)0.50;
                            //LATE_TIME = Convert.ToDecimal(LATE_TIME);
                        }
                        else if (item.FK_ATTEND_CODE == "S" || item.FK_ATTEND_CODE == "M" || item.FK_ATTEND_CODE == "PH" || item.FK_ATTEND_CODE == "L")
                        {
                            item.LEAVE = 1;
                        }
                        else if (item.FK_ATTEND_CODE == "W")
                        {
                            item.WEEKLY_OFF = 1;
                        }
                        else if (item.FK_ATTEND_CODE == "A")
                        {
                            item.ABSENT = 1;
                        }
                    }
                    T_DAILY_ATTEND_Dto _Dto = new T_DAILY_ATTEND_Dto();
                    _Dto.t_dailyattend = dailyAttendList;
                    var result = await Services.PostAsync<List<T_DAILY_ATTEND_Dto>>("/api/DailyAttend/SaveOrUpdateAll", _Dto);
                    if (result.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = result.Message;
                    }
                    //}
                    //ViewBag.Data = ssreverse;
                    ViewBag.Data = ConvertListToDataTable(ssreverse);

                }

            }
            return View();
        }

        public static string ExtractBracketValue(string input)
        {
            var regex = new Regex(@"\((.*?)\)");
            var match = regex.Match(input);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }


        private string ImportExcelOrCSV()
        {
            DataTable dt = new DataTable();
            var request = Request.Files["FileUpload"];
            if (request != null)
            {
                if (request.ContentLength > 0)
                {
                    string extension = Path.GetExtension(request.FileName).ToLower();
                    string query = null;
                    string connString = "";

                    string[] validFileTypes = { ".xls", ".xlsx", ".csv" };
                    string path1 = Path.Combine(Server.MapPath("~/Content/Uploads"), request.FileName);

                    if (!Directory.Exists(Server.MapPath("~/Content/Uploads")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Content/Uploads"));
                    }

                    if (validFileTypes.Contains(extension))
                    {
                        if (System.IO.File.Exists(path1))
                        {
                            System.IO.File.Delete(path1);
                        }

                        request.SaveAs(path1);

                        if (extension == ".csv")
                        {
                            dt = Utilities.ConvertCSVtoDataTable(path1);
                            ViewBag.Data = dt;
                        }
                        else if (extension.Trim() == ".xls")
                        {
                            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                            dt = Utilities.ConvertXSLXtoDataTable(path1, connString);
                            ViewBag.Data = dt;
                        }
                        else if (extension.Trim() == ".xlsx")
                        {
                            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            dt = Utilities.ConvertXSLXtoDataTable(path1, connString);
                            ViewBag.Data = dt;
                        }

                        //TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = "Please Upload Files in .xls, .xlsx or .csv format";
                    }
                }
            }
            else
            {
                TempData["toastrError"] = "Please Upload Files in .xls, .xlsx or .csv format";
            }
            var json = JsonConvert.SerializeObject(dt);
            

            // Parse JSON string into a JArray
            JArray dataArray = JArray.Parse(json);

            // Iterate over each object in the array
            foreach (var item in dataArray)
            {
                foreach (var property in item.Children<JProperty>())
                {
                    if (property.Value.Type == JTokenType.Float)
                    {
                        // Get the float value and check if it can be converted to an integer
                        double doubleValue = property.Value.Value<double>();
                        if (doubleValue == Math.Floor(doubleValue))
                        {
                            property.Value = Convert.ToInt32(doubleValue);
                        }
                    }
                }
            }

            // Serialize back to JSON
            string cleanedJson = dataArray.ToString(Formatting.Indented);
            return cleanedJson;
        }
        private DataTable ConvertListToDataTable(List<Daily_Attend_ExcelUpload> list)
        {
            var table = new DataTable();
            // Add columns to the DataTable based on the properties of Daily_Attend_ExcelUpload
            table.Columns.Add("EmployeeID"); // Example column, add others as needed
                                             // Add rows to the DataTable
            foreach (var item in list)
            {
                var row = table.NewRow();
                row["EmployeeID"] = item.EmployeeID;
                // Add other properties here
                table.Rows.Add(row);
            }
            return table;
        }

        public async Task<ActionResult> UploadPF()
        {
            var EmpTypeList = await Services.GetAsync<List<M_EMP_TYPE>>("/api/EmpType/GetAll");
            List<ModelMaster> EmpTypeLst = EmpTypeList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.description
            }).ToList();
            ViewBag.EmpType = EmpTypeLst;
            var DepartmentList = await Services.GetAsync<List<M_DEPARTMENT>>("/api/Department/GetAll");
            List<ModelMaster> DepartmentListLst = DepartmentList.Data.Select(x => new ModelMaster
            {
                Value = x.pK_DEPT_CODE,
                Text = x.description
            }).ToList();
            ViewBag.Department = DepartmentListLst;
            return View();
        }
    }
}