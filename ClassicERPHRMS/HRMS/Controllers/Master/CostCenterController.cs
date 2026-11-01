//using iTextSharp.text.pdf.qrcode;
using HRMS.Models;
using HRMS.Models.Master;
using PagedList;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HRMS.Controllers.Master
{
    public class CostCenterController : Controller
    {

        // GET: CostCenter
        public async Task<ActionResult> CostCenter(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var responce1 = await Services.GetAsync<PageModel<M_COST_CENTRE>>(string.Format("/api/CostCentre/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
            if (responce1.IsSuccessStatusCode)
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
            var usersAsIPagedList = new StaticPagedList<M_COST_CENTRE>(responce1.Data.value.results, pageNumber, pageSize, responce1.Data.value.rowCount);
            return View(usersAsIPagedList);
            }
            else
            {
                TempData["toastrError"] = responce1.Message;
                return View();
            }
        }

        public async Task<ActionResult> Cost_CenterAddEdit(int Id = 0)
        {
            M_COST_CENTRE _COST_CENTRE = new M_COST_CENTRE();
            if (Id > 0)
            {
                var mst_dept = await Services.GetAsync<M_COST_CENTRE>("/api/CostCentre/GetByID?id=" + Id);
                _COST_CENTRE = mst_dept.Data;
                return View("Cost_CenterAddEdit", _COST_CENTRE);
            }
            else
            {
                return View("Cost_CenterAddEdit", _COST_CENTRE);
            }
        }

       
        public async Task<ActionResult> AddOrUpdate(M_COST_CENTRE entity)
        {
            try
            {
               
                entity.user_entdt_new=DateTime.Now;
              
               
                
                entity.user_name_new = "";
                entity.dtag = "";
                entity.ptype = "S";
                
                M_COST_CENTRE_Dto _Dto = new M_COST_CENTRE_Dto();
                _Dto.m_costcentre = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_COST_CENTRE>("/api/CostCentre/GetByCode?search=" + entity.pK_CODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_CODE + " Dublicate Data Found";
                        return RedirectToAction("Cost_CenterAddEdit");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_COST_CENTRE_Dto>>("/api/CostCentre/SaveOrUpdateAll", _Dto);
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
                    var responce = await Services.PostAsync<List<M_COST_CENTRE_Dto>>("/api/CostCentre/SaveOrUpdateAll", _Dto);
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
            return RedirectToAction("CostCenter");
        }
    }
}