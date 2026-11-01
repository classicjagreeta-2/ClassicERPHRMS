    using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using HRMS.Models.Master;
using HRMS.Models;
using PagedList;
using System.Threading.Tasks;

namespace HRMS.Controllers.Master
{
    public class MastStateController : Controller
    {
        // GET: MastState
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce = await Services.GetAsync<PageModel<M_STATE>>(string.Format("/api/State/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString));
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
            var usersAsIPagedList = new StaticPagedList<M_STATE>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            M_STATE m_STATE = new M_STATE();
            if (Convert.ToInt32(Id) <= 0)
            {
                return View(m_STATE);
            }

            var state = await Services.GetAsync<M_STATE>("/api/State/GetByID?id=" + Id);
            m_STATE = state.Data;
            return View(m_STATE);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_STATE entity)
        {
            try
            {

                if (entity.ptaX_NO == null)
                {

                    entity.ptaX_NO = "";
                }
                
                if( entity.esI_NO == null) {

                    entity.esI_NO = "";

                }
                entity.user_name_new = "";
           
                entity.dtag = "";
                
                entity.ptype = "S";
            
                entity.user_entdt_new = DateTime.Now;
                M_STATE_Dto _Dto = new M_STATE_Dto();
                _Dto.m_state = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_STATE>>("/api/State/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.pK_STATE_CODE == entity.pK_STATE_CODE).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.pK_STATE_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                   
                    else
                    {
                        var responce = await Services.PostAsync<List<M_STATE_Dto>>("/api/State/SaveOrUpdateAll", _Dto);

                        if (Response.StatusCode == 200)
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
                    var responce = await Services.PostAsync<List<M_STATE_Dto>>("/api/State/SaveOrUpdateAll", _Dto);

                    if (Response.StatusCode == 200)
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