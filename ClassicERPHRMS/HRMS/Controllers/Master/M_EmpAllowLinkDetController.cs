
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;
using static HRMS.Models.Model_Common;
using System.Threading.Tasks;
using System.Drawing.Printing;
using System.Web.UI;
using System.Data.SqlClient;
using PagedList;
using CrystalDecisions.Shared;
using System.Web.UI.WebControls;
using static iTextSharp.text.pdf.events.IndexEvents;


namespace HRMS.Controllers.Master
{
    public class M_EmpAllowLinkDetController : Controller
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

        // GET: M_EmpAllowLinkDet
        //[HttpPost]
        public async Task<ActionResult> M_EmpAllowLinkDet(string EmpId, string UnitCode, string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                var EmpName = await Services.GetAsync<M_EMPMAS>(string.Format("/api/Employee/GetByCode?EMPCODE={0}", EmpId));
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
                var responce = await Services.GetAsync<PageModel<M_EMP_ALLOW_DET>>(string.Format("/api/EmpAllowanceDet/GetByPage?unit={0}&EmpId={1}&search={2}&page={3}&pageSize={4}", UnitCode, EmpId, searchString, page, pageSize)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_EMP_ALLOW_DET>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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

        
        public async Task<ActionResult> AddEditModal(string Id, string Unit, string EmpId)
        {
            var allow = await Services.GetAsync<List<M_ALLOWANCE>>(string.Format("/api/Allowance/GetByUnit?unit={0}", Unit));
            ViewBag.Allow = allow.Data.OrderBy(x => x.pK_alL_CODE).ToList();
            ViewBag.DropdownListAllowMode = allow.Data;


            var currency = await Services.GetAsync<List<M_CURRENCY>>("/api/Currency/GetAll");
            List<ModelMaster> currencyLst = currency.Data.Select(x => new ModelMaster
            {
                Value = x.PK_CURR_CODE,
                Text = x.PK_CURR_CODE + "-" + x.DESCRIPTION

            }).ToList();

            ViewBag.Attend = currencyLst;

            M_EMP_ALLOW_DET m_allow_link = new M_EMP_ALLOW_DET();
            var val = int.TryParse(Id, out int res);
            int id = res;
            if (id <= 0)
            {
                m_allow_link.FK_EMPCODE = EmpId;
                m_allow_link.UNIT = Unit;
                m_allow_link.ACTIVE_STATUS = true;
                m_allow_link.INCL_OT = true;
                
                return View("~/Views/M_EmpAllowLinkDet/M_EmpAllowLinkDet_AddEdit.cshtml", m_allow_link);
            }
            var allow_link = await Services.GetAsync<M_EMP_ALLOW_DET>(string.Format("/api/EmpAllowanceDet/GetByID?id=" + Id));
            //allow_link.Data._allowanceDesc = allow_link.Data.FK_ALL_CODE.Substring(allow_link.Data.FK_ALL_CODE.IndexOf(':') + 1).Trim();
            allow_link.Data._allowanceDesc = allow_link.Data.FK_ALL_CODE;
            allow_link.Data.FK_ALL_CODE = allow_link.Data.FK_ALL_CODE.Substring(0, allow_link.Data.FK_ALL_CODE.IndexOf(':')).Trim();
            m_allow_link = allow_link.Data;

            return View("~/Views/M_EmpAllowLinkDet/M_EmpAllowLinkDet_AddEdit.cshtml", m_allow_link);
        }

        [HttpPost]
        public async Task<ActionResult> StoreOrEdit(M_EMP_ALLOW_DET m_ALLOW_LINK)
        {
            try
            {
                m_ALLOW_LINK.DTAG = "";
                m_ALLOW_LINK.USER_NAME_NEW = "";
                if (m_ALLOW_LINK.ALL_PERCENT == null)
                {

                    m_ALLOW_LINK.ALL_PERCENT = 0;
                    
                }
                if (m_ALLOW_LINK.ALL_LIMIT == null)
                {

                    m_ALLOW_LINK.ALL_LIMIT = 0;

                }
                M_EMP_ALLOW_DET_Dto _Dto = new M_EMP_ALLOW_DET_Dto();
                _Dto.m_empallowdet = m_ALLOW_LINK;
                var response = await Services.PostAsync<List<M_EMP_ALLOW_DET_Dto>>("/api/EmpAllowanceDet/SaveOrUpdateAll", _Dto);
                if (response.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                }
                else
                {
                    TempData["toastrError"] = response.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_EmpAllowLinkDet", new { EmpId = m_ALLOW_LINK.FK_EMPCODE, UnitCode = m_ALLOW_LINK.UNIT });
        }

        

    }
}

