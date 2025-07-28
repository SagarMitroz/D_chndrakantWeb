using D_Chandrakant.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using ZXing.Rendering;
using System.IO;
using ZXing.Common;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using D_Chandrakant.DataModels;
using IronBarCode;
using ZXing.QrCode.Internal;
using Microsoft.AspNetCore.Http;
using BitMiracle.LibTiff.Classic;
//using static ZXing.QrCode.Internal.Version;


namespace D_Chandrakant.Controllers
{
    public class AdminController : Controller
    {


        private readonly IConfiguration _configuration;
      

        private readonly Logger _logger;



        private readonly TailordbContext _tailordbContext;
        IWebHostEnvironment hostingenvironment;
        private readonly AutocompleteService _autocompleteService;

        public AdminController(Logger logger, TailordbContext tailordbContext, IConfiguration configuration, IWebHostEnvironment hc, AutocompleteService autocompleteService)
        {
            _tailordbContext = tailordbContext;
            hostingenvironment = hc;
            _autocompleteService = autocompleteService;
            _configuration = configuration;
            _logger = logger;
        }
        public IActionResult DashBoard()
        {
            return View();
        }
        public IActionResult EmployeeManagement()
        {
            EmpModel model = new EmpModel();
            var EmpList = _tailordbContext.Emps.Where(x=>x.RecStatus=="A").ToList();
            foreach (var emp in EmpList)
            {
                var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == emp.DeptFk)?.DeptName;
                EmpModel e = new EmpModel();
                e.Id = emp.Id;
                e.Id = emp.Id;
                e.Name = emp.Name;
                e.DeptFk = finddepartment;
                e.Address = emp.Address;
                e.AccountNo = emp.AccountNo;
                e.PfNo = emp.PfNo;
                e.EmpType = emp.EmpType;
                e.ProfileImg = emp.ProfileImg;
                e.EmpType=emp.EmpType;
                e.MobileNo = emp.Mobile;
                e.SalaryEmp = emp.Salary;
                model.Employees.Add(e);
            }

            return View(model);
        }

        //[HttpGet]
        //public IActionResult SerchProfile(int id)
        //{
        //    EmpModel e = new EmpModel();
        //    var EmpList = _tailordbContext.Emps.FirstOrDefault(x=>x.Id==id);
        //    if (EmpList != null)
        //    {
        //        var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == EmpList.DeptFk)?.DeptName;

        //        //  e.Id = EmpList.Id;
        //        e.Name = EmpList.Name;
        //        e.DeptFk = finddepartment;
        //        e.Address = EmpList.Address;
        //        e.ProfileImg = EmpList.ProfileImg;

        //    }



        //    return Json(e);
        //}



        public IActionResult GetSupplierbyname(string term)
        {
            var students = _autocompleteService.GetEmpByPartialName(term);
            var result = students.Select(s => new { id = s.Id, label = s.Name });
            return Json(result);
        }




        [HttpGet]
        public IActionResult GetEmployeesByEmpTpe(string type)
        {
            EmpModel model = new EmpModel();
          //  var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x. == department)?.Id;
            var employees = _tailordbContext.Emps.Where(x => x.EmpType == type && x.RecStatus=="A").ToList();
            foreach (var emp in employees)
            {
                var findd = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == emp.DeptFk)?.DeptName;
                EmpModel e = new EmpModel();
                e.Id = emp.Id;
                e.Id = emp.Id;
                e.Name = emp.Name;
                e.DeptFk = findd;
                e.Address = emp.Address;
                e.AccountNo = emp.AccountNo;
                e.PfNo = emp.PfNo;
                e.EmpType = emp.EmpType;
                e.ProfileImg = emp.ProfileImg;
                e.EmpType = emp.EmpType;
                e.MobileNo = emp.Mobile;
                e.SalaryEmp = emp.Salary;
                model.Employees.Add(e);
            }

            return Json(model);
        }

        [HttpGet]
        public IActionResult GetEmployeesByDepartment(string department)
        {
            EmpModel model = new EmpModel();
            var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.DeptName == department)?.Id;
            var employees = _tailordbContext.Emps.Where(x => x.DeptFk == finddepartment && x.RecStatus=="A").ToList();
            foreach (var emp in employees)
            {
                var findd = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == emp.DeptFk)?.DeptName;
                EmpModel e = new EmpModel();
                e.Id = emp.Id;
                e.Id = emp.Id;
                e.Name = emp.Name;
                e.DeptFk = findd;
                e.Address = emp.Address;
                e.AccountNo = emp.AccountNo;
                e.PfNo = emp.PfNo;
                e.EmpType = emp.EmpType;
                e.ProfileImg = emp.ProfileImg;
                e.EmpType = emp.EmpType;
                e.MobileNo = emp.Mobile;
                e.SalaryEmp = emp.Salary;
                model.Employees.Add(e);
            }

            return Json(model);
        }

        [HttpGet]
        public IActionResult SerchProfileN(int id)
        {
            
            if(id >=1 && id<=9)
            {
                var a = "0" + id;
                var barcodeValue = a.ToString();

                EmpModel e = new EmpModel();


                // Create a BarcodeWriterPixelData instance
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Width = 300,
                        Height = 100, // Height for barcode only
                        Margin = 10
                    }
                };

                // Generate the barcode as PixelData
                var pixelData = writer.Write(barcodeValue);

                // Create a new Bitmap with additional space for text
                using (var bitmapWithText = new Bitmap(pixelData.Width, pixelData.Height + 40)) // Add space for text below the barcode
                {
                    // Lock the bitmap's bits
                    var bitmapData = bitmapWithText.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                            pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmapWithText.UnlockBits(bitmapData);
                    }

                    // Draw the barcode value text below the barcode with larger font size
                    using (var graphics = Graphics.FromImage(bitmapWithText))
                    {
                        // Draw the barcode value text below the barcode with larger font size
                        using (var font = new Font("Arial", 16)) // Adjust font size here
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            var stringFormat = new StringFormat
                            {
                                Alignment = StringAlignment.Center
                            };
                            var textRectangle = new RectangleF(0, pixelData.Height, bitmapWithText.Width, 40);
                            graphics.DrawString(barcodeValue, font, brush, textRectangle, stringFormat);
                        }
                    }

                    // Save the Bitmap as PNG file
                    string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = Path.Combine(path, "barcode.png");
                    bitmapWithText.Save(filePath, ImageFormat.Png);

                    // Construct the URL for the saved barcode image
                    string imageUrl = "/BarcodeFile/barcode.png";

                    // Populate the EmpModel with data
                    var empList = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);
                    if (empList != null)
                    {
                        var findDepartment = _tailordbContext.Departments.FirstOrDefault(x => x.Id == empList.DeptFk)?.DeptName;

                        e.Name = empList.Name;
                        e.DeptFk = findDepartment;
                        e.Address = empList.Address;
                        e.ProfileImg = empList.ProfileImg;
                        e.MobileNo = empList.Mobile;
                        e.barcodeUrl = imageUrl;
                    }

                    // Return JSON response with EmpModel data
                  
                }
                return Json(e);
            }
            else
            {
                var barcodeValue = id.ToString();

                EmpModel e = new EmpModel();


                // Create a BarcodeWriterPixelData instance
                var writer = new BarcodeWriterPixelData
                {
                    Format = BarcodeFormat.CODE_128,
                    Options = new EncodingOptions
                    {
                        Width = 300,
                        Height = 100, // Height for barcode only
                        Margin = 10
                    }
                };

                // Generate the barcode as PixelData
                var pixelData = writer.Write(barcodeValue);

                // Create a new Bitmap with additional space for text
                using (var bitmapWithText = new Bitmap(pixelData.Width, pixelData.Height + 40)) // Add space for text below the barcode
                {
                    // Lock the bitmap's bits
                    var bitmapData = bitmapWithText.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                        ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                    try
                    {
                        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                            pixelData.Pixels.Length);
                    }
                    finally
                    {
                        bitmapWithText.UnlockBits(bitmapData);
                    }

                    // Draw the barcode value text below the barcode with larger font size
                    using (var graphics = Graphics.FromImage(bitmapWithText))
                    {
                        // Draw the barcode value text below the barcode with larger font size
                        using (var font = new Font("Arial", 16)) // Adjust font size here
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            var stringFormat = new StringFormat
                            {
                                Alignment = StringAlignment.Center
                            };
                            var textRectangle = new RectangleF(0, pixelData.Height, bitmapWithText.Width, 40);
                            graphics.DrawString(barcodeValue, font, brush, textRectangle, stringFormat);
                        }
                    }

                    // Save the Bitmap as PNG file
                    string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string filePath = Path.Combine(path, "barcode.png");
                    bitmapWithText.Save(filePath, ImageFormat.Png);

                    // Construct the URL for the saved barcode image
                    string imageUrl = "/BarcodeFile/barcode.png";

                    // Populate the EmpModel with data
                    var empList = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);
                    if (empList != null)
                    {
                        var findDepartment = _tailordbContext.Departments.FirstOrDefault(x => x.Id == empList.DeptFk)?.DeptName;

                        e.Name = empList.Name;
                        e.DeptFk = findDepartment;
                        e.Address = empList.Address;
                        e.ProfileImg = empList.ProfileImg;
                        e.MobileNo = empList.Mobile;
                        e.barcodeUrl = imageUrl;
                    }

                    // Return JSON response with EmpModel data
                   
                }
                return Json(e);
            }
            
        }


        //[HttpGet]
        //public IActionResult SerchProfileN(int id)
        //{
        //    EmpModel e = new EmpModel();
        //    var barcodeValue = id.ToString();

        //    // Create a BarcodeWriter instance
        //    var writer = new BarcodeWriterPixelData
        //    {
        //        Format = BarcodeFormat.CODE_128,
        //        Options = new EncodingOptions
        //        {
        //            Width = 300,
        //            Height = 60,
        //            Margin = 10
        //        }
        //    };

        //    // Generate the barcode as PixelData
        //    var pixelData = writer.Write(barcodeValue);

        //    // Create a new Bitmap with additional space for text
        //    using (var bitmapWithText = new Bitmap(pixelData.Width, pixelData.Height + 30))
        //    {
        //        // Lock the bitmap's bits
        //        var bitmapData = bitmapWithText.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
        //            ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
        //        try
        //        {
        //            System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
        //                pixelData.Pixels.Length);
        //        }
        //        finally
        //        {
        //            bitmapWithText.UnlockBits(bitmapData);
        //        }

        //        // Draw the barcode value text below the barcode
        //        using (var graphics = Graphics.FromImage(bitmapWithText))
        //        using (var font = new Font("Arial", 16))
        //        using (var brush = new SolidBrush(Color.Black))
        //        {
        //            var stringFormat = new StringFormat
        //            {
        //                Alignment = StringAlignment.Center
        //            };
        //            var textRectangle = new RectangleF(0, pixelData.Height, bitmapWithText.Width, 30);
        //            graphics.DrawString(barcodeValue, font, brush, textRectangle, stringFormat);
        //        }

        //        // Save the Bitmap as PNG file
        //        string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
        //        if (!Directory.Exists(path))
        //        {
        //            Directory.CreateDirectory(path);
        //        }
        //        string filePath = Path.Combine(path, "barcode.png");
        //        bitmapWithText.Save(filePath, ImageFormat.Png);

        //        // Construct the URL for the saved barcode image
        //        string imageUrl = "/BarcodeFile/barcode.png";

        //        // Populate the EmpModel with data
        //        var empList = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);
        //        if (empList != null)
        //        {
        //            var findDepartment = _tailordbContext.Departments.FirstOrDefault(x => x.Id == empList.DeptFk)?.DeptName;

        //            e.Name = empList.Name;
        //            e.DeptFk = findDepartment;
        //            e.Address = empList.Address;
        //            e.ProfileImg = empList.ProfileImg;
        //            e.MobileNo = empList.Mobile;
        //            e.barcodeUrl = imageUrl;
        //        }

        //        // Return JSON response with EmpModel data
        //        return Json(e);
        //    }
        //}





        //[HttpGet]
        //public IActionResult SerchProfileN(int id)
        //{
        //    EmpModel e = new EmpModel();
        //    var BarcodeText = Convert.ToString(id);

        //    GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
        //    barcode.ResizeTo(300, 60);
        //    barcode.AddBarcodeValueTextBelowBarcode();
        //    barcode.ChangeBarCodeColor(Color.Black);
        //    barcode.SetMargins(10);
        //    string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }
        //    String filePath = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile/barcode.png");
        //    barcode.SaveAsPng(filePath);
        //    string filName = Path.GetFileName(filePath);
        //    string ImageUrl = "/BarcodeFile/" + filName;

        //    //ViewBag.barcode1=ImageUrl;


        //    var EmpList = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);
        //    if (EmpList != null)
        //    {
        //        var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == EmpList.DeptFk)?.DeptName;

        //        //  e.Id = EmpList.Id;
        //        e.Name = EmpList.Name;
        //        e.DeptFk = finddepartment;
        //        e.Address = EmpList.Address;
        //        e.ProfileImg = EmpList.ProfileImg;
        //        e.MobileNo = EmpList.Mobile;
        //        e.barcodeUrl = ImageUrl;
        //    }

        //    return Json(e);
        //}

        [HttpGet]
        public IActionResult PrintIcard(int id)
        {
            EmpModel e = new EmpModel();
            var BarcodeText = Convert.ToString(id);

            GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
            barcode.ResizeTo(300, 60);
            barcode.AddBarcodeValueTextBelowBarcode();
            barcode.ChangeBarCodeColor(Color.Black);
            barcode.SetMargins(10);
            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String filePath = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile/barcode.png");
            barcode.SaveAsPng(filePath);
            string filName = Path.GetFileName(filePath);
            string ImageUrl = "/BarcodeFile/" + filName;

            //ViewBag.barcode1=ImageUrl;


            var EmpList = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);
            if (EmpList != null)
            {
                var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == EmpList.DeptFk)?.DeptName;

                //  e.Id = EmpList.Id;
                e.Name = EmpList.Name;
                e.DeptFk = finddepartment;
                e.Address = EmpList.Address;
                e.ProfileImg = EmpList.ProfileImg;
                e.MobileNo = EmpList.Mobile;
                e.barcodeUrl = ImageUrl;
            }

            return View(e);
        }


        //public IActionResult SerchProfile(int id)
        //{
        //    // Find employee details from database
        //    var emp = _tailordbContext.Emps.FirstOrDefault(x => x.Id == id);

        //    if (emp != null)
        //    {
        //        // Create barcode writer instance
        //        BarcodeWriter writer = new BarcodeWriter();
        //        writer.Format = BarcodeFormat.CODE_128; // You can choose any barcode format you prefer

        //        // Generate barcode image
        //        Bitmap barcodeBitmap = writer.Write(emp.Id.ToString());

        //        // Convert barcode image to byte array
        //        byte[] byteArray;
        //        using (MemoryStream stream = new MemoryStream())
        //        {
        //            barcodeBitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //            byteArray = stream.ToArray();
        //        }

        //        // Return barcode image as base64 string
        //        string base64Image = Convert.ToBase64String(byteArray);

        //        // Construct response object
        //        var response = new
        //        {
        //            EmpId = emp.Id,
        //            Name = emp.Name,
        //            DeptFk = emp.DeptFk,
        //            Address = emp.Address,
        //            ProfileImg = emp.ProfileImg,
        //            BarcodeImage = base64Image
        //        };

        //        // Return JSON response
        //        return Json(response);
        //    }
        //    else
        //    {
        //        // If employee not found, return empty response
        //        return Json(null);
        //    }
        //}

        [HttpGet]
        public IActionResult DList()
        {
            List<DepartmentModel> departmentList = new List<DepartmentModel>();
            var finddepartment = this._tailordbContext.Departments.ToList();
            foreach (var item in finddepartment)
            {
                DepartmentModel departmentModel = new DepartmentModel();
                departmentModel.Id = item.Id;
                departmentModel.DeptName = item.DeptName;

                departmentList.Add(departmentModel);
            }
            return Json(departmentList);
        }

        public IActionResult AddEmp()
        {


            return View();
        }

        [HttpPost]
        public IActionResult AddEmp(EmpModel model)
        {
            string filename = "";

            var newEmployeeId = 0;



            if (model.Name != " " && model.Name != null)
            {
                 
                var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.DeptName == model.DeptFk)?.Id;

                Emp employeeObject = new Emp();
                employeeObject.Id = model.Id;
                employeeObject.Name = model.Name;
                employeeObject.Address = model.Address;
                employeeObject.AccountNo = model.AccountNo;
                employeeObject.DeptFk = Convert.ToInt32(model.DeptFk);
                employeeObject.PfNo = model.PfNo;
                employeeObject.EmpType = model.EmpType;
                employeeObject.ProfileImg = filename;
                employeeObject.RecStatus = model.RecStatus;
                employeeObject.Mobile = model.MobileNo;
                if(employeeObject.EmpType=="Permanent")
                {
                    employeeObject.Salary = model.SalaryEmp;
                }
                else
                {
                    employeeObject.Salary = 0;
                }
               
                _tailordbContext.Emps.Add(employeeObject);
                _tailordbContext.SaveChanges();


                // Now, employeeObject.Id will contain the ID assigned by the database
                 newEmployeeId = employeeObject.Id;

            }

            model.Id = newEmployeeId;

            filename = UploadImage(model);

            var empaval = _tailordbContext.Emps.FirstOrDefault(x=>x.Id== newEmployeeId);

            if (empaval != null)
            {
                empaval.ProfileImg = filename;
                _tailordbContext.SaveChanges();
            }   

            return RedirectToAction("EmployeeManagement", "Admin");
        }


        private string UploadImage(EmpModel model)
        {
            string filename = "";
            if (model.ImgFile != null)
            {
                string uploadfolder = Path.Combine(hostingenvironment.WebRootPath, "ProfileImg");
              //  filename = Guid.NewGuid().ToString() + "_" + model.ImgFile.FileName;
                filename = "Emp-"+ model.Id + ".jpg";
                string filepath = Path.Combine(uploadfolder, filename);
                model.ImgFile.CopyTo(new FileStream(filepath, FileMode.Create));
            }
            return filename;

        }

        // Edit Employee Info ----------------------------

        // POST: Update Employee
        [HttpPost]
        public IActionResult UpdateEmp(string Id,string Name,string mobile,string DeptFk,string AccountNo,string PfNo,string EmpType,string Address,string salary,IFormFile ProfileImg)
        {
            EmpModel model = new EmpModel();
            model.Id =Convert.ToInt32(Id);
            model.Name = Name;
            model.DeptFk = DeptFk;
            model.AccountNo = AccountNo;
            model.PfNo = PfNo;
            model.EmpType = EmpType;
            model.Address = Address;
            model.ImgFile = ProfileImg;
         //   model.ProfileImg =;
            model.MobileNo = mobile;
            model.SalaryEmp = Convert.ToDouble(salary);

           var employee = _tailordbContext.Emps.FirstOrDefault(x => x.Id == model.Id);
            if (employee != null)
            { 
                        employee.Id = model.Id;
                        employee.Name = model.Name;
                        employee.Address = model.Address;
                var dept = _tailordbContext.Departments.FirstOrDefault(x => x.DeptName == Convert.ToString(model.DeptFk))?.Id;
               
                if (dept!=null)
                {
                    employee.DeptFk = dept;

                }
               else 
                {
                    var dept1 = _tailordbContext.Departments.FirstOrDefault(x => x.Id == Convert.ToInt32(model.DeptFk))?.Id;
                    employee.DeptFk = dept1;
                }

                employee.PfNo = model.PfNo;
                        employee.EmpType = model.EmpType;
                        employee.AccountNo = model.AccountNo;
                        employee.Mobile = model.MobileNo;
                        employee.RecStatus= model.RecStatus;
                if(employee.EmpType=="Permanent")
                {
                    employee.Salary = model.SalaryEmp;
                }
                else
                {
                    employee.Salary = 0;
                }

                
                string filename = "";
                //if (model.ImgFile != null)
                //{  if (employee.ProfileImg != null)
                //    {
                //        string uploadfolder = Path.Combine(hostingenvironment.WebRootPath, "ProfileImg");
                //        if(System.IO.File.Exists(uploadfolder))
                //        {
                //            filename = "Emp-" + Id + ".jpg";

                //            var newpath=""+ uploadfolder + "/"+ filename + "";

                //            if (System.IO.File.Exists(newpath))
                //            {
                //                System.IO.File.Delete(newpath);
                //            }


                //        }

                //    }
                //    filename = UploadImage(model);
                //}
                var filename1 = "";
                if (model.ImgFile != null)
                {
                    if (employee.ProfileImg != null)
                    {
                        string uploadFolder = Path.Combine(hostingenvironment.WebRootPath, "ProfileImg");
                         filename = "Emp-" + employee.Id + ".jpg";
                        string filePath = Path.Combine(uploadFolder, filename);

                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                    }

                    // Upload the new image
                    filename = UploadImage(model);
                }


                if (model.ImgFile != null)
                {
                    employee.ProfileImg = filename;
                }
               
                _tailordbContext.SaveChanges();


                   
                  
                
                return Json(new { success = true, message = "Data updated successfully." });
            }
            // If ModelState is not valid or other errors occur
            else
            {

                return Json(new { success = false, message = "Invalid data. Please check the form fields." });
            }
        }


        public IActionResult DeleteEmp(int Id)
        {
            var Employee = _tailordbContext.Emps.FirstOrDefault(x => x.Id == Id);

            
            _tailordbContext.Emps.Remove(Employee);
            _tailordbContext.SaveChanges();

            return Json(Employee);
        }



        public IActionResult Accounts()
        {


            return View();
        }
