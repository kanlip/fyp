using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FYP.Models;
using System.Dynamic;
using System.Data.Common;
using System.Data.SqlClient;



// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP.Controllers
{
    public class AppointmentController : Controller
    {
        private AppDbContext _dbContext;

        public AppointmentController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Staff curStaff()
        {
            Staff cStaff = HttpContext.Session.GetObject<Staff>("staff");
            return cStaff;
        }
        /*
        [HttpGet]
        public IActionResult Get()
        {
            var result = from appt in _dbContext.Appointment
                         join cus in _dbContext.Customer on appt.AppId equals cus.CustomerId
                         into Details
                         from defaultVal in Details.DefaultIfEmpty()
                         select new
                         {
                             AppId = appt.AppId,
                             AppDate = appt.AppDate,
                             AppTime = appt.AppTime,
                             AppStatus = appt.AppStatus,
                             AppDetail = appt.AppDetail,
                         };
            ViewData["results"] = result;
            return View();

        } */
        [HttpGet]
        public IActionResult Get()
        {
            var result = from appt in _dbContext.Appointment
                         join cus in _dbContext.Customer on appt.CusId equals cus.CustomerId
                         into Details
                         from defaultVal in Details.DefaultIfEmpty()
                         select new
                         {
                             AppId = appt.AppId,
                             AppDate = appt.AppDate,
                             AppStatus = appt.AppStatus,
                             AppDetail = appt.AppDetail,
                         };
            ViewData["results"] = result;
            return View();

        }


        /*
        public IActionResult Index()
        {
            if (curStaff() != null)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                List<Appointment> model = dbs.Where(a => a.StaffId == curStaff().StaffId).OrderBy(a => a.AppDate)
                 .ThenBy(a => a.AppId)
                  .ToList();
                return View(model);
            }
            else
                return RedirectToAction("Authenticate", "Account");
        } */
        /*
        public IActionResult Index()
        {
            if (curStaff() != null)
            {
                var vm = new ViewModel();
                vm.Appointments = _dbContext.Appointment.ToList();
                vm.Customers = _dbContext.Customer.ToList();
                return View(vm);
            }
            else
                return RedirectToAction("Authenticate", "Account");
        }
        */


        public IActionResult Index()
        {
            if (curStaff() != null)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                List<Appointment> model = dbs.Where(a => a.StaffId == curStaff().StaffId).OrderBy(a => a.AppDate)
                 .ThenBy(a => a.AppId)
                  .ToList();

                var vm = new ViewModel();
                vm.Appointments = model;
                vm.Customers = _dbContext.Customer.ToList();
                return View(vm);
            }
            else
                return RedirectToAction("Authenticate", "Account");
        }


        public IActionResult PastIndex()
        {
            if (curStaff() != null)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                List<Appointment> model = dbs.Where(a => a.StaffId == curStaff().StaffId).OrderBy(a => a.AppDate)
                 .ThenBy(a => a.AppId)
                  .ToList();

                var vm = new ViewModel();
                vm.Appointments = model;
                vm.Customers = _dbContext.Customer.ToList();
                return View(vm);
            }
            else
                return RedirectToAction("Authenticate", "Account");
        }

        

        public IActionResult Update(int id)
        {
            if (curStaff() != null)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                Appointment tAppointment = dbs.Where(p => p.AppId == id)
                                      .FirstOrDefault();

                if (tAppointment != null)
                {
                    return View(tAppointment);
                }
                else
                    TempData["Msg"] = "Appointment not found!";

                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authenticate", "Account");
        }

        /*
        [HttpPost]
        public ActionResult Update(Appointment selAppointment)
        {
            if (ModelState.IsValid)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                //Appointment app = dbs.Where(appt => appt.AppId == selAppointment.AppId).FirstOrDefault()
                Appointment app = dbs.Where(appt => appt.AppId == selAppointment.AppId).FirstOrDefault();

                
                DbSet<Customer> dbc = _dbContext.Customer;
                Customer cus = dbc.Where(p => p.CustomerId == app.CusId).FirstOrDefault();
                
                if (app != null )
                {
                    //app.AppId = selAppointment.AppId;
                    app.AppTitle = selAppointment.AppTitle;
                    app.AppDate = selAppointment.AppDate;
                    app.AppDetail = selAppointment.AppDetail;
                    app.AppColor = selAppointment.AppColor;

                    if (_dbContext.SaveChanges() == 1)
                    {
                        
                        TempData["Msg"] = "Successfully updated Appointment details!";
                    }
                    else
                    {
                        TempData["Msg"] = "Failed to update database!";
                    }
                }
                else
                {
                    TempData["Msg"] = "Unable to Update Appointment!";
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered!";
            }
            return RedirectToAction("Index");
        }
        */

        [HttpPost]
        public ActionResult Update(Appointment selAppointment)
        {
            if (ModelState.IsValid)
            {
                DbSet<Appointment> dbs = _dbContext.Appointment;
                Appointment app = dbs.Where(appt => appt.AppDate == selAppointment.AppDate).FirstOrDefault();
                Appointment test = dbs.Where(appt => appt.AppId == selAppointment.AppId).FirstOrDefault();

                DbSet<Customer> dbc = _dbContext.Customer;
                Customer cus = dbc.Where(p => p.CustomerId == test.CusId).FirstOrDefault();

                if (app == null)
                {
                    test.AppTitle = selAppointment.AppTitle;
                    test.AppDate = selAppointment.AppDate;
                    test.AppDetail = selAppointment.AppDetail;
                    test.AppColor = selAppointment.AppColor;

                    if (_dbContext.SaveChanges() == 1)
                    {


                        TempData["MsgS"] = "Successfully updated Appointment details!";
                    }
                    else
                    {
                        TempData["Msg"] = "Failed to update database!";
                    }
                }
                else
                {
                    TempData["Msg"] = "Unable to Update Appointment!";
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered!";
            }
            return RedirectToAction("Index");
        }



        /*
        public IActionResult GetEvents()
        {
            var events = _dbContext.Events.ToList();
            return new JsonResult(events);
            //return new JsonResult(events{ Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
        }
        */

        // this one can work

        /*
    public IActionResult GetEvents()
    {
        var appointments = _dbContext.Appointment.ToList();
        return new JsonResult(appointments);
        //return new JsonResult(events{ Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
    } */


        public IActionResult GetEvents()
        {
            DbSet<Appointment> dbs = _dbContext.Appointment;
            List<Appointment> model = dbs.Where(a => a.StaffId == curStaff().StaffId).ToList();

            var appointments = model;
            return new JsonResult(appointments);
            //return new JsonResult(events{ Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet }
        }


        public IActionResult ViewFeedBack(int id)
        {
            if (curStaff() != null)
            {
                DbSet<Feedback> dbs = _dbContext.Feedback;
                Feedback tFeedback = dbs.Where(p => p.Id == id)
                                      .FirstOrDefault();

                if (tFeedback != null)
                {
                    return View(tFeedback);
                }
                else
                    TempData["Msg"] = "Feedback not found!";

                return RedirectToAction("PastIndex");
            }
            else
                return RedirectToAction("Authenticate", "Account");


        }
        [HttpPost]
        public ActionResult ViewFeedBack(Feedback selFeedback)
        {
            if (ModelState.IsValid)
            {
                DbSet<Feedback> dbs = _dbContext.Feedback;
                Feedback tFeedback = dbs.Where(fb => fb.Id == selFeedback.Id).FirstOrDefault();

                if (tFeedback != null)
                {
                    //tFeedback.Id = selFeedback.Id;
                    tFeedback.FeedBackTime = selFeedback.FeedBackTime;
                    tFeedback.CustomerComment = selFeedback.CustomerComment;
                    tFeedback.StaffComment = selFeedback.StaffComment;
                    //tFeedback.AId = selFeedback.AId;
                    //tFeedback.CId = selFeedback.CId;

                    if (_dbContext.SaveChanges() == 1)
                    {
                        TempData["MsgS"] = "Successfully updated Appointment details!";
                    }
                    else
                    {
                        TempData["Msg"] = "Failed to update database!";
                    }
                }
                else
                {
                    TempData["Msg"] = "Appointment not found!";
                }
            }
            else
            {
                TempData["Msg"] = "Invalid information entered!";
            }
            return RedirectToAction("PastIndex");
        }

        public IActionResult Report()
        {
            if (curStaff() != null && curStaff().Role.Equals("P"))
            {

                String test = String.Format(@"SELECT a.cus_id , COUNT(*) as 'Count' ,c.Name
                                            FROM Appointment a, Customer c, Staff s
                                            WHERE a.cus_id = c.CustomerId
                                            AND s.staffId = a.Staff_id
											AND a.Staff_id = {0}
											AND a.AppColor='cyan'
                                            GROUP BY a.cus_id, c.Name
                                            ORDER BY COUNT(*) DESC; ", curStaff().StaffId);

                var list = DBUtl.GetList(test);
                ViewData["reports"] = list;

            }
            return View();

        }

        public IActionResult ViewCustomer(int id)
        {
            if (curStaff() != null)
            {
                DbSet<Customer> dbs = _dbContext.Customer;
                Customer tCustomer = dbs.Where(p => p.CustomerId == id)
                                      .FirstOrDefault();

                if (tCustomer != null)
                {
                    return View(tCustomer);
                }
                else
                    TempData["Msg"] = "Customer not found!";

                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Authenticate", "Account");
        }

    }
}
