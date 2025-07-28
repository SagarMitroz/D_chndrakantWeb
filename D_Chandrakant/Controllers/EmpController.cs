using D_Chandrakant.DataModels;
using D_Chandrakant.Models;
using IronBarCode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.VisualBasic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.Common;
using ZXing.PDF417.Internal;
using ZXing;
using ZXing.QrCode.Internal;

namespace D_Chandrakant.Controllers
{
    public class EmpController : Controller
    {
        private readonly TailordbContext _tailordbContext;
        IWebHostEnvironment hostingenvironment;
        public EmpController(ILogger<EmpController> logger, TailordbContext tailordbContext, IWebHostEnvironment hc)
        {
            _tailordbContext = tailordbContext;
            hostingenvironment = hc;
        }



        [HttpGet]
        public IActionResult EmpPage()
        {
            //string IdString = "NA";
            //if (HttpContext.Session != null)
            //    if (HttpContext.Session.GetString("IDNew") != null)
            //    {
            //        IdString = HttpContext.Session.GetString("IDNew").ToString();
            //    }
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetInt32("ID").ToString();
                var userrId = HttpContext.Session.GetInt32("ID");


                var Username = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == userrId).Name;
                if (Username != null)
                {
                    ViewBag.Username = Username;
                }
                var Department = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == userrId).DeptFk;
                if (Username != null)
                {
                    var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == Department).DeptName;
                    ViewBag.Dept = finddepartment;
                }


            }


            return View();
        }


        // POST: /Product/Search 


        public IActionResult SearchInfo(int id)
        {
            if (id != null && id > 0)
            {
                var userId = HttpContext.Session.GetInt32("ID");
                BillInfo billInfo = new BillInfo();


                var mesurment = this._tailordbContext.Measurements.FirstOrDefault(m => m.Id == id);
                if (mesurment != null)
                {
                    var main = mesurment.Qty;
                    var remainingqty = mesurment.Qty;

                    var empWorkData1 = _tailordbContext.Empworks.Where(x => x.MeasurementFk == id && x.RecStatus == "A").ToList();
                    if (empWorkData1.Count > 0)
                    {
                        var status1 = empWorkData1.Where(x => x.Dept == 1).Sum(x => x.CompletedOrder);
                        var status2 = empWorkData1.Where(x => x.Dept == 2).Sum(x => x.CompletedOrder);
                        var status3 = empWorkData1.Where(x => x.Dept == 3).Sum(x => x.CompletedOrder);

                        var emp = _tailordbContext.Emps.FirstOrDefault(x => x.Id == userId);

                        if (emp != null)
                        {
                            if (emp.DeptFk == 1)
                            {
                                if (status1 == 0)
                                {
                                    remainingqty = main;
                                }
                                else if (status1 <= main)
                                {
                                    remainingqty = main - status1;
                                }
                            }
                            else if (emp.DeptFk == 2)
                            {
                                if (status1 == 0)
                                {
                                    remainingqty = 0;
                                }
                                else if (status2 <= status1)
                                {
                                    remainingqty = status1 - status2;
                                }
                            }
                            else if (emp.DeptFk == 3)
                            {
                                if (status2 == 0)
                                {
                                    remainingqty = 0;
                                }
                                else if (status3 <= status2)
                                {
                                    remainingqty = status2 - status3;
                                }
                            }
                        }


                        billInfo.MeasuremntId = mesurment.Id;
                        billInfo.MemoNo = mesurment.MemoNo;
                        billInfo.OrderedQty = mesurment.Qty;
                        billInfo.ItemId = Convert.ToInt32(mesurment.ItemId);

                        var billdetail = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx);
                        billInfo.CustName = billdetail.CustomerName;
                        billInfo.CustName = billdetail.CustomerName;

                        var itemdetails = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId);
                        billInfo.ItemName = itemdetails.Name;
                        billInfo.Remaining = (double)remainingqty;
                    }
                    else
                    {
                        var emp = _tailordbContext.Emps.FirstOrDefault(x => x.Id == userId);

                        if (emp != null)
                        {
                            if (emp.DeptFk == 1)
                            {

                                billInfo.MeasuremntId = mesurment.Id;
                                billInfo.MemoNo = mesurment.MemoNo;
                                billInfo.OrderedQty = mesurment.Qty;
                                billInfo.ItemId = Convert.ToInt32(mesurment.ItemId);

                                var billdetail = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx);
                                billInfo.CustName = billdetail.CustomerName;
                                billInfo.CustName = billdetail.CustomerName;

                                var itemdetails = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId);
                                billInfo.ItemName = itemdetails.Name;
                                billInfo.Remaining = (double)remainingqty;
                            }
                            else
                            {


                                billInfo.MeasuremntId = mesurment.Id;
                                billInfo.MemoNo = mesurment.MemoNo;
                                billInfo.OrderedQty = mesurment.Qty;
                                billInfo.ItemId = Convert.ToInt32(mesurment.ItemId);

                                var billdetail = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx);
                                billInfo.CustName = billdetail.CustomerName;
                                billInfo.CustName = billdetail.CustomerName;

                                var itemdetails = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId);
                                billInfo.ItemName = itemdetails.Name;
                                billInfo.Remaining = (double)0;
                            }

                        }
                    }


                }


                return Json(new { success = true, billInfo = billInfo, msg = "sucsses" });
            }
            else
            {
                //return Json(new { success = true, msg = "Please Enter Barcode Id first!" });
                return Json(new { success = true, msg = "कृपया बारकोड स्कॅन करा किंवा बारकोड आय. डी एंटर  करा. " });

            }
        }



        /******************************************************************************/



        /*
          public IActionResult SerchProfileN(int id)
        {
            EmpModel e = new EmpModel();
            var BarcodeText = Convert.ToString(id);

            GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
            barcode.ResizeTo(300, 60);
            barcode.AddBarcodeValueTextBelowBarcode();
            barcode.ChangeBarCodeColor(Color.Black);
            barcode.SetMargins(10);
            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile");
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String filePath = Path.Combine(hostingenvironment.WebRootPath, "BarcodeFile/barcode.png");
            barcode.SaveAsPng(filePath);    
            string filName=Path.GetFileName(filePath);
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

            return Json(e);
        }
         
         * */


        /******************************************************************************/

        [HttpPost]
        public IActionResult completedQty(int id, int COrderqty)
        {

            List<string> barcodePaths = new List<string>();


            var userId = HttpContext.Session.GetInt32("ID");
            DateTime date = DateTime.Now;
            if (id != null && id > 0 && COrderqty != null)
            {


                if (COrderqty != null && COrderqty > 0)
                {
                    double memonoshow = 0;
                    int? deptid = 0;
                    var employedata = _tailordbContext.Emps.Where(x => x.Id == userId && x.RecStatus == "A").FirstOrDefault();
                    if (employedata != null)
                    {
                        deptid = employedata.DeptFk;
                    }

                    var empWorkData1 = _tailordbContext.Empworks.Where(x => x.MeasurementFk == id && x.RecStatus == "A").ToList();
                    if (empWorkData1.Count > 0)
                    {
                        var mesurment = _tailordbContext.Measurements.FirstOrDefault(m => m.Id == id);
                        memonoshow = (double)mesurment.MemoNo;
                        if (mesurment != null)
                        {
                            var mainqty = mesurment.Qty;

                            var status1 = empWorkData1.Where(x => x.Dept == 1).Sum(x => x.CompletedOrder);
                            var status2 = empWorkData1.Where(x => x.Dept == 2).Sum(x => x.CompletedOrder);
                            var status3 = empWorkData1.Where(x => x.Dept == 3).Sum(x => x.CompletedOrder);


                            if (deptid == 1)
                            {
                                if (mainqty == status1)
                                {
                                    //  return Json(new { success = true, error = "Cutting is already completed" });
                                    return Json(new { success = true, error = "ह्या ऑर्डरची कटींग पूर्ण झालेली आहे. " });
                                }
                                else if ((status1 + COrderqty) <= mainqty)
                                {
                                    ClearBarcodeDirectory();
                                    var BarcodeText = Convert.ToString(id);
                                    //List<string> barcodePaths = new List<string>();

                                    for (int i = 0; i < COrderqty; i++)
                                    {
                                        //GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
                                        //barcode.ResizeTo(300, 60);
                                        //barcode.AddBarcodeValueTextBelowBarcode();
                                        //barcode.ChangeBarCodeColor(Color.Black);
                                        //barcode.SetMargins(10);

                                        //string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                        //if (!Directory.Exists(path))
                                        //{
                                        //    Directory.CreateDirectory(path);
                                        //}

                                        //string fileName = $"barcode_{id}_{i + 1}.png";
                                        //string filePath = Path.Combine(path, fileName);
                                        //barcode.SaveAsPng(filePath);
                                        //barcodePaths.Add($"/BarcodeMFile/{fileName}");

                                        //var writer = new BarcodeWriterPixelData
                                        //{
                                        //    Format = BarcodeFormat.CODE_128,
                                        //    Options = new EncodingOptions
                                        //    {
                                        //        Width = 300,
                                        //        Height = 100, // Height for barcode only
                                        //        Margin = 10
                                        //    }
                                        //};

                                        // Generate the barcode as PixelData
                                        //var pixelData = writer.Write(BarcodeText);

                                        // Create a new Bitmap with additional space for text
                                        //using (var bitmapWithText = new Bitmap(pixelData.Width, pixelData.Height + 40)) // Add space for text below the barcode
                                        //{
                                        //    // Lock the bitmap's bits
                                        //    var bitmapData = bitmapWithText.LockBits(new Rectangle(0, 0, pixelData.Width, pixelData.Height),
                                        //        ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                                        //    try
                                        //    {
                                        //        System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                                        //            pixelData.Pixels.Length);
                                        //    }
                                        //    finally
                                        //    {
                                        //        bitmapWithText.UnlockBits(bitmapData);
                                        //    }

                                        //    // Draw the barcode value text below the barcode with larger font size
                                        //    using (var graphics = Graphics.FromImage(bitmapWithText))
                                        //    {
                                        //        // Draw the barcode value text below the barcode with larger font size
                                        //        using (var font = new Font("Arial", 16)) // Adjust font size here
                                        //        using (var brush = new SolidBrush(Color.Black))
                                        //        {
                                        //            var stringFormat = new StringFormat
                                        //            {
                                        //                Alignment = StringAlignment.Center
                                        //            };
                                        //            var textRectangle = new RectangleF(0, pixelData.Height, bitmapWithText.Width, 40);
                                        //            graphics.DrawString(BarcodeText, font, brush, textRectangle, stringFormat);
                                        //        }
                                        //    }

                                        //    // Save the Bitmap as PNG file
                                        //    string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                        //    if (!Directory.Exists(path))
                                        //    {
                                        //        Directory.CreateDirectory(path);
                                        //    }
                                        //    //string filePath = Path.Combine(path, "barcode.png");
                                        //    //bitmapWithText.Save(filePath, ImageFormat.Png);

                                        //    //// Construct the URL for the saved barcode image
                                        //    //string imageUrl = "/BarcodeMFile/barcode.png";
                                        //    //barcodePaths.Add(imageUrl);
                                        //    string fileName = $"barcode_{id}_{i + 1}.png";
                                        //    string filePath = Path.Combine(path, fileName);
                                        //    bitmapWithText.Save(filePath);
                                        //    barcodePaths.Add($"/BarcodeMFile/{fileName}");
                                        //}
                                    }
                                    var data = new Empwork
                                    {
                                        MeasurementFk = id,
                                        MemoNo = mesurment.MemoNo,
                                        MemoSeries = mesurment.MemoSeries,
                                        OrderedQty = mesurment.Qty,
                                        ItemFk = Convert.ToInt32(mesurment.ItemId),
                                        CompletedOrder = COrderqty,
                                        CustomerName = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx)?.CustomerName,
                                        BillDetailFk = this._tailordbContext.Billdetails.FirstOrDefault(m => m.BillHeaderId == mesurment.BillHeaderIdIdx)?.Id,
                                        ItemName = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId)?.Name,
                                        EmpIdfk = userId,
                                        Dept = deptid,
                                        RemaimingQty = mesurment.Qty - COrderqty,
                                        Date = date,
                                        RecStatus = "A",
                                        RateId = this._tailordbContext.Rateemps.FirstOrDefault(m => m.ItemIdFk == (mesurment.ItemId)).Id,
                                        ItemRate = this._tailordbContext.Rateemps.FirstOrDefault(m => Convert.ToInt32(m.ItemIdFk) == Convert.ToInt32(mesurment.ItemId)).CuttingR
                                    };

                                    _tailordbContext.Empworks.Add(data);
                                    _tailordbContext.SaveChanges();
                                }
                                else
                                {
                                    //return Json(new { success = true, error = "Please Enter correct Qty." });
                                    return Json(new { success = true, error = "एंटर केलेली प्रती ही बाकी प्रती पेक्षा कमी असावी. एंटर केलेली प्रती तपासा. " });
                                }
                            }




                            else if (deptid == 2 && status1 <= mainqty && status1 != 0)
                            {
                                if (mainqty == status2)
                                {
                                    //return Json(new { success = true, error = "stiching is already completed" });
                                    return Json(new { success = true, error = "ह्या ऑर्डरची शिलाई पूर्ण झालेली आहे. " });
                                }
                                else if (mainqty > status2 && status1 >= (status2 + COrderqty))
                                {
                                    ClearBarcodeDirectory();
                                    var BarcodeText = Convert.ToString(id);
                                    //List<string> barcodePaths = new List<string>();

                                    for (int i = 0; i < COrderqty; i++)
                                    {
                                        //GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
                                        //barcode.ResizeTo(300, 60);
                                        //barcode.AddBarcodeValueTextBelowBarcode();
                                        //barcode.ChangeBarCodeColor(Color.Black);
                                        //barcode.SetMargins(10);

                                        //string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                        //if (!Directory.Exists(path))
                                        //{
                                        //    Directory.CreateDirectory(path);
                                        //}

                                        //string fileName = $"barcode_{id}_{i + 1}.png";
                                        //string filePath = Path.Combine(path, fileName);
                                        //barcode.SaveAsPng(filePath);
                                        //barcodePaths.Add($"/BarcodeMFile/{fileName}");

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
                                        var pixelData = writer.Write(BarcodeText);

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
                                                    graphics.DrawString(BarcodeText, font, brush, textRectangle, stringFormat);
                                                }
                                            }

                                            // Save the Bitmap as PNG file
                                            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                            if (!Directory.Exists(path))
                                            {
                                                Directory.CreateDirectory(path);
                                            }
                                            string fileName = $"barcode_{id}_{i + 1}.png";
                                            string filePath = Path.Combine(path, fileName);
                                            bitmapWithText.Save(filePath);
                                            barcodePaths.Add($"/BarcodeMFile/{fileName}");
                                        }
                                    }
                                    var data = new Empwork
                                    {
                                        MeasurementFk = id,
                                        MemoNo = mesurment.MemoNo,
                                        MemoSeries = mesurment.MemoSeries,
                                        OrderedQty = mesurment.Qty,
                                        ItemFk = Convert.ToInt32(mesurment.ItemId),
                                        CompletedOrder = COrderqty,
                                        CustomerName = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx)?.CustomerName,
                                        BillDetailFk = this._tailordbContext.Billdetails.FirstOrDefault(m => m.BillHeaderId == mesurment.BillHeaderIdIdx)?.Id,
                                        ItemName = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId)?.Name,
                                        EmpIdfk = userId,
                                        Dept = deptid,
                                        RemaimingQty = mesurment.Qty - COrderqty,
                                        Date = date,
                                        RecStatus = "A",
                                        RateId = this._tailordbContext.Rateemps.FirstOrDefault(m => m.ItemIdFk == (mesurment.ItemId)).Id,
                                        ItemRate = this._tailordbContext.Rateemps.FirstOrDefault(m => Convert.ToInt32(m.ItemIdFk) == Convert.ToInt32(mesurment.ItemId)).StretchingR


                                    };

                                    _tailordbContext.Empworks.Add(data);
                                    _tailordbContext.SaveChanges();
                                }
                                else
                                {
                                    // return Json(new { success = true, error = "Invalid Qty Enter" });
                                    return Json(new { success = true, error = "एंटर केलेली प्रती ही बाकी प्रती पेक्षा कमी असावी. एंटर केलेली प्रती तपासा. " });
                                }
                            }


                            else if (deptid == 3 && status1 <= mainqty && status2 <= mainqty && status2 != 0)
                            {
                                if (mainqty == status3)
                                {
                                    //return Json(new { success = true, error = "Ironing is already completed" });
                                    return Json(new { success = true, error = "ह्या ऑर्डरची ईस्त्री पूर्ण झालेली आहे. " });
                                }
                                else if (mainqty > status3 && status2 >= (status3 + COrderqty))
                                {

                                    ClearBarcodeDirectory();

                                    var BarcodeText = Convert.ToString(id);
                                    //List<string> barcodePaths = new List<string>();

                                    for (int i = 0; i < COrderqty; i++)
                                    {


                                        //GeneratedBarcode barcode = IronBarCode.BarcodeWriter.CreateBarcode(BarcodeText, BarcodeWriterEncoding.Code128);
                                        //barcode.ResizeTo(300, 60);
                                        //barcode.AddBarcodeValueTextBelowBarcode();
                                        //barcode.ChangeBarCodeColor(Color.Black);
                                        //barcode.SetMargins(10);

                                        //string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                        //if (!Directory.Exists(path))
                                        //{
                                        //    Directory.CreateDirectory(path);
                                        //}

                                        //string fileName = $"barcode_{id}_{i + 1}.png";
                                        //string filePath = Path.Combine(path, fileName);
                                        //barcode.SaveAsPng(filePath);
                                        //barcodePaths.Add($"/BarcodeMFile/{fileName}");



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
                                        var pixelData = writer.Write(BarcodeText);

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
                                                    graphics.DrawString(BarcodeText, font, brush, textRectangle, stringFormat);
                                                }
                                            }

                                            // Save the Bitmap as PNG file
                                            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                            if (!Directory.Exists(path))
                                            {
                                                Directory.CreateDirectory(path);
                                            }
                                            string fileName = $"barcode_{id}_{i + 1}.png";
                                            string filePath = Path.Combine(path, fileName);
                                            bitmapWithText.Save(filePath);
                                            barcodePaths.Add($"/BarcodeMFile/{fileName}");
                                        }
                                    }
                                    var data = new Empwork
                                    {
                                        MeasurementFk = id,
                                        MemoNo = mesurment.MemoNo,
                                        MemoSeries = mesurment.MemoSeries,
                                        OrderedQty = mesurment.Qty,
                                        ItemFk = Convert.ToInt32(mesurment.ItemId),
                                        CompletedOrder = COrderqty,
                                        CustomerName = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx)?.CustomerName,
                                        BillDetailFk = this._tailordbContext.Billdetails.FirstOrDefault(m => m.BillHeaderId == mesurment.BillHeaderIdIdx)?.Id,
                                        ItemName = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId)?.Name,
                                        EmpIdfk = userId,
                                        Dept = deptid,
                                        RemaimingQty = mesurment.Qty - COrderqty,
                                        Date = date,
                                        RecStatus = "A",
                                        RateId = this._tailordbContext.Rateemps.FirstOrDefault(m => m.ItemIdFk == (mesurment.ItemId)).Id,
                                        ItemRate = this._tailordbContext.Rateemps.FirstOrDefault(m => Convert.ToInt32(m.ItemIdFk) == Convert.ToInt32(mesurment.ItemId)).IorningR

                                    };

                                    _tailordbContext.Empworks.Add(data);
                                    _tailordbContext.SaveChanges();
                                }
                                else
                                {
                                    // return Json(new { success = true, error = "Invalid Qty Enter" });
                                    return Json(new { success = true, error = "एंटर केलेली प्रती ही बाकी प्रती पेक्षा कमी असावी. एंटर केलेली प्रती तपासा. " });
                                }
                            }
                            else
                            {
                                //  return Json(new { success = true, error = "Cutting is pending" });

                                return Json(new { success = true, error = "ह्या ऑर्डरची शिलाई बाकी आहे." });
                            }
                        }




                    }
                    else
                    {
                        if (deptid == 1)
                        {

                            var mesurment = _tailordbContext.Measurements.FirstOrDefault(m => m.Id == id);
                            memonoshow = (double)mesurment.MemoNo;
                            if (mesurment != null)
                            {
                                if (mesurment.Qty >= COrderqty)
                                {
                                    ClearBarcodeDirectory();

                                    var BarcodeText = Convert.ToString(id);


                                    for (int i = 0; i < COrderqty; i++)
                                    {
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
                                        var pixelData = writer.Write(BarcodeText);

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
                                                    graphics.DrawString(BarcodeText, font, brush, textRectangle, stringFormat);
                                                }
                                            }

                                            // Save the Bitmap as PNG file
                                            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
                                            if (!Directory.Exists(path))
                                            {
                                                Directory.CreateDirectory(path);
                                            }

                                            string fileName = $"barcode_{id}_{i + 1}.png";
                                            string filePath = Path.Combine(path, fileName);
                                            bitmapWithText.Save(filePath);
                                            barcodePaths.Add($"/BarcodeMFile/{fileName}");
                                        }
                                    }

                                    var data = new Empwork
                                    {
                                        MeasurementFk = id,
                                        MemoNo = mesurment.MemoNo,
                                        MemoSeries = mesurment.MemoSeries,
                                        OrderedQty = mesurment.Qty,
                                        ItemFk = Convert.ToInt32(mesurment.ItemId),
                                        CompletedOrder = COrderqty,
                                        CustomerName = this._tailordbContext.Billheaders.FirstOrDefault(m => m.Id == mesurment.BillHeaderIdIdx)?.CustomerName,
                                        BillDetailFk = this._tailordbContext.Billdetails.FirstOrDefault(m => m.BillHeaderId == mesurment.BillHeaderIdIdx)?.Id,
                                        ItemName = this._tailordbContext.Items.FirstOrDefault(x => x.Id == mesurment.ItemId)?.Name,
                                        EmpIdfk = userId,
                                        Dept = deptid,
                                        RemaimingQty = mesurment.Qty - COrderqty,
                                        Date = date,
                                        RecStatus = "A",
                                        RateId = this._tailordbContext.Rateemps.FirstOrDefault(m => m.ItemIdFk == (mesurment.ItemId)).Id,
                                        ItemRate = this._tailordbContext.Rateemps.FirstOrDefault(m => Convert.ToInt32(m.ItemIdFk) == Convert.ToInt32(mesurment.ItemId)).CuttingR

                                    };

                                    _tailordbContext.Empworks.Add(data);
                                    _tailordbContext.SaveChanges();

                                }
                                else
                                {
                                    // return Json(new { success = true, error = "Invalid quantity " });
                                    return Json(new { success = true, error = "एंटर केलेली प्रती ही बाकी प्रती पेक्षा कमी असावी. एंटर केलेली प्रती तपासा. " });
                                }

                            }
                        }
                        else
                        {
                            // return Json(new { success = true, error = "Stitching is pending " });
                            return Json(new { success = true, error = "ह्या ऑर्डरची कटींग बाकी आहे." });
                        }

                    }



                    //return Json(new { success = true, error = "Record added successfully" });
                    return Json(new { success = true, error = "डेटा यशस्वीरित्या जतन केला.", barcodes = barcodePaths , dept=deptid, memoshow=memonoshow });
                }
                else
                {
                    //return Json(new { success = true, error = "Please enter Qty first." });   
                    return Json(new { success = true, error = "ऑर्डर प्रती  एंटर  करा." });
                }
            }
            else
            {
                //return Json(new { success = true, error = "Please Enter Barcode Id first!" });
                return Json(new { success = true, error = "कृपया बारकोड स्कॅन करा किंवा बारकोड आय. डी एंटर करा." });
            }

            //return Json("Record added successfully");
        }





        /***************************************************************************/


        private void ClearBarcodeDirectory()
        {
            string path = Path.Combine(hostingenvironment.WebRootPath, "BarcodeMFile");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    System.IO.File.Delete(file);
                }
            }
        }



        /*************************************************************************/

        public IActionResult Cart()
        {
            DateTime date = DateTime.Today;
            var userId = HttpContext.Session.GetInt32("ID");
            if (HttpContext.Session.GetInt32("ID") != null)
            {
                ViewBag.Mysession = HttpContext.Session.GetInt32("ID").ToString();
                var userrId = HttpContext.Session.GetInt32("ID");


                var Username = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == userrId).Name;
                if (Username != null)
                {
                    ViewBag.Username = Username;
                }
                var Department = this._tailordbContext.Emps.FirstOrDefault(x => x.Id == userrId).DeptFk;
                if (Username != null)
                {
                    var finddepartment = this._tailordbContext.Departments.FirstOrDefault(x => x.Id == Department).DeptName;
                    ViewBag.Dept = finddepartment;
                }


            }

            cardlistviewModelList BillIn = new cardlistviewModelList();

            var empWList = _tailordbContext.Empworks.Where(x => x.EmpIdfk == userId && x.RecStatus == "A" && x.Date.Value.Date == date);
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


        // Delete Cart items from Cart page                    
        //public IActionResult DeleteItem(int SrNo,int MeasuremntId)
        //  {
        //      var record= _tailordbContext.Empworks.Where(x => x.MeasurementFk== MeasuremntId).ToList();
        //      if (record.Count()>0)
        //      {
        //          foreach(var item in record)
        //          { 
        //              if((item.Dept==2 || item.Dept==3) && (item.CompletedOrder>0 || item.CompletedOrder<= item.OrderedQty))
        //              {


        //              }
        //              else
        //              {
        //                  var item1 = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
        //                  _tailordbContext.Empworks.Remove(item1);
        //                  _tailordbContext.SaveChanges();

        //              }
        //          }
        //      }

        //      return RedirectToAction("Cart");
        //  }


        //public IActionResult DeleteItem(int SrNo, int MeasuremntId)
        //{
        //    var records = _tailordbContext.Empworks.Where(x => x.MeasurementFk == MeasuremntId).ToList();

        //    if (records.Count() > 0)
        //    {
        //        bool shouldDelete = true; // Assume deletion is allowed initially

        //        foreach (var item in records)
        //        {
        //            // Check if Department is 2 or 3
        //            if (item.Dept == 2 || item.Dept == 3)
        //            {
        //                shouldDelete = false; // Mark deletion as not allowed
        //                break; // No need to continue checking other records
        //            }
        //        }

        //        if (shouldDelete)
        //        {
        //            var itemToDelete = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);

        //            if (itemToDelete != null)
        //            {
        //                _tailordbContext.Empworks.Remove(itemToDelete);
        //                _tailordbContext.SaveChanges();
        //            }
        //        }
        //    }

        //    return RedirectToAction("Cart");
        //}


        //public IActionResult DeleteItem(int SrNo, int MeasuremntId)
        //{
        //    var records = _tailordbContext.Empworks.Where(x => x.MeasurementFk == MeasuremntId).ToList();

        //    bool department1Present = records.Any(r => r.Dept == 1);
        //    bool department2Present = records.Any(r => r.Dept == 2);
        //    bool department3Present = records.Any(r => r.Dept == 3);

        //    if (department1Present && (department2Present || department3Present))
        //    {
        //        // If Department 1 has associated records with Department 2 or 3, deletion is not allowed
        //        return RedirectToAction("Cart");
        //    }

        //    if (department2Present && records.Any(r => r.Dept == 3))
        //    {
        //        // If Department 2 has associated records with Department 3, deletion is not allowed
        //        return RedirectToAction("Cart");
        //    }

        //    // Deletion is allowed if there are no Department 1 records with Department 2 or 3,
        //    // and if there are no Department 2 records with Department 3
        //    var itemToDelete = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
        //    if (itemToDelete != null)
        //    {
        //        _tailordbContext.Empworks.Remove(itemToDelete);
        //        _tailordbContext.SaveChanges();
        //    }

        //    return RedirectToAction("Cart");
        //}

        [HttpPost]
        //public IActionResult DeleteItem(int SrNo, int MeasuremntId)
        //{
        //    var records = _tailordbContext.Empworks.Where(x => x.MeasurementFk == MeasuremntId).ToList();
        //    var singlerecords = _tailordbContext.Empworks.Where(x => x.SrNo == SrNo).FirstOrDefault();

        //    if(singlerecords != null )
        //    {
        //        if(singlerecords.Dept != 3)
        //        {
        //            var totalqty = singlerecords.OrderedQty;
        //            var completqty = singlerecords.CompletedOrder;

        //            var allcomletqty = records.Where(x => x.Dept == singlerecords.Dept && x.RecStatus=="A").ToList().Sum(x => x.CompletedOrder);

        //            var nextdeptid = singlerecords.Dept + 1;

        //            var allnextdeptcomletqty = records.Where(x => x.Dept == nextdeptid && x.RecStatus=="A").ToList().Sum(x => x.CompletedOrder);


        //            if (allcomletqty == allnextdeptcomletqty)
        //            {
        //                //return Json(new { success = true, error = "this item deoes not delete" });
        //                return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //            }
        //            else if (allnextdeptcomletqty < allcomletqty)
        //            {
        //                var a = allcomletqty - completqty;

        //                if (a >= allnextdeptcomletqty)
        //                {
        //                    //delet the record
        //                    var itemToDelete2 = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
        //                    if (itemToDelete2 != null)
        //                    {
        //                        _tailordbContext.Empworks.Remove(itemToDelete2);
        //                        _tailordbContext.SaveChanges();

        //                       // return Json(new { success = true, error = "deleted" });
        //                        return Json(new { success = true, error = "डिलीट केले." });
        //                    }
        //                    else
        //                    {
        //                       // return Json(new { success = true, error = "this item deoes not delete2" });
        //                        return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //                    }
        //                }
        //                else
        //                {
        //                   // return Json(new { success = true, error = "this item deoes not delete" });
        //                    return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //                    //sorry the this item already processing on next stage
        //                }
        //            }
        //            else
        //            {
        //               // return Json(new { success = true, error = "this item deoes not delete" });
        //                return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //            }
        //        }
        //        else
        //        {
        //            //delet the record
        //            var itemToDelete1 = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
        //            if (itemToDelete1 != null)
        //            {
        //                _tailordbContext.Empworks.Remove(itemToDelete1);
        //                _tailordbContext.SaveChanges();
        //                // return Json(new { success = true, error = "deleted" });
        //                return Json(new { success = true, error = "डिलीट केले." });
        //            }
        //            else
        //            {
        //               // return Json(new { success = true, error = "this item deoes not delete" });
        //                return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //            }
        //        }

        //    }
        //    else
        //    {
        //        //return Json(new { success = true, error = "this item deoes not delete" });
        //        return Json(new { success = true, error = "ह्या ऑर्डरचे पुढचे काम झाले आहे, त्यामुळे डिलीट होणार नाही." });
        //    }

        //    //bool department1Present = records.Any(r => r.Dept == 1);
        //    //bool department2Present = records.Any(r => r.Dept == 2);
        //    //bool department3Present = records.Any(r => r.Dept == 3);

        //    //if (!department1Present && (department2Present || department3Present))
        //    //{
        //    //    // If Department 1 is not present but Department 2 or 3 is present, deletion is not allowed
        //    //    return RedirectToAction("Cart");
        //    //}

        //    //if (department1Present && (department2Present || department3Present))
        //    //{
        //    //    // If Department 1 is present and has associated records with Department 2 or 3, deletion is not allowed
        //    //    return RedirectToAction("Cart");
        //    //}

        //    //if (department2Present && records.Any(r => r.Dept == 3))
        //    //{
        //    //    // If Department 2 has associated records with Department 3, deletion is not allowed
        //    //    return RedirectToAction("Cart");
        //    //}

        //    //// Deletion is allowed in all other cases
        //    //var itemToDelete = _tailordbContext.Empworks.FirstOrDefault(x => x.SrNo == SrNo);
        //    //if (itemToDelete != null)
        //    //{
        //    //    _tailordbContext.Empworks.Remove(itemToDelete);
        //    //    _tailordbContext.SaveChanges();
        //    //}

        //    //  return RedirectToAction("Cart");
        //}


        //send count to Cart button
        [HttpGet]
        public IActionResult GetEmpCount()
        {
            DateTime date = DateTime.Today;
            var userId = HttpContext.Session.GetInt32("ID");

            var empCount = _tailordbContext.Empworks.Where(x => x.EmpIdfk == userId && x.RecStatus == "A" && x.Date.Value.Date == date).Count();
            return Json(new { count = empCount });
        }




        [HttpPost]
        public JsonResult PrintLabel(string memoText, string barcode)
        {
            try
            {
                var tspl = $@"
SIZE 50 mm, 25 mm
GAP 2 mm, 0
CLS
TEXT 20,20,""0"",0,1,1,""डि.चंद्रकांत""
TEXT 300,20,""0"",0,1,1,""अ. नगर""
TEXT 80,80,""0"",0,1,1,""{memoText}""
BARCODE 80,120,""128"",60,1,0,2,2,""{barcode}""
PRINT 1
";

                string directory = @"C:\Temp";
                string filePath = Path.Combine(directory, "label.prn");

                // ✅ Ensure directory exists
                System.IO.Directory.CreateDirectory(directory);

                // ✅ Write TSPL file
                System.IO.File.WriteAllText(filePath, tspl, System.Text.Encoding.UTF8);

                // ✅ Send file to printer
                string printerName = "Bar_Code_Printer_T-9650_Plus"; // Share name of the thermal printer
                var psi = new System.Diagnostics.ProcessStartInfo("cmd.exe", $"/C copy /B \"{filePath}\" \"\\\\localhost\\{printerName}\"")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                System.Diagnostics.Process.Start(psi);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
