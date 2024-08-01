using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School_Management_System.Data;
using School_Management_System.Models;
using SchoolManagementSystem.Domain.Models;

namespace School_Management_System.Controllers
{
    public class MarkController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MarkController(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public IActionResult Index()
        {
            var students = _context.Students.ToList();
            var subjects = _context.Subjects.ToList();
            var marks = _context.Marks.ToList();

            var viewModel = new MarksEntryViewModel
            {
                Students = students,
                Subjects = subjects,
                Marks = new Dictionary<int, Dictionary<int, int>>()
            };

            foreach (var student in students)
            {
                viewModel.Marks[student.StudentId] = new Dictionary<int, int>();
                foreach (var subject in subjects)
                {
                    var existingMark = marks.FirstOrDefault(m => m.StudentId == student.StudentId && m.SubjectId == subject.SubjectId);
                    viewModel.Marks[student.StudentId][subject.SubjectId] = existingMark?.Score ?? 0;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SaveMarks(MarksEntryViewModel model)
        {
            foreach (var studentMarks in model.Marks)
            {
                var studentId = studentMarks.Key;
                foreach (var subjectMarks in studentMarks.Value)
                {
                    var subjectId = subjectMarks.Key;
                    var mark = subjectMarks.Value;

                    var existingMark = _context.Marks.FirstOrDefault(m => m.StudentId == studentId && m.SubjectId == subjectId);
                    if (existingMark == null)
                    {
                        _context.Marks.Add(new Mark
                        {
                            StudentId = studentId,
                            SubjectId = subjectId,
                            Score = mark
                        });
                    }
                    else
                    {
                        existingMark.Score = mark;
                    }
                }
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
