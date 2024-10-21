using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uni_Manager.Entity;

namespace Uni_Manager.Manager
{
    public class UneversityManager
    {
        public List<Student> Students { get; set; } = [];
        public List<Teacher> Teachers { get; set; } = [];
        public List<Faculty> Faculties { get; set; } = [];
        public List<Exam> Exams { get; set; } = [];

        public void addStudent()
        {
            string? savePath = ConfigurationManager.AppSettings["PathImportStudents"];


            try
            {
                string? sStudents = File.ReadAllText(savePath);
                Students = JsonSerializer.Deserialize<List<Student>>(sStudents);
                Console.Clear();
                foreach (Student student in Students)
                {
                    // Stampa i dettagli del lavoratore in una sola riga
                 Console.WriteLine(student.ToString());
                }

            }


            catch (Exception ex)
            {

                throw;
            }
        }
        public void addTeacher()
        {
            string savePath = ConfigurationManager.AppSettings["PathImportTeachers"];


            try
            {
                string sTeachers = File.ReadAllText(savePath);
                Teachers = JsonSerializer.Deserialize<List<Teacher>>(sTeachers);
                foreach (Teacher teacher in Teachers)
                {
                    // Stampa i dettagli del lavoratore in una sola riga
                        Console.WriteLine(teacher.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void addFaculty()
        {
            string savePath = ConfigurationManager.AppSettings["PathImportFaculties"];


            try
            {
                string sFaculties = File.ReadAllText(savePath);
                Faculties = JsonSerializer.Deserialize<List<Faculty>>(sFaculties);
                foreach (Faculty faculty in Faculties)
                {

                    Faculties.ForEach(f =>
                    {
                        Students.ForEach(s =>
                        {
                            if (s.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                f.Students.Add(s);
                            }
                        });
                    });
                    Faculties.ForEach(f =>
                    {
                        Teachers.ForEach(t =>
                        {
                            if (t.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                f.Teachers.Add(t);
                            }
                        });
                    });// Stampa i dettagli del lavoratore in una sola riga
                    Console.WriteLine(faculty.ToString());
                }
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void ExportFaculty()
        {
            string url = ConfigurationManager.AppSettings["PathExportFaculties"];
            string sFaculties = JsonSerializer.Serialize(Faculties);
            Faculties.ForEach(f =>
        {
                Students.ForEach(s =>
                {
                    if (s.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        f.Students.Add(s);
                    }
                });
            });
           Faculties.ForEach(f =>
            {
                Teachers.ForEach(t =>
                {
                    if (t.Department.Equals(f.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        f.Teachers.Add(t);
                    }
                });
            });


            Exams.ForEach(e =>
            {
               Students.ForEach(s =>
                {
                    if (e.StudentMatricola.Equals(s.Matricola, StringComparison.OrdinalIgnoreCase))
                    {
                        s.Exams.Add(e);

                    }
                });
            });



          

            File.WriteAllText(url, sFaculties);



        }
        public void addExam()
        {
            string savePath = ConfigurationManager.AppSettings["PathImportExams"];


            try
            {
                string sExams = File.ReadAllText(savePath);
                Exams = JsonSerializer.Deserialize<List<Exam>>(sExams);
                foreach (Exam exam in Exams)
                {
                    // Stampa i dettagli del lavoratore in una sola riga
                    Console.WriteLine(exam.ToString());
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
