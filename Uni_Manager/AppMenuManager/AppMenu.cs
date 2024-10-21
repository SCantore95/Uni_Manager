using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uni_Manager.Manager;
using Uni_Manager.Enum;
using Uni_Manager.Service;
using Uni_Manager.Repository;


namespace Uni_Manager.AppMenuManager
{
    public class AppMenu
    {
        public static void Show(StudentsService studentService,TeacherService teacherService,FacultiesService facultiesService, ExamsService examService)
        {
            bool exitLoop = false;
            string lineSeparator = new('-', 75);
            // WorkerManager workerManager = new();
            //UneversityManager uneversityManager = new();
            try
            {
                while (!exitLoop)
                {
                    Console.Clear();
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" Benvenuto in University Corporation");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine();
                    Console.WriteLine(" 1.Gestione Studenti");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 2.Gestione Professori");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 3.Gestione Esami");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 4.Export Faculty");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 5.Esci");
                   
                    Console.WriteLine(lineSeparator);
                    Console.Write("Eseguire scelta da 1 a 4: ");

                    //ConsoleKeyInfo sceltaString = Console.ReadKey();
                    string? sceltaString = Console.ReadLine();
                    bool resultChoice = int.TryParse(sceltaString, out int scelta);

                    switch ((AppMenuEnum)scelta)
                    {
                        case AppMenuEnum.OptionsStudent:
                            
                            OptionStudents.OptionsStudent(studentService);
                            break;
 
                        case AppMenuEnum.OptionsTeacher:
                            OptionTeachers.OptionsTeacher(teacherService, facultiesService);

                            break;
                        case AppMenuEnum.OptionsExam:
                            OptionExams.OptionsExam( examService,teacherService, facultiesService,studentService);

                            break;
                        case AppMenuEnum.Exit:
                            Console.WriteLine("\nUscita dal programma");
                            exitLoop = true;
                            break;
                            
                        
                     
                        default:
                            Console.WriteLine("\nScelta non valida");
                            break;
                    }

                    Console.WriteLine("Premere un tasto per continuare");
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

