using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HRMS.Models.Master;
using HRMS.Models;
using PagedList;
using System.Threading.Tasks;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class M_PtaxController : Controller
    {
        // GET: M_Ptax
        public async Task<ActionResult> M_Ptax(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_PTAX>>(string.Format("/api/Ptax/GetByPage?page={0}&pageSize={1}", page, pageSize)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_PTAX>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
                    var stateData = await Services.GetAsync<List<M_STATE>>("/api/State/GetAll");
                    foreach (var item in usersAsIPagedList)
                    {
                        var statenamae = stateData.Data.FirstOrDefault(u => u.pK_STATE_CODE == item.fK_STATE_CODE);
                        if (statenamae != null)
                        {
                            item.fK_STATE_CODE = statenamae.pK_STATE_CODE + " , " + statenamae.descn;
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
            try
            {
                var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
                List<ModelMaster> unitDp = unit.Data.Select(x => new ModelMaster
                {
                    Value = x.code,
                    Text = x.code + "-" + x.name

                }).ToList();
                ViewBag.Unit = unitDp;
                var state = await Services.GetAsync<List<M_STATE>>("/api/State/GetAll");
                List<ModelMaster> Lst = state.Data.Select(x => new ModelMaster
                {
                    Value = x.pK_STATE_CODE,
                    Text = x.pK_STATE_CODE + "-" + x.name

                }).ToList();
                ViewBag.State = Lst;
                M_PTAX model = new M_PTAX();
                if (Id > 0)
                {
                    var response = await Services.GetAsync<M_PTAX>("/api/Ptax/GetByID?id=" + Id);
                    model = response.Data;
                    return View("AddEditModal", model);
                }
                else
                {
                    return View("AddEditModal", model);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_PTAX entity)
        {
            //int i = 1;
            try
            {
              
                entity.useR_NAME_NEW = "";
                
                entity.useR_ENTDT_NEW = DateTime.Now;
              
                entity.dtag = "";
                if (entity.amounT_PERCENT == null)
                {

                    entity.amounT_PERCENT = 0m;
                }
                entity.ptype = "S";
                //entity.pK_PTAX_CODE = "POO"+i++ ;
                entity.pK_PTAX_CODE = "";
                M_PTAX_DETAILS _Dto = new M_PTAX_DETAILS();
                _Dto.m_ptax = entity;
                var responce = await Services.PostAsync<List<M_PTAX_DETAILS>>("/api/Ptax/SaveOrUpdateAll", _Dto);
                //var responce = await Services.PostAsync<M_PTAX>("/api/Ptax/SaveOrUpdateAll", _Dto);
                if (responce.IsSuccessStatusCode == true)
                {
                    TempData["toastrSuccess"] = "Data has been successfully submitted";
                   
                }
                else
                {
                    TempData["toastrError"] =  responce.Message;
                }
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("M_Ptax");
        }
    }
}