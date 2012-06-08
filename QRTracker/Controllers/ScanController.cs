using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QRTracker.Models;
using QRTracker.service;
using com.google.zxing;
using com.google.zxing.common;
using System.DirectoryServices;

namespace QRTracker.Controllers
{
    public class ScanController : Controller
    {
        
        UserService userService = new UserService();
        RightService rightService = new RightService();
        
       [HttpPost]
        public ActionResult Upload(string role)
        {
            string userName = OtherFunctions.StripDomain(User.Identity.Name);
            int userId = userService.GetUserId(userName);
            // Пользователю запрещен просмотр
            if (!userService.AllowView(userId))
            {
                return RedirectToAction("ShowError", "Actions", new { resultType = Constants.ResultTypes.UnAuthorized.ToString() });
            }
           
            var imageData = new System.Web.Helpers.WebImage(Request.InputStream);
            MemoryStream stream = new MemoryStream(imageData.GetBytes());
            Image image = Image.FromStream(stream);
           string dirName = "/content/tempImages/" + userName;
           try
           {
               if (!Directory.Exists(dirName))
               {
                   Directory.CreateDirectory(dirName);
               }
               image.Save(dirName + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jpg");
           }
           catch(Exception exc)
           {
               
           }
           string qrText = DecodeImage(image);

            // Не распознано
            if (qrText == null)
            {
                return RedirectToAction("ShowError", "Actions", new{resultType = Constants.ResultTypes.DecodeError.ToString()});
            }

            QrDoc qrDoc = new QrDoc(qrText);

            // ошибка парсинга
            if (!qrDoc.IsInited)
            {
                return RedirectToAction("ShowError", "Actions", new { resultType = Constants.ResultTypes.ParseError.ToString() });
            }

            else
            {
                int ationId;
                if (int.TryParse(role, out ationId))
                {
                    qrDoc.ActionId = ationId;
                }
                
                TempData["qrDoc"] = qrDoc;
                return RedirectToAction("Index", "Actions");
            }
                
               


        }

        private int GetUserId(string userName)
        {
            userName = OtherFunctions.StripDomain(User.Identity.Name);
            int userId = userService.GetUserId(userName);
            return userId;
        }


        private string DecodeImage(Image image)
        {
            //image = image.Substring("data:image/png;base64,".Length);

            //var buffer = Convert.FromBase64String(image);
            //// TODO: I am saving the image on the hard disk but
            //// you could do whatever processing you want with it
            //string documentName = DateTime.Now.ToString("yyyyMMddHHmmssfff")+".png";
            //System.IO.File.WriteAllBytes(Server.MapPath("~/app_data/"+documentName), buffer);
            //byte[] bytes = Convert.FromBase64String(image);



            //Image _image;
            //using (MemoryStream ms = new MemoryStream(bytes))
            //{
            //    _image = Image.FromStream(ms);
            //}
            Result result = null;
            try
            {
                Reader reader = new com.google.zxing.MultiFormatReader();
                Bitmap image1 = new Bitmap(image);
                LuminanceSource source = new RGBLuminanceSource(image1, image1.Width, image1.Height);
                BinaryBitmap bitmap = new BinaryBitmap(new HybridBinarizer(source));
                result = reader.decode(bitmap);
                //QRCodeDecoder decoder = new QRCodeDecoder();
                ////QRCodeDecoder.Canvas = new ConsoleCanvas();
                //String decodedString = decoder.decode(new QRCodeBitmapImage(new Bitmap(_image)));
                ////txtDecodedData.Text = decodedString;
            }
            catch
            {
                return null;
            }
            return result.Text;
            
        }

        

        
        
        public ActionResult Index()
        {
            var rights = rightService.GetRoles(GetUserId(User.Identity.Name));
            return PartialView(rights);
        }

    }
}
