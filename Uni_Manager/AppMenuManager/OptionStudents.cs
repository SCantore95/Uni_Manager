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
    public class OptionStudents
    {
        public static void OptionsStudent(StudentsService studentService)
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
                    Console.WriteLine(" Benvenuto in University Corporation");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine();
                    Console.WriteLine(" 1.Inserimento Studente");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 2.Visualizzazione Studenti");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 3.Modifica Studente");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 4.Elimina Studente");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 5.Ricerca Studente");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 6.Inserimento sul db");
                    Console.WriteLine(lineSeparator);
                    Console.WriteLine(" 7.Ritorna al menu' principale");

                    Console.WriteLine(lineSeparator);
                    Console.Write("Eseguire scelta da 1 a 7: ");

                    //ConsoleKeyInfo sceltaString = Console.ReadKey();
                    string? sceltaString = Console.ReadLine();
                    bool resultChoice = int.TryParse(sceltaString, out int scelta);

                    switch ((AppMenuStudentEnum)scelta)
                    {
                       
                           
                        case AppMenuStudentEnum.AddStudent:
                            
                            studentService.AddStudent(studentService.studentRepository.Students);

                            break;
                            case AppMenuStudentEnum.ViewStudent:
                            studentService.PrintStudents(studentService.studentRepository.Students);
                            break;
                        case AppMenuStudentEnum.EditStudent:
                            studentService.UpdateStudent(studentService.studentRepository.Students);
                            // uneversityManager.ExportFaculty();
                            break;
                        case AppMenuStudentEnum.RemoveStudent:
                            studentService.DeleteStudentByMatricola(studentService.studentRepository.Students);
                            break;

                         case AppMenuStudentEnum.SearchStudent:
                            studentService.SearchStudent(studentService.studentRepository.Students);
                            break;

                        case AppMenuStudentEnum.InsertDbStudents:
                             studentService.InsertDbStudents(studentService.studentRepository.Students);
                            break;
                        case AppMenuStudentEnum.RerturnToMenu:
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

