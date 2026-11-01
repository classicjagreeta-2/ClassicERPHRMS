using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;
using HRMS.Models.TRN;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class Allow_Calc_ModeController : Controller
    {
        // GET: Allow_Calc_Mode

        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce = await Services.GetAsync<PageModel<M_AllOW_CALC_MODE>>(string.Format("/api/AllowCalcMode/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString));
            if (responce.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    //searchString = currentFilter;
                    //responce = responce.Data.Where(k=>k.suB_DEPT_DESCN == searchString).ToList();
                }
                ViewBag.CurrentFilter = searchString;
                switch (sortOrder)
                {

                }
                ViewBag.PageSize = pageSize;
                int pageNumber = (page ?? 1);
                var usersAsIPagedList = new StaticPagedList<M_AllOW_CALC_MODE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                return View(usersAsIPagedList);
            }
            else
            {
                TempData["toastrError"] = responce.Message;
                return View();
            }
        }
        [HttpGet]
        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> Lst = unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = Lst;
            M_AllOW_CALC_MODE cALC_MODE = new M_AllOW_CALC_MODE();
            if (Convert.ToInt32(Id) <= 0)
            {
                return View(cALC_MODE);
            }

            var mst_allow = await Services.GetAsync<M_AllOW_CALC_MODE>("/api/AllowCalcMode/GetByID?id=" + Id);
            cALC_MODE = mst_allow.Data;
            return View(cALC_MODE);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_AllOW_CALC_MODE entity)
        {
            try
            {
                entity.useR_ENTDT = DateTime.Now;
                entity.dtag = "";
                entity.useR_NAME_NEW = "";
                entity.ptype = "";


                M_ALLOW_CALC_MODE_Dto _Dto = new M_ALLOW_CALC_MODE_Dto();
                _Dto.m_allowcalcmode = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_AllOW_CALC_MODE>>("/api/AllowCalcMode/GetAll");
                   var dupcalmode = responce2.Data.Where(x => x.pK_CALC_MODE == entity.pK_CALC_MODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.pK_CALC_MODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_ALLOW_CALC_MODE_Dto>>("/api/AllowCalcMode/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_ALLOW_CALC_MODE_Dto>>("/api/AllowCalcMode/SaveOrUpdateAll", _Dto);
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