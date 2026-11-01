using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models.Master;
using HRMS.Models;
using static HRMS.Models.Model_Common;
using System.Threading.Tasks;
using PagedList;
using System.Reflection;

namespace HRMS.Controllers.Master
{
    public class M_DeductionController : Controller
    {
        // GET: M_Deduction
        public async Task<ActionResult> M_Deduction( string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_DEDUCTION>>(string.Format("/api/Deduction/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_DEDUCTION>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    var unitData = await Services.GetAsync<List<M_UNIT>>("/api/unit/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {
                        

                            var unitName = unitData.Data.FirstOrDefault(u => u.code == item.unit);
                        if (unitName != null)
                        {
                            item.unit = unitName.code + " : " + unitName.name;
                        }
                    }
                    var DEDUCTIONDATA = await Services.GetAsync<List<M_DEDUCT_CALC_MODE>>("/api/DeductCalcMode/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {


                        var DEDUCTIONNAME = DEDUCTIONDATA.Data.FirstOrDefault(u => u.pK_CALC_MODE == item.fK_CALC_MODE);
                        if (DEDUCTIONNAME != null)
                        {
                            item.fK_CALC_MODE = DEDUCTIONNAME.pK_CALC_MODE + " : " + DEDUCTIONNAME.description;
                        }
                    }
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
            var DlCalMode = await Services.GetAsync<List<M_DEDUCT_CALC_MODE>>("/api/DeductCalcMode/GetAll");
            List<ModelMaster> calM = DlCalMode.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CALC_MODE,
                Text = x.pK_CALC_MODE + "-" + x.description

            }).ToList();
            ViewBag.calM = calM;

            var Unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = Unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;

            M_DEDUCTION model = new M_DEDUCTION();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_DEDUCTION>("/api/Deduction/GetByID?id=" + Id);
                model = response.Data;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_DEDUCTION entity)
        {
            try
            {
               
                entity.useR_NAME_NEW = "";
                if (entity.unit ==  null )
                {
                    entity.unit = "";
                }
                if ( entity.shorT_NAME == null)
                {
                    entity.shorT_NAME = "";
                }
                if (entity.fK_CALC_MODE == null)
                {
                    entity.fK_CALC_MODE = "";
                }
                entity.useR_ENTDT_NEW = DateTime.Now;
              
              
                entity.dtag = "";

                if (entity.incL_PF == null )
                {
                    entity.incL_PF = entity.bool_incL_PF ? true : false;
                }
                if (entity.incL_ESI == null)
                {
                    entity.incL_ESI = entity.bool_incL_ESI ? true : false;
                }
                if (entity.incL_PTAX == null)
                {
                    entity.incL_PTAX = entity.bool_incL_PTAX ? true : false;
                }
                if (entity.incL_TAXABLE == null)
                {
                    entity.incL_TAXABLE = entity.bool_incL_TAXABLE ? true : false;
                }

           
                entity.deducT_TYPE = "";
                entity.suspencE_AC = "";
                entity.voucheR_ADV_AC = "";
                entity.voucheR_CREDIT = "";
                entity.includE_VOUCHER = "";
                entity.sundrY_CREDITOR = "";
                entity.voucheR_HEAD = "";
                entity.ptype = "S";
                M_DEDUCTION_Dto _Dto = new M_DEDUCTION_Dto();
                _Dto.m_deduction = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_DEDUCTION>("/api/Deduction/GetByCode?search=" + entity.pK_DEDUCT_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_DEDUCT_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_DEDUCTION_Dto>>("/api/Deduction/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_DEDUCTION_Dto>>("/api/Deduction/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("M_Deduction");
        }
    }
}