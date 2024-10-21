using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni_Manager.Manager;
using Uni_Manager.Enum;
using Uni_Manager.Service;
using Uni_Manager.Entity;

namespace Uni_Manager.AppMenuManager
{
    public class OptionExams
    {
        public static void OptionsExam(ExamsService examService,TeacherService teacherService, FacultiesService facultiesService,StudentsService studentService)
        {
            bool exitLoop = false;
            string lineSeparator = new('-', 75);
            // WorkerManager workerManager = new();
            UneversityManager uneversityManager = new();
            try
            {
                while (!exitLoop)
                {
                    Console.Clear();
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine("Benvenuto in University Corporation");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 1.Inserimento Esame");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 2.Visualizza Esami");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 3.Modifica Esame");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 4.Elimina Esame");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 5.Ricerca Esame");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 6.Inserimento sul db");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 7.Ritorna al menu' principale");

                    Console.WriteLine(lineSeparator);
                    Console.Write("Eseguire scelta da 1 a 7: ");

                    //ConsoleKeyInfo sceltaString = Console.ReadKey();
                    string? sceltaString = Console.ReadLine();
                    bool resultChoice = int.TryParse(sceltaString, out int scelta);
                    var matters = facultiesService.facultyRepository.Faculties.SelectMany(f => f.Matters).ToList();

                    

                    switch ((AppMenuExamEnum)scelta)
                    {
                       
                         
                        case AppMenuExamEnum.AddExam:
                            // Ora chiama il metodo sull'istanza
                            examService.AddExam(examService.examRepository.Exams, matters, teacherService.teacherRepository.Teachers, studentService.studentRepository.Students);
                            break;
                        case AppMenuExamEnum.ViewExam:
                            examService.PrintExam(examService.examRepository.Exams);
                            break;
                        case AppMenuExamEnum.EditExam:
                            examService.UpdateExam(examService.examRepository.Exams);


                            break;
                        case AppMenuExamEnum.RemoveExam:
                             examService.DeleteExamByExamCode(examService.examRepository.Exams);
                            break;
                           
                        case AppMenuExamEnum.SearchExam:
                            examService.SearchExam(examService.examRepository.Exams);
                            break;
                        case AppMenuExamEnum.InsertDbExam:
                            examService.InsertDbExam(examService.examRepository.Exams);
                            break;
                        case AppMenuExamEnum.RerturnToMenu:
                            Console.WriteLine("\nStai tornando al menu principale");
                            exitLoop = true;
                            break;
                        default:
                            Console.WriteLine("\nScelta non valida");
                            break;
                    }

                    
                    _ = Console.ReadKey();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($":ATTENZIONE: Errore imprevisto {ex.Message}");
            }
        }
    }
}

