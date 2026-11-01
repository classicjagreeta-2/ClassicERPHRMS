using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using static HRMS.Models.Model_Common;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Reflection.Emit;
using HRMS.Utility;
using System.Data;
using Org.BouncyCastle.Ocsp;
using NLog.Fluent;
///need to enable 32 bit in iis application tool for running crystal report
namespace HRMS.Controllers.Payroll
{
    public class PayslipController : Controller
    {
        
        // GET: Payslip
        public async Task<ActionResult> Payslip(string EMPCODE,string select_employee, string MonthCode, string Unit = "", string sortOrder = "", string currentFilter = "", string searchString = "", int? page = 1, int pageSize = 15)
        {
            if (EMPCODE == "Select")
            {
                EMPCODE = "";
            }
            var result = await Services.GetAsync<PageModel<PaySlipList>>(string.Format("/api/Payimage/GetPaySlipByPage?Monthcode={0}&unit={1}&paytype=P&Ptype=S&page={2}&pageSize={3}&search={4}", MonthCode, Unit, page, pageSize, searchString));
            ViewBag.CurrentFilter = searchString;
            switch (sortOrder)
            {

            }
            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);
            var usersAsIPagedList = new StaticPagedList<PaySlipList>(result.Data.value.results, pageNumber, pageSize, result.Data.value.rowCount);

