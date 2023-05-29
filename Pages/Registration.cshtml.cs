
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
   
    public List<SelectListItem> StudentOptions { get; set; }
    
    public IActionResult OnGet(string selectedStudentId, AcademicRecord message)
    {
        StudentOptions = Students.Select(s => new SelectListItem { Value = s.StudentId, Text = s.StudentName }).ToList();

        // Set the selectedStudentId property based on the query parameter value
        SelectedStudentId = selectedStudentId;
        if (selectedStudentId  != null)
        {
            AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(selectedStudentId);
        }

        return Page();
    }
    public IActionResult OnPostRegister(string selectedStudentId, AcademicRecord message)
    {
        StudentOptions = Students.Select(s => new SelectListItem { Value = s.StudentId, Text = s.StudentName }).ToList();

        // Set the selectedStudentId property based on the query parameter value
        SelectedStudentId = selectedStudentId;
        if (selectedStudentId  != null)
        {
             AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(selectedStudentId);
        }

        return Page();
    }
    
    public IActionResult OnPostStudentSelected()
    {
        Console.WriteLine("OnPostStudentSelected");
        var selectedValues = Request.Form["CourseCode"];
        // AcademicRecord newAcademicRecord  = new AcademicRecord();
        AcademicRecord? newRecord = null;
        foreach (var selectedValue in selectedValues)
        {
            // Process each selected checkbox value as needed
            // ...
             newRecord = new AcademicRecord
            {
                StudentId = SelectedStudentId,
                CourseCode = selectedValue
            };
            DataAccess.AddAcademicRecord(newRecord);
            // AcademicRecords.Add(newRecord);
            // DataAccess.AddAcademicRecord(new AcademicRecord
            //  {
            //      StudentId = SelectedStudentId,
            //      CourseCode = Request.Form["CourseCode"]
            //  });
             
        }
        
        // DataAccess.AddAcademicRecord(newRecord);
        Console.WriteLine(DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId));
         AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
         foreach (var record in AcademicRecords)
         {
             Console.WriteLine(record.CourseCode);
             
         }
        // DataAccess.AddAcademicRecord(newAcademicRecord);

        return RedirectToPage("Registration", new { selectedStudentId = SelectedStudentId });
        
    }

}