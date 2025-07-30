using DDDProject.Stakeholders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDProject.Reports_Meetings;

namespace DDDProject.Services
{
    public class PersonalSupervisorPage
    {
        public void PersonalSupervisorDashboard(PersonalSupervisor PS)
        {
            bool exitPersonalSupervisorPage = false;

            while (!exitPersonalSupervisorPage)
            {
                Console.WriteLine("\nHere you can review your students' weekly reports, manage meeting requests and organise your own meetings.\n");
                Console.WriteLine("\nPlease choose an option or type 4 to Logout\n");
                Console.WriteLine("1. Review Students Weekly Reports.");
                Console.WriteLine("2. Manage incoming meeting requests.");
                Console.WriteLine("3. Schedule a new meeting with your students.");
                Console.WriteLine("4. Logout.\n");

                string PersonalSupervisorChoice = Console.ReadLine();

                switch (PersonalSupervisorChoice)
                {
                    case "1":
                        SupervisorReviewReport(PS);
                        break;
                    case "2":
                        ManageIncomingMeetings(PS);
                        break;
                    case "3":
                        PersonalSupervisorScheduleAMeeting(PS);
                        break;
                    case "4":
                        InitialLoginPage initialLoginPage = new InitialLoginPage();
                        initialLoginPage.LoginPage();
                        break;
                    default:
                        Console.WriteLine("That is an invalid option. Please try again.");
                        PersonalSupervisorDashboard(PS);
                        break;
                }
            }
        }

        public void SupervisorReviewReport(PersonalSupervisor PS)
        {
            if (PS.AssignedStudents.Count == 0)
            {
                Console.WriteLine("You have no assigned students.");
                return;
            }

            Console.WriteLine("\nSelect a student to review their reports:\n");
            for (int i = 0; i < PS.AssignedStudents.Count; i++)
            {
                Console.WriteLine($"\n{i + 1}. {PS.AssignedStudents[i].StudentName}. ID number: {PS.AssignedStudents[i].StudentID}.\n");
            }

            Console.WriteLine("\nEnter the number of the student you would like to review: \n");
            int PSStudentReportChoice;
            if (int.TryParse(Console.ReadLine(), out PSStudentReportChoice) && PSStudentReportChoice > 0 && PSStudentReportChoice <= PS.AssignedStudents.Count)
            {
                Student chosenStudent = PS.AssignedStudents[PSStudentReportChoice - 1];
                DisplayReports(chosenStudent);

            }
        }

        public void ManageIncomingMeetings(PersonalSupervisor PS)
        {
            Console.WriteLine("Incoming Meeting Requests:");

            foreach (var student in PS.AssignedStudents)
            {
                foreach (var meeting in student.Meetings)
                {
                    if (meeting.Status == MeetingStatus.Pending)
                    {
                        Console.WriteLine($"Meeting request from {student.StudentName}");
                        Console.WriteLine("1. Accept");
                        Console.WriteLine("2. Reject");
                        Console.WriteLine("Choose an option by selecting a number: ");
                        string Meetingchoice = Console.ReadLine();

                        if (Meetingchoice == "1")
                        {
                            meeting.Status = MeetingStatus.Accepted;
                            Console.WriteLine($"Meeting with {student.StudentName} scheduled for {meeting.MeetingDateTime}");

                        }
                        else if (Meetingchoice == "2")
                        {
                            meeting.Status = MeetingStatus.Rejected;
                            Console.WriteLine($"Meeting with {student.StudentName} has been rejected");
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice, please try again");
                        }
                    }
                }
            }
        }

        public void PersonalSupervisorScheduleAMeeting(PersonalSupervisor PS)
        {
            Console.WriteLine("Choose a student to schedule a meeting with:");

            for (int i = 0; i < PS.AssignedStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {PS.AssignedStudents[i].StudentName}");
            }

            Console.WriteLine("Enter the number of the student you want to schedule a meeting with:");
            int studentChoice = int.Parse(Console.ReadLine());

            if (studentChoice > 0 && studentChoice <= PS.AssignedStudents.Count)
            {
                var student = PS.AssignedStudents[studentChoice - 1];
                Console.WriteLine($"Scheduling a meeting with {student.StudentName}.");

                Console.WriteLine("\nPlease enter the date that best suits you for the meeting in this format: 01-01-2024.\n");
                DateTime meetingDate = DateTime.Parse(Console.ReadLine());
            }
        }

        private void DisplayReports(Student student)
        {
            Console.WriteLine($"\n Reports for {student.StudentName}: \n");

            if (student.Reports.Count == 0)
            {
                Console.WriteLine("No reports have been submitted yet.");
                return;
            }

            for (int i = 0; i < student.Reports.Count; i++)
            {
                var StudentReport = student.Reports[i];
                Console.WriteLine($"{i + 1}. Date of submission: {student.Reports[i].SubmissionDate}");

            }
        }
    }
}
