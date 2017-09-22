using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;



namespace FYP.Controllers
{
    public class AdministratorController : Controller
    {
        private AppDbContext _dbContext;

        public AdministratorController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Staff curStaff()
        {
            Staff cStaff = HttpContext.Session.GetObject<Staff>("staff");
            return cStaff;
        }
        public IActionResult Index()
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                DbSet<CorporateProfile> dbs = _dbContext.CorporateProfile;
                List<CorporateProfile> model = dbs.OrderBy(a => a.Cpid).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }
        public IActionResult Edit(string id)
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                DbSet<CorporateProfile> dbs = _dbContext.CorporateProfile;
                CorporateProfile profile = dbs.Where(p => p.Cpid == id)
                                      .FirstOrDefault();

                if (profile != null)
                {
                    return View(profile);
                }
                else
                    TempData["Msg"] = "Patient not found!";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }
        [HttpPost]
        public IActionResult Edit(CorporateProfile profile)
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                if (ModelState.IsValid)
                {
                    DbSet<CorporateProfile> dbs = _dbContext.CorporateProfile;
                    CorporateProfile cprofile = dbs.Where(p => p.Cpid == profile.Cpid)
                                          .FirstOrDefault();

                    if (cprofile != null)
                    {
                        cprofile.Name = profile.Name;
                        cprofile.AboutUs = profile.AboutUs;
                        cprofile.Address = profile.Address;
                        cprofile.Contact = profile.Contact;
                        cprofile.Defination = profile.Defination;
                        cprofile.Services = profile.Services;
                        cprofile.Vision = profile.Vision;
                        cprofile.Email = profile.Email;
                        cprofile.Fblink = profile.Fblink;
                        cprofile.Twitlink = profile.Twitlink;
                        cprofile.Linkinlink = profile.Linkinlink;

                        string msg = "";
                        if (_dbContext.SaveChanges() == 1)
                        {
                            msg = "Corporate Profile updated. ";
                            TempData["MsgS"] = msg;
                        }

                        else if (_dbContext.SaveChanges() != 1)
                        {
                            TempData["Msg"] = "No Changes Information same as before";
                        }

                    }
                    else
                        TempData["Msg"] = "Patient not found!";

                }
                else
                    TempData["Msg"] = "Invalid input!";
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }
        public IActionResult Report()
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                var list = DBUtl.GetList(@"SELECT a.cus_id , COUNT(*) as 'Count' ,c.Name
                                            FROM Appointment a, Customer c
                                            WHERE a.cus_id = c.CustomerId
                                            AND a.AppStatus='A'
                                            GROUP BY a.cus_id, c.Name
                                            ORDER BY COUNT(*) DESC; ");
                ViewData["reports"] = list;
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }


        }
        public IActionResult WReport()
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                var list = DBUtl.GetList(@"SELECT a.cus_id , COUNT(*) as 'Count' ,c.Name 
                                            FROM Appointment a, Customer c
                                            WHERE a.cus_id = c.CustomerId
                                            AND a.AppStatus='A'
                                            AND DATEDIFF(day, a.AppDate, CURRENT_TIMESTAMP) <=7
                                            GROUP BY a.cus_id, c.Name
                                            ORDER BY COUNT(*) DESC;");
                ViewData["Wreports"] = list;
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }
        public IActionResult MReport()
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                var list = DBUtl.GetList(@"SELECT a.cus_id , COUNT(*) as 'Count' ,c.Name 
                                        FROM Appointment a, Customer c
                                        WHERE a.cus_id = c.CustomerId
                                        AND a.AppStatus='A'
                                        AND DATEDIFF(day, a.AppDate, CURRENT_TIMESTAMP) <=31
                                        GROUP BY a.cus_id, c.Name
                                        ORDER BY COUNT(*) DESC;");
                ViewData["Mreports"] = list;
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }


    }
}





