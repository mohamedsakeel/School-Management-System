using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public string IndexNumber { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}
