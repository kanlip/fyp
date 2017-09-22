using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using GemBox.Spreadsheet;




namespace FYP.Controllers
{
    public class StaffController : Controller
    {
        private AppDbContext _dbContext;

        private IHostingEnvironment _env;

        public StaffController(AppDbContext dbContext, IHostingEnvironment environment)
        {
            _dbContext = dbContext;
            _env = environment;

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
                DbSet<Staff> dbs = _dbContext.Staff;
                List<Staff> model = dbs.OrderBy(a => a.Role).ToList();
                var list = DBUtl.GetList(@"SELECT Staff.Name, Staff.StaffId FROM Staff, Appointment
                                            WHERE Staff.StaffId = Appointment.Staff_id
                                            AND AppStatus='A'
                                            AND AppDate >= Convert(datetime, CURRENT_TIMESTAMP )
                                            GROUP BY Name , StaffId;");
                ViewData["StaffwA"] = list;


                return View(model);
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");  
            }

        }

        public IActionResult Edit(int id)
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                DbSet<Staff> dbs = _dbContext.Staff;
                Staff profile = dbs.Where(p => p.StaffId == id)
                                      .FirstOrDefault();


                if (profile != null)
                {

                    ViewData["pic"] = profile.Email;
                    return View(profile);
                }
                else
                    TempData["Msg"] = "Staff not found!";

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }
        [HttpPost]
        public IActionResult Edit(Staff profile, IFormFile file)
        {
            if (curStaff() != null && curStaff().Role == "A")
            {


                if (ModelState.IsValid)
                {
                    DbSet<Staff> dbs = _dbContext.Staff;
                    Staff cprofile = dbs.Where(p => p.StaffId == profile.StaffId)
                                          .FirstOrDefault();

                    if (cprofile != null)
                    {

                        cprofile.Name = profile.Name;
                        cprofile.PhoneNo = profile.PhoneNo;
                        cprofile.Role = profile.Role;
                        cprofile.Email = profile.Email;

                        string dateOB = $"{profile.Dob:yyyy-MM-dd}";
                        cprofile.Dob = Convert.ToDateTime(dateOB);
                       

                        string fname = "";

                        if (file != null)
                        {
                            string pathdelete = _env.WebRootPath + String.Format(@"/images/staff_{0}.jpg", profile.Email);
                            if (TryToDelete(pathdelete) == true)
                            {
                                fname = _env.WebRootPath + String.Format(@"/images/staff_{0}.jpg", profile.Email);

                            }



                        }

                        string msg = "";
                        if (_dbContext.SaveChanges() == 1 || UploadFile(file, fname))
                        {



                            msg = String.Format("Staff {0} Profile updated", cprofile.Name);
                            string template = @"HI {0},<br/><br/>
                                        Your account has been <b>Updated</b>.<br/><br/>
                                       

                                        <br/><br/><text style='color:#58bd91'>Aptive Physiotherapy</text>";
                            string title = String.Format(@"Changes made to {0} account", cprofile.Name);
                            string message = String.Format(template, cprofile.Name);
                            string result;
                            EmailUtl.SendEmail(cprofile.Email, title, message, out result);
                            TempData["MsgS"] = msg;

                        }
                        else if (_dbContext.SaveChanges() != 1 || UploadFile(file, fname) == false)
                        {
                            TempData["Msg"] = "No Changes Information same as before";
                        }

                        else
                            TempData["Msg"] = "Staff not found!";

                    }

                    else
                        TempData["Msg"] = "Invalid input!";
                }
                else
                {
                    TempData["Msg"] = "Invalid Information entered";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }



        public IActionResult Delete(int id)
        {
            if (curStaff() != null && curStaff().Role == "A")
            {

                DbSet<Staff> dbs = _dbContext.Staff;
                Staff cprofile = dbs.Where(p => p.StaffId == id)
                                      .FirstOrDefault();

                if (cprofile != null)
                {
                    dbs.Remove(cprofile);
                    string msg = "";
                    if (_dbContext.SaveChanges() == 1)
                    {

                        msg = String.Format("Staff {0} has been deleted", cprofile.Name);
                        TempData["MsgS"] = msg;
                    }
                    else
                        TempData["Msg"] = "Fail to delete";

                }

                else
                    TempData["Msg"] = "Staff not found!";

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }

        public IActionResult Create()
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }
        [HttpPost]
        public IActionResult Create(Staff staff, IFormFile file)
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                if (ModelState.IsValid)
                {

                    try
                    {

                        int add = DBUtl.ExecSQL(@"INSERT INTO Staff(Name,Email,PhoneNo,Password,Role,Gender,DOB)
                                        VALUES('{0}','{1}',{2},HASHBYTES('SHA1','{3}'),'{4}','{5}','{6}')", staff.Name,
                                            staff.Email, staff.PhoneNo, Request.Form["conpassword"].ToString(), staff.Role, staff.Gender, staff.Dob);


                        string fname = "";

                        if (file != null)
                        {

                            fname = _env.WebRootPath + String.Format(@"/images/staff_{0}.jpg", staff.Email);


                        }
                        try
                        {
                            UploadFile(file, fname);
                        }
                        catch (Exception)
                        {

                        }


                        if (add == 1)
                        {
                            string template = @"HI {0},<br/><br/>
                                    Your account has been <b>Created</b>.<br/><br/>
                                    Your Login ID is {1} <br/>
                                    Your Password is {2}
                                    <br/><br/><text style='color:#58bd91'>Aptive Physiotherapy</text>";

                            string title = String.Format(@"Account Created {0}", staff.Name);

                            string message = String.Format(template, staff.Name, staff.Email, Request.Form["conpassword"].ToString());
                            string result;
                            EmailUtl.SendEmail(staff.Email, title, message, out result);
                            TempData["MsgS"] = "New Staff added!";
                        }
                        else
                            TempData["Msg"] = "Failed to update database";
                    }
                    catch (SqlException ex)
                    {

                        TempData["Error"] = String.Format("The Email {0} existed in the databse", staff.Email);
                        return RedirectToAction("Create");
                    }


                }
                else
                {

                    foreach (var key in ModelState.Keys)
                    {
                        if (ModelState[key].Errors.Count != 0)
                        {
                            TempData["Error"] = ModelState[key].Errors[0].ErrorMessage;
                            return RedirectToAction("Create");
                        }
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }
        //for update method to delete current file
        static bool TryToDelete(string f)
        {
            try
            {

                System.IO.File.Delete(f);
                return true;
            }
            catch (IOException)
            {

                return false;
            }
        }

        public IActionResult UploadMulti()
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }


        [HttpPost]
        public IActionResult UploadMulti(IFormFile file)
        {

            if (curStaff() != null && curStaff().Role == "A")
            {
                if (file != null)
                {
                    string fname = @"UploadedFiles/Staff.xlsx";
                    if (UploadFile(file, fname))
                    {
                        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                        var workbook = ExcelFile.Load(_env.WebRootPath + "/" + fname);

                        // Select active worksheet.
                        var worksheet = workbook.Worksheets.ActiveWorksheet;

                        var ws = workbook.Worksheets.ActiveWorksheet.Rows.Count;

                        int numRows = ws;


                        string letter = "ABCDEFG";

                        for (int row = 1; row <= numRows; row++)
                        {
                            List<dynamic> lst = new List<dynamic>();
                            for (int i = 0; i < letter.Length; i++)
                            {

                                lst.Add(worksheet.Cells[letter[i] + row.ToString()].GetFormattedValue());


                            }
                            int number = int.Parse(lst[2]);
                            int add = DBUtl.ExecSQL(@"INSERT INTO Staff(Name,Email,PhoneNo,Password,Role,Gender,DOB)
                                Values('" + lst[0] + "','" + lst[1] + "'," + number + ",HASHBYTES('SHA1','" + lst[3] + "'),'" + lst[4] +
                                "','" + lst[5] + "','" + lst[6] + "')");


                        }


                        TempData["MsgS"] += "Uploaded to " + _env.WebRootPath + "/ " + fname;
                    }
                    else
                        TempData["Msg"] = "fail";
                }

                return View("UploadMulti");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }

        }
        private bool UploadFile(IFormFile ufile, string fname)
        {
            if (ufile.Length > 0)
            {

                string fullpath = Path.Combine(_env.WebRootPath, fname);
                using (var fileStream = new FileStream(fullpath, FileMode.Create))
                {
                    ufile.CopyToAsync(fileStream);
                }
                return true;



            }
            return false;
        }


        public IActionResult ExportToExcel()
        {
            SqlConnection cnn;
            string connectionString = null;
            string sql = null;
            string data = null;
            int i = 0;
            int j = 0;

            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Excel.Application();
            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            connectionString = "Server = tcp:fypdb2017.database.windows.net,1433; Initial Catalog = FYP; Persist Security Info = False; User ID = Kan@fypdb2017; Password = Republ!c01; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
            cnn = new SqlConnection(connectionString);
            cnn.Open();
            sql = "SELECT * FROM Staff";
            SqlDataAdapter dscmd = new SqlDataAdapter(sql, cnn);
            DataSet ds = new DataSet();
            dscmd.Fill(ds);

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                for (j = 0; j <= ds.Tables[0].Columns.Count - 1; j++)
                {
                    data = ds.Tables[0].Rows[i].ItemArray[j].ToString();
                    xlWorkSheet.Cells[i + 1, j + 1] = data;
                }
            }
            try
            {
                xlWorkBook.SaveAs("BackUpStaff.xls", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                releaseObject(xlWorkSheet);
                releaseObject(xlWorkBook);
                releaseObject(xlApp);

                TempData["MsgS"] = "Excel file created to /Documents Folder";
            }
            catch (Exception ex)
            {
                TempData["Msg"] = "Excel file export failed";
            }

            return RedirectToAction("Index");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                TempData["Msg"] = "Exception Occured while releasing object " + ex.ToString();

            }
            finally
            {
                GC.Collect();
            }
        }



        public IActionResult EmailNotifi()
        {
            if (curStaff() != null && curStaff().Role.Equals("A"))
            {
                try
                {
                    var list = DBUtl.GetList(@"SELECT c.Email ,c.Name,a.AppDate FROM Customer c , Appointment a
                                            WHERE c.CustomerId = a.cus_id
                                            AND a.AppStatus='A'
                                            AND DATEDIFF(day, a.AppDate, CURRENT_TIMESTAMP)<=1
                                            AND DATEDIFF(MONTH,a.AppDate,CURRENT_TIMESTAMP)=0;");

                    foreach (var item in list)
                    {
                        string template = @"Dear {0},<br/><br/>
                                    Your Appointment is on {1} <br/><br/>
                                     
                                    Aptive Physiotherapy";

                        string title = "**NOTIFICATION** Appointment at Aptive Physiotherapy";

                        string message = String.Format(template, item.Name, item.AppDate);
                        string result;
                        EmailUtl.SendEmail(item.Email, title, message, out result);
                    }

                    TempData["MsgS"] = "The Email notification has been sent to the Customers";
                }
                catch (Exception ex)
                {
                    TempData["Msg"] = "Fail to send Email notification to the customer";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }




        }



        public IActionResult ImportCustomer()
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }
        [HttpPost]
        public IActionResult ImportCustomer(IFormFile file)
        {
            if (curStaff() != null && curStaff().Role == "A")
            {
                if (file != null)
                {
                    string fname = @"UploadedFiles/Customer.xlsx";
                    if (UploadFile(file, fname))
                    {
                        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
                        var workbook = ExcelFile.Load(_env.WebRootPath + "/" + fname);

                        // Select active worksheet.
                        var worksheet = workbook.Worksheets.ActiveWorksheet;

                        var ws = workbook.Worksheets.ActiveWorksheet.Rows.Count;

                        int numRows = ws;


                        string letter = "ABCDEFGHI";

                        for (int row = 1; row <= numRows; row++)
                        {
                            List<dynamic> lst = new List<dynamic>();
                            for (int i = 0; i < letter.Length; i++)
                            {

                                lst.Add(worksheet.Cells[letter[i] + row.ToString()].GetFormattedValue());


                            }
                            int number = int.Parse(lst[3]);
                            int add = DBUtl.ExecSQL(@"INSERT INTO Customer(Name,Gender,Address,PhoneNo,Email,Password,DOB,NextofKin,KinNo)
                                Values('"+lst[0]+"','"+lst[1]+"','"+lst[2]+"',"+number+",'"+lst[4]+"',HASHBYTES('SHA1','"+lst[5]+"'),'"+lst[6]+"','"+lst[7]+"','"+lst[8]+"')");


                        }


                        TempData["MsgS"] += "Uploaded to " + _env.WebRootPath + "/ " + fname;
                    }
                    else
                        TempData["Msg"] = "fail";
                }

                return View("ImportCustomer");
            }
            else
            {
                return RedirectToAction("Authenticate", "Account");
            }
        }


    }


}












