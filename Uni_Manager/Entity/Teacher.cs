using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;

using Uni_Manager.Entity;

namespace Uni_Manager.Entity
{
    public class Teacher : Person
    {
        public string TeacherCode { get; set; }
        public Matter TeachedMatter { get; set; }
        public string Department { get; set; }
          public TeacherRoleEnum Role { get; set; }
        public Teacher(string name, string sureName, int age, string gender, string department, string teacherCode, Matter teachedMatter, TeacherRoleEnum role)
           : base(name, sureName, age, gender)
        {
            TeacherCode = teacherCode;
            TeachedMatter = teachedMatter;
            Department = department;
            Role = role;

        }
        public override string ToString()
        {
            return $"{base.ToString()}|Codice Intesegnante: {TeacherCode}| Materia: {TeachedMatter}| Dipartimento: {Department}| Role: {Role}";
        }
    }
   
}
