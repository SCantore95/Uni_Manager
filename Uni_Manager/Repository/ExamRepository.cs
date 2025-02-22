using System;
using System.Configuration;
using System.Text.Json;
using Uni_Manager.Entity;


namespace Uni_Manager.Repository
{

    public class ExamRepository
    {
        public List<Exam> Exams { get; set; } = [];

        public void ImportExams()
        {
            string url = ConfigurationManager.AppSettings["PathImportExams"];
            try
            {
                string sExams = File.ReadAllText(url);
                Exams = JsonSerializer.Deserialize<List<Exam>>(sExams);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
