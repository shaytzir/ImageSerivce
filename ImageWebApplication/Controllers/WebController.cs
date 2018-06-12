using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ImageWebApplication.Models;
using System.Threading;
using Infrastructure.Enums;
using Infrastructure.Modal;
using Infrastructure.Event;
using Newtonsoft.Json;
using System;

namespace ImageWebApplication.Controllers
{
    public class WebController : Controller
    {
        static AppConfig appConfig = new AppConfig();
        static Logs logs = new Logs();
        static Approve approve = new Approve();
        static Photos photos = new Photos();
        static ImageWeb imageWeb = new ImageWeb();
        private static string m_handlerToRemove;
        public object objLock;



        // GET: First
        public ActionResult Approve()
        {
            return View(approve);
        }

        [HttpGet]
        public ActionResult MainView()
        {
            ViewBag.Status = imageWeb.Status;
            if (imageWeb.Status.Equals("On") && appConfig.InitDone==false)
            {
                appConfig.WaitWithBlock();
            }
            ViewBag.NumOfPhotos = appConfig.NumOfPhotos;
            return View(imageWeb);
        }


        [HttpGet]
        public ActionResult Photos()
        {
            photos.PhotosList.Clear();
            photos.GetAllPhotos(appConfig.OutputDir);
            return View(photos.PhotosList);
        }

        public ActionResult PhotoDelete(string fullUrlThumb, string dir)
        {
            PhotoInfo photoToDelete = new PhotoInfo(fullUrlThumb,dir);
            return View(photoToDelete);
        }
        public ActionResult DeletePhotoOK(string fullThumbUrl)
        {
            photos.DeletePhoto(fullThumbUrl);
            return RedirectToAction("Photos");

        }
        /// <summary>
        /// Photoes the view.
        /// </summary>
        /// <param name="photoToViewPath">The photo to view path.</param>
        /// <param name="dir">The dir.</param>
        /// <returns></returns>
        public ActionResult PhotoView(string photoToViewPath, string dir)
        {
            PhotoInfo photo = new PhotoInfo(photoToViewPath, dir);
            return View(photo);
        }

        public ActionResult Config()
        {
            return View(appConfig);
        }

        public ActionResult Logs()
        {
            return View(logs);
        }

        [HttpGet]
        public ActionResult FilterLogs(string filter)
        {
            return PartialView("LogsTable", logs.FilterLogs(filter));
        }

        public ActionResult ApproveDeleteHandler(string handlerToRemove)
        {
            m_handlerToRemove = handlerToRemove;
            return RedirectToAction("Approve");

        }
       
       public ActionResult DeleteHandlerOK()
        {
            JObject json = new JObject();
            json["CommandID"] = (int)CommandEnum.CloseCommand;
            json["Handler"] = m_handlerToRemove;
            appConfig.AskToRemoveHandler(JsonConvert.SerializeObject(json));
            return RedirectToAction("Config");
        }
    }
}
