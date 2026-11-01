using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using HRMS.Models;
using HRMS.Models.Master;

using Newtonsoft.Json;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class Deduct_Calc_ModeController : Controller
    {
        // GET: Deduct_Calc_Mode
        public async Task<ActionResult> Deduct_Calc_Mode(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_DEDUCT_CALC_MODE>>(string.Format("/api/DeductCalcMode/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_DEDUCT_CALC_MODE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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


        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            M_DEDUCT_CALC_MODE _MODE = new M_DEDUCT_CALC_MODE();
            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> un = unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Ut = un;
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_DEDUCT_CALC_MODE>("/api/DeductCalcMode/GetByID?id=" + Id);
                _MODE = response.Data;
                return View("AddEditModal", _MODE);
            }
            else
            {
                return View("AddEditModal", _MODE);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_DEDUCT_CALC_MODE entity)
        {
            try
            {


                entity.useR_ENTDT_NEW = DateTime.Now;


                entity.dtag = "";
                entity.oS_USER = "";

                entity.terminaL_ID = "";
                entity.useR_NAME_NEW = "";
                entity.ptype = "S";
                M_DEDUCT_CALC_MODE_Dto _Dto = new M_DEDUCT_CALC_MODE_Dto();
                _Dto.m_deductcalcmode = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_DEDUCT_CALC_MODE>>("/api/DeductCalcMode/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.pK_CALC_MODE == entity.pK_CALC_MODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.pK_CALC_MODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_DEDUCT_CALC_MODE_Dto>>("/api/DeductCalcMode/SaveOrUpdateAll", _Dto);
                        if (responce.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = responce.Message;
                        }
                    }
                }
                else
                {


                    var responce = await Services.PostAsync<List<M_DEDUCT_CALC_MODE_Dto>>("/api/DeductCalcMode/SaveOrUpdateAll", _Dto);
                    if (responce.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                    }
                    else
                    {
                        TempData["toastrError"] = responce.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("Deduct_Calc_Mode");
        }
    }
}