using System;
using Uni_Manager.Entity;
using Uni_Manager.Repository;
using System.Configuration;
using System.Text.Json;
using Microsoft.Data.SqlClient;


namespace Uni_Manager.Service { 

public class StudentsService
{
 public StudentRepository studentRepository { get; set;}= new();
        string lineSeparator = new('-', 145);
       
        public void AddStudent(List<Student> Students)
        {
            Console.Clear();
            bool isValidField = false;
            string matricola = string.Empty;
            string name = string.Empty;
            string surename = string.Empty;
            string age;

            try
            {
                Console.Clear();
                Console.WriteLine("Inserimento Studente");

                // Inserimento matricola
                while (!isValidField)
                {
                    try
                    {
                        Console.Write("Inserire Numero Matricola (4 caratteri): ");
                        matricola = Console.ReadLine();

                        // Verifica lunghezza e unicità della matricola
                        if (matricola.Length == 4)
                        {
                            if (Students.Any(s => s.Matricola == matricola))
                            {
                                throw new ArgumentException("La matricola inserita è già assegnata a uno studente.");
                            }
                            isValidField = true;
                        }
                        else
                        {
                            throw new ArgumentException("La matricola deve essere di 4 caratteri.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore: " + ex.Message + ". Reinserire il dato.");
                    }
                }

                isValidField = false; // Reset per il campo successivo

                while (!isValidField)
                {
                    try
                    {
                        Console.Write("Inserire Nome (almeno 3 caratteri): ");
                        name = Console.ReadLine();

                        if (name.Length >= 3)
                        {
                            isValidField = true;
                        }
                        else
                        {
                            throw new ArgumentException("Il nome deve essere di almeno 3 caratteri.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore: " + ex.Message + ". Reinserire il dato.");
                    }
                }

                isValidField = false; // Reset per il campo successivo

                while (!isValidField)
                {
                    try
                    {
                        Console.Write("Inserire Cognome (almeno 3 caratteri): ");
                        surename = Console.ReadLine();

                        if (surename.Length >= 3)
                        {
                            isValidField = true;
                        }
                        else
                        {
                            throw new ArgumentException("Il cognome deve essere di almeno 3 caratteri.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Errore: " + ex.Message + ". Reinserire il dato.");
                    }
                }

                isValidField = false; // Reset per il campo successivo

                Console.Write("Età: ");
                int ageInt = 0;

                while (!isValidField)
                {
                    age = Console.ReadLine();

                    if (int.TryParse(age, out ageInt) && ageInt >= 18)
                    {
                        isValidField = true;
                    }
                    else if (string.IsNullOrEmpty(age))
                    {
                        Console.WriteLine("Valore nullo.");
                    }
                    else
                    {
                        Console.WriteLine("Età non valida.");
                    }
                }

                isValidField = false; // Reset per il campo successivo

                // Scelta del genere
                List<string> optionsGender = new List<string> { "Maschio", "Femmina" };
                int selectedIndexGender = 0;
                ConsoleKeyInfo keyGender;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Scegli il genere:");
                    for (int i = 0; i < optionsGender.Count; i++)
                    {
                        if (i == selectedIndexGender)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine(optionsGender[i]);
                        Console.ResetColor();
                    }

                    keyGender = Console.ReadKey(true);

                    if (keyGender.Key == ConsoleKey.UpArrow)
                    {
                        selectedIndexGender = (selectedIndexGender > 0) ? selectedIndexGender - 1 : optionsGender.Count - 1;
                    }
                    else if (keyGender.Key == ConsoleKey.DownArrow)
                    {
                        selectedIndexGender = (selectedIndexGender < optionsGender.Count - 1) ? selectedIndexGender + 1 : 0;
                    }

                } while (keyGender.Key != ConsoleKey.Enter);

                string gender = optionsGender[selectedIndexGender];

                isValidField = false; // Reset per il campo successivo

                // Scelta della facoltà
                List<string> options = new List<string>
        {
            "Facoltà di Giurisprudenza", "Facoltà di Economia", "Facoltà di Ingegneria",
            "Facoltà di Medicina", "Facoltà di Lettere e Filosofia", "Facoltà di Scienze",
            "Facoltà di Architettura", "Facoltà di Scienze Politiche", "Facoltà di Psicologia"
        };
                int selectedIndex = 0;
                ConsoleKeyInfo key;

                do
                {
                    Console.Clear();
                    Console.WriteLine("Scegli la facoltà:");

                    for (int i = 0; i < options.Count; i++)
                    {
                        if (i == selectedIndex)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }

                    key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.UpArrow)
                    {
                        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Count - 1;
                    }
                    else if (key.Key == ConsoleKey.DownArrow)
                    {
                        selectedIndex = (selectedIndex < options.Count - 1) ? selectedIndex + 1 : 0;
                    }

                } while (key.Key != ConsoleKey.Enter);

                string department = options[selectedIndex];

                isValidField = false; // Reset per il campo successivo

                // Anno di iscrizione
                Console.WriteLine("Anno di iscrizione:");
                string date = string.Empty;
                int dateInt = 0;

                while (!isValidField)
                {
                    date = Console.ReadLine();

                    if (date.Length == 4 && int.TryParse(date, out dateInt))
                    {
                        isValidField = true;
                    }
                    else
                    {
                        Console.WriteLine("Data non valida.");
                    }
                }

                // Creazione e aggiunta dello studente
                Student newStudent = new Student(name, surename, ageInt, gender, matricola, department, dateInt);

                bool dbInsertSuccess = InsertDbStudents(new List<Student> { newStudent });
                if (dbInsertSuccess)
                {
                    Console.WriteLine("Studente inserito correttamente nel database.");
                }
                else
                {
                    Console.WriteLine("Errore durante l'inserimento dello studente nel database.");
                }

                // Serializzazione e salvataggio degli studenti aggiornati
                string savePath = ConfigurationManager.AppSettings["PathImportStudents"];
                string sStudent = JsonSerializer.Serialize(newStudent, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(savePath, sStudent);

               
                Console.WriteLine("Studente inserito correttamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore durante l'inserimento dello studente: " + ex.Message);
            }
        }
        public void PrintStudents(List<Student> Students)
        {
            Console.Clear();
            foreach (Student student in Students)
            {
                // Stampa i dettagli dello studente
                Console.WriteLine(student.ToString());
                Console.WriteLine(lineSeparator);
                
            }
        }

        public void UpdateStudent(List<Student> Students)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Inserisci la matricola dello studente da modificare: ");
                string matricola = Console.ReadLine();
                var matchingStudents = Students.Where(s => s.Matricola.Equals(matricola, StringComparison.CurrentCultureIgnoreCase)).ToList();

                if (matchingStudents.Count == 0)
                {
                    Console.WriteLine("Studente non trovato.");
                    return;
                }

                Student studentToUpdate;

                if (matchingStudents.Count > 1)
                {
                    Console.WriteLine("ERRORE: Trovati più studenti con la stessa matricola! Selezionare quale matricola modificare:");
                    for (int i = 0; i < matchingStudents.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Nome: {matchingStudents[i].Name}, Cognome: {matchingStudents[i].SureName}, Età: {matchingStudents[i].Age}, Dipartimento: {matchingStudents[i].Department}");
                    }

                    int selectedIndex;
                    do
                    {
                        Console.WriteLine("Inserire il numero dello studente da selezionare: ");
                    } while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > matchingStudents.Count);

                    studentToUpdate = matchingStudents[selectedIndex - 1];
                }
                else
                {
                    studentToUpdate = matchingStudents[0];
                }

                // Aggiornamento dei campi
                Console.WriteLine("Studente trovato! Aggiornare i campi (lasciare vuoto per mantenere i valori attuali): ");
                Console.WriteLine($"Nome attuale: {studentToUpdate.Name}. Inserire nuovo nome (oppure lascia vuoto): ");
                string newName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newName)) studentToUpdate.Name = newName;

