using DDDProject.Stakeholders;
using System;
using System.Linq;

namespace DDDProject.Services
{
    public class SeniorTutorPage
    {
        public void SeniorTutorDashboard(SeniorTutor ST)
        {
            Console.WriteLine($"\nWelcome {ST.SeniorTutorName}, Senior Tutor Dashboard\n");

            bool exitSeniorTutorPage = false;

            while (!exitSeniorTutorPage)
            {
                Console.WriteLine("1. View all Personal Supervisors and their Students");
                Console.WriteLine("2. Check recent student reports");
                Console.WriteLine("3. Logout\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewSupervisorsAndStudents(ST);
                        break;
                    case "2":
                        CheckRecentReports(ST);
                        break;
                    case "3":
                        exitSeniorTutorPage = true;
                        InitialLoginPage loginPage = new InitialLoginPage();
                        loginPage.LoginPage();
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        //----------------------------------------------------------------------
        // -------------------- VIEW SUPERVISORS AND THE STUDENTS --------------
        //----------------------------------------------------------------------

        private void ViewSupervisorsAndStudents(SeniorTutor ST)
        {
            Console.WriteLine("\nSupervisors and their Students:\n");

            if (ST.AssignedPersonalSupervisors.Count == 0)
            {
                Console.WriteLine("No supervisors assigned.");
                return;
            }

            foreach (var supervisor in ST.AssignedPersonalSupervisors)
            {
                Console.WriteLine($"Supervisor: {supervisor.PersonalSupervisorName} (ID: {supervisor.PersonalSupervisorID})");

                if (supervisor.AssignedStudents.Count == 0)
                {
                    Console.WriteLine("   No students assigned.\n");
                }
                else
                {
                    foreach (var student in supervisor.AssignedStudents)
                    {
                        Console.WriteLine($"   Student: {student.StudentName} (ID: {student.StudentID})");
                    }
                }
                Console.WriteLine();
            }
        }

        //----------------------------------------------------------------------
        // -------------------- CHECK RECENT REPORTS ---------------------------
        //----------------------------------------------------------------------

        private void CheckRecentReports(SeniorTutor ST)
        {
            Console.WriteLine("\nStudent Reports from File:\n");

            if (!File.Exists("reports.txt"))
            {
                Console.WriteLine("No reports file found.");
                return;
            }

            var lines = File.ReadAllLines("reports.txt");

            if (lines.Length == 0)
            {
                Console.WriteLine("No reports available.");
                return;
            }

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 4)
                {
                    string studentID = parts[0];
                    string studentName = parts[1];
                    string submissionDate = parts[2];
                    string reportContent = parts[3];

                    Console.WriteLine($"Student: {studentName} (ID: {studentID})");
                    Console.WriteLine($"Date Submitted: {submissionDate}");
                    Console.WriteLine($"Report Content: {reportContent}");
                    Console.WriteLine("---------------------------------------------------");
                }
            }
        }




    }
}
