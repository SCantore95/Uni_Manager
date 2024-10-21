using System;
using Uni_Manager.AppMenuManager;
using Uni_Manager.Repository;

namespace Uni_Manager.Service;

public class AppMenuService
{
   
    private ExamsService examService = new ExamsService();
    private FacultiesService facultyService = new FacultiesService();
    private TeacherService teacherService = new TeacherService();

    public StudentsService studentService { get; set; } = new StudentsService();

  

    // Metodo per importare tutti i dati
    public void ImportAll()
    {
        // Importazione dati da repository
        facultyService.facultyRepository.ImportFaculty();
        studentService.studentRepository.ImportStudents();
        teacherService.teacherRepository.ImportTeachers();
        examService.examRepository.ImportExams();

        // Associazione studenti alle facoltà
        facultyService.facultyRepository.Faculties.ForEach(f =>
        {
            studentService.studentRepository.Students.ForEach(s =>
            {
                if (s.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                {
                    f.Students.Add(s);

                }
            });
        });

        // Associazione docenti alle facoltà
        facultyService.facultyRepository.Faculties.ForEach(f =>
        {
            teacherService.teacherRepository.Teachers.ForEach(t =>
            {
                if (t.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                {
                    f.Teachers.Add(t);
                }
            });
        });


        AppMenu.Show(studentService, teacherService, facultyService, examService);
       
        facultyService.facultyRepository.ExportFaculty();
     

    }
}