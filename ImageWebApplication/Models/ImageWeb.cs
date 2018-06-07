using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ImageWebApplication.Models;
using ImageWebApplication.Communication;
using System.IO;
using System.Reflection;

namespace ImageWebApplication.Models
{
    public class ImageWeb
    {
        public bool m_Connect;

        public ImageWeb()
        {
            WebClient client = WebClient.Instance;
            m_Connect = client.Connected;
            //Get the directory of the students file.
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            String Root = Directory.GetCurrentDirectory();
            string path = Path.Combine(Root, @"App_Data\students.txt");
            string[] students = File.ReadAllLines(path);
            Students = new List<Student>();
            SetProp(client, students);
        }
        public void SetProp(WebClient client, string[] students)
        {
            if (!m_Connect)
            {
                Status = "Off";
            }
            else
            {
                Status = "On";
                //Default.
                NumOfPhotos = 0;
            }
            for (int i = 0; i < students.Length; i++)
            {
                //Split the strings of the ditails from the file.
                string[] details = students[i].Split(' ');
                Student student = new Student();
                student.FirstName = details[0];
                student.LastName = details[1];
                student.IDNum = details[2];
                //Add the studetnt to the list.
                Students.Add(student);
            }
        }
        public void PhotosNum(string imagesPath)
        {
            NumOfPhotos = 0;
            if (imagesPath != null)
            {
                string path = Path.Combine(imagesPath, "Thumbnails");
                if (Directory.Exists(path))
                {
                    NumOfPhotos = Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories).Count();
                }
            }
        }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "NumOfPhotos")]
        public int NumOfPhotos { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Students")]
        public List<Student> Students { get; set; }
    }
}