using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models.Master;
using static HRMS.Models.Model_Common;
using HRMS.Models;
using System.Threading.Tasks;
using PagedList;
using System.Web.UI.WebControls;

namespace HRMS.Controllers.Master
{
    public class M_EmpDeductLinkDetController : Controller
    {

        [HttpGet]
        public async Task<ActionResult> SelectEmp()
        {

            return View();
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
            return Json("", JsonRequestBehavior.AllowGet);
        }


        public async Task<JsonResult> GetUnitDropdown(string searchString = null, int? page = 1, int pageSize = 5)
        {
            var response = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={1}", page, pageSize, Utility.SessionHelper.GetUser().getUserName));


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        // GET: M_EmpDeductLinkDet
        public async Task<ActionResult> M_EmpDeductLinkDet(string Employee, string EmpId, string UnitCode, string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {

            try
            {

                var EmpName = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByCode?EMPCODE={0}", Employee));
                if (EmpName.IsSuccessStatusCode)
                {
                    ViewBag.DisplayName = EmpName.Data.pK_EMPCODE + " " + EmpName.Data.fname + " " + EmpName.Data.mname + " " + EmpName.Data.lname;
                    ViewBag.EmployeeCode = EmpName.Data.pK_EMPCODE;
                    ViewBag.Unit = UnitCode;

                }
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                //var responce = await Services.GetAsync<PageModel<M_EMP_ALLOW_DET>>(string.Format("/api/EmpAllowanceDet/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
                var responce = await Services.GetAsync<PageModel<M_EMP_DEDUCT_DET>>(string.Format("/api/EmpDeductionDet/GetByPage?EmpId={0}&page={1}&pageSize={2}", Employee, page, pageSize)); // Helpers.UserInfo.ModuleCode
                if (responce.IsSuccessStatusCode)
                {
                    if (!string.IsNullOrEmpty(searchString))
                    {

                    }
                    ViewBag.CurrentFilter = searchString;
                    switch (sortOrder)
                    {

                    }
                    ViewBag.PageSize = pageSize;
                    int pageNumber = (page ?? 1);
                    var usersAsIPagedList = new StaticPagedList<M_EMP_DEDUCT_DET>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    return View(usersAsIPagedList);
                }
                else
                {
                    TempData["toastrError"] = responce.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        [HttpPost]
        public async Task<ActionResult> AddEditModal(string Id, string UnitCode, string Employee)
        {
            ViewBag.CurrentYear = DateTime.Now.Year;
            var deduc = await Services.GetAsync<List<M_DEDUCTION>>(string.Format("/api/Deduction/GetByUnit?unit={0}", UnitCode));
            List<ModelMaster> Lstt = deduc.Data.Select(x => new ModelMaster
            {
                Value = x.pK_DEDUCT_CODE,
                Text = x.pK_DEDUCT_CODE + "-" + x.description
            }).ToList();
            ViewBag.At = Lstt;
            var currency = await Services.GetAsync<List<M_CURRENCY>>("/api/Currency/GetAll");
            List<ModelMaster> currencyLst = currency.Data.Select(x => new ModelMaster
            {
                Value = x.PK_CURR_CODE,
                Text = x.PK_CURR_CODE + "-" + x.DESCRIPTION

            }).ToList();

            ViewBag.Attend = currencyLst;

            M_EMP_DEDUCT_DET m_deduct_link = new M_EMP_DEDUCT_DET();
            var val = int.TryParse(Id, out int res);
            int id = res;
            if (id <= 0)
            {
                m_deduct_link.EFF_DATE = DateTime.Now;
                m_deduct_link.FK_EMPCODE = Employee;
                m_deduct_link.UNIT = UnitCode;
                m_deduct_link.ACTIVE_STATUS = true;
                return View("~/Views/M_EmpDeductLinkDet/_M_EmpDeductLinkDet_AddEdit.cshtml", m_deduct_link);
            }
            var deduct_link = await Services.GetAsync<M_EMP_DEDUCT_DET>(string.Format("/api/EmpDeductionDet/GetByID?id=" + Id));
            deduct_link.Data.DeductionDesc = deduct_link.Data.FK_DEDUCT_CODE;
            deduct_link.Data.FK_DEDUCT_CODE = deduct_link.Data.FK_DEDUCT_CODE.Substring(0, deduct_link.Data.FK_DEDUCT_CODE.IndexOf(':')).Trim();
            m_deduct_link = deduct_link.Data;

            return View("~/Views/M_EmpDeductLinkDet/_M_EmpDeductLinkDet_AddEdit.cshtml", m_deduct_link);

        }




        [HttpPost]
        public async Task<ActionResult> StoreOrEdit(M_EMP_DEDUCT_DET m_DEDUCT_LINK)
        {
            try
            {
                m_DEDUCT_LINK.CLOSED_ON = DateTime.Now.Date;
                m_DEDUCT_LINK.DTAG = "";
                m_DEDUCT_LINK.USER_NAME_NEW = "";
                if (m_DEDUCT_LINK.ADVANCE_AMOUNT == null)
                {

                    m_DEDUCT_LINK.ADVANCE_AMOUNT = 0;

                }
                if (m_DEDUCT_LINK.NUMBER_INST == null)
                {

                    m_DEDUCT_LINK.NUMBER_INST = 0;

                }
                if (m_DEDUCT_LINK.DEDUCT_RS == null)
                {

                    m_DEDUCT_LINK.DEDUCT_RS = 0;

                }

                M_EMP_DEDUCT_DET_Dto _Dto = new M_EMP_DEDUCT_DET_Dto();
                _Dto.m_empdeductdet = m_DEDUCT_LINK;

                var isexpire = false;
                if (m_DEDUCT_LINK.ID > 0)
                {
                    var response = await Services.PostAsync<List<M_EMP_DEDUCT_DET_Dto>>("/api/EmpDeductionDet/SaveOrUpdateAll", _Dto);
                    if (response.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = response.Message;
                    }
                }
                else
                {
                    if (DateTime.Now.Date > m_DEDUCT_LINK.DDATE)
                    {
                        isexpire = true;
                    }
                    if (m_DEDUCT_LINK.ACTIVE_STATUS == true || m_DEDUCT_LINK.ACTIVE_STATUS == false)
                    {
                        isexpire = true;
                    }
                    if (isexpire == true || isexpire == false )
                    {
                        var response = await Services.PostAsync<List<M_EMP_DEDUCT_DET_Dto>>("/api/EmpDeductionDet/SaveOrUpdateAll", _Dto);
                        if (response.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = response.Message;
                        }
                    }
                    else if (m_DEDUCT_LINK.DDATE == DateTime.Now)
                    {
                        var response = await Services.PostAsync<List<M_EMP_DEDUCT_DET_Dto>>("/api/EmpDeductionDet/SaveOrUpdateAll", _Dto);
                        if (response.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = response.Message;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_EmpDeductLinkDet", new { Employee = m_DEDUCT_LINK.FK_EMPCODE, UnitCode = m_DEDUCT_LINK.UNIT });
        }

    }
}

