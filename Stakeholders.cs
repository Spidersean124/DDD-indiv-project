
using DDDProject.Reports_Meetings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDProject.Stakeholders
{
    public class PersonalSupervisor
    {
        public int PersonalSupervisorID { get; set; }
        public string PersonalSupervisorName { get; set; }

        public SeniorTutor AssignedST { get; set; }
        public List<Student> AssignedStudents { get; set; } = new List<Student>();

        public void AssignStudents(List<Student> students)
        {
            foreach (var student in students)
            {
                if (!AssignedStudents.Contains(student))
                {
                    AssignedStudents.Add(student);
                    student.AssignedPS = this;
                }
            }
        }
    }

    public class SeniorTutor
    {
        public int SeniorTutorID { get; set; }
        public string SeniorTutorName { get; set; }

        public List<PersonalSupervisor> AssignedPersonalSupervisors { get; set; } = new List<PersonalSupervisor>();

        public void AssignPersonalSupervisor(List<PersonalSupervisor> personalSupervisors)
        {
            foreach (var personalSupervisor in personalSupervisors)
            {
                if (!AssignedPersonalSupervisors.Contains(personalSupervisor))
                {
                    AssignedPersonalSupervisors.Add(personalSupervisor);
                    personalSupervisor.AssignedST = this;
                }
            }
        }
    }

    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }

        public PersonalSupervisor AssignedPS { get; set; }
        public List<Report> Reports { get; set; } = new List<Report>();
        public List<Meetings> Meetings { get; set; } = new List<Meetings>();
    }
}
