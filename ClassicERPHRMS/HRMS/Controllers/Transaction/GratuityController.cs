using HRMS.Models.Master;
using HRMS.Models.TRN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Transaction
{
    public class GratuityController : Controller
    {
        // GET: Gratuity       
        public ActionResult GetGratuity()
        {
            try
            {
                //Model_SessionOtherProject jsonDataR = getCookies.GetStoredCookies();
                //var FinancialYear = jsonDataR.FinancialYearName;
                //string startDate = FinancialYear.Split('-')[0].Trim();
                //DateTime date = DateTime.ParseExact(startDate, "dd/MM/yyyy", null);

                //string monthPrev = date.ToString("yyyyMM");
                //string monthNow = DateTime.Now.ToString("yyyyMM");
                //int prevMonthCode = int.Parse(monthPrev);
                //int currentMonthCode = int.Parse(monthNow);

                //model.IsForDll = true;
                //ViewBag.Month = servicesObj.SubmitPost<List<M_MONTH>>("api/PAY/PAY_Master/GetData_M_MONTH", model).Data
                //    .Where(a => int.Parse(a.PK_MONTHCODE) >= int.Parse(monthPrev) && int.Parse(a.PK_MONTHCODE) <= int.Parse(monthNow)).OrderByDescending(a => a.PK_MONTHCODE).ToList();
                ////model.FK_CODE = "202406";
                //var GratuityLeave = servicesObj.SubmitPost<List<M_LEAVE_PARAM>>("api/PAY/PAY_Master/GetData_M_LEAVE_PARAM", model).Data;
                //List<ModelMaster> Lst = GratuityLeave.Select(x => new ModelMaster
                //{
                //    Value = x.FROM_PERIOD.ToString("dd/MM/yyyy"),
                //    Text = x.FROM_PERIOD.ToString("dd/MM/yyyy") + "  -  " + x.TO_PERIOD.ToString("dd/MM/yyyy")

                //}).ToList();
                //// var AttenCd = attend.Data.
                //ViewBag.GratuityLeave = Lst;
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GratuityReport(string PK_MONTHCODE, string FROM_PERIOD)
        {
            var MonthCode = PK_MONTHCODE;
            try
            {
                var response = (dynamic)null;
                var response2 = (dynamic)null;
                //SpDefinedParamters spDefinedParamters = new SpDefinedParamters();
                //Model_MasterDynamicSp model_MasterDynamicSp = new Model_MasterDynamicSp();

                //Model_MasterSpParamters model_MasterSpParamters = new Model_MasterSpParamters();
                //model_MasterSpParamters.SpId = 15;
                //model_MasterSpParamters.MonthCode = MonthCode;
                //model_MasterDynamicSp = spDefinedParamters.HP_T_GRATUITY_CALC_15(model_MasterSpParamters);

                //List<T_GRATUITY> Daily_Sp = servicesObj.SubmitGetList<T_GRATUITY>("api/Td/Td_Report/GetGratuityData", model_MasterDynamicSp);

                //var LoginDetails = Utility.GetLoggedInUserDetails();
                //Model_SessionOtherProject jsonDataR = getCookies.GetStoredCookies();
                //foreach (var item in Daily_Sp)
                //{
                //    item.EFF_DATE = DateTime.Now;
                //    item.USER_NAME = LoginDetails.UserId;
                //    item.USER_ENTDT = DateTime.Now;
                //    item.OS_USER = jsonDataR.Os_user;
                //    item.TERMINAL_ID = jsonDataR.Terminal_id;
                //    item.DTAG = "";
                //    //if (item.UNIT==null) { item.UNIT = ""; }
                //    //item.USER_NAME_NEW = LoginDetails.UserId;
                //    //item.USER_ENTDT_NEW = DateTime.Now;
                //}

                //response = servicesObj.SubmitPost<List<T_GRATUITY>>("api/PAY/PAY_Master/Add_T_GRATUITY", Daily_Sp);


                //model_MasterSpParamters.SpId = 18;
                //model_MasterSpParamters.MonthCode = MonthCode;
                //model_MasterSpParamters.FrmDate = FROM_PERIOD;
                //model_MasterSpParamters.Code = "";
                //model_MasterDynamicSp = spDefinedParamters.HP_LEAVE_FULL_SETTLE_CALC_18(model_MasterSpParamters);

                //List<T_LEAVE> leave = servicesObj.SubmitGetList<T_LEAVE>("api/Td/Td_Report/GetLeaveEncashData", model_MasterDynamicSp);

                //foreach (var item in leave)
                //{
                //    item.EFF_DATE = DateTime.Now;
                //    item.USER_NAME = LoginDetails.UserId;
                //    item.USER_ENTDT = DateTime.Now;
                //    item.OS_USER = jsonDataR.Os_user;
                //    item.TERMINAL_ID = jsonDataR.Terminal_id;
                //    item.DTAG = "";
                //}

                //response2 = servicesObj.SubmitPost<List<T_LEAVE>>("api/PAY/PAY_Master/Add_T_LEAVE", leave);

                if (response.StatusCode == 1 && response2.StatusCode == 1)
                {
                    TempData["SuccessMessage"] = "Data has been successfully submitted.";
                    return RedirectToAction("GetGratuity");
                }
                else
                {
                    TempData["ErrorMessage"] = "Something Went Wrong.";
                    return RedirectToAction("GetGratuity");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"{ex.Message}";
                return RedirectToAction("GetGratuity");
            }
        }
    }
}