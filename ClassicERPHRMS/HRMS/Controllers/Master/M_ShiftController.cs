using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class M_ShiftController : Controller
    {
        // GET: M_Shift
        public async Task<ActionResult> M_Shift(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_SHIFT>>(string.Format("/api/Shift/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_SHIFT>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
        //[HttpPost]
        public async Task<ActionResult> AddEditModal(int Id = 0)
        {
            var minutes = new List<int>();
            for (int i = 0; i < 60; i++)
            {
                minutes.Add(i);
            }
            ViewBag.MinutesList = new SelectList(minutes);

            var unit = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
            List<ModelMaster> uu = unit.Data.Select(x => new ModelMaster
            {
                Value = x.code,
                Text = x.code + "-" + x.name

            }).ToList();
            ViewBag.Unit = uu;
            M_SHIFT model = new M_SHIFT();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_SHIFT>("/api/Shift/GetByID?id=" + Id);
                model = response.Data;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_SHIFT entity)
        {
            try
            {            
                entity.saT_END_AT = "";
                entity.generaL_SHIFT = "";               
                entity.useR_NAME_NEW = "";                
                entity.useR_ENTDT_NEW = DateTime.Now;                
                entity.dtag = "";                
                entity.ptype = "S";
                M_SHIFT_Dto _Dto = new M_SHIFT_Dto();
                _Dto.m_shift = entity;
                var responce = await Services.PostAsync<List<M_SHIFT_Dto>>("/api/Shift/SaveOrUpdateAll", _Dto);
                if (Response.StatusCode == 200)
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
            return RedirectToAction("M_Shift");
        }
    }
}
