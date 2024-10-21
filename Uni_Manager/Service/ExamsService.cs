using System;
using System.Configuration;
using System.Text.Json;
using Uni_Manager.Entity;
using Uni_Manager.Repository;
using Uni_Manager.Entity;
using Microsoft.Data.SqlClient;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Uni_Manager.Service;

public class ExamsService
{
    public ExamRepository examRepository { get; set; } = new();

    public void AddExam(List<Exam> exams, List<Matter> matters, List<Teacher> teachers, List<Student> students)
    {
        bool isValidField = false;

        try
        {
            Console.Clear();
            Console.WriteLine("Inserimento Nuovo Esame");

            // Selezione della materia
            Matter selectedMatter = null;
            while (!isValidField)
            {
                Console.WriteLine("Seleziona una Materia (codice): ");
                foreach (var matter in matters)
                {
                    Console.WriteLine($"Codice: {matter.MatterCode} - Nome: {matter.Name}");
                }
                string matterCode = Console.ReadLine();
                selectedMatter = matters.FirstOrDefault(m => m.MatterCode == matterCode);

                if (selectedMatter != null)
                {
                    isValidField = true;
                }
                else
                {
                    Console.WriteLine("Materia non trovata, riprova.");
                }
            }

            isValidField = false; // Reset per il prossimo campo

            // Selezione dell'insegnante
            Teacher selectedTeacher = null;
            while (!isValidField)
            {
                Console.WriteLine("Seleziona un Insegnante (codice): ");
                foreach (var teacher in teachers)
                {
                    Console.WriteLine($"Codice: {teacher.TeacherCode} - Nome: {teacher.Name} {teacher.SureName}");
                }
                string teacherCode = Console.ReadLine();
                selectedTeacher = teachers.FirstOrDefault(t => t.TeacherCode == teacherCode);

                if (selectedTeacher != null)
                {
                    isValidField = true;
                }
                else
                {
                    Console.WriteLine("Insegnante non trovato, riprova.");
                }
            }

            isValidField = false; // Reset per il prossimo campo

            // Selezione dello studente
            Student selectedStudent = null;
            while (!isValidField)
            {
                Console.WriteLine("Seleziona uno Studente (matricola): ");
                foreach (var student in students)
                {
                    Console.WriteLine($"Matricola: {student.Matricola} - Nome: {student.Name} {student.SureName}");
                }
                string studentMatricola = Console.ReadLine();
                selectedStudent = students.FirstOrDefault(s => s.Matricola == studentMatricola);

                if (selectedStudent != null)
                {
                    isValidField = true;
                }
                else
                {
                    Console.WriteLine("Studente non trovato, riprova.");
                }
            }

            isValidField = false; // Reset per il prossimo campo

            // Inserimento della data dell'esame
            DateTime examDate = DateTime.MinValue;
            while (!isValidField)
            {
                Console.WriteLine("Inserisci la data dell'esame (dd/MM/yyyy): ");
                string inputDate = Console.ReadLine();

                if (DateTime.TryParseExact(inputDate, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out examDate))
                {
                    isValidField = true;
                }
                else
                {
                    Console.WriteLine("Data non valida, riprova.");
                }
            }

            isValidField = false; // Reset per il prossimo campo

            // Inserimento del risultato
            int result = 0;
            while (!isValidField)
            {
                Console.WriteLine("Inserisci il risultato dell'esame (voto numerico): ");
                string inputResult = Console.ReadLine();

                if (int.TryParse(inputResult, out result) && result >= 0 && result <= 30)
                {
                    isValidField = true;
                }
                else
                {
                    Console.WriteLine("Voto non valido, riprova.");
                }
            }

            // Genera un codice univoco per l'esame
            string examCode = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();

            // Crea il nuovo esame
            Exam newExam = new Exam(examCode, selectedMatter, selectedTeacher.TeacherCode, selectedStudent.Matricola, examDate, result);
           

            bool dbInsertSuccess = InsertDbExam(new List<Exam> { newExam });
            if (dbInsertSuccess)
            {
                Console.WriteLine("Studente inserito correttamente nel database.");
            }
            else
            {
                Console.WriteLine("Errore durante l'inserimento dello studente nel database.");
            }

            // Aggiungi l'esame alla lista degli esami
            exams.Add(newExam);

            Console.WriteLine("Esame aggiunto con successo!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore durante l'inserimento dell'esame: " + ex.Message);
        }
    }
    public void PrintExam(List<Exam> exams)
    {
        Console.Clear();
        foreach (Exam exam in exams)
        {
            // Stampa i dettagli dello studente
            Console.WriteLine(exam.ToString());
        }
    }
    public void SearchExam(List<Exam> exams)
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Inserisci il codice dell'esame da cercare: ");
            string examCode = Console.ReadLine();

            // Cerca l'esame direttamente con il codice
            var exam = exams.Find(e => e.ExamCode.Equals(examCode, StringComparison.CurrentCultureIgnoreCase));

