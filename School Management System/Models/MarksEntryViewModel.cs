using SchoolManagementSystem.Domain.Models;

namespace School_Management_System.Models
{
    public class MarksEntryViewModel
    {
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }
        public Dictionary<int, Dictionary<int, int>> Marks { get; set; } // StudentId -> (SubjectId -> Mark)
    }
}
