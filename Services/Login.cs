using System;
using DDDProject.Stakeholders;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDProject.DataSaving;

namespace DDDProject.Services
{
    public class InitialLoginPage
    {
        private List<SeniorTutor> seniorTutors;
        private List<PersonalSupervisor> personalSupervisors;
        private List<Student> students;
        private StudentDataManager dataManager;

        public void LoginPage()
        {
            dataManager = new StudentDataManager();

            seniorTutors = new List<SeniorTutor>
            {
                new SeniorTutor { SeniorTutorID = 301, SeniorTutorName = "Dr. Evie Boyle" }
            };

            personalSupervisors = new List<PersonalSupervisor>
            {
                new PersonalSupervisor { PersonalSupervisorID = 201, PersonalSupervisorName = "Barry Allen" },
                new PersonalSupervisor { PersonalSupervisorID = 202, PersonalSupervisorName = "Susan Carrol" }
            };

            seniorTutors[0].AssignPersonalSupervisor(new List<PersonalSupervisor> { personalSupervisors[0], personalSupervisors[1] });

            students = new List<Student>
            {
                new Student { StudentID = 101, StudentName = "Patrick Parker" },
                new Student { StudentID = 103, StudentName = "Charlie Baron" },
                new Student { StudentID = 102, StudentName = "Sean Cooney" },
                new Student { StudentID = 104, StudentName = "Maddy Hall" }
            };

            personalSupervisors[0].AssignStudents(new List<Student> { students[0], students[1] });
            personalSupervisors[1].AssignStudents(new List<Student> { students[2], students[3] });

            Console.WriteLine("Welcome to the School of Computer Science!\n");
            Console.WriteLine("This is your Digital Reporting and Monitoring Software for both Students and Teachers.");
            Console.WriteLine("Keeping your university experience running smoothly.\n");

            Console.WriteLine("Please identify yourself: ");
            Console.WriteLine("1. Student.");
            Console.WriteLine("2. Personal Supervisor.");
            Console.WriteLine("3. Senior Tutor.\n");

            string userType = Console.ReadLine();

            switch (userType)
            {
                case "1":
                    StudentLogIn_Page();
                    break;
                case "2":
                    PersonalSupervisorLogIn_Page();
                    break;
                case "3":
                    SeniorTutorLogIn_Page();
                    break;
                default:
                    Console.WriteLine("That is an invalid option. Please try again.");
                    LoginPage();
                    break;
            }
        }

        private void StudentLogIn_Page()
        {
            Console.WriteLine("\nPlease enter your Student ID\n");
            if (int.TryParse(Console.ReadLine(), out int StudentID))
            {
                Student student = students.Find(s => s.StudentID == StudentID);

                if (student != null)
                {
                    Console.WriteLine($"\nWelcome, {student.StudentName}. Your Personal Supervisor is {student.AssignedPS.PersonalSupervisorName}\n");

                    StudentPage studentPage = new StudentPage();
                    studentPage.StudentDashboard(student);
                }
                
                //error handling
                else
                {
                    Console.WriteLine("\nNo student found with that ID. Please try again.");
                    StudentLogIn_Page(); // retry
                }
            }
            else
            {
                Console.WriteLine("\nInvalid input. Please enter a numeric Student ID.");
                StudentLogIn_Page(); // retry
            }
        }


        private void PersonalSupervisorLogIn_Page()
        {
            Console.WriteLine("\nPlease enter your Personal Supervisor ID\n");
            int psID = Convert.ToInt32(Console.ReadLine());
            PersonalSupervisor personalSupervisor = personalSupervisors.Find(ps => ps.PersonalSupervisorID == psID);

            if (personalSupervisor != null)
            {
                Console.WriteLine($"\nWelcome, {personalSupervisor.PersonalSupervisorName}. Your Senior Tutor is {personalSupervisor.AssignedST.SeniorTutorName}");

                PersonalSupervisorPage personalSupervisorPage = new PersonalSupervisorPage();
                personalSupervisorPage.PersonalSupervisorDashboard(personalSupervisor);
            }
        }

        private void SeniorTutorLogIn_Page()
        {
            Console.WriteLine("\nPlease enter your Senior Tutor ID\n");
            int stID = Convert.ToInt32(Console.ReadLine());
            SeniorTutor seniorTutor = seniorTutors.Find(st => st.SeniorTutorID == stID);

            if (seniorTutor != null)
            {
                Console.WriteLine($"\nWelcome, {seniorTutor.SeniorTutorName}");

                SeniorTutorPage seniorTutorPage = new SeniorTutorPage();
                seniorTutorPage.SeniorTutorDashboard(seniorTutor);
            }
        }
    }
}
