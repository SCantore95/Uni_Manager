using Microsoft.Data.SqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text.Json;
using Uni_Manager.Entity;
using Uni_Manager.Repository;

namespace Uni_Manager.Service;

public class TeacherService
{
    public TeacherRepository teacherRepository {get;set;} =new();
    public void PrintTeachers(List<Teacher> Teachers)
    {
        Console.Clear();
        foreach (Teacher teacher in Teachers)
        {
            // Stampa i dettagli dello studente
            Console.WriteLine(teacher.ToString());
        }
    }
    public void SearchTeachers(List<Teacher> Teachers)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Inserisci il codice dell'insegnante da cercare: ");
            string TeacherCode = Console.ReadLine();

            // Cerca l'insegnante direttamente con il codice
            var teacher = Teachers.Find(t => t.TeacherCode.Equals(TeacherCode, StringComparison.CurrentCultureIgnoreCase));

            if (teacher != null)
            {
                Console.WriteLine(teacher.ToString());
            }
            else
            {
                Console.WriteLine("Insegnante non trovato.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la ricerca dell'insegnante: {ex.Message}");
        }
    }
    public void DeleteTeacherstByMatricola(List<Teacher> Teachers)
    {
        Console.Clear();
        Console.WriteLine("Inserisci la matricola dello studente da eliminare: ");
        string TeacherCode = Console.ReadLine();

        // Cerca lo studente direttamente con la matricola
        var teacher = Teachers.Find(t => t.TeacherCode.Equals(TeacherCode, StringComparison.CurrentCultureIgnoreCase));

        if (teacher != null)
        {
            Teachers.Remove(teacher);
            List<Teacher> teachersToDelete = new List<Teacher> { teacher };
            // Chiamata per eliminare dallo database
            bool dbDeleteSuccess = DeleteDbTeachers(teachersToDelete);

            if (dbDeleteSuccess)
            {
                Console.WriteLine("Insegnante rimosso con successo dal database.");
            }
            else
            {
                Console.WriteLine("Errore durante l'eliminazione dell insegnante dal database.");
            }
            Console.WriteLine("Insegnante rimosso con successo.");
        }
        else
        {
            Console.WriteLine("Insegnante non trovato.");
        }

        // Serializzazione e salvataggio degli studenti aggiornati
        string savePath = ConfigurationManager.AppSettings["PathImportTeachers"];
        string tTeacher = JsonSerializer.Serialize(Teachers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(savePath, tTeacher);
    }
    public void AddTeacher(List<Teacher> Teachers, List<Faculty> faculties)
    {
        bool isValidField = false;
        string teacherCode = string.Empty;
        string name                                                                                                                                                                                                                                                                                                                                                                                                        = string.Empty;
        string surename = string.Empty;
        string age;

        try
        {
            Console.Clear();
            Console.WriteLine("Inserimento Insegnante");

            // Inserimento matricola
            while (!isValidField)
            {
                try
                {
                    Console.Write("Inserire Numero Matricola (4 caratteri): ");
                    teacherCode = Console.ReadLine();

                    // Verifica lunghezza e unicità della matricola
                    if (teacherCode.Length == 4)
                    {
                        if (Teachers.Any(t => t.TeacherCode == teacherCode))
                        {
                            throw new ArgumentException("La matricola inserita è già assegnata a un insegnante.");
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
            TeacherRoleEnum Role = new();


            List<String> optionsRole = ["Professore", "Assistente"];
            int selectedIndexRole = 0;
            ConsoleKeyInfo keyRole;

            do
            {
                Console.Clear();
                Console.WriteLine("Ruolo:");
                for (int i = 0; i < optionsRole.Count; i++)
                {
                    if (i == selectedIndexRole)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(optionsRole[i]);
                    Console.ResetColor();
                }

                keyRole = Console.ReadKey(true);

                if (keyRole.Key == ConsoleKey.UpArrow)
                {
                    selectedIndexRole = (selectedIndexRole > 0) ? selectedIndexRole - 1 : optionsRole.Count - 1;
                }
                else if (keyRole.Key == ConsoleKey.DownArrow)
                {
                    selectedIndexRole = (selectedIndexRole < optionsRole.Count - 1) ? selectedIndexRole + 1 : 0;
                }

            } while (keyRole.Key != ConsoleKey.Enter);


            List<String> optionsMatter = faculties.Find(f => f.Name == department).Matters.Select(m => m.Name).ToList();
            Matter matter = new();
            int selectedIndexMatter = 0;
            ConsoleKeyInfo keyMatter;

            do
            {
                Console.Clear();
                Console.WriteLine("Materia:");
                for (int i = 0; i < optionsMatter.Count; i++)
                {
                    if (i == selectedIndexMatter)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine(optionsMatter[i]);
                    Console.ResetColor();
                }

                keyMatter = Console.ReadKey(true);

                if (keyMatter.Key == ConsoleKey.UpArrow)
                {
                    selectedIndexMatter = (selectedIndexMatter > 0) ? selectedIndexMatter - 1 : optionsMatter.Count - 1;
                }
                else if (keyMatter.Key == ConsoleKey.DownArrow)
                {
                    selectedIndexMatter = (selectedIndexMatter < optionsMatter.Count - 1) ? selectedIndexMatter + 1 : 0;
                }

            } while (keyMatter.Key != ConsoleKey.Enter);

            Console.Clear();

            optionsMatter.ForEach(o =>
            {


                if (optionsMatter.IndexOf(o) == selectedIndexMatter)
                {

                    matter = faculties.Find(f => f.Name == department).Matters.Find(m => m.Name == o);
                    return;
                }
            });
            isValidField = false;



            switch (selectedIndexRole)
            {
                case 0:
                    Role = TeacherRoleEnum.Teacher;
                    break;
                case 1:
                    Role = TeacherRoleEnum.Assistant;

                    break;
            }
            isValidField = false;
            // Creazione e aggiunta dello studente
           Teacher newTeacher = new Teacher(name, surename, ageInt, gender, department, teacherCode, matter, Role);
            bool dbInsertSuccess = InsertDbTeachers(new List<Teacher> { newTeacher });
            if (dbInsertSuccess)
            {
                Console.WriteLine("Insegnante inserito correttamente nel database.");
            }
            else
            {
                Console.WriteLine("Errore durante l'inserimento dell'insegnante  nel database.");
            }
            // Serializzazione e salvataggio degli studenti aggiornati
            string savePath = ConfigurationManager.AppSettings["PathImportTeachers"];
            string tTeacher = JsonSerializer.Serialize(newTeacher, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(savePath, tTeacher);


            Console.WriteLine("Insegnante inserito correttamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore durante l'inserimento dello studente: " + ex.Message);
        }
    }
    #region "Update Methods"

    public void UpdateTeacher(List<Teacher> Teachers)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Inserisci la matricola dell'insegnante da modificare: ");
            string teacherCode = Console.ReadLine();
            var matchingTeachers = Teachers.Where(s => s.TeacherCode.Equals(teacherCode, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (matchingTeachers.Count == 0)
            {
                Console.WriteLine("Docente non trovato.");
                return;
            }

            Teacher teacherToUpdate = null;

            // Se ci sono più insegnanti con lo stesso codice
            if (matchingTeachers.Count > 1)
            {
                Console.WriteLine("ERRORE: Trovati più docenti con lo stesso codice! Selezionare quale codice modificare:");
                for (int i = 0; i < matchingTeachers.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Nome: {matchingTeachers[i].Name}, Cognome: {matchingTeachers[i].SureName}, Età: {matchingTeachers[i].Age}, Dipartimento: {matchingTeachers[i].Department},  Materia: {matchingTeachers[i].TeachedMatter}");
                }

                int selectedIndex;
                do
                {
                    Console.WriteLine("Inserire il numero del docente da selezionare: ");
                } while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > matchingTeachers.Count);

                teacherToUpdate = matchingTeachers[selectedIndex - 1];
            }
            else
            {
                teacherToUpdate = matchingTeachers[0];
            }

            // Aggiornamento dei campi
            Console.WriteLine($"Docente trovato! Aggiornare i campi (lasciare vuoto per mantenere i valori attuali): ");

            string newCode = PromptForUpdate("Codice attuale", teacherToUpdate.TeacherCode);
            if (!string.IsNullOrWhiteSpace(newCode) && !Teachers.Any(s => s.TeacherCode == newCode))
            {
                teacherToUpdate.TeacherCode = newCode;
                Console.WriteLine("Codice aggiornato con successo.");
            }

            string newName = PromptForUpdate("Nome attuale", teacherToUpdate.Name);
            if (!string.IsNullOrWhiteSpace(newName)) teacherToUpdate.Name = newName;

            string newSurname = PromptForUpdate("Cognome attuale", teacherToUpdate.SureName);
            if (!string.IsNullOrWhiteSpace(newSurname)) teacherToUpdate.SureName = newSurname;

            string newAge = PromptForUpdate("Età attuale", teacherToUpdate.Age.ToString());
            if (!string.IsNullOrWhiteSpace(newAge) && int.TryParse(newAge, out int age)) teacherToUpdate.Age = age;

            string newDepartment = PromptForUpdate("Dipartimento attuale", teacherToUpdate.Department);
            if (!string.IsNullOrWhiteSpace(newDepartment)) teacherToUpdate.Department = newDepartment;

            // Aggiorna il database
            if (UpdateDbTeachers(new List<Teacher> { teacherToUpdate }))
            {
                string savePath = ConfigurationManager.AppSettings["PathImportTeachers"];
                string tTeacher = JsonSerializer.Serialize(Teachers, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(savePath, tTeacher);
                Console.WriteLine("Docente aggiornato con successo.");
            }
            else
            {
                Console.WriteLine("Errore durante l'aggiornamento dell'insegnante nel database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }

    // Funzione di supporto per semplificare l'input dell'utente
    private string PromptForUpdate(string label, string currentValue)
    {
        Console.WriteLine($"{label}: {currentValue}. Inserire nuovo valore (oppure lascia vuoto): ");
        return Console.ReadLine();
    }


    #endregion
    public bool InsertDbTeachers(List<Teacher> Teachers)
    {
        bool insertOK = true;

        string query = "INSERT INTO Teacher (TeacherCode, name, surename, age, gender, department, TeachedMatterCode, Role) " +
                       "VALUES (@TeacherCode, @Name, @SureName, @Age, @Gender, @Department, @TeachedMatterCode, @Role)";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var teacher in Teachers)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Aggiungi i parametri con Add per specificare il tipo
                        command.Parameters.Add("@TeacherCode", SqlDbType.VarChar).Value = teacher.TeacherCode;
                        command.Parameters.Add("@name", SqlDbType.VarChar).Value = teacher.Name;
                        command.Parameters.Add("@surename", SqlDbType.VarChar).Value = teacher.SureName;
                        command.Parameters.Add("@age", SqlDbType.Int).Value = teacher.Age;
                        command.Parameters.Add("@gender", SqlDbType.VarChar).Value = teacher.Gender;
                        command.Parameters.Add("@department", SqlDbType.VarChar).Value = teacher.Department;
                        command.Parameters.Add("@TeachedMatterCode", SqlDbType.VarChar).Value = teacher.TeachedMatter.MatterCode; // Assicurati di accedere al codice corretto della materia
                        command.Parameters.Add("@Role", SqlDbType.VarChar).Value = teacher.Role.ToString(); // Converti l'enum in stringa

                        try
                        {
                            // Esegui la query e ottieni il numero di righe affette
                            int affectedRows = command.ExecuteNonQuery();
                            Console.WriteLine($"L'insegnante con Codice: {teacher.TeacherCode} è stato aggiunto nel DB.");
                            // Controlla il numero di righe affette
                            if (affectedRows == 0)
                            {
                                insertOK = false; // Inserimento fallito per questo insegnante
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            // Controlla se il codice di errore corrisponde a una violazione del vincolo di unicità
                            if (sqlEx.Number == 2627) // Codice di errore SQL Server per violazione del vincolo di unicità
                            {
                                Console.WriteLine($"L'insegnante con Codice: {teacher.TeacherCode} già esiste. Sto saltando.");
                                insertOK = true; // Continua con il prossimo insegnante
                            }
                            else
                            {
                                // Se si tratta di un'altra eccezione SQL, rilancia l'eccezione
                                Console.WriteLine($"Errore SQL: {sqlEx.Message}");
                                throw;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log o gestione dell'eccezione
            Console.WriteLine($"Errore durante l'inserimento degli insegnanti nel database: {ex.Message}");
            throw; // Rilancia l'eccezione
        }
        finally
        {
            // La connessione viene chiusa automaticamente
        }

        return insertOK;
    }

    public bool DeleteDbTeachers(List<Teacher> Teachers)
    {
        bool deleteOK = true;

        // Query di DELETE basata sulla matricola
        string query = "DELETE FROM Teacher WHERE TeacherCode = @TeacherCode";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var teacher in Teachers)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Aggiungi il parametro per la matricola
                        command.Parameters.AddWithValue("@TeacherCode", teacher.TeacherCode);

                        try
                        {
                            // Esegui la query e verifica il numero di righe affette
                            int affectedRows = command.ExecuteNonQuery();

                            // Se nessuna riga è stata cancellata, imposta deleteOK su false
                            if (affectedRows == 0)
                            {
                                Console.WriteLine($"L'insegnante  con Matricola: {teacher.TeacherCode} non esiste nel database.");
                                deleteOK = false;
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            // Gestione dell'eccezione SQL
                            Console.WriteLine($"Errore SQL durante l'eliminazione dell' insegnante con Matricola: {teacher.TeacherCode}. Messaggio: {sqlEx.Message}");
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
    public bool UpdateDbTeachers(List<Teacher> Teachers)
    {
        bool updateOK = true;

        // Query di UPDATE
        string query = "UPDATE Teacher SET TeacherCode = @TeacherCode,name = @name, surename = @surename, age= @age,gender= @gender,department= @department,TeachedMatterCode=  @TeachedMatterCode WHERE TeacherCode = @TeacherCode";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var teacher in Teachers)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TeacherCode", teacher.TeacherCode);
                        command.Parameters.AddWithValue("@name", teacher.Name);
                        command.Parameters.AddWithValue("@surename", teacher.SureName);
                        command.Parameters.AddWithValue("@age", teacher.Age);
                        command.Parameters.AddWithValue("@gender", teacher.Gender);
                        command.Parameters.AddWithValue("@department", teacher.Department);
                        command.Parameters.AddWithValue("@TeachedMatterCode", teacher.TeachedMatter.MatterCode);
                       
                        try
                        {
                            int affectedRows = command.ExecuteNonQuery();
                            Console.WriteLine($"L'insegnante con Codice: {teacher.TeacherCode} è stato aggiornato nel DB.");
                            if (affectedRows == 0)
                            {
                                Console.WriteLine($"Lo studente con Matricola: {teacher.TeacherCode} non esiste nel database.");
                                updateOK = false;
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            Console.WriteLine($"Errore SQL durante l'aggiornamento dello studente con Matricola: {teacher.TeacherCode}. Messaggio: {sqlEx.Message}");
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
