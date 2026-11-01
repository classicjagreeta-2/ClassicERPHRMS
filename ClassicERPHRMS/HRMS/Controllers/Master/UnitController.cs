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
using toastr.Net.OptionEnums;
using toastr.Net;
using System.Text.RegularExpressions;

namespace HRMS.Controllers.Master
{
    public class UnitController : Controller
    {
        public async Task<ActionResult> Unit(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            var sas = Request.Headers["User-Agent"];
            ViewBag.MessageToster = Notification.Show("System of down hh", "Error", type: ToastType.Error);

            try
            {


                ViewBag.CurrentSort = sortOrder;
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
                var responce1 = await Services.GetAsync<PageModel<M_UNIT>>(string.Format("/api/Unit/GetByPage?page={0}&pageSize={1}&search={2}", page, pageSize, searchString)); // Helpers.UserInfo.ModuleCode
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
                    var usersAsIPagedList = new StaticPagedList<M_UNIT>(responce1.Data.value.results, pageNumber, pageSize, responce1.Data.value.rowCount);
                    return View(usersAsIPagedList);
                }
                else
                {
                    TempData["toastrError"] = responce1.Message;
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        //[HttpPost]
        public async Task<ActionResult> Unit_AddEdit(int Id = 0)
        {
            //var getState = await Services.GetAsync<MAST_STATE>("/api/State/GetAll");
            M_UNIT unit = new M_UNIT();
            if (Id > 0)
            {
                var mst_dept = await Services.GetAsync<M_UNIT>("/api/Unit/GetByID?id=" + Id);
                unit = mst_dept.Data;
            }
            return View("Unit_AddEdit", unit);
        }

        [HttpPost]

        public async Task<ActionResult> Unit_AddEdit(M_UNIT entity)
        {

            try
            {
                if (ModelState.IsValid)

                    entity.useR_NAME_NEW = "";

                entity.useR_ENTDT_NEW = DateTime.Now;
                entity.oS_USER = "";

                entity.dtag = "";
                if (entity.city == null)
                {
                    entity.city = "";
                }
                entity.ptype = "S";
                entity.dire = "";
                entity.cstdt = DateTime.Now;
                entity.lstdt = DateTime.Now;
                M_UNIT_DTO _Dto = new M_UNIT_DTO();
                _Dto.m_unit = entity;

                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<List<M_UNIT>>("/api/Unit/GetAll");
                    var dupcalmode = responce2.Data.Where(x => x.code == entity.code).FirstOrDefault();

                    if (dupcalmode != null)
                    {
                        TempData["toastrError"] = dupcalmode.code + " Dublicate Data Found";
                        return RedirectToAction("Unit_AddEdit");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_WEEKLY_OFF_Dto>>("/api/Unit/SaveOrUpdateAll", _Dto);

                        if (responce.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                            return RedirectToAction("Unit");
                        }
                        else
                        {
                            var Message = responce.Message.Split(':').Take(3).Select(i => i).Aggregate((i, j) => i + " " + j);
                            TempData["toastrError"] = Message.ToString();
                        }
                    }
                }
                else
                {
                    var responce = await Services.PostAsync<List<M_WEEKLY_OFF_Dto>>("/api/Unit/SaveOrUpdateAll", _Dto);

                    if (responce.IsSuccessStatusCode == true)
                    {
                        TempData["toastrSuccess"] = "Data has been successfully submitted";
                        return RedirectToAction("Unit");
                    }
                    else
                    {
                        var Message = responce.Message.Split(':').Take(3).Select(i => i).Aggregate((i, j) => i + " " + j);
                        TempData["toastrError"] = Message.ToString();
                    }

                }

                return View(entity);
            }
            catch (Exception ex)
            {
                TempData["toastrError"] = ex.Message;
            }
            return RedirectToAction("Unit");
        }










    }
}