            if (exam != null)
            {
                Console.WriteLine("\nEsame trovato:");
                Console.WriteLine(exam.ToString());
            }
            else
            {
                Console.WriteLine("Esame non trovato.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore durante la ricerca dell'esame: {ex.Message}");
        }
    }

    public void DeleteExamByExamCode(List<Exam> exams)
    {
        Console.Clear();
        Console.WriteLine("Inserisci la matricola dello studente da eliminare: ");
        string examCode = Console.ReadLine();

        // Cerca lo studente direttamente con la matricola
        var exam = exams.Find(e => e.ExamCode.Equals(examCode, StringComparison.CurrentCultureIgnoreCase));
        if (exam != null)
        {
            exams.Remove(exam);
            List<Exam> examsToDelete = new List<Exam> { exam };
            Console.WriteLine("Esame rimosso con successo.");
         
            bool dbDeleteSuccess = DeleteDbExam(examsToDelete);

            if (dbDeleteSuccess)
            {
                Console.WriteLine("Insegnante rimosso con successo dal database.");
            }
            else
            {
                Console.WriteLine("Errore durante l'eliminazione dell esame dal database.");
            }
            Console.WriteLine("Esame rimosso con successo.");
        }
    
      

        // Serializzazione e salvataggio degli studenti aggiornati
        string savePath = ConfigurationManager.AppSettings["PathImportExams"];
        string eExam = JsonSerializer.Serialize(exams, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(savePath, eExam);
    }

    public void UpdateExam(List<Exam> exams)
    {
        try
        {
            Console.WriteLine("Inserisci il codice dell'esame da modificare: ");
            string examCode = Console.ReadLine();
            var matchingExams = exams.Where(e => e.ExamCode.Equals(examCode, StringComparison.CurrentCultureIgnoreCase)).ToList();

            if (matchingExams.Count == 0)
            {
                Console.WriteLine("Esame non trovato.");
                return;
            }

            Exam examToUpdate;

            if (matchingExams.Count > 1)
            {
                Console.WriteLine("ERRORE: Trovati più esami con lo stesso codice! Selezionare quale esame modificare:");
                for (int i = 0; i < matchingExams.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Codice Esame: {matchingExams[i].ExamCode}, Materia: {matchingExams[i].MatterExam?.Name}, Insegnante: {matchingExams[i].TeacherCode}, Matricola Studente: {matchingExams[i].StudentMatricola}, Data Esame: {matchingExams[i].ExamDate}, Risultato: {matchingExams[i].Result}");
                }

                int selectedIndex;
                do
                {
                    Console.WriteLine("Inserire il numero dell'esame da selezionare: ");
                } while (!int.TryParse(Console.ReadLine(), out selectedIndex) || selectedIndex < 1 || selectedIndex > matchingExams.Count);

                examToUpdate = matchingExams[selectedIndex - 1];
            }
            else
            {
                examToUpdate = matchingExams[0];
            }

            // Aggiornamento dei campi
            Console.WriteLine("Esame trovato! Aggiornare i campi (lasciare vuoto per mantenere i valori attuali): ");

            Console.WriteLine($"Materia attuale: {examToUpdate.MatterExam?.Name}. Inserire nuova materia (oppure lascia vuoto): ");
            string newMatter = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newMatter))
            {
                // Supponendo che Matter è una classe che ha una proprietà Name
                examToUpdate.MatterExam = new Matter { Name = newMatter }; // Modifica secondo la struttura di Matter
            }

            Console.WriteLine($"Insegnante attuale: {examToUpdate.TeacherCode}. Inserire nuovo codice insegnante (oppure lascia vuoto): ");
            string newTeacherCode = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTeacherCode)) examToUpdate.TeacherCode = newTeacherCode;

            Console.WriteLine($"Matricola dello studente attuale: {examToUpdate.StudentMatricola}. Inserire nuova matricola (oppure lascia vuoto): ");
            string newStudentMatricola = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newStudentMatricola)) examToUpdate.StudentMatricola = newStudentMatricola;

            Console.WriteLine($"Data esame attuale: {examToUpdate.ExamDate.ToShortDateString()}. Inserire nuova data (formato: gg/mm/yyyy oppure lascia vuoto): ");
            string newExamDate = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newExamDate) && DateTime.TryParse(newExamDate, out DateTime examDate)) examToUpdate.ExamDate = examDate;

            Console.WriteLine($"Risultato attuale: {examToUpdate.Result}. Inserire nuovo risultato (oppure lascia vuoto): ");
            string newResult = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newResult) && int.TryParse(newResult, out int result)) examToUpdate.Result = result;

            // Aggiornamento nel database
            bool updateSuccess = UpdateDbExams(new List<Exam> { examToUpdate });

            if (updateSuccess)
            {
                // Serializzazione e salvataggio degli esami aggiornati
                string savePath = ConfigurationManager.AppSettings["PathImportExams"]; // Presupponendo che esista un path per gli esami
                string sExam = JsonSerializer.Serialize(exams, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(savePath, sExam);

                Console.WriteLine("Esame aggiornato con successo.");
            }
            else
            {
                Console.WriteLine("Errore durante l'aggiornamento dell'esame nel database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Errore: {ex.Message}");
        }
    }


    public bool InsertDbExam(List<Exam> exams)
    {
        bool insertOK = true;

        string query = "INSERT INTO Exam (ExamCode, MatterExamCode,MatterName, TeacherCode, StudentMatricola, ExamDate, Result) " +
                       "VALUES (@ExamId, @MatterId,@MatterName, @TeacherId, @StudentId, @Date, @Grade)";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var exam in exams)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExamId", exam.ExamCode);
                        command.Parameters.AddWithValue("@MatterId", exam.MatterExam.MatterCode);
                        command.Parameters.AddWithValue("@MatterName", exam.MatterExam.Name);
                        command.Parameters.AddWithValue("@TeacherId", exam.TeacherCode);
                        command.Parameters.AddWithValue("@StudentId", exam.StudentMatricola);
                        command.Parameters.AddWithValue("@Date", exam.ExamDate);
                        command.Parameters.AddWithValue("@Grade", exam.Result);

                        try
                        {
                            // Esegue la query e ottiene il numero di righe interessate
                            int affectedRows = command.ExecuteNonQuery();
                            Console.WriteLine($"L'esame con ExamId: {exam.ExamCode} è stato inserito.");
                            // Controlla se nessuna riga è stata modificata (fallimento dell'inserimento)
                            if (affectedRows == 0)
                            {
                                insertOK = false; // L'inserimento è fallito per questo esame
                            }
                        }
                        catch (SqlException sqlEx)
                        {


                            // Controlla se il codice di errore corrisponde a una violazione di vincolo univoco
                            if (sqlEx.Number == 2627) // Codice errore SQL Server per violazione di vincolo univoco
                            {
                                Console.WriteLine($"L'esame con ExamId: {exam.ExamCode} esiste già. Sto saltando.");
                                insertOK = true; // Continua con il prossimo esame
                            }
                            else
                            {
                                // Se è un'eccezione SQL diversa, rilancia l'eccezione
                                throw;
                            }
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Errore generale durante l'inserimento degli esami: " + ex.Message);
            insertOK = false;
        }

        return insertOK;
    }
    public bool DeleteDbExam(List<Exam> exams)
    {
        bool deleteOK = true;

        // Query di DELETE basata sulla matricola
        string query = "DELETE FROM Exam WHERE ExamCode = @ExamCode";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var exam in exams)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        // Aggiungi il parametro per la matricola
                        command.Parameters.AddWithValue("@ExamCode", exam.ExamCode);

                        try
                        {
                            // Esegui la query e verifica il numero di righe affette
                            int affectedRows = command.ExecuteNonQuery();

                            // Se nessuna riga è stata cancellata, imposta deleteOK su false
                            if (affectedRows == 0)
                            {
                                Console.WriteLine($"L'esame  con codice: {exam.ExamCode} non esiste nel database.");
                                deleteOK = false;
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            // Gestione dell'eccezione SQL
                            Console.WriteLine($"Errore SQL durante l'eliminazione dell' esame con codcie: {exam.ExamCode}. Messaggio: {sqlEx.Message}");
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
    public bool UpdateDbExams(List<Exam> exams)
    {
        bool updateOK = true;

        // Query di UPDATE
        string query = @"
                UPDATE Exam 
                SET 
                    MatterExamCode = @MatterExamCode,
                    MatterName = @MatterName,
                    TeacherCode = @TeacherCode,
                    StudentMatricola = @StudentMatricola,
                    ExamDate = @ExamDate,
                    Result = @Result 
                WHERE 
                    ExamCode = @ExamCode;";

        try
        {
            using (var connection = new SqlConnection(DbManager.ConnectionString))
            {
                connection.Open();

                foreach (var exam in exams)
                {
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ExamCode", exam.ExamCode);
                        command.Parameters.AddWithValue("@MatterExamCode", exam.MatterExam.MatterCode);
                        command.Parameters.AddWithValue("@TeacherCode", exam.TeacherCode);
                        command.Parameters.AddWithValue("@StudentMatricola", exam.StudentMatricola);
                        command.Parameters.AddWithValue("@ExamDate", exam.ExamDate);
                        command.Parameters.AddWithValue("@Result", exam.Result);
                        command.Parameters.AddWithValue("@MatterName", exam.MatterExam.Name);

                        try
                        {
                            int affectedRows = command.ExecuteNonQuery();
                            Console.WriteLine($"L'esame con codice: {exam.ExamCode} e stato inserito nel database.");
                     
                            if (affectedRows == 0)
                            {
                                Console.WriteLine($"'esame con codice: {exam.ExamCode} non esiste nel database.");
                                updateOK = false;
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            Console.WriteLine($"Errore SQL durante l'aggiornamento dell' esame con codice: {exam.ExamCode}. Messaggio: {sqlEx.Message}");
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
