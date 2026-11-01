using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class M_AllowanceController : Controller
    {
        // GET: M_Allowance
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce = await Services.GetAsync<PageModel<M_ALLOWANCE>>(string.Format("/api/Allowance/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString));
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
            var usersAsIPagedList = new StaticPagedList<M_ALLOWANCE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                var unitData = await Services.GetAsync<List<M_UNIT>>("/api/unit/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var unitName = unitData.Data.FirstOrDefault(u => u.code == item.unit);
                    if (unitName != null)
                    {
                        item.unit = unitName.code + " - " + unitName.name;
                    }
                }
                var allowcalock = await Services.GetAsync<List<M_AllOW_CALC_MODE>>("/api/AllowCalcMode/GetAll");
                foreach (var item in usersAsIPagedList)
                {


                    var allowcalockname = allowcalock.Data.FirstOrDefault(u => u.pK_CALC_MODE == item.fK_CALC_MODE);
                    if (allowcalockname != null)
                    {
                        item.fK_CALC_MODE = allowcalockname.pK_CALC_MODE + " - " + allowcalockname.description;
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

        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            var ailowcal = await Services.GetAsync<List<M_AllOW_CALC_MODE>>("/api/AllowCalcMode/GetAll");
            List<ModelMaster> all = ailowcal.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CALC_MODE,
                Text = x.pK_CALC_MODE + "-" + x.description

            }).ToList();
            ViewBag.AllowCal = all;
            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            M_ALLOWANCE m_ALLOWANCE = new M_ALLOWANCE();
            if (Convert.ToInt32(Id) <= 0)
            {
                return View(m_ALLOWANCE);
            }
            var mst_allow = await Services.GetAsync<M_ALLOWANCE>("/api/Allowance/GetByID?id=" + Id);
            m_ALLOWANCE = mst_allow.Data;
            return View(m_ALLOWANCE);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_ALLOWANCE entity)
        {
            try
            {
                
                entity.useR_NAME_NEW = "";
                
                
                entity.useR_ENTDT_NEW = DateTime.Now;
                
         
                entity.dtag = "";


                if (entity.shorT_NAME == null & entity.voucheR_HEAD == null)
                {

                    entity.shorT_NAME = "";
                    entity.voucheR_HEAD = "";
                }
                if (entity.incL_PF == null & entity.incL_ESI == null)
                {
                    entity.incL_PF = entity.bool_INCL_PF ? true : false;
                    entity.incL_ESI = entity.bool_INCL_ESI ? true : false;
                    
                    
                }
                if (entity.incL_PTAX == null & entity.incL_TAXABLE == null)
                {
                    entity.incL_PTAX = entity.bool_INCL_PTAX ? true : false;
                    entity.incL_TAXABLE = entity.bool_INCL_TAXABLE ? true : false;
                }
                if (entity.incL_OT == null )
                {
                    entity.incL_OT = entity.bool_INCL_OT ? true : false;
                }
                entity.incL_GROSS = entity.incL_GROSS? true: false;
                entity.emP_WELFARE = entity.emP_WELFARE? true : false; 
                entity.voucheR_DEBIT = entity.voucheR_DEBIT ? true : false;
                entity.voucheR_OT_AC = entity.voucheR_OT_AC ? true : false;
                entity.includE_VOUCHER = entity.includE_VOUCHER ? true : false;
                entity.transactioN_MASTER = "";
                entity.ptype = "S";
                M_ALLOWANCE_Dto _Dto = new M_ALLOWANCE_Dto();
                _Dto.m_allowance = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_ALLOWANCE>("/api/Allowance/GetByCode?search=" + entity.pK_alL_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_alL_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_ALLOWANCE_Dto>>("/api/Allowance/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_ALLOWANCE_Dto>>("/api/Allowance/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Index");
        }

    }
}

