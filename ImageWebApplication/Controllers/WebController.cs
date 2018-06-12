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
        static Photos photos = new Photos();
        static ImageWeb imageWeb = new ImageWeb();
        private static string m_handlerToRemove;

        /// <summary>
        /// returns a view for the main window of the project
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult MainView()
        {
            ViewBag.Status = imageWeb.Status;
            //gets inside condition only in the first time showing this view
            if (imageWeb.Status.Equals("On") && appConfig.InitDone==false)
            {
                appConfig.WaitWithBlock();
            }
            ViewBag.NumOfPhotos = appConfig.NumOfPhotos;
            return View(imageWeb);
        }

        /// <summary>
        /// returns a view for all the photos in the service.
        /// </summary>
        /// <returns> the photos view</returns>
        [HttpGet]
        public ActionResult Photos()
        {
            //reset the old
            photos.PhotosList.Clear();
            //get all of the photos in the serivce
            photos.GetAllPhotos(appConfig.OutputDir);
            return View(photos.PhotosList);
        }

        /// <summary>
        /// returns a view which asks if we're sure we want to delete a specific photo
        /// </summary>
        /// <param name="fullUrlThumb">The full URL for the photo thumbnail.</param>
        /// <param name="dir">The output directory of the image.</param>
        /// <returns></returns>
        public ActionResult PhotoDelete(string fullUrlThumb, string dir)
        {
            PhotoInfo photoToDelete = new PhotoInfo(fullUrlThumb,dir);
            return View(photoToDelete);
        }
        /// <summary>
        /// occurs when the user approves to delete a specific photo.
        /// deletes it and return to the photos view
        /// </summary>
        /// <param name="fullThumbUrl">The full thumb URL.</param>
        /// <returns></returns>
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

        /// <summary>
        /// view for the configuration settings
        /// </summary>
        /// <returns></returns>
        public ActionResult Config()
        {
            return View(appConfig);
        }

        /// <summary>
        /// view the logs
        /// </summary>
        /// <returns></returns>
        public ActionResult Logs()
        {
            logs.RefreshLogs();
            return View(logs);
        }

        [HttpGet]
        public ActionResult FilterLogs(string filter)
        {
            return PartialView("LogsTable", logs.FilterLogs(filter));
        }

        /// <summary>
        /// Approves the delete handler.
        /// </summary>
        /// <param name="handlerToRemove">The handler to remove.</param>
        /// <returns></returns>
        public ActionResult Approve(string handlerToRemove)
        {
            m_handlerToRemove = handlerToRemove;
            return View();

        }

        /// <summary>
        /// occurs when the user approves he wants to delete the handler
        /// </summary>
        /// <returns></returns>
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
