using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ImageWebApplication.Models;
using System.Threading;

namespace ImageWebApplication.Controllers
{
    public class WebController : Controller
    {
        static AppConfig appConfig = new AppConfig();
        static ImageWeb imageWeb = new ImageWeb();
        static Approve approve = new Approve();
        // GET: First
        public ActionResult Approve()
        {
            return View(approve);
        }

        [HttpGet]
        public ActionResult MainView()
        {
            string imagePath = appConfig.OutputDir;
            imageWeb.PhotosNum(imagePath);
            return View(imageWeb);
        }

        public ActionResult Config()
        {

            return View(appConfig);
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
