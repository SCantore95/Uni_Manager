using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uni_Manager.Entity;

namespace Uni_Manager.Repository
{
    public class TeacherRepository
    {
        public List<Teacher> Teachers { get; set; } = [];
        public void ImportTeachers()
        {
            string? path = ConfigurationManager.AppSettings["PathImportTeachers"];
            try
            {
                string sTeacher = File.ReadAllText(path);
                Teachers = JsonSerializer.Deserialize<List<Teacher>>(sTeacher);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
}
