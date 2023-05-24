using System.Collections;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AcademicManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // This is the namespace of the AcademicManagement project

namespace Lab2.Pages;

public class Registration : PageModel
{
    // public string[] Students = new string[5] { "James", "John", "Jane", "Jill", "Jack" };
    public List<Student> Students = DataAccess.GetAllStudents();
    public List<Course> Courses = DataAccess.GetAllCourses();
    public List<AcademicRecord> AcademicRecords = new List<AcademicRecord>();

    // ------------------------------------------------------------
    [BindProperty] 
    public string SelectedStudentId { get; set; }
   
    public List<SelectListItem> StudentOptions { get; set; }
    
    // public IActionResult OnGetHandleStudentSelection(string selectedStudentId, string selectedStudentName)
    // {
    //     // Handle the selected student
    //     // You can access the selectedStudentId and selectedStudentName parameters here
    //     // Perform any necessary logic or operations based on the selected student
    //     Console.WriteLine("Selected student ID: " + selectedStudentId);
    //     return RedirectToPage("Index");
    // }
    
    public IActionResult OnGet(string selectedStudentId)
    {
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
        foreach (var selectedValue in selectedValues)
        {
            // Process each selected checkbox value as needed
            // ...
            // Console.WriteLine(selectedValue);
            // Console.WriteLine(SelectedStudentId);
        }
        
        // add the new academic record to the list of academic records
        AcademicRecord newAcademicRecord = new AcademicRecord
        {
            StudentId = SelectedStudentId,
            CourseCode = Request.Form["CourseCode"]
        };
        DataAccess.AddAcademicRecord(newAcademicRecord);
        // Console.Write(newAcademicRecord.CourseCode+ "HEREE");
         AcademicRecords.Add(newAcademicRecord);
         
        // DataAccess.GetAllAcademicRecords().Add(newAcademicRecord);
        // AcademicRecords = DataAccess.GetAllAcademicRecords();
        //  Console.Write(AcademicRecords.Count+ "After Post");
        
        
        return RedirectToPage();
        
    }

    //public IEnumerable Students { get; set; }
}