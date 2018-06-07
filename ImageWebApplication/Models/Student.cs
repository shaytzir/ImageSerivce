using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ImageWebApplication.Models
{
    public class Student
    {
        static int count = 0;
        public Student()
        {
            count++;
            ID = count;
        }
        public void copy(Student emp)
        {
            FirstName = emp.FirstName;
            IDNum = emp.IDNum;
            LastName = emp.LastName;
        }
        [Required]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "IDNum")]
        public string IDNum { get; set; }

    }
}