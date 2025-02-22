using System;
using System.Configuration;
using System.Text.Json;
using Uni_Manager.Entity;


namespace Uni_Manager.Repository
{

    public class FacultyRepository
    {
        public List<Faculty> Faculties { get; set; } = [];

        public void ImportFaculty()
        {


            string url = ConfigurationManager.AppSettings["PathImportFaculties"];
            try
            {
                string sFaculty = File.ReadAllText(url);
                Faculties = JsonSerializer.Deserialize<List<Faculty>>(sFaculty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void ExportFaculty()
        {
            string url = ConfigurationManager.AppSettings["PathExportFaculties"];
            string sFaculties = JsonSerializer.Serialize(Faculties);
            File.WriteAllText(url, sFaculties);
        }
    }
}