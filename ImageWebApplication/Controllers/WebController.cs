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
            appConfig.WaitWithBlock();
            ViewBag.NumOfPhotos = appConfig.NumOfPhotos;
            return View(imageWeb);
        }


        [HttpGet]
        public ActionResult Photos()
        {
            photos.GetAllPhotos(appConfig.OutputDir);
            return View(photos.ImageList);
        }

        public ActionResult DeletePhotoOK(string thumbUrl)
        {
           // photos.DeletePhoto(thumbUrl);
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
        public JObject GetLogs(string filter)
        {
            JObject obj = (JObject)JToken.FromObject(logs.GetLogInfo(filter));
            return obj;
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


        /*[HttpGet]
        public JObject GetStudent()
        {
            JObject data = new JObject();
            data["FirstName"] = "Kuky";
            data["LastName"] = "Mopy";
            return data;
        }*/

        /*[HttpPost]
        public JObject GetStudent(string name, int salary)
        {
            foreach (var empl in Students)
            {
                if (empl.IDNum > salary || name.Equals(name))
                {
                    JObject data = new JObject();
                    data["FirstName"] = empl.FirstName;
                    data["LastName"] = empl.LastName;
                    data["Salary"] = empl.IDNum;
                    return data;
                }
            }
            return null;
        }*/

        // GET: First/Details
        /*public ActionResult Details()
        {
            return View(Students);
        }

        // GET: First/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: First/Create
        [HttpPost]
        public ActionResult Create(Student emp)
        {
            try
            {
                Students.Add(emp);

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        // GET: First/Edit/5
        public ActionResult Edit(int id)
        {
            foreach (Student emp in Students)
            {
                if (emp.ID.Equals(id))
                {
                    return View(emp);
                }
            }
            return View("Error");
        }

        // POST: First/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Student empT)
        {
            try
            {
                foreach (Student emp in Students)
                {
                    if (emp.ID.Equals(id))
                    {
                        emp.copy(empT);
                        return RedirectToAction("Index");
                    }
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Error");
            }
        }

        // GET: First/Delete/5
        public ActionResult Delete(int id)
        {
            int i = 0;
            foreach (Student emp in Students)
            {
                if (emp.ID.Equals(id))
                {
                    Students.RemoveAt(i);
                    return RedirectToAction("Details");
                }
                i++;
            }
            return RedirectToAction("Error");
        }*/
    }
}
