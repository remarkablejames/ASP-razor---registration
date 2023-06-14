
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
    // initialize sortBy session variable
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
            
            // sort by grade if the session variable is set
            if (HttpContext.Session.GetString("sortBy") == "Grade")
            {
                AcademicRecords = AcademicRecords.OrderBy(r => r.Grade).ToList();
            }
            else if (HttpContext.Session.GetString("sortBy") == "CourseCode")
            {
                AcademicRecords = AcademicRecords.OrderBy(r => r.CourseCode).ToList();
            }
            else
            {
                AcademicRecords = AcademicRecords.OrderBy(r => r.StudentId).ToList();
            }
            

        }
        else
        {
            ErrorMessage = " ";
        }
        
        return Page();
    }
    public IActionResult OnPostRegister(string selectedStudentId, string msg)
    {

        Console.WriteLine("Run: OnPostRegister ");
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
            ErrorMessage = "You must select a student!";
        }
        // sort by grade if the session variable is set
        if (HttpContext.Session.GetString("sortBy") == "Grade")
        {
            AcademicRecords = AcademicRecords.OrderBy(r => r.Grade).ToList();
        }
        else if (HttpContext.Session.GetString("sortBy") == "CourseCode")
        {
            AcademicRecords = AcademicRecords.OrderBy(r => r.CourseCode).ToList();
        }
        else
        {
            AcademicRecords = AcademicRecords.OrderBy(r => r.StudentId).ToList();
        }

        return Page();
    }
    
    public IActionResult OnPostStudentSelected()
    {
        var selectedValues = Request.Form["CourseCode"];
        
        if (selectedValues.Count== 0 )
        {
           
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
            msg = "The student has registered for the following courses:";


        }
        
        return RedirectToPage("Registration", new { selectedStudentId = SelectedStudentId, msg = msg });

        
    }
    // ------------------------------------------------------------
    public IActionResult OnPostSubmitGrades()
    {
        var courseCodes = Request.Form["courseCodes"];
        var grade = Request.Form["Grade"];
        
        AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
        foreach (var record in AcademicRecords)
        {
            
            for (int i = 0; i < courseCodes.Count(); i++)
            {
                if (record.CourseCode == courseCodes[i])
                {
                    record.Grade = Convert.ToDouble(grade[i]);
                }
            }
            
            Console.WriteLine("--------------------");

        }
        // AcademicRecords = DataAccess.GetAcademicRecordsByStudentId(SelectedStudentId);
        return RedirectToPage("Registration", new { selectedStudentId = SelectedStudentId, msg = msg });

    }
    
    // ------------------------------------------------------------
    
    public IActionResult OnPostInvokeFunction(string parameterValue2, string selectedStudentId)
    {
        var parameterValue = Request.Form["sortParam"];
        // update the session variable
        HttpContext.Session.SetString("sortBy", parameterValue);
        Console.WriteLine("Run: OnPostInvokeFunction");
        Console.WriteLine("selectedStudentId: " + selectedStudentId);
        return RedirectToPage("Registration", new { selectedStudentId = SelectedStudentId, msg = msg });
    }
    

}