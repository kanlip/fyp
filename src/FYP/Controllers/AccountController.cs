using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System.Web;

using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FYP.Controllers
{
    public class AccountController : Controller
    {
        private AppDbContext _dbContext;

        public AccountController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Staff curStaff()
        {
            Staff cStaff = HttpContext.Session.GetObject<Staff>("staff");
            return cStaff;
        }

        private Customer curCustomer()
        {
            Customer cCus = HttpContext.Session.GetObject<Customer>("customer");
            return cCus;
        }

        [HttpGet]
        public IActionResult Authenticate()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Authenticate(Login login)
        {
            if (curStaff() == null || curCustomer() == null)
            {
                login.UserId = login.UserId.Replace("'", "''"); // prevent sql injection
                login.Password = login.Password.Replace("'", "''"); // prevent sql injection

                string sql =
                    String.Format(@"SELECT * FROM Staff 
                           WHERE Email = '{0}'
                                 AND Password = HASHBYTES('SHA1', '{1}')", login.UserId, login.Password);
                
                DbSet<Staff> dbs = _dbContext.Staff;
                Staff staff = dbs.FromSql(sql).FirstOrDefault();

                string sqlc =
                   String.Format(@"SELECT * FROM Customer 
                           WHERE Email = '{0}'
                                 AND Password = HASHBYTES('SHA1', '{1}')", login.UserId, login.Password);
                DbSet<Customer> db = _dbContext.Customer;
                Customer cus = db.FromSql(sqlc).FirstOrDefault();
                if (staff != null)
                {
                    
                    staff.Password = null;
                    HttpContext.Session.SetObject("staff", staff);
                    return RedirectToAction("Index", "Home");
                }
                if(cus!=null)
                {
                    cus.Password = null;
                    HttpContext.Session.SetObject("customer", cus);
                    return RedirectToAction("Index", "Home");
                }
              
                ViewData["msg"] = "Login failed";
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
