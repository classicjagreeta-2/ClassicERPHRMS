using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HRMS.Models;
using HRMS.Models.Master;
using Newtonsoft.Json;
using PagedList;
using static HRMS.Models.Model_Common;

namespace HRMS.Controllers.Master
{
    public class Reason_of_LeaveController : Controller
    {
        // GET: Reason_of_Leave
        public async Task<ActionResult> Reason_of_Leave(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            try
            {
                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce = await Services.GetAsync<PageModel<M_REASON_OF_LEAVING>>(string.Format("/api/ReasonOfLeaving/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                var usersAsIPagedList = new StaticPagedList<M_REASON_OF_LEAVING>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
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
            var typeList = new List<ModelMaster>
            {
            new ModelMaster { Value = "E", Text = "EMPLOYEEMASTER" },
            new ModelMaster { Value = "L", Text = "LEAVE" }
            };
            List<ModelMaster> Lst = typeList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text

            }).ToList();
            ViewBag.typeList = Lst;
            M_REASON_OF_LEAVING model = new M_REASON_OF_LEAVING();
            if (Id > 0)
            {
                var response = await Services.GetAsync<M_REASON_OF_LEAVING>("/api/ReasonOfLeaving/GetByID?id=" + Id);
                model = response.Data;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_REASON_OF_LEAVING entity)
        {
            try
            {
                entity.useR_ENTDT_NEW = DateTime.Now;
                entity.useR_NAME_NEW = "";
                entity.dtag = "";
                entity.ptype = "S";
                
                M_REASON_OF_LEAVING_Dto _Dto = new M_REASON_OF_LEAVING_Dto();
                _Dto.m_reasonofleaving = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_REASON_OF_LEAVING>("/api/ReasonOfLeaving/GetByCode?search=" + entity.pK_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_CODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {

                        var responce = await Services.PostAsync<List<M_REASON_OF_LEAVING_Dto>>("/api/ReasonOfLeaving/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_REASON_OF_LEAVING_Dto>>("/api/ReasonOfLeaving/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("Reason_of_Leave");
        }
    }
}