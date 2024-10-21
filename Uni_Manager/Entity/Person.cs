using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni_Manager.Entity
{
    public abstract class Person
    {
        public string Name { get; set; }
        public string SureName { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Person(string name, string sureName, int age, string gender)
        {
            Name = name;
            SureName = sureName;
            Age = age;
            Gender = gender;    
             
        }
        public Person()
        {

        }

        public override string ToString()
        {
            return $"|Nome:{Name}| Cognome: {SureName}| Eta': {Age}| Sesso: {Gender}";
        }
    }
}
