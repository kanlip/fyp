using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FYP.Models;

namespace FYP.Controllers
{
    public class CustomerController : Controller
    {
        private AppDbContext _dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int add = DBUtl.ExecSQL(@"INSERT INTO Customer(Name,Gender,Address,PhoneNo,Email,Password,HomeNo,DOB,NextofKin,KinNo)
                                            VALUES('{0}','{1}','{2}',{3},'{4}',HASHBYTES('SHA1','{5}'),{6},'{7}','{8}',{9})",
                                            customer.Name,customer.Gender,customer.Address,customer.PhoneNo,customer.Email, Request.Form["conpassword"].ToString(),
                                            customer.HomeNo,customer.Dob,customer.NextofKin,customer.KinNo);
                    if(add == 1)
                    {
                        string template = @"HI {0},<br/><br/>
                                    Your account has been <b>Created</b>.<br/><br/>
                                    Your Login ID is {1} <br/>
                                    Your Password is {2}
                                    <br/><br/><text style='color:#58bd91'>Aptive Physiotherapy</text>";

                        string title = String.Format(@"Account Created {0}", customer.Name);

                        string message = String.Format(template, customer.Name, customer.Email, Request.Form["conpassword"].ToString());
                        string result;
                        EmailUtl.SendEmail(customer.Email, title, message, out result);
                        TempData["MsgS"] = "Sign up complete";
                    }
                    else
                    {
                        TempData["Error"] = "Fail to update database";
                    }
                }
                catch(Exception ex)
                {
                    return RedirectToAction("Index","Home");
                }
            }
            return RedirectToAction("Index", "Home");

        }
    }
}
