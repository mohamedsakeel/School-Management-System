using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagementSystem.Domain.Models
{
    public class Mark
    {
        public int MarkId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Score { get; set; }

        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
