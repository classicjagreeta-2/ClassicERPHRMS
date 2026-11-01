using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

using HRMS_VIEWMODEL24;
using HRMS.Models.Master;
using HRMS.Models;
using PagedList;
using System.Threading.Tasks;
using static HRMS.Models.Model_Common;
using System.Security.Principal;
using System.Linq.Expressions;


namespace HRMS.Controllers.Master
{
    public class MastEmpController : Controller
    {
        // GET: MastEmp
        public async Task<ViewResult> Index(string sortOrder, string currentFilter, string searchString, int? page = 1, int pageSize = 25)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var responce = await Services.GetAsync<PageModel<M_EMPMAS>>(string.Format("/api/Employee/GetByPage?search={0}&page={1}&pageSize={2}", searchString, page, pageSize));
            if (!String.IsNullOrEmpty(searchString))
            {

            }
            switch (sortOrder)
            {

            }
            ViewBag.PageSize = pageSize;
            int pageNumber = (page ?? 1);
            var dataAsIPagedList = new StaticPagedList<M_EMPMAS>(responce.Data.value.results, pageNumber, pageSize, responce.Data.value.rowCount);
            return View(dataAsIPagedList);
        }
        public async Task<ActionResult> AddEditModal(string empcode, int Id = 0)
        {
            var bloodList = new List<ModelMaster>
            {
            new ModelMaster { Value = "1", Text = "B+" },
            new ModelMaster { Value = "2", Text = "B-." },
            new ModelMaster { Value = "3", Text = "A+" },
            new ModelMaster { Value = "4", Text = "A-" },
            new ModelMaster { Value = "5", Text = "AB+" },
            new ModelMaster { Value = "6", Text = "AB-" },
            new ModelMaster { Value = "7", Text = "O+" },
            new ModelMaster { Value = "8", Text = "O-" }
            };
            List<ModelMaster> blood = bloodList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text

            }).ToList();
            ViewBag.Blood = blood;


            var TitleList = new List<ModelMaster>
            {
            new ModelMaster { Value = "Mr", Text = "Mr." },
            new ModelMaster { Value = "Mrs", Text = "Mrs." },
            new ModelMaster { Value = "Ms", Text = "Ms." }
            };
            List<ModelMaster> Lst = TitleList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text

            }).ToList();
            ViewBag.UserTitle = Lst;

            var GenderList = new List<ModelMaster>
            {
            new ModelMaster { Value = "M", Text = "Male" },
            new ModelMaster { Value = "F", Text = "Female" },
            new ModelMaster { Value = "O", Text = "Other" }
            };
            List<ModelMaster> Gender = GenderList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text

            }).ToList();
            ViewBag.Gender = Gender;

            var PaymentList = new List<ModelMaster>
            {
            new ModelMaster { Value = "1", Text = "Cash" },
            new ModelMaster { Value = "2", Text = "Online" },
            new ModelMaster { Value = "3", Text = "Cheque" }
            };
            List<ModelMaster> Payment = PaymentList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text
            }).ToList();
            ViewBag.Payment = Payment;

            var YesNoList = new List<ModelMaster>
            {
            new ModelMaster { Value = "true", Text = "Yes" },
            new ModelMaster { Value = "false", Text = "No" }
            };
            List<ModelMaster> YesNo = YesNoList.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text
            }).ToList();
            ViewBag.YesNo = YesNo;


            var Actype = new List<ModelMaster>
            {
            new ModelMaster { Value = "S", Text = "SAVINGS" },
            new ModelMaster { Value = "C", Text = "CURRENT" }
            };
            List<ModelMaster> ACCOUNTTYPE = Actype.Select(x => new ModelMaster
            {
                Value = x.Value,
                Text = x.Value + "-" + x.Text
            }).ToList();
            ViewBag.Account = ACCOUNTTYPE;

            M_EMPMAS m_EMPMAS = new M_EMPMAS();

            var SHIFTCODE = await Services.GetAsync<List<M_SHIFT>>("/api/Shift/GetAll");
            List<ModelMaster> SHIFT = SHIFTCODE.Data.Select(x => new ModelMaster
            {
                Value = x.pK_SHIFT_CODE,
                Text = x.pK_SHIFT_CODE + "-" + x.description

            }).ToList();
            ViewBag.Shift = SHIFT;

            var REASONOFLEAVE = await Services.GetAsync<List<M_REASON_OF_LEAVING>>("/api/ReasonOfLeaving/GetAll");
            List<ModelMaster> LEAVE = REASONOFLEAVE.Data.Select(x => new ModelMaster
            {
                Value = x.pK_CODE,
                Text = x.pK_CODE + "-" + x.description

            }).ToList();
            ViewBag.Reason = LEAVE;

            var COUNTRY = await Services.GetAsync<List<M_COUNTRY>>("/api/Country/GetAll");
            List<ModelMaster> COUNTRYNAME = COUNTRY.Data.Select(x => new ModelMaster
            {
                Value = x.pK_COUNTRY_CODE,
                Text = x.pK_COUNTRY_CODE + "-" + x.description

            }).ToList();
            ViewBag.Country = COUNTRYNAME;





            //var Bh21responce2 = await Services.GetAsync<List<M_SHIFT>>(string.Format("/api/Shift/GetByUnit?unit={0}", m_EMPMAS.unit));
            //var Bh21responce2 = await Services.GetAsync<List<M_SHIFT>>(string.Format("/api/Shift/GetAll"));
            //ViewBag.Shift = Bh21responce2.Data.Where(k=>k.unit == m_EMPMAS.unit &&k.pK_SHIFT_CODE == m_EMPMAS.fK_SHIFT_CODE).ToList();


            if (Id > 0)
            {

                var m_EMPMASw2 = await Services.GetAsync<M_EMPMAS>("/api/Employee/GetByCode?EMPCODE=" + empcode);
                m_EMPMAS = m_EMPMASw2.Data;
                return View("AddEditModal", m_EMPMAS);
            }


            return View("AddEditModal", m_EMPMAS);
        }
        [HttpPost]
        public async Task<ActionResult> AddOrUpdate(M_EMPMAS entity)
        {
            try
            {


                entity.useR_NAME_NEW = "";


                entity.useR_ENTDT_NEW = DateTime.Now;

                entity.dtag = "";
                if (entity.title == null)
                {
                    entity.title = "";
                }
                if (entity.mname == null)
                {
                    entity.mname = "";
                }
                if (entity.shorT_NAME == null)
                {
                    entity.shorT_NAME = "";
                }
                if (entity.namE_IN_VALID_DOC == null)
                {
                    entity.namE_IN_VALID_DOC = "";
                }
                if (entity.g_TITLE == null)
                {
                    entity.g_TITLE = "";
                }
                if (entity.g_FNAME == null)
                {
                    entity.g_FNAME = "";
                }
                if (entity.g_LNAME == null)
                {
                    entity.g_LNAME = "";
                }
                if (entity.g_MNAME == null)
                {
                    entity.g_MNAME = "";
                }
                if (entity.perM_MOBILENO == null)
                {
                    entity.perM_MOBILENO = 0;
                }
                if (entity.emaiL_ID == null)
                {
                    entity.emaiL_ID = "";
                }
                if (entity.emeR_CONCT_MOBILE == null)
                {
                    entity.emeR_CONCT_MOBILE = "";
                }
                if (entity.emeR_CONCT_NAME == null)
                {
                    entity.emeR_CONCT_NAME = "";
                }
                if (entity.emeR_CONCT_RELATION == null)
                {
                    entity.emeR_CONCT_RELATION = "";
                }
                if (entity.chQ_CSH == null)
                {
                    entity.chQ_CSH = "";
                }
                if (entity.aadhaR_NO == null)
                {
                    entity.aadhaR_NO = "";
                }
                if (entity.voteR_ID == null)
                {
                    entity.voteR_ID = "";
                }
                if (entity.leG_CODE == null)
                {
                    entity.leG_CODE = "";
                }
                if (entity.dateofbirth == null)
                {
                    entity.dateofbirth = DateTime.Now;
                }
                if (entity.dateofleaving == null)
                {
                    entity.dateofleaving = null;
                }
                if (entity.lefT_SERVICE == null)
                {
                    entity.lefT_SERVICE = false;
                }

                if (entity.dog == null)
                {
                    entity.dog = DateTime.Now;
                }
                if (entity.fK_REASON_CODE == null)
                {
                    entity.fK_REASON_CODE = "";
                }
                if (entity.bloodgroup == null)
                {
                    entity.bloodgroup = "";
                }
                if (entity.tnno == null)
                {
                    entity.tnno = "";
                }
                if (entity.pfno == null)
                {
                    entity.pfno = "";
                }
                if (entity.doJ_EPS == null)
                {
                    entity.doJ_EPS = DateTime.Now;
                }
                if (entity.pensioN_NO == null)
                {
                    entity.pensioN_NO = "";
                }
                if (entity.doL_EPS == null)
                {
                    entity.doL_EPS = DateTime.Now;
                }
                if (entity.esino == null)
                {
                    entity.esino = "";
                }
                if (entity.doJ_EPF == null)
                {
                    entity.doJ_EPF = DateTime.Now;
                }
                if (entity.dog == null)
                {
                    entity.dog = DateTime.Now;
                }
                if (entity.pancardno == null)
                {
                    entity.pancardno = "";
                }
                if (entity.doL_EPF == null)
                {
                    entity.doL_EPF = DateTime.Now;
                }
                if (entity.uan == null)
                {
                    entity.uan = "";
                }
                if (entity.transferred == null)
                {
                    entity.transferred = DateTime.Now;
                }
                if (entity.transferredto == null)
                {
                    entity.transferredto = "";
                }
                if (entity.passportno == null)
                {
                    entity.passportno = "";
                }
                if (entity.passportissuedon == null)
                {
                    entity.passportissuedon = DateTime.Now;
                }
                if (entity.p_RENEWALDATE == null)
                {
                    entity.p_RENEWALDATE = null;
                }
                if (entity.pasprT_ISD_FRM == null)
                {
                    entity.pasprT_ISD_FRM = "";
                }
                if (entity.perM_HOUSENO == null)
                {
                    entity.perM_HOUSENO = "";
                }
                if (entity.perM_STREETNAME1 == null)
                {
                    entity.perM_STREETNAME1 = "";
                }
                if (entity.perM_STREETNAME2 == null)
                {
                    entity.perM_STREETNAME2 = "";
                }
                if (entity.perM_CITY == null)
                {
                    entity.perM_CITY = "";
                }
                if (entity.perM_PINCODE == null)
                {
                    entity.perM_PINCODE = 0;
                }
                if (entity.fK_COUNTRY_CODE_PERM == null)
                {
                    entity.fK_COUNTRY_CODE_PERM = "";
                }
                if (entity.perM_RESTEL1 == null)
                {
                    entity.perM_RESTEL1 = 0;
                }
                if (entity.perM_RESTEL2 == null)
                {
                    entity.perM_RESTEL2 = 0;
                }
                if (entity.temP_HOUSENO == null)
                {
                    entity.temP_HOUSENO = "";
                }
                if (entity.temP_STREETNAME1 == null)
                {
                    entity.temP_STREETNAME1 = "";
                }
                if (entity.temP_STREETNAME2 == null)
                {
                    entity.temP_STREETNAME2 = "";

                }
                if (entity.temP_CITY == null)
                {
                    entity.temP_CITY = "";
                }
                if (entity.temP_PINCODE == null)
                {
                    entity.temP_PINCODE = 7;
                }
                if (entity.fK_COUNTRY_CODE_TEMP == null)
                {
                    entity.fK_COUNTRY_CODE_TEMP = "";
                }
                if (entity.temP_RESTEL1 == null)
                {
                    entity.temP_RESTEL1 = +91;
                }
                if (entity.temP_RESTEL2 == null)
                {
                    entity.temP_RESTEL2 = +91;
                }
                if (entity.temP_MOBILENO == null)
                {
                    entity.temP_MOBILENO = +91;
                }
                if (entity.temP_POSTOFF == null)
                {
                    entity.temP_POSTOFF = "";

                }
                if (entity.temP_POLICEST == null)
                {
                    entity.temP_POLICEST = "";
                }
                if (entity.banK_NAME == null)
                {
                    entity.banK_NAME = "";
                }
                if (entity.fK_COUNTRY_CODE_BANK == null)
                {
                    entity.fK_COUNTRY_CODE_BANK = "";
                }
                if (entity.branch == null)
                {
                    entity.branch = "";
                }
                if (entity.aC_NO == null)
                {
                    entity.aC_NO = "";
                }
                if (entity.svngS_CRNT == null)
                {
                    entity.svngS_CRNT = "";
                }
                if (entity.ibaN_NO == null)
                {
                    entity.ibaN_NO = "";
                }
                if (entity.acode == null)
                {
                    entity.acode = "";
                }
                if (entity.reF_CODE == null)
                {
                    entity.reF_CODE = "";
                }
                if (entity.ssubcode == null)
                {
                    entity.ssubcode = "";
                }
                if (entity.sacode == null)
                {
                    entity.sacode = "";

                }


                entity.ifsC_CODE = "";
                entity.svngS_CRNT = "";
                entity.swifT_CODE = "";
                entity.whatsapP_NO = "";
                entity.perM_POSTOFF = "";
                entity.fK_STATE_CODE = "";
                entity.perM_POLICEST = "";
                entity.emP_IMAGE_PATH = "";
                entity.fK_REASON_CODE = "";
                entity.fK_COUNTRY_CODE_EMP = "";
                entity.fK_COUNTRY_CODE_BANK = "";
                entity.fK_COUNTRY_CODE_PERM = " ";
                entity.fK_COUNTRY_CODE_TEMP = " ";

                entity.tnno = "";
                entity.g_LNAME = "";
                entity.shorT_NAME = "";
                entity.emeR_CONCT_RELATION = "";

                entity.ptype = "S";
                M_EMPMAS_Dto _Dto = new M_EMPMAS_Dto();
                _Dto.m_empmas = entity;
                if (entity.id == 0)
                {
                    var responce2 = await Services.GetAsync<M_EMPMAS>("/api/Employee/GetByCode?EMPCODE=" + entity.pK_EMPCODE);
                    if (responce2.Data != null)
                    {
                        TempData["toastrError"] = responce2.Data.pK_EMPCODE + " Dublicate Data Found";
                        return RedirectToAction("AddEditModal");
                    }
                    else
                    {
                        var responce = await Services.PostAsync<List<M_EMPMAS_Dto>>("/api/Employee/SaveOrUpdateAll", _Dto);
                        if (responce.IsSuccessStatusCode == true)
                        {
                            TempData["toastrSuccess"] = "Data has been successfully submitted";
                        }
                        else
                        {
                            TempData["toastrError"] = "Error " + responce.Message;
                        }

                    }
                }
                else
                {
                    var responce = await Services.PostAsync<List<M_EMPMAS_Dto>>("/api/Employee/SaveOrUpdateAll", _Dto);
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