/************************** Track status *********************************************************************************/
        public IActionResult TrackStatus()
        {
           

            return View();
        }
        public IActionResult TrackStatusbyMeasure()
        {


            return View();
        }

        [HttpGet]
        public IActionResult Statusinfo(int memo,string series)
        {
            StatusViewModel b = new StatusViewModel();


            var list = _tailordbContext.Billdetails.FirstOrDefault(x => x.MemoNo == memo && x.MemoSeries==series);
            if (list != null)
            {
                b.MemoNo = list.MemoNo;
                var A = this._tailordbContext.Billheaders.FirstOrDefault(x => x.MemoNo == memo && x.MemoSeries == series);
                b.Name = A.CustomerName;
                b.Address = A.CustomerAddress;
                b.Contact = A.CustomerMobile;
                b.OrderDate = A.MemoDate.ToString();

                return Json(new { success = true, msg = "Data Found",data=b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }

           
           // return Json(b);
        }

       public IActionResult SearchInfoCut(int memo, string series)
       {
          
            StatusViewModel b = new StatusViewModel();

            

            var rec = this._tailordbContext.Measurements.Where(x => x.MemoNo == memo && x.MemoSeries == series).ToList();
            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if(rec.Count > 0)
            {
                foreach (var item in rec)
                {

                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MemoNo == item.MemoNo && x.ItemFk == item.ItemId && x.Dept == 1 && x.MemoSeries == series && x.RecStatus == "A").ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        //e.CompQty = 0;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;

                       
                    }

                    b.Lists.Add(e);

                }


                
                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO && SumO != 0)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }

                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }
           
          //  return Json(b);
       }



        public IActionResult SearchInfoStitch(int memo, string series)
        {
            
            StatusViewModel b = new StatusViewModel();

           

            var rec = this._tailordbContext.Measurements.Where(x => x.MemoNo == memo && x.MemoSeries == series).ToList();

            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if (rec.Count > 0)
            {
                foreach (var item in rec)
                {
                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MemoNo == item.MemoNo && x.ItemFk == item.ItemId && x.Dept == 2 && x.MemoSeries == series && x.RecStatus=="A").ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;
                    }
                    b.Lists.Add(e);
                }
                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }

                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }

           
        }

        public IActionResult SearchInfoIron(int memo, string series)
        {
            StatusViewModel b = new StatusViewModel();
           


            var rec = this._tailordbContext.Measurements.Where(x => x.MemoNo == memo && x.MemoSeries == series).ToList();

            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if (rec.Count > 0)
            {
                foreach (var item in rec)
                {
                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MemoNo == item.MemoNo && x.ItemFk == item.ItemId && x.Dept == 3 && x.MemoSeries == series && x.RecStatus == "A").ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;
                    }
                    b.Lists.Add(e);
                }
                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }


                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }
            


           // return Json(b);
        }

        //public IActionResult SearchInfoDeliver(int memo)
        //{
        //    var record = this._tailordbContext.Empworks.Where(x => x.MemoNo == memo).ToList();
        //    StatusViewModel b = new StatusViewModel();
        //    var s = 0;

        //    foreach (var item in record)
        //    {
        //        StatusViewModel e = new StatusViewModel();
        //        var rec = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == item.EmpIdfk && x.DeptFk == 1);
        //        var recS = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == item.EmpIdfk && x.DeptFk == 2);
        //        var condition = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == item.EmpIdfk && x.DeptFk == 3);
        //        var recM=this._tailordbContext.Billdetails.FirstOrDefault(x=>x.Id==item.BillDetailFk && x.IsDelivered==1);
        //        if(recM != null && rec!=null && recS!=null && condition!=null)
        //        {
        //            e.OrderedQty = item.OrderedQty;
        //            e.CompQty = item.CompletedOrder;
        //            e.ItemName = item.ItemName;
        //            b.Lists.Add(e);
        //            if (item.RemaimingQty == 0 && (item.CompletedOrder == item.OrderedQty))
        //            {


        //                s = 1;
        //            }
        //            else if (item.RemaimingQty > 0 || (item.CompletedOrder < item.OrderedQty))
        //            {

        //                s = 2;
        //            }
        //        }
        //    }
        //    if (s == 1)
        //    {
        //        b.WorkStatus = "Completed";
        //    }
        //    else if (s == 2)
        //    {
        //        b.WorkStatus = "In progress";
        //    }
        //    else
        //    {
        //        b.WorkStatus = "Pending";
        //    }

        //    return Json(b);
        //}

        public IActionResult SearchInfoDeliver(int memo, string series)
        {
            var record = this._tailordbContext.Billdetails.Where(x => x.MemoNo == memo && x.MemoSeries == series).ToList();
            StatusViewModel b = new StatusViewModel();
            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;

            foreach (var item in record)
            {
                StatusViewModel e = new StatusViewModel();
                if(item.IsDelivered==1)
                {
                    e.CompQty = item.Qty;
                    e.ItemName= item.ItemName;
                    e.OrderedQty=item.Qty;
                    sumA = sumA + e.OrderedQty;
                    sumB = sumB + e.CompQty;
                }
                else
                {
                    e.CompQty = 0;
                    var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                    e.ItemName = ItemN;
                    e.OrderedQty = item.Qty;
                    sumC = sumC + item.Qty;
                }
                b.Lists.Add(e);
            }
            double? SumO = sumA + sumC;
            double? SumD = sumB;
            if (SumD == SumO)
            {
                b.WorkStatus = "Completed";
            }
            else if (SumD < SumO && SumD != 0)
            {
                b.WorkStatus = "In progress";
            }
            else
            {
                b.WorkStatus = "Pending";
            }

            return Json(b);
        }




        //****************************************************************************************************//
        public IActionResult StatusinfoMeasure(int measurement)
        {
            StatusViewModel b = new StatusViewModel();


            var list = _tailordbContext.Measurements.FirstOrDefault(x => x.Id == measurement);
            if (list != null)
            {
                b.MemoNo = list.MemoNo;
                var A = this._tailordbContext.Billheaders.FirstOrDefault(x => x.Id == list.BillHeaderIdIdx);
                b.Name = A.CustomerName;
                b.Address = A.CustomerAddress;
                b.Contact = A.CustomerMobile;
                b.OrderDate = A.MemoDate.ToString();

                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }
        }


        public IActionResult SearchInfoCutMeasure(int measurement)
        {


            StatusViewModel b = new StatusViewModel();



            var rec = this._tailordbContext.Measurements.Where(x => x.Id == measurement).ToList();
            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if (rec.Count > 0)
            {
                foreach (var item in rec)
                {

                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MeasurementFk == item.Id && x.ItemFk == item.ItemId && x.Dept == 1 && x.RecStatus == "A").ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        //e.CompQty = 0;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;


                    }

                    b.Lists.Add(e);

                }



                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO && SumO != 0)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }

                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }




        }



        public IActionResult SearchInfoStritchMeasure(int measurement)
        {

            StatusViewModel b = new StatusViewModel();



            var rec = this._tailordbContext.Measurements.Where(x => x.Id == measurement).ToList();
            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if (rec.Count > 0)
            {
                foreach (var item in rec)
                {
                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MeasurementFk == item.Id && x.ItemFk == item.ItemId && x.Dept == 2 && x.RecStatus == "A").ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;
                    }
                    b.Lists.Add(e);
                }
                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }

                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }

        }


        public IActionResult SearchInfoIronMeasure(int measurement)
        {

            StatusViewModel b = new StatusViewModel();



            var rec = this._tailordbContext.Measurements.Where(x =>x.Id==measurement).ToList();

            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;
            if (rec.Count > 0)
            {
                foreach (var item in rec)
                {
                    StatusViewModel e = new StatusViewModel();
                    var condition = this._tailordbContext.Empworks.Where(x => x.MeasurementFk == item.Id && x.ItemFk == item.ItemId && x.Dept == 3 && x.RecStatus == "A" ).ToList();
                    if (condition.Count > 0)
                    {
                        var completedqty = condition.Sum(x => x.CompletedOrder);
                        var mainqty = condition.Select(x => x.OrderedQty).FirstOrDefault();
                        var itemname = condition.Select(x => x.ItemName).FirstOrDefault();

                        e.OrderedQty = mainqty;
                        e.ItemName = itemname;
                        e.CompQty = completedqty;
                        sumA = sumA + e.OrderedQty;
                        sumB = sumB + e.CompQty;
                    }
                    else
                    {
                        e.OrderedQty = item.Qty;
                        var ItemN = _tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemId).Name;
                        e.ItemName = ItemN;
                        e.CompQty = 0;
                        sumC = sumC + item.Qty;
                    }
                    b.Lists.Add(e);
                }
                double? SumO = sumA + sumC;
                double? SumD = sumB;
                if (SumD == SumO)
                {
                    b.WorkStatus = "Completed";
                }
                else if (SumD < SumO && SumD != 0)
                {
                    b.WorkStatus = "In progress";
                }
                else
                {
                    b.WorkStatus = "Pending";
                }


                return Json(new { success = true, msg = "Data Found", data = b });
            }
            else
            {
                return Json(new { success = true, msg = "Data Not Found" });
            }


        }


        public IActionResult SearchInfoDeliverMeasure(int measurement)
        {
            var recA=_tailordbContext.Measurements.FirstOrDefault(x=>x.Id== measurement);   
            var record = this._tailordbContext.Billdetails.Where(x => x.BillHeaderId==recA.BillHeaderIdIdx).ToList();
            StatusViewModel b = new StatusViewModel();
            double? sumA = 0;
            double? sumB = 0;
            double? sumC = 0;

            foreach (var item in record)
            {
                StatusViewModel e = new StatusViewModel();
                if (item.IsDelivered == 1)
                {
                    e.CompQty = item.Qty;
                    e.ItemName = item.ItemName;
                    e.OrderedQty = item.Qty;
                    sumA = sumA + e.OrderedQty;
                    sumB = sumB + e.CompQty;
                }
                else
                {
                    e.CompQty = 0;
                    e.ItemName = item.ItemName;
                    e.OrderedQty = item.Qty;
                    sumC = sumC + item.Qty;
                }
                b.Lists.Add(e);
            }
            double? SumO = sumA + sumC;
            double? SumD = sumB;
            if (SumD == SumO)
            {
                b.WorkStatus = "Completed";
            }
            else if (SumD < SumO && SumD != 0)
            {
                b.WorkStatus = "In progress";
            }
            else
            {
                b.WorkStatus = "Pending";
            }

            return Json(b);
        }



        //***************************************************************************************************//

        //******************************* Employee Payroll  ***************************************************//
        public IActionResult EmpPayroll()
        {
           
            EmpSalViewModel model = new EmpSalViewModel();
          

            return View(model);
        }



        //*****************
        public IActionResult EmpPayrollJ(DateTime? startDate, DateTime? endDate)
        {
            //DateTime? endDate = DateTime.Now;
            //if (startDate != null || eDate != null)
            //{
            //    endDate = eDate.Value.AddDays(1);
            //}
            EmpSalViewModel model = new EmpSalViewModel();
            var empList = _tailordbContext.Emps.Where(x => x.EmpType == "Pay on Work").ToList();
            if (startDate== endDate)
            {

               
               

                foreach (var item in empList)
                {
                    double? S = 0;
                    double? q = 0;

                    var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == startDate.Value.Date && x.RecStatus == "A").ToList();
                    if (RecA.Count() > 0)
                    {
                        EmpSalViewModel e = new EmpSalViewModel();
                        e.Id = item.Id;
                        e.Name = item.Name;
                        e.EmpType = item.EmpType;
                        var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                        e.DeptName = findDept;
                        e.AccountNo = item.AccountNo;
                        foreach (var A in RecA)
                        {
                            q += A.CompletedOrder;
                            S += A.CompletedOrder * A.ItemRate;

                        }
                        e.ComOrder = q;
                        e.Salary = S;


                        model.salaries.Add(e);
                    }


                }


                return Json(model);
            }
            else
            {
                foreach (var item in empList)
                {
                    double? S = 0;

                    double? q = 0;
                    var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= startDate && x.Date.Value.Date <= endDate && x.RecStatus == "A").ToList();
                    if (RecA.Count() > 0)
                    {
                        EmpSalViewModel e = new EmpSalViewModel();
                        e.Id = item.Id;
                        e.Name = item.Name;
                        e.EmpType = item.EmpType;
                        var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                        e.DeptName = findDept;
                        e.AccountNo = item.AccountNo;
                        foreach (var A in RecA)
                        {
                            q += A.CompletedOrder;
                            S += A.CompletedOrder * A.ItemRate;

                        }
                        e.ComOrder = q;
                        e.Salary = S;


                        model.salaries.Add(e);
                    }


                }
                return Json(model);
            }




           
        }


        //******************

        //public IActionResult EmpPayrollbyDate(DateTime date)
        //{

        //}









        //public IActionResult EmpPayrollbyDateRange(DateTime dateA, DateTime dateB)
        //{

        //    EmpSalViewModel model = new EmpSalViewModel();
        //    var empList = _tailordbContext.Emps.Where(x=>x.EmpType == "Pay on Work").ToList();

           


        //    return Json(model);
        //}






        //[HttpPost]
        //public async Task<IActionResult> EmpPayrollbyDateRange(DateTime dateA, DateTime dateB)
        //{
        //    EmpSalViewModel model = new EmpSalViewModel();
        //    var empList = _tailordbContext.Emps.ToList();

        //    foreach (var item in empList)
        //    {
        //        double? S = 0;

        //        string date1 = "2024-04-01";
        //        string date2 = "2024-04-16";

        //        DateTime startDate = DateTime.ParseExact(dateA, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //        DateTime endDate = DateTime.ParseExact(dateB, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //        List<string> datesInRange = GetDatesBetween(startDate, endDate);

        //        // Now you have a list of dates between date1 and date2

        //        List<Empwork> empwork = new List<Empwork>();

        //        foreach (string date in datesInRange)
        //        {
        //           // Console.WriteLine(date.ToString("yyyy-MM-dd"));

        //            var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date== date).ToList();
        //            if (RecA.Count > 0)
        //            {
        //                foreach(var i in RecA)
        //                {
        //                    empwork.Add(i);
        //                }
        //            }
        //        }



        //      //  var datelis=RecA.Select(x => x.Date).ToList();

        //        if (empwork.Count() > 0)
        //        {
        //            EmpSalViewModel e = new EmpSalViewModel();
        //            e.Id = item.Id;
        //            e.Name = item.Name;
        //            e.EmpType = item.EmpType;
        //            var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
        //            e.DeptName = findDept;

        //            foreach (var A in empwork)
        //            {

        //                var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
        //                if (RecB != null)
        //                {
        //                    if (A.Dept == 1)
        //                    {
        //                        S = S + A.CompletedOrder * RecB.CuttingR;
        //                    }
        //                    else if (A.Dept == 2)
        //                    {
        //                        S = S + A.CompletedOrder * RecB.StretchingR;
        //                    }
        //                    else if (A.Dept == 3)
        //                    {
        //                        S = S + A.CompletedOrder * RecB.IorningR;
        //                    }
        //                }
        //            }

        //            e.Salary = S;


        //            model.salaries.Add(e);
        //        }


        //    }



        //    return Json(new { success=true,data= model.salaries });
        //}

        //private List<string> GetDatesBetween(DateTime startDate, DateTime endDate)
        //{
        //    List<string> dates = new List<string>();
        //    for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        //    {
        //        dates.Add(date.ToString("yyyy-MM-dd"));
        //    }
        //    return dates;
        //}

        //*******************************************************************************************************************




        public IActionResult EmpPayrollPerm()
        {
            //DateTime date = DateTime.Today;
            EmpSalViewModel model = new EmpSalViewModel();
            //var empList = _tailordbContext.Emps.ToList();
            //model.date = date;
            //foreach (var item in empList)
            //{
            //    double? S = 0;
            //    double? q = 0;

            //    var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == date).ToList();
            //    if (RecA.Count() > 0)
            //    {
            //        EmpSalViewModel e = new EmpSalViewModel();
            //        e.Id = item.Id;
            //        e.Name = item.Name;
            //        e.EmpType = item.EmpType;
            //        var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
            //        e.DeptName = findDept;
            //        e.AccountNo=item.AccountNo;
            //        foreach (var A in RecA)
            //        {

            //            var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
            //            if (RecB != null)
            //            {
            //                if (A.Dept == 1)
            //                {
            //                    q += A.CompletedOrder;
            //                    S = S + A.CompletedOrder * RecB.CuttingR;
            //                }
            //                else if (A.Dept == 2)
            //                {
            //                    q += A.CompletedOrder;
            //                    S = S + A.CompletedOrder * RecB.StretchingR;
            //                }
            //                else if (A.Dept == 3)
            //                {
            //                    q += A.CompletedOrder;
            //                    S = S + A.CompletedOrder * RecB.IorningR;
            //                }
            //            }
            //        }
            //        e.ComOrder = q;
            //        e.Salary = S;


            //        model.salaries.Add(e);
            //    }


            //}


            return View(model);
        }



        public IActionResult EmpPayrollbyDatePerm(DateTime date)
        {

            EmpSalViewModel model = new EmpSalViewModel();
            var empList = _tailordbContext.Emps.Where(x => x.EmpType == "Permanent").ToList();

            foreach (var item in empList)
            {
                double? S = 0;
                double? q = 0;

                var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == date && x.RecStatus == "A").ToList();
                if (RecA.Count() > 0)
                {
                    EmpSalViewModel e = new EmpSalViewModel();
                    e.Id = item.Id;
                    e.Name = item.Name;
                    e.EmpType = item.EmpType;
                    var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                    e.DeptName = findDept;
                    e.AccountNo = item.AccountNo;
                    foreach (var A in RecA)
                    {

                        var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
                        if (RecB != null)
                        {
                            if (A.Dept == 1)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.CuttingR;
                            }
                            else if (A.Dept == 2)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.StretchingR;
                            }
                            else if (A.Dept == 3)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.IorningR;
                            }
                        }
                    }
                    e.ComOrder = q;
                    e.Salary = S;


                    model.salaries.Add(e);
                }


            }


            return Json(model);
        }









        public IActionResult EmpPayrollbyDateRangePerm(DateTime dateA, DateTime dateB)
        {

            EmpSalViewModel model = new EmpSalViewModel();
            var empList = _tailordbContext.Emps.Where(x => x.EmpType == "Permanent").ToList();

            foreach (var item in empList)
            {
                double? S = 0;

                double? q = 0;
                var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= dateA && x.Date.Value.Date <= dateB && x.RecStatus == "A").ToList();
                if (RecA.Count() > 0)
                {
                    EmpSalViewModel e = new EmpSalViewModel();
                    e.Id = item.Id;
                    e.Name = item.Name;
                    e.EmpType = item.EmpType;
                    var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                    e.DeptName = findDept;
                    e.AccountNo = item.AccountNo;
                    foreach (var A in RecA)
                    {

                        var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
                        if (RecB != null)
                        {
                            if (A.Dept == 1)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.CuttingR;
                            }
                            else if (A.Dept == 2)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.StretchingR;
                            }
                            else if (A.Dept == 3)
                            {
                                q += A.CompletedOrder;
                                S = S + A.CompletedOrder * RecB.IorningR;
                            }
                        }
                    }
                    e.ComOrder = q;
                    e.Salary = S;


                    model.salaries.Add(e);
                }


            }


            return Json(model);
        }










        //******************************************************************************************************************
        public IActionResult EmpPayrollSet()
        {
            RateViewModel rate = new RateViewModel();

            var ListR=_tailordbContext.Rateemps.ToList();    
            foreach(var item in ListR)
            {
                RateViewModel r = new RateViewModel();
                var findItemName=_tailordbContext.Items.FirstOrDefault(x => x.Id == item.ItemIdFk)?.Name;
                r.Name=findItemName;
                r.CuttingR=item.CuttingR;
                r.StretchingR=item.StretchingR; 
                r.IorningR=item.IorningR;
                r.ItemId=item.ItemIdFk;


                rate.Products.Add(r);   

            }




            return View(rate);
        }