            var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll"); //28.08.2025 12:27PM            
            List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name
            }).ToList();
            ViewBag.Unit = Lst;
            ViewBag.Value = Lst;
            ViewBag.Text = Lst;

            var m_month = new List<M_MONTH>();
            if (!string.IsNullOrEmpty(Unit))
            {
                var result1 = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetByUnit?unit={0}", Unit));
                m_month = result1.Data;
            }
            else
            {
                var result1 = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetAll"));
                m_month = result1.Data;
            }

            if (m_month.Count > 0)
            {
                var dresult1 = m_month.Select(u => u.pK_MONTHCODE).Distinct();
                var monthList = dresult1.Select(x => new ModelMaster
                {
                    Value = x,
                    Text = x + " - " + DateTime.ParseExact(x, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM")
                }).ToList();
                ViewBag.Month = monthList;
            }
            else
            {
                ViewBag.Month = m_month;
            }


            if (!string.IsNullOrEmpty(EMPCODE))
            {
                var url = string.Format("/api/Employee/GetByCode?EMPCODE={0}", EMPCODE);
                var response = await Services.GetAsync<M_EMPMAS>(url);
                ViewBag.emplist = new List<SelectListItem>
                                            {
                                                new SelectListItem {
                                                    Text = response.Data.pK_EMPCODE + "-" + response.Data.fname,
                                                                Value = response.Data.pK_EMPCODE,
                                                                Selected = true
                                                }
                                            };

            }
            return View(usersAsIPagedList);
        }

        [HttpPost]
        public async Task<ActionResult> Payslip(string select_employee, string MonthCode, string Unit = "", string paytype = "P", string Ptype = "S")
        {
            try
            {
                if (select_employee == "0"|| select_employee== "Select")
                {
                    select_employee = "";
                }
                //var employeeIdList = employee_ids.Split(',').ToList();
                var unitddl = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
                List<ModelMaster> Lst = unitddl.Data.Select(x => new ModelMaster
                {
                    Value = x.code,
                    Text = x.code + "-" + x.name
                }).ToList();
                ViewBag.Unit = Lst;
                var m_month = new List<M_MONTH>();
                if (!string.IsNullOrEmpty(Unit))
                {
                    var result1 = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetByUnit?unit={0}", Unit));
                    m_month = result1.Data;
                }
                else
                {
                    var result1 = await Services.GetAsync<List<M_MONTH>>(string.Format("/api/Month/GetAll"));
                    m_month = result1.Data;
                }

                if (m_month.Count > 0)
                {
                    var dresult1 = m_month.Select(u => u.pK_MONTHCODE).Distinct();
                    var monthList = dresult1.Select(x => new ModelMaster
                    {
                        Value = x,
                        Text = x + "-" + DateTime.ParseExact(x, "yyyyMM", CultureInfo.InvariantCulture).ToString("MMM")
                    }).ToList();
                    ViewBag.Month = monthList;
                }
                else
                {
                    ViewBag.Month = m_month;
                }
                var result = (dynamic)null;
                if (!string.IsNullOrEmpty(select_employee))
                {
                    result = await Services.GetAsync<T_PAYIMAGE>(string.Format("/api/Payimage/GetCalculatePayByCode?code={0}&Monthcode={1}&unit={2}&paytype={3}&Ptype={4}", select_employee, MonthCode, Unit, paytype, Ptype));
                }
                else
                {
                    result = await Services.GetAsync<T_PAYIMAGE>(string.Format("/api/Payimage/GetCalculatePaySlipAll?&Monthcode={0}&unit={1}&paytype={2}&Ptype={3}", MonthCode, Unit, paytype, Ptype));
                }


                if (result.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = result.Message;
                }
                return RedirectToAction("Payslip");
                //return View();
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.InnerException.Message;
                return RedirectToAction("Payslip");
            }

        }
        public async Task<ActionResult> ViewPayslip(int Id)
        {
            return View();
        }
        public byte[] GetReportData(object dt)
        {
            List<byte[]> files = new List<byte[]>();
            ReportDocument rd = new ReportDocument();
            var path = Path.Combine(Server.MapPath("../CrystalReports"), "TD_RptPaySlip.rpt");
            rd.Load(path);
            rd.DataSourceConnections.Clear();
            rd.SetDataSource(dt);
            rd.DataDefinition.FormulaFields["Header1"].Text = "'Header1'";
            rd.DataDefinition.FormulaFields["CompAddress"].Text = "'CompAddress'";
            rd.DataDefinition.FormulaFields["CompTelFax"].Text = "'CompTelFax'";
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            files.Add(Utilities.PrepareBytes(stream));
            byte[] buffer = (Utilities.MergeReports(files).ToArray());
            return buffer;
        }
        public void Pdf_DownLoadWithByte(byte[] buffer)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/pdf";

            //merge the all reports & show the reports            
            Response.BinaryWrite(buffer);
            Response.End();
        }
        ///need to enable 32 bit in iis application tool for running crystal report
        public async Task<ActionResult> PayslipPDF(string MonthCode, string UNIT, string EmpCode)///need to enable 32 bit in iis application tool for running crystal report
        {
            try
            {
                var result = await Services.GetAsync<List<payimagedittocopy>>(string.Format("/api/Payimage/GetPaySlipAll?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", MonthCode, UNIT, EmpCode));
                List<object> payslipList = new List<object>();
                foreach (var item in result.Data)
                {
                    var t_PAYIMAGE = new 
                    {
                        CompAddress = "",
                        LOCA = "",
                        UNIT = item.UNIT,
                        FK_EMPCODE = item.EMPCODE,
                        TITLE = item.TITLE,
                        FNAME = item.FNAME,
                        MNAME = item.MNAME,
                        LNAME = item.LNAME,
                        SEX = item.SEX,
                        BANK_NAME = item.BANK_NAME,
                        AC_NO = item.AC_NO,
                        IBAN_NO = item.IBAN_NO != null?item.IBAN_NO:null,
                        DATEOFBIRTH = item.DATEOFBIRTH,
                        DATEOFJOINING = item.DATEOFJOINING,
                        DEPARTMENT = item.DESCRIPTION,
                        CURRENCY = item.CDESCN,
                        SUBUNIT = "",
                        SHT_DESCN = "",
                        INTERNATIONAL_FMT = "",
                        DESG_DESCN = item.DESG_DESCN,
                        MONTH = "",
                        YR = "",
                        BASIC_PAY = item.BASIC_PAY,
                        ACT_BASIC_PAY = item.ACT_BASIC_PAY,
                        DA_PAY = item.DA_PAY,
                        ACT_DA_PAY = item.ACT_DA_PAY,
                        OTHER_DED_AMT = item.OTHER_DED_AMT,
                        TOTAL_ALLOW_AMT =Convert.ToDecimal(item.HRA_PAY) +Convert.ToDecimal(item.CONV_PAY) + Convert.ToDecimal(item.TOTAL_ALLOW_AMT),
                        EARNED_WAGES = item.EARNED_WAGES,
                        NET_PAY = item.NET_PAY,
                        NET_SAL_PAID = item.NET_SAL_PAID,
                        ABSENT = item.ABSENT,
                        DAYS_PRESENT = item.DAYS_PRESENT,
                        PAID_HOLIDAY = item.PAID_HOLIDAY,
                        TOT_PAYABLE_DAYS = item.DAYS_PRESENT,
                        PAID_LEAVE = item.PAID_LEAVE,
                        WEEK_LEAVE = item.WEEK_LEAVE,
                        SICK_LEAVE = item.PAID_LEAVE,
                        MATERNITY_LEAVE = item.PAID_LEAVE,
                        BALANCE_LEAVE = "",
                        TOTAL_DED_AMT = item.TOTAL_DED_AMT,
                        MONTHDAYS = "",
                        //logo_fld = @"C:\inetpub\wwwroot\Agrifields\AgriFieldsLogo\LOGO.PNG",
                        logo_fld = "",
                        DESC_1 = item.DEDUCT_CODE+" "+ item.DEDUCT_DESC,
                        AMT_1 = item.OTHER_DED_AMT,
                        DESC_2 = "",
                        AMT_2 = "",
                        DESC_3 = "",
                        AMT_3 = "",
                        BALANCE_S = "",
                        BALANCE_M = "",
                        BALANCE_L = "",
                    };
                    payslipList.Add(t_PAYIMAGE);
                }

                ReportDocument rd = new ReportDocument();
                //ReportClass rd = new ReportClass();
                var path = Path.Combine(Server.MapPath("~/CrystalReports"), "TD_RptPaySlip.rpt");
                rd.Load(path);
                rd.SetDataSource(payslipList);
                //rd.SetDataSource(result.Data);
                rd.DataDefinition.FormulaFields["Header1"].Text = "'COMPANY NAME'";
                //rd.DataDefinition.FormulaFields["CompAddress"].Text = "COMPANY NAME ADDRESS";
                //rd.DataDefinition.FormulaFields["CompTelFax"].Text = "Tel    34235245      :  Fax   1414141      :  Email Id : test@mail.com ";
                Response.Buffer = false;
                Response.ClearContent();
                Response.ClearHeaders();
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                //return File(stream, "application/pdf", "CustomerList.pdf");
                Response.AppendHeader("Content-Disposition", "inline; filename=PaySlip_" + MonthCode + ".pdf");
                return new FileStreamResult(stream, "application/pdf");
            }
            catch (Exception ex)
            {
                Log.Error("Login-page started..." + ex.Message + ex.InnerException);
                TempData["toastrError"] = ex.InnerException;
                throw;
            }
        }
        public async Task<JsonResult> GetEmployee(string UNIT, string searchString = null, int? page = 1, int pageSize = 25)
        {
            var url = string.Format("/api/Employee/GetByPage?page={0}&pageSize={1}", page, pageSize);
            if (!string.IsNullOrEmpty(UNIT))
            {
                url = string.Format("/api/Employee/GetByPage?page={0}&pageSize={1}&unit={2}", page, pageSize, UNIT);
                if (!string.IsNullOrEmpty(searchString))
                {
                    url = string.Format("/api/Employee/GetByPage?search={0}&page={1}&pageSize={2}&unit={3}", searchString, page, pageSize, UNIT);
                }
                var response = await Services.GetAsync<PageModel<M_EMPMAS>>(url);
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            return Json("No data found");
        }
        public async Task<ActionResult> PayslipPDF1(string MonthCode, string UNIT, string EmpCode)
        {
            try
            {
                var result = await Services.GetAsync<List<payimagedittocopy>>(string.Format("/api/Payimage/GetPaySlipAll?Monthcode={0}&unit={1}&Empcode={2}&paytype=P&Ptype=S", MonthCode, UNIT, EmpCode));
                List<object> payslipList = new List<object>();
                foreach (var item in result.Data)
                {
                    var t_PAYIMAGE = new
                    {
                        CompAddress = "",
                        LOCA = "",
                        UNIT = item.UNIT,
                        FK_EMPCODE = item.EMPCODE,
                        TITLE = item.TITLE,
                        FNAME = item.FNAME,
                        MNAME = item.MNAME,
                        LNAME = item.LNAME,
                        SEX = item.SEX,
                        BANK_NAME = item.BANK_NAME,
                        AC_NO = item.AC_NO,
                        IBAN_NO = item.IBAN_NO != null,
                        DATEOFBIRTH = item.DATEOFBIRTH,
                        DATEOFJOINING = item.DATEOFJOINING,
                        DEPARTMENT = item.DESCRIPTION,
                        CURRENCY = item.CDESCN,
                        SUBUNIT = "",
                        SHT_DESCN = "",
                        INTERNATIONAL_FMT = "",
                        DESG_DESCN = item.DESG_DESCN,
                        MONTH = "",
                        YR = "",
                        BASIC_PAY = item.BASIC_PAY,
                        ACT_BASIC_PAY = item.ACT_BASIC_PAY,
                        DA_PAY = item.DA_PAY,
                        ACT_DA_PAY = item.ACT_DA_PAY,
                        OTHER_DED_AMT = item.OTHER_DED_AMT,
                        TOTAL_ALLOW_AMT = item.TOTAL_ALLOW_AMT,
                        EARNED_WAGES = item.EARNED_WAGES,
                        NET_PAY = item.NET_PAY,
                        NET_SAL_PAID = item.NET_SAL_PAID,
                        ABSENT = item.ABSENT,
                        DAYS_PRESENT = item.DAYS_PRESENT,
                        PAID_HOLIDAY = item.PAID_HOLIDAY,
                        TOT_PAYABLE_DAYS = item.DAYS_PRESENT,
                        PAID_LEAVE = item.PAID_LEAVE,
                        WEEK_LEAVE = item.WEEK_LEAVE,
                        SICK_LEAVE = item.PAID_LEAVE,
                        MATERNITY_LEAVE = item.PAID_LEAVE,
                        BALANCE_LEAVE = "",
                        TOTAL_DED_AMT = item.TOTAL_DED_AMT,
                        MONTHDAYS = "",
                        logo_fld = @"C:\inetpub\wwwroot\Agrifields\AgriFieldsLogo\LOGO.PNG",
                        DESC_1 = "",
                        AMT_1 = "",
                        DESC_2 = "",
                        AMT_2 = "",
                        DESC_3 = "",
                        AMT_3 = "",
                        BALANCE_S = "",
                        BALANCE_M = "",
                        BALANCE_L = "",
                    };
                    payslipList.Add(t_PAYIMAGE);
                }
                byte[] buffer = GetReportData(payslipList);
                Pdf_DownLoadWithByte(buffer);
            }
            catch (Exception ex)
            {
                Log.Debug("Testing ");
                Log.Info(ex.Message);
                TempData["toastrError"] = ex.Message;
                throw;
            }
            return null;
        }
    }
}