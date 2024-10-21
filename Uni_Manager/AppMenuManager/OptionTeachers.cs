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
    public class OptionTeachers
    {
        public static void OptionsTeacher(TeacherService teacherService,FacultiesService facultiesService)
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
                    Console.WriteLine();
                    Console.WriteLine(" 1.Inserimento Insegnante");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 2.Visuallizza Insegnanti");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 3.Modifica Insegnante");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 4.Elimina  Insegnante");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 5.Ricerca  Insegnante");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 6.Inserimento sul db");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 7.Ritorna al menu' principale");
                    Console.Write(" Eseguire scelta da 1 a 7: ");

                    //ConsoleKeyInfo sceltaString = Console.ReadKey();
                    string? sceltaString = Console.ReadLine();
                    bool resultChoice = int.TryParse(sceltaString, out int scelta);

                    switch ((AppMenuTeacherEnum)scelta)
                    {
                       
                           
                        case AppMenuTeacherEnum.AddTeacher:
                            //  uneversityManager.addExam();
                            teacherService.AddTeacher(teacherService.teacherRepository.Teachers, facultiesService.facultyRepository.Faculties);


                            break;
                        case AppMenuTeacherEnum.ViewTeacher:
                            teacherService.PrintTeachers(teacherService.teacherRepository.Teachers);
                            break;
                        case AppMenuTeacherEnum.EditTeacher:
                            teacherService.UpdateTeacher(teacherService.teacherRepository.Teachers);

                            break;
                        case AppMenuTeacherEnum.RemoveTeacher:
                            teacherService.DeleteTeacherstByMatricola(teacherService.teacherRepository.Teachers);
                            break;
                        case AppMenuTeacherEnum.SearchTeacher:
                            teacherService.SearchTeachers(teacherService.teacherRepository.Teachers);

                            break;
                        case AppMenuTeacherEnum.InsertDbTeacher:
                            teacherService.InsertDbTeachers(teacherService.teacherRepository.Teachers);
                            
                            break;
                        case AppMenuTeacherEnum.RerturnToMenu:
                            Console.WriteLine("\nStai tornando al menu' principale");
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

