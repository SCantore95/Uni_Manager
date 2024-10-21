using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Uni_Manager.Entity
{
    public class Exam
    {
        [Key]
        public string ExamCode { get; set; }

        public Matter MatterExam { get; set; }

        public string TeacherCode { get; set; }

        public string StudentMatricola { get; set; }

        public DateTime ExamDate { get; set; }
        public int Result { get; set; }

        public Exam(string examCode, Matter matterExam, string teacherCode, string studentMatricola, DateTime examDate, int result)
        {
            ExamCode = examCode;
            MatterExam = matterExam;
            TeacherCode = teacherCode;
            StudentMatricola = studentMatricola;
            ExamDate = examDate;
            Result = result;
        }
        public override string ToString()
        { 
            return $"{ExamCode} | {MatterExam} | {TeacherCode} | {StudentMatricola} | {ExamDate} | {Result}";
        }
    }
    

}
