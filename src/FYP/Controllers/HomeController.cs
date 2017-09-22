using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Data.SqlClient;

namespace FYP.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
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
            
            DbSet<CorporateProfile> dbs = _dbContext.CorporateProfile;
            List<CorporateProfile> model = dbs.OrderBy(a => a.Cpid).ToList();
            return View(model);
        }
       
    }
}