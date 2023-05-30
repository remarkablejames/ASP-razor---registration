
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // This is the namespace of the AcademicManagement project

namespace Lab2.Pages;

public class Registration : PageModel
{
    private readonly string dataFilePath = "./data/academic_records.json";
    public List<Student> Students = DataAccess.GetAllStudents();
    public List<Course> Courses = DataAccess.GetAllCourses();
    public List<AcademicRecord> AcademicRecords = new List<AcademicRecord>();

    // ------------------------------------------------------------
    [BindProperty] 
    public string SelectedStudentId { get; set; }

    public bool IsStudentSelected = false;
    public string ErrorMessage = "";
    public string msg = "";
   
    public List<SelectListItem> StudentOptions { get; set; }
    
    public IActionResult OnGet(string selectedStudentId, string msg )
    {
        ErrorMessage = msg;
        StudentOptions = Students.Select(s => new SelectListItem { Value = s.StudentId, Text = s.StudentName }).ToList();

        // Set the selectedStudentId property based on the query parameter value
        SelectedStudentId = selectedStudentId;
        if (selectedStudentId != null)
        {
            AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(selectedStudentId);

        }
        else
        {
            ErrorMessage = " ";
        }
        
        return Page();
    }
    public IActionResult OnPostRegister(string selectedStudentId, string msg)
    {
        Console.WriteLine(msg + "hello");
        
        StudentOptions = Students.Select(s => new SelectListItem { Value = s.StudentId, Text = s.StudentName }).ToList();

        // Setting the selectedStudentId property based on the query parameter value
        SelectedStudentId = selectedStudentId;
        if (selectedStudentId != null)
        {
            AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(selectedStudentId);
            if (AcademicRecords.Count != 0)
            {
                ErrorMessage = "The student has registered for the following courses:";
            }
            else
            {
                ErrorMessage = " The student has not registered any course. Please select course(s) to register";
            }

        }
        else
        {
            ErrorMessage = "Please select a student....";
        }

        return Page();
    }
    
    public IActionResult OnPostStudentSelected()
    {
        var selectedValues = Request.Form["CourseCode"];
        
        if (selectedValues.Count== 0 )
        {
            Console.WriteLine("No course selected");
            msg = "You must select at least one course!";

        }
        else
        {
            // AcademicRecord newAcademicRecord  = new AcademicRecord();
            foreach (var selectedValue in selectedValues)
            {
                var newRecord = new AcademicRecord
                {
                    StudentId = SelectedStudentId,
                    CourseCode = selectedValue,
                
                };
                DataAccess.AddAcademicRecord(newRecord);
            }
        
            AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
            foreach (var record in AcademicRecords)
            {
                Console.WriteLine(record.CourseCode);
             
            }
            msg = "The student has registered for the following courses:";
            
            
        }
        return RedirectToPage("Registration", new { selectedStudentId = SelectedStudentId, msg = msg });

        
    }

}