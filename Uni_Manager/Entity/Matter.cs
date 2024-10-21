using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni_Manager.Entity
{
    public class Matter
    {
        [Key]
        public string MatterCode { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public Matter() { }
        public Matter(string name, string matterCode, string departmentName)
        {
            MatterCode = matterCode;
            Name = name;
            DepartmentName = departmentName;
        }
        public override string ToString()
        { 
            return $"|Nome materia: {Name}| MatterCode: {MatterCode}| Nome dipartimento: {DepartmentName}|";
        }
       
    }

}