//************************************ Report Screen *****************************************************************//
        public IActionResult EmployeeRec()
        {
            EmpSalViewModel model = new EmpSalViewModel();
            var empList = _tailordbContext.Emps.ToList();

            foreach (var item in empList)
            {
                double? S = 0;
                double?qt = 0;

                var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.RecStatus == "A").ToList();
                if (RecA.Count() > 0)
                {
                    EmpSalViewModel e = new EmpSalViewModel();
                    e.Id = item.Id;
                    e.Name = item.Name;
                    e.EmpType = item.EmpType;
                    var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                    e.DeptName = findDept;
                    e.PfNo= item.PfNo;
                    e.AccountNo = item.AccountNo;

                    foreach (var A in RecA)
                    {
                        qt = qt + A.CompletedOrder;
                        var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
                        if (RecB != null)
                        {
                            if (A.Dept == 1)
                            {
                                S = S + A.CompletedOrder * RecB.CuttingR;
                            }
                            else if (A.Dept == 2)
                            {
                                S = S + A.CompletedOrder * RecB.StretchingR;
                            }
                            else if (A.Dept == 3)
                            {
                                S = S + A.CompletedOrder * RecB.IorningR;
                            }
                        }
                    }
                    e.ComOrder = qt;
                    e.Salary = S;


                    model.salaries.Add(e);
                }


            }


            return View(model);
        }

        //public IActionResult EmployeeRecByDate(string date)
        //{
        //    EmpSalViewModel model = new EmpSalViewModel();
        //    var empList = _tailordbContext.Emps.ToList();
        //    if (empList.Count > 0)
        //    {
        //        foreach (var item in empList)
        //        {
        //            double? S = 0;
        //            double? qt = 0;

        //            var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date == date).ToList();
        //            if (RecA.Count() > 0)
        //            {
        //                EmpSalViewModel e = new EmpSalViewModel();
        //                e.Id = item.Id;
        //                e.Name = item.Name;
        //                e.EmpType = item.EmpType;
        //                var findDept = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
        //                e.DeptName = findDept;
        //                e.PfNo = item.PfNo;
        //                e.AccountNo = item.AccountNo;

        //                foreach (var A in RecA)
        //                {
        //                    qt = qt + A.CompletedOrder;
        //                    var RecB = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);
        //                    if (RecB != null)
        //                    {
        //                        if (A.Dept == 1)
        //                        {
        //                            S = S + A.CompletedOrder * RecB.CuttingR;
        //                        }
        //                        else if (A.Dept == 2)
        //                        {
        //                            S = S + A.CompletedOrder * RecB.StretchingR;
        //                        }
        //                        else if (A.Dept == 3)
        //                        {
        //                            S = S + A.CompletedOrder * RecB.IorningR;
        //                        }
        //                    }
        //                }
        //                e.ComOrder = qt;
        //                e.Salary = S;


        //                model.salaries.Add(e);
        //            }


        //        }


        //        return Json(model);
        //    }
        //    else
        //    {
        //        return Json(new { success = true, msg = "Data Not Found" });
        //    }

        //}


        public IActionResult CustomerwiseRec()
        {

            DateTime date = DateTime.Today;
           


            return View();
        }

        public IActionResult CustomerwiseRecJ(DateTime? startDate, DateTime? endDate, string supplier)
        {
            //DateTime? endDate = DateTime.Now;
            //if (startDate != null || eDate != null)
            //{
            //    endDate = eDate.Value.AddDays(1);
            //}
            List<CustemorWiseViewModel> EmployeeR = new List<CustemorWiseViewModel>();
         if(startDate==endDate)
            {

                if (supplier != "" && supplier != null)
                {
                    var empList = _tailordbContext.Emps.Where(x => x.Name == supplier).ToList();

                    foreach (var item in empList)
                    {
                        List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
                        CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


                        var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == startDate.Value.Date && x.RecStatus == "A").ToList();
                        if (RecA.Count > 0)
                        {
                            double? totalQty = 0;
                            double? total = 0;
                            foreach (var A in RecA)
                            {
                                double? qt = 0;
                                var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                                getEmp.EmpName = item.Name;
                                getEmp.EmpDept = deptname;
                                getEmp.EmpId = item.Id;

                                CustomerRecordModel itemlist1 = new CustomerRecordModel();

                                //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

                                itemlist1.CName = A.CustomerName;
                                itemlist1.MemoNo = A.MemoNo;

                                qt = qt + A.CompletedOrder;
                                itemlist1.ComOrder = qt;
                                itemlist1.Rate = A.ItemRate;

                                itemlist1.Total = qt * A.ItemRate;
                                totalQty += A.CompletedOrder;
                                total += itemlist1.Total;
                                itemlist.Add(itemlist1);

                                getEmp.itemlists = itemlist;
                            }
                            getEmp.TComOrder = totalQty;
                            getEmp.TotalS = total;


                            EmployeeR.Add(getEmp);
                        }





                    }


                    return Json(EmployeeR);
                }
                else
                {

                    var empList = _tailordbContext.Emps.ToList();

                    foreach (var item in empList)
                    {
                        List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
                        CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


                        var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == startDate.Value.Date && x.RecStatus == "A").ToList();
                        if (RecA.Count > 0)
                        {
                            double? totalQty = 0;
                            double? total = 0;
                            foreach (var A in RecA)
                            {
                                double? qt = 0;
                                var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                                getEmp.EmpName = item.Name;
                                getEmp.EmpDept = deptname;
                                getEmp.EmpId = item.Id;

                                CustomerRecordModel itemlist1 = new CustomerRecordModel();

                                //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

                                itemlist1.CName = A.CustomerName;
                                itemlist1.MemoNo = A.MemoNo;

                                qt = qt + A.CompletedOrder;
                                itemlist1.ComOrder = qt;
                                itemlist1.Rate = A.ItemRate;

                                itemlist1.Total = qt * A.ItemRate;
                                totalQty += A.CompletedOrder;
                                total += itemlist1.Total;
                                itemlist.Add(itemlist1);

                                getEmp.itemlists = itemlist;
                            }
                            getEmp.TComOrder = totalQty;
                            getEmp.TotalS = total;


                            EmployeeR.Add(getEmp);
                        }





                    }


                    return Json(EmployeeR);


                }


               


            }
            else
            {

                if (supplier != "" && supplier != null)
                {
                    var empList = _tailordbContext.Emps.Where(x => x.Name == supplier).ToList();
                    foreach (var item in empList)
                    {
                        List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
                        CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


                        var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= startDate && x.Date.Value.Date <= endDate && x.RecStatus == "A").ToList();
                        if (RecA.Count > 0)
                        {
                            double? totalQty = 0;
                            double? total = 0;
                            foreach (var A in RecA)
                            {
                                double? qt = 0;
                                var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                                getEmp.EmpName = item.Name;
                                getEmp.EmpDept = deptname;
                                getEmp.EmpId = item.Id;

                                CustomerRecordModel itemlist1 = new CustomerRecordModel();

                                //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

                                itemlist1.CName = A.CustomerName;
                                itemlist1.MemoNo = A.MemoNo;

                                qt = qt + A.CompletedOrder;
                                itemlist1.ComOrder = qt;
                                itemlist1.Rate = A.ItemRate;

                                itemlist1.Total = qt * A.ItemRate;
                                totalQty += A.CompletedOrder;
                                total += itemlist1.Total;
                                itemlist.Add(itemlist1);




                                getEmp.itemlists = itemlist;
                            }
                            getEmp.TComOrder = totalQty;
                            getEmp.TotalS = total;


                            EmployeeR.Add(getEmp);
                        }





                    }


                    return Json(EmployeeR);
                }
                else
                {
                    var empList = _tailordbContext.Emps.ToList();
                    foreach (var item in empList)
                    {
                        List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
                        CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


                        var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= startDate && x.Date.Value.Date <= endDate && x.RecStatus == "A").ToList();
                        if (RecA.Count > 0)
                        {
                            double? totalQty = 0;
                            double? total = 0;
                            foreach (var A in RecA)
                            {
                                double? qt = 0;
                                var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
                                getEmp.EmpName = item.Name;
                                getEmp.EmpDept = deptname;
                                getEmp.EmpId = item.Id;

                                CustomerRecordModel itemlist1 = new CustomerRecordModel();

                                //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

                                itemlist1.CName = A.CustomerName;
                                itemlist1.MemoNo = A.MemoNo;

                                qt = qt + A.CompletedOrder;
                                itemlist1.ComOrder = qt;
                                itemlist1.Rate = A.ItemRate;

                                itemlist1.Total = qt * A.ItemRate;
                                totalQty += A.CompletedOrder;
                                total += itemlist1.Total;
                                itemlist.Add(itemlist1);



                                getEmp.itemlists = itemlist;
                            }
                            getEmp.TComOrder = totalQty;
                            getEmp.TotalS = total;


                            EmployeeR.Add(getEmp);
                        }





                    }


                    return Json(EmployeeR);
                }
            }
            
        }

        //public IActionResult CustomerwiseRecByDate(DateTime date)
        //{
        //    List<CustemorWiseViewModel> EmployeeR = new List<CustemorWiseViewModel>();
        //    var empList = _tailordbContext.Emps.ToList();

        //    foreach (var item in empList)
        //    {
        //        List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
        //        CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


        //        var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date == date && x.RecStatus == "A").ToList();
        //        if (RecA.Count > 0)
        //        {
        //            double? totalQty = 0;
        //            double? total = 0;
        //            foreach (var A in RecA)
        //            {
        //                double? qt = 0;
        //                var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
        //                getEmp.EmpName = item.Name;
        //                getEmp.EmpDept = deptname;
        //                getEmp.EmpId = item.Id;
                       
        //                    CustomerRecordModel itemlist1 = new CustomerRecordModel();

        //                    //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

        //                    itemlist1.CName = A.CustomerName;
        //                    itemlist1.MemoNo = A.MemoNo;

        //                    qt = qt + A.CompletedOrder;
        //                    itemlist1.ComOrder = qt;
        //                    itemlist1.Rate = A.ItemRate;

        //                    itemlist1.Total = qt *A.ItemRate;
        //                    totalQty += A.CompletedOrder;
        //                    total += itemlist1.Total;
        //                    itemlist.Add(itemlist1);
                       

        //                //else if (A.Dept == 2)
        //                //{
        //                //    CustomerRecordModel itemlist1 = new CustomerRecordModel();

        //                //    var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

        //                //    itemlist1.CName = A.CustomerName;
        //                //    itemlist1.MemoNo = A.MemoNo;

        //                //    qt = qt + A.CompletedOrder;
        //                //    itemlist1.ComOrder = qt;
        //                //    itemlist1.Rate = Rates.StretchingR;
        //                //    itemlist1.Total = qt * Rates.StretchingR;
        //                //    totalQty += A.CompletedOrder;
        //                //    total += itemlist1.Total;
        //                //    itemlist.Add(itemlist1);
        //                //}

        //                //else if (A.Dept == 3)
        //                //{
        //                //    CustomerRecordModel itemlist1 = new CustomerRecordModel();

        //                //    var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

        //                //    itemlist1.CName = A.CustomerName;
        //                //    itemlist1.MemoNo = A.MemoNo;

        //                //    qt = qt + A.CompletedOrder;
        //                //    itemlist1.ComOrder = qt;
        //                //    itemlist1.Rate = Rates.IorningR;
        //                //    itemlist1.Total = qt * Rates.IorningR;
        //                //    totalQty += A.CompletedOrder;
        //                //    total += itemlist1.Total;
        //                //    itemlist.Add(itemlist1);
        //                //}


        //                getEmp.itemlists = itemlist;
        //            }
        //            getEmp.TComOrder = totalQty;
        //            getEmp.TotalS = total;


        //            EmployeeR.Add(getEmp);
        //        }





        //    }


        //    return Json(EmployeeR);
        //}



        //public IActionResult CustomerwiseRecByDateRAnge(DateTime dateA, DateTime dateB,string EN)
        //{
        //    List<CustemorWiseViewModel>EmployeeR = new List<CustemorWiseViewModel>();
            
            


        //    if (EN != ""&& EN!=null)
        //    {
        //        var empList = _tailordbContext.Emps.Where(x=>x.Name==EN).ToList();
        //        foreach (var item in empList)
        //        {
        //            List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
        //            CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


        //            var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= dateA && x.Date.Value.Date <= dateB && x.RecStatus == "A").ToList();
        //            if (RecA.Count > 0)
        //            {
        //                double? totalQty = 0;
        //                double? total = 0;
        //                foreach (var A in RecA)
        //                {
        //                    double? qt = 0;
        //                    var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
        //                    getEmp.EmpName = item.Name;
        //                    getEmp.EmpDept = deptname;
        //                    getEmp.EmpId = item.Id;

        //                    CustomerRecordModel itemlist1 = new CustomerRecordModel();

        //                    //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

        //                    itemlist1.CName = A.CustomerName;
        //                    itemlist1.MemoNo = A.MemoNo;

        //                    qt = qt + A.CompletedOrder;
        //                    itemlist1.ComOrder = qt;
        //                    itemlist1.Rate = A.ItemRate;

        //                    itemlist1.Total = qt * A.ItemRate;
        //                    totalQty += A.CompletedOrder;
        //                    total += itemlist1.Total;
        //                    itemlist.Add(itemlist1);


                          

        //                    getEmp.itemlists = itemlist;
        //                }
        //                getEmp.TComOrder = totalQty;
        //                getEmp.TotalS = total;


        //                EmployeeR.Add(getEmp);
        //            }





        //        }


        //        return Json(EmployeeR);
        //    }
        //    else
        //    {
        //        var empList = _tailordbContext.Emps.ToList();
        //        foreach (var item in empList)
        //        {
        //            List<CustomerRecordModel> itemlist = new List<CustomerRecordModel>();
        //            CustemorWiseViewModel getEmp = new CustemorWiseViewModel();


        //            var RecA = _tailordbContext.Empworks.Where(x => x.EmpIdfk == item.Id && x.Dept == item.DeptFk && x.Date.Value.Date >= dateA && x.Date.Value.Date <= dateB && x.RecStatus == "A").ToList();
        //            if (RecA.Count > 0)
        //            {
        //                double? totalQty = 0;
        //                double? total = 0;
        //                foreach (var A in RecA)
        //                {
        //                    double? qt = 0;
        //                    var deptname = _tailordbContext.Departments.FirstOrDefault(x => x.Id == item.DeptFk)?.DeptName;
        //                    getEmp.EmpName = item.Name;
        //                    getEmp.EmpDept = deptname;
        //                    getEmp.EmpId = item.Id;

        //                    CustomerRecordModel itemlist1 = new CustomerRecordModel();

        //                    //var Rates = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == A.ItemFk);

        //                    itemlist1.CName = A.CustomerName;
        //                    itemlist1.MemoNo = A.MemoNo;

        //                    qt = qt + A.CompletedOrder;
        //                    itemlist1.ComOrder = qt;
        //                    itemlist1.Rate = A.ItemRate;

        //                    itemlist1.Total = qt * A.ItemRate;
        //                    totalQty += A.CompletedOrder;
        //                    total += itemlist1.Total;
        //                    itemlist.Add(itemlist1);



        //                    getEmp.itemlists = itemlist;
        //                }
        //                getEmp.TComOrder = totalQty;
        //                getEmp.TotalS = total;


        //                EmployeeR.Add(getEmp);
        //            }





        //        }


        //        return Json(EmployeeR);
        //    }


            
        //}


        public IActionResult AddEmployee()
        {
            return View();
        }



        [HttpGet]
        public IActionResult ItemList()
        {


            var existingItems = _tailordbContext.Rateemps.Select(item => item.ItemIdFk).ToList();

            // Fetch the item list from the database excluding the items that are already in the dropdown
            var itemList = _tailordbContext.Items
                .Where(item => !existingItems.Contains(item.Id)) // Exclude existing items
                .Select(item => new ItemViewModel
                {
                    Id = item.Id,
                    Name = item.Name
                })
                .ToList();




            //var itemList = _tailordbContext.Items.Select(item => new ItemViewModel
            //{
            //    Id = item.Id,
            //    Name = item.Name
            //}).ToList();

            return Json(itemList);
        }

        public IActionResult AddItemRate()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddItemRate(RateViewModel product)
        {
            var Condition = _tailordbContext.Rateemps.FirstOrDefault(x => x.ItemIdFk == product.ItemId);

            if (Condition != null)
            {
               
            }
            else { 
            if(product.ItemId!=null)
            {
              
                Rateemp obj=new Rateemp();


                obj.ItemIdFk = product.ItemId;
             
                obj.StretchingR=product.StretchingR;
                obj.CuttingR=product.CuttingR;
                obj.IorningR=product.IorningR;  

                _tailordbContext.Add(obj);
                _tailordbContext.SaveChanges();

                }
            }

            return RedirectToAction("EmpPayrollSet", "Admin");
        }


       


        [HttpPost]
        public IActionResult UpdateItemRate(RateViewModel product)
        {
            if (ModelState.IsValid)
            {
                var obj=_tailordbContext.Items.FirstOrDefault(x=>x.Name==product.Name)?.Id;
              var rec=_tailordbContext.Rateemps.FirstOrDefault(x=>x.ItemIdFk==obj);
                if (rec != null)
                {
                    rec.StretchingR = product.StretchingR;
                    rec.IorningR = product.IorningR;
                    rec.CuttingR = product.CuttingR;
                    _tailordbContext.SaveChanges();
                }

                return Json(new { success = true, message = "Data updated successfully." });
            }
           
            else
            {

                return Json(new { success = false, message = "Invalid data. Please check the form fields." });
            }
        }



        //*****************************************************************************************
        [HttpGet]
        public IActionResult GetMonthlyReport(DateTime? startDate, DateTime? endDate)
        {
        DateTime? newEndDate=DateTime.Now;
            if (startDate != null || endDate != null)
            {
                 newEndDate = endDate.Value.AddDays(1);
            }
            List<BillHeaderMvModel> billheadermonthlyreport = new List<BillHeaderMvModel>();
            var billHeaders = _tailordbContext.Billheaders
                .Where(b => b.MemoDate >= startDate && b.MemoDate <= newEndDate)
                .ToList();

            foreach (var item in billHeaders)
            {
                List<itemlist> itemlist = new List<itemlist>();
                BillHeaderMvModel getbillHeader = new BillHeaderMvModel();
                getbillHeader.CustomerName = item.CustomerName;
                getbillHeader.CustomerAddress = item.CustomerAddress;
                getbillHeader.MemoNo = item.MemoNo;
                //getbillHeader.MemoDate = item.MemoDate;
                getbillHeader.MemoDate = $"{item.MemoDate:dd-MM-yyyy}";
                getbillHeader.CustomerMobile = item.CustomerMobile;
                //getbillHeader.DeliveryDate = item.DeliveryDate;
                getbillHeader.DeliveryDate = $"{item.DeliveryDate:dd-MM-yyyy}";

                var billdetailsTable = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id).ToList();
                if (billdetailsTable != null)
                {

                    foreach (var item2 in billdetailsTable)
                    {
                        itemlist itemlist1 = new itemlist();

                        itemlist1.ItemId = item2.ItemId;
                        itemlist1.ItemName = item2.ItemName;
                        itemlist1.Qty = item2.Qty;
                        itemlist1.Rate = item2.Rate;
                        itemlist1.Amount = item2.Amount;

                        itemlist.Add(itemlist1);
                    }


                }


                getbillHeader.itemlists = itemlist;
                billheadermonthlyreport.Add(getbillHeader);
            }

            return View(billheadermonthlyreport);
        }



        //use store procedure 
      
        public IActionResult GetMonthlyReportJson(DateTime? startDate, DateTime? endDate)
        {
            List<BillHeaderMvModel> billheadermonthlyreport = new List<BillHeaderMvModel>();
            DateTime? newEndDate = DateTime.Now;
            if (startDate != null || endDate != null)
            {
                newEndDate = endDate.Value.AddDays(1);
            }
            using (var command = _tailordbContext.Database.GetDbConnection().CreateCommand())
            {

                command.CommandText = "GetMonthlyReportSP";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("@startDate", startDate));
                command.Parameters.Add(new MySqlParameter("@endDate", newEndDate));

                _tailordbContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    int currentMemoNo = 0;
                    //int currentMemoNo1 = 0;
                    BillHeaderMvModel currentBillHeader = null;

                    while (result.Read())
                    {
                        bool isable = true;

                        int memoNo = Convert.ToInt32(result["MemoNo"]);
                        var itemAmount = Convert.ToDouble(result["Amount"]);

                        if (memoNo != currentMemoNo)
                        {
                            currentBillHeader = new BillHeaderMvModel
                            {
                                CustomerName = result["CustomerName"].ToString(),
                                CustomerAddress = result["CustomerAddress"].ToString(),
                                MemoNo = memoNo,
                                MemoDate = result["MemoDate"].ToString(),
                                CustomerMobile = result["CustomerMobile"].ToString(),
                                DeliveryDate = result["DeliveryDate"].ToString(),

                                totalAmount = 0,
                                ItemId = Convert.ToInt32(result["ItemId"]),
                                ItemName = result["ItemName"].ToString(),
                                Qty = Convert.ToInt32(result["Qty"]),
                                Rate = Convert.ToDouble(result["Rate"]),
                                Amount = itemAmount
                            };

                            billheadermonthlyreport.Add(currentBillHeader);
                            currentMemoNo = memoNo;
                            isable = false;
                        }

                        currentBillHeader.totalAmount += itemAmount;

                        if (isable)
                        {
                            var item = new itemlist
                            {
                                ItemId = Convert.ToInt32(result["ItemId"]),
                                ItemName = result["ItemName"].ToString(),
                                Qty = Convert.ToInt32(result["Qty"]),
                                Rate = Convert.ToDouble(result["Rate"]),
                                Amount = itemAmount
                            };

                            currentBillHeader.itemlists.Add(item);
                        }
                    }
                }
            }

            return Json(billheadermonthlyreport);
        }



        //Open New Window And Save the PDF





        //
        public IActionResult getmydata(DateTime? startDate, DateTime? endDate)
        {
            List<BillHeaderMvModel> billheadermonthlyreport = new List<BillHeaderMvModel>();
            DateTime? newEndDate = DateTime.Now;
            if (startDate != null || endDate != null)
            {
                newEndDate = endDate.Value.AddDays(1);
            }
            using (var command = _tailordbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "GetMonthlyReportSP";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new MySqlParameter("@startDate", startDate));
                command.Parameters.Add(new MySqlParameter("@endDate", newEndDate));

                _tailordbContext.Database.OpenConnection();

                using (var result = command.ExecuteReader())
                {
                    int currentMemoNo = 0;
                    BillHeaderMvModel currentBillHeader = null;

                    while (result.Read())
                    {
                        int memoNo = Convert.ToInt32(result["MemoNo"]);

                        if (memoNo != currentMemoNo)
                        {

                            currentBillHeader = new BillHeaderMvModel
                            {
                                CustomerName = result["CustomerName"].ToString(),
                                CustomerAddress = result["CustomerAddress"].ToString(),
                                MemoNo = memoNo,
                                MemoDate = result["MemoDate"].ToString(),
                                CustomerMobile = result["CustomerMobile"].ToString(),
                                DeliveryDate = result["DeliveryDate"].ToString(),
                                totalAmount = 0
                            };

                            billheadermonthlyreport.Add(currentBillHeader);
                            currentMemoNo = memoNo;
                        }


                        var itemAmount = Convert.ToDouble(result["Amount"]);
                        currentBillHeader.totalAmount += itemAmount;

                        var item = new itemlist
                        {
                            ItemId = Convert.ToInt32(result["ItemId"]),
                            ItemName = result["ItemName"].ToString(),
                            Qty = Convert.ToInt32(result["Qty"]),
                            Rate = Convert.ToDouble(result["Rate"]),
                            Amount = itemAmount
                        };

                        currentBillHeader.itemlists.Add(item);
                    }
                }
            }

   
            return View(billheadermonthlyreport);
        }
        //*****************************************************

        public IActionResult GetDeliveryReport()
        {
            return View();
        }


        //****************************************************

        //public IActionResult EmployeeSalary()
        //{
        //    EmpSalaryViewMode salary = new EmpSalaryViewMode();

        //    var ListR = _tailordbContext.Empolyeesalaries.ToList();
        //    foreach (var item in ListR)
        //    {
        //        EmpSalaryViewMode r = new EmpSalaryViewMode();
        //        var findItemName = _tailordbContext.Emps.FirstOrDefault(x => x.Id == item.EmpIdFk);

        //        if (findItemName != null)
        //        {
        //            r.EmpName = findItemName.Name;
        //            r.EmpType = findItemName.EmpType;
        //            r.EmpIdFk = item.EmpIdFk;
        //            r.AdvanceSalary = item.AdvanceSalary;
        //            r.AdvanceSalaryDate = item.AdvanceSalaryDate;
        //            //r.AdvanceSalaryDate = $"{item.AdvanceSalaryDate:dd-MM-yyyy}";
        //            r.PendingSalary = item.PendingSalary;


        //            salary.Salary.Add(r);
        //        }


        //    }
        //    return View(salary);
        //}
        //public IActionResult gettrasaction(int employeid, DateTime fromDate, DateTime toDate)
        //{
        //    _logger.Log("--------------------------------------------------------------------------------------------");
        //    _logger.Log("gettrasaction  accessed.");
        //    List<SalaryListViewModel> model = new List<SalaryListViewModel>();

        //    if (employeid != 0)
        //    {
        //        TempData["idd"] = employeid;

        //        _logger.Log($"gettrasaction  {employeid}.");
        //        if (!IsDateNotBeforeTenYears(fromDate) && !IsDateNotBeforeTenYears(toDate))
        //        {
        //            _logger.Log($"gettrasaction  {employeid}.");
        //            DateTime Date = DateTime.Now;
        //            _logger.Log($"gettrasaction Date {Date}.");
        //            var newdateforsave = Date.ToString("yyyy-MM-dd HH:mm:ss");
        //            _logger.Log($"gettrasaction  newdateforsave {newdateforsave}.");
        //            DateTime? startdateTimenew = DateTime.ParseExact(newdateforsave, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //            _logger.Log($"gettrasaction startdateTimenew {startdateTimenew}.");
        //            string[] format =
        //            {
        //                "d/M/yyyy h:mm:ss tt",
        //                "d/MM/yyyy h:mm:ss tt",
        //                "dd/M/yyyy h:mm:ss tt",
        //                "dd/MM/yyyy h:mm:ss tt",
        //                "yyyy/MM/dd",
        //                "yyyy-MM-dd",
        //                "yyyy-MM-dd hh:mm:ss",
        //                "yyyy-MM-dd HH:mm:ss",
        //                "yyyy-dd-MM hh:mm:ss",


        //            };
        //            var newDateString = "";
        //            DateTime givenDate;
        //            if (DateTime.TryParseExact(startdateTimenew.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate))
        //            {
        //                _logger.Log($"gettrasaction startdateTimenew {startdateTimenew}.");

        //                newDateString = givenDate.ToString("MM/dd/yyyy HH:mm:ss");

        //                _logger.Log($"gettrasaction newDateString {newDateString}.");
        //            }

        //            string updatedDateString = "";

        //            if (newDateString.Contains("-"))
        //            {
        //                updatedDateString = newDateString.Replace("-", "/");

        //                newDateString = updatedDateString;
        //            }

        //            _logger.Log($"gettrasaction newDateString {newDateString}.");
        //            DateTime givenDate111 = DateTime.ParseExact(newDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        //            _logger.Log($"gettrasaction givenDate111 {givenDate111}.");
        //            // Calculate the starting date of the month
        //            fromDate = new DateTime(givenDate111.Year, givenDate111.Month, 1);
        //            _logger.Log($"gettrasaction fromDate {fromDate}.");
        //            // Calculate the ending date of the month
        //            toDate = fromDate.AddMonths(1).AddDays(-1);
        //            _logger.Log($"gettrasaction toDate {toDate}.");
        //        }


        //        string[] format1 =
        //        {
        //                "d/M/yyyy h:mm:ss tt",
        //                "d/MM/yyyy h:mm:ss tt",
        //                "dd/M/yyyy h:mm:ss tt",
        //                "dd/MM/yyyy h:mm:ss tt",
        //                "MM/dd/yyyy HH:mm:ss",
        //                "MM/dd/yyyy HH:mm:ss tt",
        //                "MM/dd/yyyy h:mm:ss",
        //                "MM/dd/yyyy h:mm:ss tt",
        //                 "yyyy/MM/dd",
        //                "yyyy-MM-dd",
        //                "yyyy-MM-dd hh:mm:ss",
        //                "yyyy-MM-dd HH:mm:ss",
        //                "yyyy-dd-MM hh:mm:ss",

        //            };
        //        var newDateString1 = "";
        //        var newDateString2 = "";
        //        DateTime givenDate1;
        //        DateTime givenDate2;
        //        _logger.Log($"gettrasaction format1 {format1}.");
        //        if (DateTime.TryParseExact(fromDate.ToString(), format1, CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate1))
        //        {
        //            _logger.Log($"gettrasaction fromDate {fromDate}.");
        //            newDateString1 = givenDate1.ToString("MM/dd/yyyy HH:mm:ss");
        //            _logger.Log($"gettrasaction newDateString1 {newDateString1}.");

        //        }

        //        if (DateTime.TryParseExact(toDate.ToString(), format1, CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate2))
        //        {
        //            _logger.Log($"gettrasaction toDate {toDate}.");
        //            newDateString2 = givenDate2.ToString("MM/dd/yyyy HH:mm:ss");

        //            _logger.Log($"gettrasaction newDateString2 {newDateString2}.");
        //        }

        //        _logger.Log($"gettrasaction  {employeid}.");
        //        DateTime givenDatestartdate = DateTime.ParseExact(newDateString1, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        //        _logger.Log($"gettrasaction givenDatestartdate {givenDatestartdate}.");
        //        DateTime givenDateendddate = DateTime.ParseExact(newDateString2, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
        //        _logger.Log($"gettrasaction  givenDateendddate {givenDateendddate}.");
        //        var empname = _tailordbContext.Emps.Where(x => x.Id == employeid).Select(x => x.Name).FirstOrDefault();
        //        _logger.Log($"gettrasaction empname {empname}.");

        //        var datalist = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employeid && x.AdvanceSalaryDate >= givenDatestartdate && x.AdvanceSalaryDate <= givenDateendddate).OrderByDescending(x => x.Id).ToList();

        //        foreach (var item in datalist)
        //        {
        //            SalaryListViewModel obj = new SalaryListViewModel();

        //            obj.Name = empname;
        //            obj.paid = item.AdvanceSalary;
        //            obj.pendingsal = item.PendingSalary;
        //            obj.date = item.AdvanceSalaryDate?.ToString("dd-MM-yyyy");

        //            model.Add(obj);
        //        }





        //    }

        //    return View(model);
        //}



        public IActionResult gettrasaction(int employeid, DateTime fromDate, DateTime toDate)
        {
            _logger.Log("--------------------------------------------------------------------------------------------");
            _logger.Log("gettrasaction accessed.");

            List<SalaryListViewModel> model = new List<SalaryListViewModel>();

            if (employeid != 0)
            {
                TempData["idd"] = employeid;
                _logger.Log($"gettrasaction employeid: {employeid}");

                // If the date is more than 10 years old, normalize to current month
                if (!IsDateNotBeforeTenYears(fromDate) && !IsDateNotBeforeTenYears(toDate))
                {
                    DateTime now = DateTime.Now;
                    fromDate = new DateTime(now.Year, now.Month, 1);
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    _logger.Log($"Dates reset to current month: fromDate={fromDate}, toDate={toDate}");
                }

                // Ensure time portion is normalized for full day range
                fromDate = fromDate.Date; // 00:00:00
                toDate = toDate.Date.AddDays(1).AddTicks(-1); // 23:59:59.9999999

                var empname = _tailordbContext.Emps
                                  .Where(x => x.Id == employeid)
                                  .Select(x => x.Name)
                                  .FirstOrDefault();
                _logger.Log($"Employee Name: {empname}");

                var datalist = _tailordbContext.Empolyeesalaries
                                  .Where(x => x.EmpIdFk == employeid &&
                                              x.AdvanceSalaryDate >= fromDate &&
                                              x.AdvanceSalaryDate <= toDate)
                                  .OrderByDescending(x => x.Id)
                                  .ToList();

                foreach (var item in datalist)
                {
                    model.Add(new SalaryListViewModel
                    {
                        Name = empname,
                        paid = item.AdvanceSalary,
                        pendingsal = item.PendingSalary,
                        date = item.AdvanceSalaryDate?.ToString("dd-MM-yyyy")
                    });
                }
            }

            return View(model);
        }




        [HttpGet]
        public JsonResult gettrasactionJson(int employeid, DateTime fromDate, DateTime toDate)
        {
            _logger.Log("gettrasaction (JSON) accessed.");
            List<SalaryListViewModel> model = new List<SalaryListViewModel>();

            if (employeid != 0)
            {
                TempData["idd"] = employeid;

                if (!IsDateNotBeforeTenYears(fromDate) && !IsDateNotBeforeTenYears(toDate))
                {
                    DateTime now = DateTime.Now;
                    fromDate = new DateTime(now.Year, now.Month, 1);
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                }

                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddTicks(-1);

                var empname = _tailordbContext.Emps
                                   .Where(x => x.Id == employeid)
                                   .Select(x => x.Name)
                                   .FirstOrDefault();

                var datalist = _tailordbContext.Empolyeesalaries
                                   .Where(x => x.EmpIdFk == employeid &&
                                               x.AdvanceSalaryDate >= fromDate &&
                                               x.AdvanceSalaryDate <= toDate)
                                   .OrderByDescending(x => x.Id)
                                   .ToList();

                foreach (var item in datalist)
                {
                    model.Add(new SalaryListViewModel
                    {
                        Name = empname,
                        paid = item.AdvanceSalary,
                        pendingsal = item.PendingSalary,
                        date = item.AdvanceSalaryDate?.ToString("dd-MM-yyyy")
                    });
                }
            }

            return Json(model);
        }

        public bool IsDateNotBeforeTenYears(DateTime date)
        {
            // Get today's date
            DateTime today = DateTime.Today;

            // Calculate the date 10 years ago from today
            DateTime tenYearsAgo = today.AddYears(-10);

            // Check if the given date is not before 10 years from today
            return date >= tenYearsAgo;
        }



    

        public IActionResult EmployeeSalary()
        {
            EmpSalaryViewMode salary = new EmpSalaryViewMode();

            var query = _tailordbContext.Empolyeesalaries.ToList();

                var employlist = _tailordbContext.Emps.ToList();
                foreach (var i in employlist)
                {
                    bool ispermanant = false;

                    if(i.EmpType== "Permanent")
                    {
                        ispermanant = true;
                    }

                    if (ispermanant)
                    {
                        var lastdata = query
                       .Where(x => x.EmpIdFk == i.Id)
                       .OrderBy(x => x.Id) // Add OrderBy to specify the sort order
                       .LastOrDefault();

                        if (lastdata != null)
                        {
                            EmpSalaryViewMode r = new EmpSalaryViewMode();


                            r.Id = i.Id;
                            r.EmpName = i.Name;
                            r.EmpType = i.EmpType;
                            r.EmpIdFk = lastdata.EmpIdFk;
                            r.AdvanceSalary = lastdata.AdvanceSalary;
                            r.AdvanceSalaryDate = lastdata.AdvanceSalaryDate;
                            r.PendingSalary = lastdata.PendingSalary;

                            salary.Salary.Add(r);

                        }
                     
                    }
                    else
                    { 
                        double? fixsal = 0.00;
                        double? mainfixsal = 0.00;
                        double? S = 0;
                        var record = _tailordbContext.Empworks.Where(x => x.EmpIdfk == i.Id && x.Dept==i.DeptFk && x.RecStatus=="A").ToList();

                        if (record.Count > 0)
                        {
                            foreach (var item in record)
                        {

                            S +=item.CompletedOrder * item.ItemRate;
                            
                            }



                            fixsal = S;
                        }

                        double? totalpaidamt = 0.00;
                        var salarydetailsnonper = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == i.Id).ToList();
                        foreach (var j in salarydetailsnonper)
                        {

                            totalpaidamt = totalpaidamt + j.AdvanceSalary;
                        }

                        mainfixsal = fixsal - totalpaidamt;

                        var lastdata1 = query
                      .Where(x => x.EmpIdFk == i.Id)
                      .OrderBy(x => x.Id) // Add OrderBy to specify the sort order
                      .LastOrDefault();

                        EmpSalaryViewMode r = new EmpSalaryViewMode();


                        r.Id = i.Id;
                        r.EmpName = i.Name;
                        r.EmpType = i.EmpType;
                        r.EmpIdFk = i.Id;
                        if(lastdata1 != null)
                        {
                            r.AdvanceSalary = lastdata1.AdvanceSalary;
                            r.AdvanceSalaryDate = lastdata1.AdvanceSalaryDate;
                        }
                        else
                        {
                            r.AdvanceSalary = 0;
                            r.AdvanceSalaryDate = null;
                        }
                      
                        r.PendingSalary = mainfixsal;

                        salary.Salary.Add(r);
                    }



                   

                }


           // }

            return View(salary);


        }



      

        [HttpGet]
        public IActionResult GetEmplyeeList()
        {
            var employeeList = _tailordbContext.Emps.Where(x=>x.RecStatus=="A").Select(emplyee => new EmployeeViewModel
            {
                Id = emplyee.Id,
                Name = emplyee.Name,
                
            }).ToList();

            return Json(employeeList);
        }

        //********************************************


        //public IActionResult AddEmployeeSalary()
        //{
        //    return View();
        //}

        [HttpPost]
        public IActionResult EmployeeSalaryJson(int deptId, int employeid)
        {
            EmpSalaryViewMode salary = new EmpSalaryViewMode();

            var query = _tailordbContext.Empolyeesalaries.ToList();

            List<Emp> employlist = new List<Emp>();

            if (employeid > 0)
            {
                employlist = _tailordbContext.Emps.Where(x => x.Id == employeid).ToList();
            }
            else if (deptId > 0)
            {
                employlist = _tailordbContext.Emps.Where(x => x.DeptFk == deptId).ToList();
            }
            else
            {
                // Optional: if no filter provided, return all or none
                employlist = _tailordbContext.Emps.ToList(); // or fetch all
            }
            foreach (var i in employlist)
            {
                bool ispermanant = false;

                if (i.EmpType == "Permanent")
                {
                    ispermanant = true;
                }

                if (ispermanant)
                {
                    var lastdata = query
                   .Where(x => x.EmpIdFk == i.Id)
                   .OrderBy(x => x.Id) // Add OrderBy to specify the sort order
                   .LastOrDefault();

                    if (lastdata != null)
                    {
                        EmpSalaryViewMode r = new EmpSalaryViewMode();


                        r.Id = i.Id;
                        r.EmpName = i.Name;
                        r.EmpType = i.EmpType;
                        r.EmpIdFk = lastdata.EmpIdFk;
                        r.AdvanceSalary = lastdata.AdvanceSalary;
                        r.AdvanceSalaryDate = lastdata.AdvanceSalaryDate;
                        r.PendingSalary = lastdata.PendingSalary;
                        r.Departmentofemployee = _tailordbContext.Departments.FirstOrDefault(x => x.Id == i.DeptFk)?.DeptName;
                        salary.Salary.Add(r);

                    }

                }
                else
                {
                    double? fixsal = 0.00;
                    double? mainfixsal = 0.00;
                    double? S = 0;
                    var record = _tailordbContext.Empworks.Where(x => x.EmpIdfk == i.Id && x.Dept == i.DeptFk && x.RecStatus == "A").ToList();

                    if (record.Count > 0)
                    {
                        foreach (var item in record)
                        {

                            S += item.CompletedOrder * item.ItemRate;

                        }



                        fixsal = S;
                    }

                    double? totalpaidamt = 0.00;
                    var salarydetailsnonper = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == i.Id).ToList();
                    foreach (var j in salarydetailsnonper)
                    {

                        totalpaidamt = totalpaidamt + j.AdvanceSalary;
                    }

                    mainfixsal = fixsal - totalpaidamt;

                    var lastdata1 = query
                  .Where(x => x.EmpIdFk == i.Id)
                  .OrderBy(x => x.Id) // Add OrderBy to specify the sort order
                  .LastOrDefault();

                    EmpSalaryViewMode r = new EmpSalaryViewMode();


                    r.Id = i.Id;
                    r.EmpName = i.Name;
                    r.EmpType = i.EmpType;
                    r.EmpIdFk = i.Id;
                    r.Departmentofemployee = _tailordbContext.Departments.FirstOrDefault(x => x.Id == i.DeptFk)?.DeptName;

                    if (lastdata1 != null)
                    {
                        r.AdvanceSalary = lastdata1.AdvanceSalary;
                        r.AdvanceSalaryDate = lastdata1.AdvanceSalaryDate;
                    }
                    else
                    {
                        r.AdvanceSalary = 0;
                        r.AdvanceSalaryDate = null;
                    }

                    r.PendingSalary = mainfixsal;

                    salary.Salary.Add(r);
                }





            }

            return Json(salary); // Optional: use Json if AJAX expects JSON
        }

        //[HttpPost]
        //public IActionResult EmployeeSalaryJ(EmpSalaryViewMode empSalary)
        //{

        //    if (empSalary != null)
        //    {
        //        var ispermanent = false;

        //        DateTime Date = DateTime.Now;

        //        var newdateforsave = Date.ToString("yyyy-MM-dd 00:00:00");
        //        DateTime? startdateTimenew = DateTime.ParseExact(newdateforsave, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);


        //        empSalary.AdvanceSalaryDate = startdateTimenew;


        //        var employedetail = _tailordbContext.Emps.FirstOrDefault(x => x.Id == empSalary.EmpIdFk);
        //        if (employedetail != null)
        //        {
        //            double? fixsal = 0.00;
        //            double? mainfixsal = 0.00;
        //            if (employedetail.EmpType== "Permanent")
        //            {
        //                ispermanent = true;
        //            }


        //            string[] format =
        //            {
        //                "d/M/yyyy h:mm:ss tt",
        //                "d/MM/yyyy h:mm:ss tt",
        //                "dd/M/yyyy h:mm:ss tt",
        //                "dd/MM/yyyy h:mm:ss tt",

        //            };
        //            var newDateString = "";
        //            DateTime givenDate;
        //            if (DateTime.TryParseExact(empSalary.AdvanceSalaryDate.ToString(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate))
        //            {

        //                newDateString = givenDate.ToString("MM/dd/yyyy HH:mm:ss");


        //            }

        //            DateTime givenDate1 = DateTime.ParseExact(newDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

        //            // Calculate the starting date of the month
        //            DateTime monthStartDate = new DateTime(givenDate1.Year, givenDate1.Month, 1);

        //            // Calculate the ending date of the month
        //            DateTime monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);
        //            DateTime privmonthStartDate = monthStartDate.AddMonths(-1);
        //            DateTime privmonthEndDate = privmonthStartDate.AddMonths(1).AddDays(-1);



        //            // Output the results
        //            Console.WriteLine("Month Start Date: " + monthStartDate.ToString("yyyy-MM-dd 00:00:00"));
        //            Console.WriteLine("Month End Date: " + monthEndDate.ToString("yyyy-MM-dd 00:00:00"));

        //            DateTime? startdateTime = DateTime.ParseExact(monthStartDate.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //            DateTime? enddateTime = DateTime.ParseExact(monthEndDate.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

        //            Empolyeesalary obj = new Empolyeesalary();

        //            if (ispermanent)
        //            {
        //                fixsal = employedetail.Salary;
        //            }
        //            else
        //            {
        //                double? S = 0;



        //                var record = _tailordbContext.Empworks.Where(x => x.EmpIdfk == employedetail.Id && x.Dept == employedetail.DeptFk && x.RecStatus == "A").ToList();

        //                if (record.Count > 0)
        //                {
        //                    foreach (var item in record)
        //                    {

        //                        S += item.CompletedOrder * item.ItemRate;

        //                    }



        //                    fixsal = S;
        //                }


        //            }


        //            double? advancesal = 0.00;

        //            if (ispermanent)
        //            {
        //                var salarydetails = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id && x.AdvanceSalaryDate >= startdateTime && x.AdvanceSalaryDate <= enddateTime).OrderBy(x => x.Id).LastOrDefault();
        //                if (salarydetails != null)
        //                {

        //                    advancesal = salarydetails.PendingSalary;
        //                    mainfixsal = salarydetails.PendingSalary;


        //                }
        //                else
        //                {
        //                    var salarydetails1 = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id && x.AdvanceSalaryDate >= privmonthStartDate && x.AdvanceSalaryDate <= privmonthEndDate).OrderBy(x => x.Id).LastOrDefault();

        //                    if (salarydetails1 != null)
        //                    {
        //                        advancesal = salarydetails1.PendingSalary;
        //                        mainfixsal = salarydetails1.PendingSalary + fixsal;


        //                    }
        //                    else
        //                    {
        //                        mainfixsal = fixsal;
        //                    }

        //                }

        //            }
        //            else
        //            {
        //                double? totalpaidamt = 0.00;
        //                var salarydetailsnonper = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id).ToList();
        //                foreach(var i in  salarydetailsnonper) {

        //                    totalpaidamt = totalpaidamt + i.AdvanceSalary;
        //                }

        //                mainfixsal = fixsal - totalpaidamt;

        //            }




        //            obj.EmpIdFk = empSalary.EmpIdFk;

        //                obj.AdvanceSalary = empSalary.AdvanceSalary;


        //                obj.AdvanceSalaryDate = empSalary.AdvanceSalaryDate;



        //                obj.PendingSalary = mainfixsal - empSalary.AdvanceSalary;

        //                _tailordbContext.Add(obj);
        //                _tailordbContext.SaveChanges();




        //        }
        //    }



        //    return Ok();
        //}

        [HttpPost]
        public IActionResult EmployeeSalaryJ(EmpSalaryViewMode empSalary)
        {
            if (empSalary != null)
            {
                bool isPermanent = false;
                DateTime now = DateTime.Now;

                // Format current date as "yyyy-MM-dd 00:00:00"
                string formattedDate = now.ToString("yyyy-MM-dd 00:00:00");
                DateTime salaryDate = DateTime.ParseExact(formattedDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                empSalary.AdvanceSalaryDate = salaryDate;

                var employee = _tailordbContext.Emps.FirstOrDefault(x => x.Id == empSalary.EmpIdFk);
                if (employee != null)
                {
                    double? fixedSalary = 0.00;
                    double? mainFixedSalary = 0.00;

                    if (employee.EmpType == "Permanent")
                        isPermanent = true;

                    // Use the already parsed salaryDate instead of string conversions
                    DateTime monthStart = new DateTime(salaryDate.Year, salaryDate.Month, 1);
                    DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    DateTime prevMonthStart = monthStart.AddMonths(-1);
                    DateTime prevMonthEnd = prevMonthStart.AddMonths(1).AddDays(-1);

                    DateTime? startDateTime = DateTime.ParseExact(monthStart.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime? endDateTime = DateTime.ParseExact(monthEnd.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    Empolyeesalary salaryRecord = new Empolyeesalary();

                    if (isPermanent)
                    {
                        fixedSalary = employee.Salary;
                    }
                    else
                    {
                        double? total = 0.0;
                        var workRecords = _tailordbContext.Empworks
                            .Where(x => x.EmpIdfk == employee.Id && x.Dept == employee.DeptFk && x.RecStatus == "A")
                            .ToList();

                        foreach (var record in workRecords)
                        {
                            total += record.CompletedOrder * record.ItemRate;
                        }

                        fixedSalary = total;
                    }

                    double? advanceSalary = 0.00;

                    if (isPermanent)
                    {
                        var currentMonthSalary = _tailordbContext.Empolyeesalaries
                            .Where(x => x.EmpIdFk == employee.Id && x.AdvanceSalaryDate >= startDateTime && x.AdvanceSalaryDate <= endDateTime)
                            .OrderBy(x => x.Id)
                            .LastOrDefault();

                        if (currentMonthSalary != null)
                        {
                            advanceSalary = currentMonthSalary.PendingSalary;
                            mainFixedSalary = currentMonthSalary.PendingSalary;
                        }
                        else
                        {
                            var prevMonthSalary = _tailordbContext.Empolyeesalaries
                                .Where(x => x.EmpIdFk == employee.Id && x.AdvanceSalaryDate >= prevMonthStart && x.AdvanceSalaryDate <= prevMonthEnd)
                                .OrderBy(x => x.Id)
                                .LastOrDefault();

                            if (prevMonthSalary != null)
                            {
                                advanceSalary = prevMonthSalary.PendingSalary;
                                mainFixedSalary = prevMonthSalary.PendingSalary + fixedSalary;
                            }
                            else
                            {
                                mainFixedSalary = fixedSalary;
                            }
                        }
                    }
                    else
                    {
                        double? totalPaid = _tailordbContext.Empolyeesalaries
                            .Where(x => x.EmpIdFk == employee.Id)
                            .Sum(x => x.AdvanceSalary) ?? 0.0;

                        mainFixedSalary = fixedSalary - totalPaid;
                    }

                    salaryRecord.EmpIdFk = empSalary.EmpIdFk;
                    salaryRecord.AdvanceSalary = empSalary.AdvanceSalary;
                    salaryRecord.AdvanceSalaryDate = empSalary.AdvanceSalaryDate;
                    salaryRecord.PendingSalary = mainFixedSalary - empSalary.AdvanceSalary;

                    _tailordbContext.Add(salaryRecord);
                    _tailordbContext.SaveChanges();
                }
            }

            return Ok();
        }




        [HttpPost]
        public IActionResult saveedietddata(string EmpName, string id, string pendingsal, string selecteddate)
        {
            _logger.Log("saveedietddata  accessed.");
            _logger.Log("saveedietddata  accessed.");
            var ispermanent = false;
            if (!string.IsNullOrEmpty(id))
            {

                DateTime Date = DateTime.Now;
                _logger.Log($"saveedietddata {Date} .");
                selecteddate = Date.ToString("yyyy-MM-dd HH:mm:ss");

                _logger.Log($"saveedietddata {Date} .");


                var employedetail = _tailordbContext.Emps.FirstOrDefault(x => x.Id == Convert.ToInt32(id));
                if (employedetail != null)
                {
                    _logger.Log($"saveedietddata {employedetail} .");
                    double? fixsal = 0.00;
                    double? mainfixsal = 0.00;
                    if (employedetail.EmpType == "Permanent")
                    {
                        ispermanent = true;
                    }
                    _logger.Log($"saveedietddata {ispermanent} .");
                    string[] format =
                    {
                        "d/M/yyyy h:mm:ss tt",
                        "d/MM/yyyy h:mm:ss tt",
                        "dd/M/yyyy h:mm:ss tt",
                        "dd/MM/yyyy h:mm:ss tt",
                        "yyyy/MM/dd",
                        "yyyy-MM-dd",
                        "yyyy-MM-dd hh:mm:ss",
                        "yyyy-MM-dd HH:mm:ss",
                        "yyyy-dd-MM hh:mm:ss",

                    };
                    _logger.Log($"saveedietddata {format} .");
                    var newDateString = "";
                    DateTime givenDate;
                    if (DateTime.TryParseExact(selecteddate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out givenDate))
                    {
                        _logger.Log($"saveedietddata {givenDate} .");

                        newDateString = givenDate.ToString("MM/dd/yyyy HH:mm:ss");

                        _logger.Log($"saveedietddata {newDateString} .");
                    }

                    string updatedDateString = "";

                    if (newDateString.Contains("-"))
                    {
                        updatedDateString = newDateString.Replace("-", "/");

                        newDateString = updatedDateString;
                    }
                   
                     

                    DateTime givenDate1 = DateTime.ParseExact(newDateString, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                    _logger.Log($"saveedietddata givenDate1 {givenDate1} .");

                    // Calculate the starting date of the month
                    DateTime monthStartDate = new DateTime(givenDate1.Year, givenDate1.Month, 1);

                    _logger.Log($"saveedietddata monthStartDate {monthStartDate} .");

                    // Calculate the ending date of the month
                    DateTime monthEndDate = monthStartDate.AddMonths(1).AddDays(-1);

                    _logger.Log($"saveedietddata monthEndDate {monthEndDate} .");
                    DateTime privmonthStartDate = monthStartDate.AddMonths(-1);

                    _logger.Log($"saveedietddata privmonthStartDate {privmonthStartDate} .");
                    DateTime privmonthEndDate = privmonthStartDate.AddMonths(1).AddDays(-1);

                    _logger.Log($"saveedietddata privmonthEndDate {privmonthEndDate} .");




                    DateTime? startdateTime = DateTime.ParseExact(monthStartDate.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    _logger.Log($"saveedietddata startdateTime {startdateTime} .");
                    DateTime? enddateTime = DateTime.ParseExact(monthEndDate.ToString("yyyy-MM-dd 00:00:00"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                    _logger.Log($"saveedietddata enddateTime {enddateTime} .");
                    Empolyeesalary obj = new Empolyeesalary();

                    if (ispermanent)
                    {
                        _logger.Log($"saveedietddata ispermanent {ispermanent} .");
                        fixsal = employedetail.Salary;
                    }
                    else
                    {
                        double? S = 0;

                       

                        var record = _tailordbContext.Empworks.Where(x => x.EmpIdfk == employedetail.Id && x.Dept == employedetail.DeptFk && x.RecStatus == "A").ToList();

                        _logger.Log($"saveedietddata record {record} .");

                        if (record.Count > 0)
                        {
                            foreach (var item in record)
                            {

                                S += item.CompletedOrder * item.ItemRate;

                            }



                            fixsal = S;
                        }
                    }
                    double? advancesal = 0.00;

                   

                    if (ispermanent)
                    {
                        var salarydetails = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id && x.AdvanceSalaryDate >= startdateTime && x.AdvanceSalaryDate <= enddateTime).OrderBy(x => x.Id).LastOrDefault();
                        if (salarydetails != null)
                        {

                            advancesal = salarydetails.PendingSalary;
                            mainfixsal = salarydetails.PendingSalary;

                          
                        }
                        else
                        {
                            var salarydetails1 = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id && x.AdvanceSalaryDate >= privmonthStartDate && x.AdvanceSalaryDate <= privmonthEndDate).OrderBy(x => x.Id).LastOrDefault();

                            if (salarydetails1 != null)
                            {
                                advancesal = salarydetails1.PendingSalary;
                                mainfixsal = salarydetails1.PendingSalary + fixsal;


                            }
                            else
                            {
                                mainfixsal = fixsal;
                            }

                        }

                    }
                    else
                    {
                        double? totalpaidamt = 0.00;
                        var salarydetailsnonper = _tailordbContext.Empolyeesalaries.Where(x => x.EmpIdFk == employedetail.Id).ToList();
                        foreach (var i in salarydetailsnonper)
                        {

                            totalpaidamt = totalpaidamt + i.AdvanceSalary;
                        }

                        mainfixsal = fixsal - totalpaidamt;

                    }



                    obj.EmpIdFk = Convert.ToInt32(id);

                    obj.AdvanceSalary = Convert.ToInt32(pendingsal);


                    obj.AdvanceSalaryDate = givenDate;


                    //   obj.AdvanceSalaryDate = startdateTimenew;
                    obj.PendingSalary = mainfixsal - Convert.ToInt32(pendingsal);

                    _tailordbContext.Add(obj);
                    _tailordbContext.SaveChanges();    

                    return Json(new { success = true, message = "Data save successfully." });

                }

                return Json(new { success = true, message = "employedetail not found." });
            }

            return Json(new { success = true, message = "invalid data" });


        }
        //****************************************












        //********************   Get Graph       *********************
        public IActionResult GetGraphJ(DateTime? startDate, DateTime? endDate,int itemfk)
        {
            if (startDate == endDate)
            {
                if (itemfk != null && itemfk > 0)
                {
                    List<Billdetail> getdata = new List<Billdetail>();
                    var data = _tailordbContext.Billheaders
                   .Where(e => e.MemoDate.Value.Date == startDate.Value.Date)
                   .ToList();

                    foreach (var item in data)
                    {
                        var billDetails = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id && x.ItemId == itemfk).ToList();
                        getdata.AddRange(billDetails);
                    }

                    var result = getdata
                        .GroupBy(e => e.ItemName)
                        .Select(g => new ItemQuantityViewModel
                        {
                            ItemName = g.Key,
                            Qty = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }
                else
                {
                    List<Billdetail> getdata = new List<Billdetail>();
                    var data = _tailordbContext.Billheaders
                  .Where(e => e.MemoDate.Value.Date == startDate.Value.Date)
                  .ToList();

                    foreach (var item in data)
                    {
                        var billDetails = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id).ToList();
                        getdata.AddRange(billDetails);
                    }

                    var result = getdata
                        .GroupBy(e => e.ItemName)
                        .Select(g => new ItemQuantityViewModel
                        {
                            ItemName = g.Key,
                            Qty = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }

            }
            else
            {
                if (itemfk != null && itemfk > 0)
                {
                    List<Billdetail> getdata = new List<Billdetail>();
                    var data = _tailordbContext.Billheaders
                   .Where(e => e.MemoDate >= startDate && e.MemoDate <= endDate)
                   .ToList();

                    foreach (var item in data)
                    {
                        var billDetails = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id && x.ItemId == itemfk).ToList();
                        getdata.AddRange(billDetails);
                    }

                    var result = getdata
                        .GroupBy(e => e.ItemName)
                        .Select(g => new ItemQuantityViewModel
                        {
                            ItemName = g.Key,
                            Qty = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }
                else
                {
                    List<Billdetail> getdata = new List<Billdetail>();
                    var data = _tailordbContext.Billheaders
                  .Where(e => e.MemoDate >= startDate && e.MemoDate <= endDate)
                  .ToList();

                    foreach (var item in data)
                    {
                        var billDetails = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id).ToList();
                        getdata.AddRange(billDetails);
                    }

                    var result = getdata
                        .GroupBy(e => e.ItemName)
                        .Select(g => new ItemQuantityViewModel
                        {
                            ItemName = g.Key,
                            Qty = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }

            }





        }



        public IActionResult GetGraph()
        {
            
                return View();
           

        }




        /***********************  Delivery Graph        *************************/

        public IActionResult DeliveryStatusJ(DateTime? startDate, DateTime? endDate,int itemfk)
        {
            if (startDate == endDate)
            {
                if (itemfk != null && itemfk > 0)
                {
                    var data = _tailordbContext.Billdetails
                               .Where(e => e.ItemDeliveryDate.Value.Date == startDate.Value.Date && e.ItemDeliveryDate <= endDate && e.ItemId == itemfk && e.Status == "Delivery Done")
                               .ToList();

                    var result = data
                        .GroupBy(e => e.ItemName)
                        .Select(g => new DeliveryStatusViewModel
                        {
                            ItemName = g.Key,
                            Status = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }
                else
                {
                    var data = _tailordbContext.Billdetails
                               .Where(e => e.ItemDeliveryDate.Value.Date == startDate.Value.Date && e.Status == "Delivery Done")
                               .ToList();

                    var result = data
                        .GroupBy(e => e.ItemName)
                        .Select(g => new DeliveryStatusViewModel
                        {
                            ItemName = g.Key,
                            Status = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }

            }
            else
            {
                if (itemfk != null && itemfk > 0)
                {
                    var data = _tailordbContext.Billdetails
                               .Where(e => e.ItemDeliveryDate >= startDate && e.ItemDeliveryDate <= endDate && e.ItemId == itemfk && e.Status == "Delivery Done")
                               .ToList();

                    var result = data
                        .GroupBy(e => e.ItemName)
                        .Select(g => new DeliveryStatusViewModel
                        {
                            ItemName = g.Key,
                            Status = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }
                else
                {
                    var data = _tailordbContext.Billdetails
                               .Where(e => e.ItemDeliveryDate >= startDate && e.ItemDeliveryDate <= endDate && e.Status == "Delivery Done")
                               .ToList();

                    var result = data
                        .GroupBy(e => e.ItemName)
                        .Select(g => new DeliveryStatusViewModel
                        {
                            ItemName = g.Key,
                            Status = (int)g.Sum(e => e.Qty)
                        })
                        .ToList();

                    return Json(result);
                }
            }


          


        }

       

        public IActionResult DeliveryStatus()
        {

           

                return View();
           

           
        }


        //*************************** Empwork Graph  *****************************
        public IActionResult GetGraphbyEmpworkJ(DateTime? startDate, DateTime? endDate,int itemfk, string supplier)
        {
            if(startDate== endDate)
            {
                
                if (supplier != null && supplier != "")
                {
                    var EID = _tailordbContext.Emps.FirstOrDefault(x => x.Name == supplier)?.Id;


                    if (itemfk != null && itemfk > 0)
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date.Value.Date== startDate.Value.Date && e.RecStatus == "A" && e.ItemFk == itemfk && e.EmpIdfk == EID).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();


                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;
                        return Json(result);
                    }
                    else
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date.Value.Date == startDate.Value.Date && e.RecStatus == "A" && e.EmpIdfk == EID).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();

                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;

                        return Json(result);
                    }
                }
                else
                {
                    if (itemfk != null && itemfk > 0)
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date.Value.Date == startDate.Value.Date && e.RecStatus == "A" && e.ItemFk == itemfk).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();
                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;

                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;
                        return Json(result);
                    }
                    else
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date.Value.Date == startDate.Value.Date && e.RecStatus == "A").ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();

                        //ViewBag.StartDate = startDate;
                        //ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        //var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        //ViewBag.Itemfk = iname;
                        //ViewBag.Supplier = supplier;


                        return Json(result);
                    }
                }

            }
            else
            {
                if (supplier != null && supplier != "")
                {
                    var EID = _tailordbContext.Emps.FirstOrDefault(x => x.Name == supplier)?.Id;


                    if (itemfk != null && itemfk > 0)
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date >= startDate && e.Date <= endDate && e.RecStatus == "A" && e.ItemFk == itemfk && e.EmpIdfk == EID).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();


                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;
                        return Json(result);
                    }
                    else
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date >= startDate && e.Date <= endDate && e.RecStatus == "A" && e.EmpIdfk == EID).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();

                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;

                        return Json(result);
                    }
                }
                else
                {
                    if (itemfk != null && itemfk > 0)
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date >= startDate && e.Date <= endDate && e.RecStatus == "A" && e.ItemFk == itemfk).ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();
                        ViewBag.StartDate = startDate;
                        ViewBag.EndDate = endDate;

                        var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        ViewBag.Itemfk = iname;
                        ViewBag.Supplier = supplier;
                        return Json(result);
                    }
                    else
                    {
                        var getdata = _tailordbContext.Empworks.Where(e => e.Date >= startDate && e.Date <= endDate && e.RecStatus == "A").ToList();

                        var result = getdata
                          .GroupBy(e => e.ItemName)
                          .Select(g => new EmpWorkViewModal
                          {
                              ItemName = g.Key,
                              QtyC = (int?)g.Where(e => e.Dept == 1).Sum(e => e.CompletedOrder),
                              QtyS = (int?)g.Where(e => e.Dept == 2).Sum(e => e.CompletedOrder),
                              QtyI = (int?)g.Where(e => e.Dept == 3).Sum(e => e.CompletedOrder),
                          })
                          .ToList();

                        //ViewBag.StartDate = startDate;
                        //ViewBag.EndDate = endDate;
                        //ViewBag.Today = today;
                        //var iname = _tailordbContext.Items.FirstOrDefault(x => x.Id == itemfk)?.Name;
                        //ViewBag.Itemfk = iname;
                        //ViewBag.Supplier = supplier;


                        return Json(result);
                    }
                }
            }

          

           



        }

        public IActionResult GetGraphbyEmpwork(DateTime? startDate, DateTime? endDate, bool? today,int itemfk,string supplier)
         {




            return View();
           
        }


        //*************************************************************
        public IActionResult ItemListG()
        {
            var itemList = _tailordbContext.Items.Select(item => new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name
            }).ToList();

            return Json(itemList);
        }

        public IActionResult EmployeeWorkSet()
        {

            DateTime date = DateTime.Today;
           
            cardlistviewModelList BillIn = new cardlistviewModelList();

            var empWList = _tailordbContext.Empworks.Where(x => x.EmpIdfk ==1 && x.RecStatus == "A" && x.Date.Value.Date == date);
            foreach (var empW in empWList)
            {
                cardlistviewModelList model = new cardlistviewModelList();
                model.SrNO = empW.SrNo;
                model.ItemName = empW.ItemName;
                model.OrderedQty = empW.OrderedQty;
                model.CompletedQty = empW.CompletedOrder;
                model.MemoNo = empW.MemoNo;
                model.MeasuremntId = (int)empW.MeasurementFk;

                BillIn.CartList.Add(model);


            }

            return View(BillIn);

           
        }

        public IActionResult GetEmployeeWorkSet(string supplier)
        {
            DateTime date = DateTime.Today;
            var EID=_tailordbContext.Emps.FirstOrDefault(x=>x.Name==supplier)?.Id;
            cardlistviewModelList BillIn = new cardlistviewModelList();

            var empWList = _tailordbContext.Empworks.Where(x => x.EmpIdfk ==EID && x.RecStatus == "A" && x.Date.Value.Date == date);


            foreach (var empW in empWList)
            {
                cardlistviewModelList model = new cardlistviewModelList();
                model.SrNO = empW.SrNo;
                model.ItemName = empW.ItemName;
                model.OrderedQty = empW.OrderedQty;
                model.CompletedQty = empW.CompletedOrder;
                model.MemoNo = empW.MemoNo;
                model.MeasuremntId = (int)empW.MeasurementFk;

                BillIn.CartList.Add(model);
            }

            return Json(BillIn);
        }




        [HttpPost]
        public IActionResult DeleteItem(int srNO, int MeasuremntId)
        {
            var records = _tailordbContext.Empworks.Where(x => x.MeasurementFk == MeasuremntId).ToList();
            var singlerecords = _tailordbContext.Empworks.Where(x => x.SrNo == srNO).FirstOrDefault();

            if (singlerecords != null)
            {
                if (singlerecords.Dept != 3)
                {
                    var totalqty = singlerecords.OrderedQty;
                    var completqty = singlerecords.CompletedOrder;

                    var allcomletqty = records.Where(x => x.Dept == singlerecords.Dept && x.RecStatus == "A").ToList().Sum(x => x.CompletedOrder);

                    var nextdeptid = singlerecords.Dept + 1;

                    var allnextdeptcomletqty = records.Where(x => x.Dept == nextdeptid && x.RecStatus == "A").ToList().Sum(x => x.CompletedOrder);


                    if (allcomletqty == allnextdeptcomletqty)
                    {
                        //return Json(new { success = true, error = "this item deoes not delete" });
                        return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
                    }
                    else if (allnextdeptcomletqty < allcomletqty)
                    {
                        var a = allcomletqty - completqty;

                        if (a >= allnextdeptcomletqty)
                        {
                            //delet the record
                            var itemToDelete2 = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == srNO);
                            if (itemToDelete2 != null)
                            {
                                _tailordbContext.Empworks.Remove(itemToDelete2);
                                _tailordbContext.SaveChanges();

                                // return Json(new { success = true, error = "deleted" });
                                return Json(new { success = true, error = "डिलीट केले." });
                            }
                            else
                            {
                                // return Json(new { success = true, error = "this item deoes not delete2" });
                                return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
                            }
                        }
                        else
                        {
                            // return Json(new { success = true, error = "this item deoes not delete" });
                            return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
                            //sorry the this item already processing on next stage
                        }
                    }
                    else
                    {
                        // return Json(new { success = true, error = "this item deoes not delete" });
                        return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
                    }
                }
                else
                {
                    //delet the record
                    var itemToDelete1 = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == srNO);
                    if (itemToDelete1 != null)
                    {
                        _tailordbContext.Empworks.Remove(itemToDelete1);
                        _tailordbContext.SaveChanges();
                        // return Json(new { success = true, error = "deleted" });
                        return Json(new { success = true, error = "डिलीट केले." });
                    }
                    else
                    {
                        // return Json(new { success = true, error = "this item deoes not delete" });
                        return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
                    }
                }

            }
            else
            {
                //return Json(new { success = true, error = "this item deoes not delete" });
                return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
            }

            //bool department1Present = records.Any(r => r.Dept == 1);
            //bool department2Present = records.Any(r => r.Dept == 2);
            //bool department3Present = records.Any(r => r.Dept == 3);

            //if (!department1Present && (department2Present || department3Present))
            //{
            //    // If Department 1 is not present but Department 2 or 3 is present, deletion is not allowed
            //    return RedirectToAction("Cart");
            //}

            //if (department1Present && (department2Present || department3Present))
            //{
            //    // If Department 1 is present and has associated records with Department 2 or 3, deletion is not allowed
            //    return RedirectToAction("Cart");
            //}

            //if (department2Present && records.Any(r => r.Dept == 3))
            //{
            //    // If Department 2 has associated records with Department 3, deletion is not allowed
            //    return RedirectToAction("Cart");
            //}

            //// Deletion is allowed in all other cases
            //var itemToDelete = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
            //if (itemToDelete != null)
            //{
            //    _tailordbContext.Empworks.Remove(itemToDelete);
            //    _tailordbContext.SaveChanges();
            //}

            //  return RedirectToAction("Cart");
        }


        //totalrecord
        public IActionResult totalrecord()
        {
            return View();
        }


        // List<Billdetail> getdata = new List<Billdetail>();
        // var data = _tailordbContext.Billheaders
        //.Where(e => e.MemoDate.Value.Date == startDate.Value.Date)
        //.ToList();

        //             foreach (var item in data)
        //             {
        //                 var billDetails = _tailordbContext.Billdetails.Where(x => x.BillHeaderId == item.Id && x.ItemId == itemfk).ToList();
        // getdata.AddRange(billDetails);
        //             }

        //             var result = getdata
        //                 .GroupBy(e => e.ItemName)
        //                 .Select(g => new ItemQuantityViewModel
        //                 {
        //                     ItemName = g.Key,
        //                     Qty = (int)g.Sum(e => e.Qty)
        //                 })
        //                 .ToList();

        //             return Json(result);




        

        [HttpGet]
        public IActionResult totalorderanddeliverygraph()
        {
            var staticDate = _tailordbContext.TblReportsettings
    .Select(r => r.ChartSettingDate)
    .FirstOrDefault();
          
            var shirtItemIds = new List<int> { 5, 10, 13, 14, 18 };
            var nehruItemIds = new List<int> {6, 16, 17, 21 };
            var safariItemIds = new List<int> {3};
            var suitItemIds = new List<int> {1,19,20,22};
            var jacketItemIds = new List<int> {2,24};
            var pantItemIds = new List<int> {4,7,8,9,11,12,15,23,26,28};
            object GetCategoryData(string name, List<int> itemIds)
            {
                //var orderCount = _tailordbContext.Billdetails
                //    .Where(b => itemIds.Contains((int)b.ItemId))
                //    .Count();
                var orderCount = (from bd in _tailordbContext.Billdetails
                                  join bh in _tailordbContext.Billheaders
                                  on bd.BillHeaderId equals bh.Id
                                  where itemIds.Contains((int)bd.ItemId)
                                        && bh.MemoDate >= staticDate
                                  select bd).Count();

                var cuttingCount = _tailordbContext.Empworks
                    .Where(e => itemIds.Contains((int)e.ItemFk) && e.Dept == 1 && e.Date>=staticDate)
                    .Count();

                var stitchingCount = _tailordbContext.Empworks
                    .Where(e => itemIds.Contains((int)e.ItemFk) && e.Dept == 2 && e.Date >= staticDate)
                    .Count();

                var ironingCount = _tailordbContext.Empworks
                    .Where(e => itemIds.Contains((int)e.ItemFk) && e.Dept == 3 && e.Date >= staticDate)
                    .Count();

                return new
                {
                    category = name,
                    order = orderCount,
                    cutting = cuttingCount,
                    stitching = stitchingCount,
                    ironing = ironingCount,
                    
                };
            }

            // Prepare response list
            var data = new List<object>
            {
                GetCategoryData("शर्ट", shirtItemIds),
                GetCategoryData("नेहरू", nehruItemIds),
                GetCategoryData("सफारी", safariItemIds),
                GetCategoryData("सुट", suitItemIds),
                GetCategoryData("जॅकीट", jacketItemIds),
                GetCategoryData("पँट", pantItemIds),
               
            };

            return Json(new
            {
                data = data,
                dateshow = staticDate  // 👈 Pass the date here
            });
        }


        [HttpPost]
        public IActionResult StartChartFromToday()
        {
            // Get today's date at 00:00:00
            var todayAtMidnight = DateTime.Today;

            // Fetch the report setting row
            var reportSetting = _tailordbContext.TblReportsettings.FirstOrDefault();

            if (reportSetting == null)
            {
                return Json(new { success = false, message = "No report setting found." });
            }

            // Update the ChartSettingDate
            reportSetting.ChartSettingDate = todayAtMidnight;

            // Save changes to the database
            _tailordbContext.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Chart started from today.",
                updatedDate = reportSetting.ChartSettingDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                today = todayAtMidnight.ToString("yyyy-MM-dd HH:mm:ss")
            });
        }

    }
}