                Console.WriteLine($"Cognome attuale: {studentToUpdate.SureName}. Inserire nuovo cognome (oppure lascia vuoto):");
                string newSureName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newSureName)) studentToUpdate.SureName = newSureName;

                Console.WriteLine($"Età attuale: {studentToUpdate.Age}. Inserire nuova età (oppure lascia vuoto): ");
                string newAge = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newAge) && int.TryParse(newAge, out int age)) studentToUpdate.Age = age;

                Console.WriteLine($"Dipartimento attuale: {studentToUpdate.Department}. Inserire nuovo dipartimento (oppure lascia vuoto): ");
                string newDepartment = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newDepartment)) studentToUpdate.Department = newDepartment;

                Console.WriteLine($"Anno di iscrizione attuale: {studentToUpdate.AnnoDiIscrizione}. Inserire nuovo anno (oppure lascia vuoto): ");
                string newAnno = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(newAnno) && int.TryParse(newAnno, out int anno)) studentToUpdate.AnnoDiIscrizione = anno;

                // Aggiornamento nel database
                bool updateSuccess = UpdateDbStudents(new List<Student> { studentToUpdate });

                if (updateSuccess)
                {
                    // Serializzazione e salvataggio degli studenti aggiornati
                    string savePath = ConfigurationManager.AppSettings["PathImportStudents"];
                    string sStudent = JsonSerializer.Serialize(Students, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(savePath, sStudent);

                    Console.WriteLine("Studente aggiornato con successo.");
                }
                else
                {
                    Console.WriteLine("Errore durante l'aggiornamento dello studente nel database.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore: {ex.Message}");
            }
        }




        public void DeleteStudentByMatricola(List<Student> Students)
        {
            Console.Clear();
            Console.WriteLine("Inserisci la matricola dello studente da eliminare: ");
            string matricola = Console.ReadLine();

            // Cerca lo studente direttamente con la matricola
            var student = Students.FirstOrDefault(s => s.Matricola.Equals(matricola, StringComparison.CurrentCultureIgnoreCase));

            if (student != null)
            {
                Students.Remove(student);
                List<Student> studentsToDelete = new List<Student> { student };

                // Chiamata per eliminare dallo database
                bool dbDeleteSuccess = DeleteDbStudents(studentsToDelete);

                if (dbDeleteSuccess)
                {
                    Console.WriteLine("Studente rimosso con successo dal database.");
                }
                else
                {
                    Console.WriteLine("Errore durante l'eliminazione dello studente dal database.");
                }
                Console.WriteLine("Studente rimosso con successo.");
            }
            else
            {
                Console.WriteLine("Studente non trovato.");
            }

            // Serializzazione e salvataggio degli studenti aggiornati
            string savePath = ConfigurationManager.AppSettings["PathImportStudents"];
            string sStudent = JsonSerializer.Serialize(Students, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(savePath, sStudent);
        }


        public void SearchStudent(List<Student> Students)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Inserisci la matricola dello studente da cercare: ");
                string matricola = Console.ReadLine();

                // Cerca lo studente direttamente con la matricola
                var student = Students.Find(s => s.Matricola.Equals(matricola, StringComparison.CurrentCultureIgnoreCase));

                if (student != null)
                {
                    Console.WriteLine(student.ToString());
                }
                else
                {
                    Console.WriteLine("Studente non trovato.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la ricerca dello studente: {ex.Message}");
            }
        }


        public bool InsertDbStudents(List<Student> students)
        {
            bool insertOK = true;

            string query = "INSERT INTO Student (matricola, department, AnnoDiIscrizione, name, surename, age, gender) " +
                           "VALUES (@Matricola, @Department, @AnnoDiIscrizione, @Name, @SureName, @Age, @Gender)";

            try
            {
                using (var connection = new SqlConnection(DbManager.ConnectionString))
                {
                    connection.Open();

                    foreach (var student in students)
                    {
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Matricola", student.Matricola);
                            command.Parameters.AddWithValue("@Name", student.Name);
                            command.Parameters.AddWithValue("@surename", student.SureName); // Ensure "SureName" matches your column
                            command.Parameters.AddWithValue("@Age", student.Age);
                            command.Parameters.AddWithValue("@Gender", student.Gender);
                            command.Parameters.AddWithValue("@Department", student.Department);
                            command.Parameters.AddWithValue("@AnnoDiIscrizione", student.AnnoDiIscrizione);

                            try
                            {
                                // Execute the query and get the number of affected rows
                                int affectedRows = command.ExecuteNonQuery();
                                Console.WriteLine($"Lo studente con Matricola: {student.Matricola} è stato isnerito nel database.");

                                // Check the number of affected rows
                                if (affectedRows == 0)
                                {
                                    insertOK = false; // Insertion failed for this student
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                // Check if the error code corresponds to a unique constraint violation
                                if (sqlEx.Number == 2627) // SQL Server error code for unique constraint violation
                                {
                                    // Log or handle the duplicate entry (optional)
                                    Console.WriteLine($"Lo studente con Matricola: {student.Matricola} gia esite. sto Saltando.");
                                    insertOK = true; // Continue with the next student
                                }
                                else
                                {
                                    // If it's a different SQL exception, rethrow it
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                throw new Exception("Error inserting students into database", ex);
            }
            finally
            {
                // Ensure the database connection is closed

            }

            return insertOK;
        }
        public bool DeleteDbStudents(List<Student> students)
        {
            bool deleteOK = true;

            // Query di DELETE basata sulla matricola
            string query = "DELETE FROM Student WHERE matricola = @Matricola";

            try
            {
                using (var connection = new SqlConnection(DbManager.ConnectionString))
                {
                    connection.Open();

                    foreach (var student in students)
                    {
                        using (var command = new SqlCommand(query, connection))
                        {
                            // Aggiungi il parametro per la matricola
                            command.Parameters.AddWithValue("@Matricola", student.Matricola);

                            try
                            {
                                // Esegui la query e verifica il numero di righe affette
                                int affectedRows = command.ExecuteNonQuery();
                                Console.WriteLine($"Lo studente con Matricola: {student.Matricola} è stato rimosso dal DB.");

                                // Se nessuna riga è stata cancellata, imposta deleteOK su false
                                if (affectedRows == 0)
                                {
                                    Console.WriteLine($"Lo studente con Matricola: {student.Matricola} non esiste nel database.");
                                    deleteOK = false;
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                // Gestione dell'eccezione SQL
                                Console.WriteLine($"Errore SQL durante l'eliminazione dello studente con Matricola: {student.Matricola}. Messaggio: {sqlEx.Message}");
                                deleteOK = false; // Segnala l'errore
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log o gestione dell'eccezione generale
                Console.WriteLine($"Errore durante l'eliminazione degli studenti dal database: {ex.Message}");
                deleteOK = false;
            }

            return deleteOK;
        }
        public bool UpdateDbStudents(List<Student> students)
        {
            bool updateOK = true;

            // Query di UPDATE
            string query = "UPDATE Student SET name = @Name, surename = @SureName, age = @Age, gender = @Gender, department = @Department, AnnoDiIscrizione = @AnnoDiIscrizione WHERE matricola = @Matricola";

            try
            {
                using (var connection = new SqlConnection(DbManager.ConnectionString))
                {
                    connection.Open();

                    foreach (var student in students)
                    {
                        using (var command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Matricola", student.Matricola);
                            command.Parameters.AddWithValue("@Name", student.Name);
                            command.Parameters.AddWithValue("@SureName", student.SureName);
                            command.Parameters.AddWithValue("@Age", student.Age);
                            command.Parameters.AddWithValue("@Gender", student.Gender);
                            command.Parameters.AddWithValue("@Department", student.Department);
                            command.Parameters.AddWithValue("@AnnoDiIscrizione", student.AnnoDiIscrizione);

                            try
                            {
                                int affectedRows = command.ExecuteNonQuery();
                                Console.WriteLine($"Lo studente con Matricola: {student.Matricola} è stato aggiornato nel DB.");
                                if (affectedRows == 0)
                                {
                                    Console.WriteLine($"Lo studente con Matricola: {student.Matricola} non esiste nel DB.");
                                    updateOK = false;
                                }
                            }
                            catch (SqlException sqlEx)
                            {
                                Console.WriteLine($"Errore SQL durante l'aggiornamento dello studente con Matricola: {student.Matricola}. Messaggio: {sqlEx.Message}");
                                updateOK = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'aggiornamento degli studenti dal database: {ex.Message}");
                updateOK = false;
            }

            return updateOK;
        }


    }

}