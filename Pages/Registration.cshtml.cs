
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // This is the namespace of the AcademicManagement project

namespace Lab2.Pages;

public class Registration : PageModel
{
    private readonly string dataFilePath = "./data/academic_records.json";
    // public string[] Students = new string[5] { "James", "John", "Jane", "Jill", "Jack" };
    public List<Student> Students = DataAccess.GetAllStudents();
    public List<Course> Courses = DataAccess.GetAllCourses();
    public List<AcademicRecord> AcademicRecords = new List<AcademicRecord>();

    // ------------------------------------------------------------
    [BindProperty] 
    public string SelectedStudentId { get; set; }
   
    public List<SelectListItem> StudentOptions { get; set; }

    public IActionResult OnGet(string selectedStudentId, AcademicRecord message)
    {
         Console.WriteLine("GET" + message.CourseCode);
        StudentOptions = Students.Select(s => new SelectListItem { Value = s.StudentId, Text = s.StudentName }).ToList();

        // Set the selectedStudentId property based on the query parameter value
        SelectedStudentId = selectedStudentId;
        if (selectedStudentId  != null)
        {
             AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(selectedStudentId);
            Console.WriteLine(AcademicRecords.Count+ "GET");
        }
        // else
        // {
        //     AcademicRecords = DataAccess.GetAllAcademicRecords();
        // }
        
        return Page();
    }
    
    public IActionResult OnPostRegister()
    {
        var selectedValues = Request.Form["CourseCode"];
        AcademicRecord newAcademicRecord  = new AcademicRecord();
        foreach (var selectedValue in selectedValues)
        {
            // Process each selected checkbox value as needed
            // ...
             newAcademicRecord = new AcademicRecord
            {
                StudentId = SelectedStudentId,
                CourseCode = Request.Form["CourseCode"]
            };
             AcademicRecords.Add(newAcademicRecord);
        }
        DataAccess.AddAcademicRecord(newAcademicRecord);
        Console.WriteLine(AcademicRecords.Count+ "POST ..");

        return RedirectToPage("Registration", new { records = AcademicRecords });
        
    }

}