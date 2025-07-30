using DDDProject.Stakeholders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using DDDProject.Reports_Meetings;
using DDDProject.DataSaving;

namespace DDDProject.Services
{
    public class StudentPage
    {
        public void StudentDashboard(Student student)
        {
            if (IsReportDueForWeek(student))
            {
                Console.WriteLine("\n A reminder to submit your report for this week as it is overdue.\n");
            }

            bool exitStudentPage = false;

            while (!exitStudentPage)
            {
                Console.WriteLine("This is where you can write up weekly reports, review past reports and schedule meetings with your personal supervisor.\n");
                Console.WriteLine("\nPlease choose an option or type 4 to Logout\n");
                Console.WriteLine("1. Write Up your weekly report.");
                Console.WriteLine("2. Review past reports.");
                Console.WriteLine("3. Schedule or cancel a meeting with your Personal Supervisor.");
                Console.WriteLine("4. Logout.\n");

                string studentChoice = Console.ReadLine();

                switch (studentChoice)
                {
                    case "1":
                        SubmitWeeklyReport(student);
                        break;
                    case "2":
                        StudentReviewPastReports(student);
                        break;
                    case "3":
                        ScheduleMeetingWithPS(student);
                        break;
                    case "4":
                        InitialLoginPage initialLoginPage = new InitialLoginPage();
                        initialLoginPage.LoginPage();
                        break;
                    default:
                        Console.WriteLine("That is an invalid option. Please try again.");
                        StudentDashboard(student);
                        break;
                }
            }
        }

        private static bool IsReportDueForWeek(Student student)
        {
            if (student.Reports.Count == 0)
            {
                return true;
            }

            DateTime lastReportDate = student.Reports[^1].SubmissionDate;
            return (DateTime.Now - lastReportDate).TotalDays > 7;
        }

        private static void SubmitWeeklyReport(Student student)
        {
            Console.WriteLine("\nPlease write up your weekly report below about your progress and your feelings.\n");
            string reportContent = Console.ReadLine();

            Report newReport = new Report
            {
                ReportContent = reportContent,
                SubmissionDate = DateTime.Now
            };

            student.Reports.Add(newReport);
            Console.WriteLine($"\nYour weekly report has been submitted and saved successfully at: {newReport.SubmissionDate}\n");
        }

        public void StudentReviewPastReports(Student student)
        {
            Console.WriteLine("\n Viewing your past reports:\n");

            if (student.Reports.Count == 0)
            {
                Console.WriteLine("There are no reports to show!.");
            }
            else
            {
                for (int i = 0; i < student.Reports.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Date: {student.Reports[i].SubmissionDate}");
                }

                Console.WriteLine("\nEnter the number of the report you would like to see or press 0 to return to the Main Menu.\n");
                string input = Console.ReadLine();

                if (int.TryParse(input, out int reportIndex) && reportIndex > 0 && reportIndex <= student.Reports.Count)
                {
                    var selectedReport = student.Reports[reportIndex - 1];
                    Console.WriteLine("\n Selected Report:\n");
                    Console.WriteLine($"Date of submission: {selectedReport.SubmissionDate}");
                    Console.WriteLine($"Report contents: {selectedReport.ReportContent}");

                }
                else if (reportIndex == 0)
                {
                    Console.WriteLine("\nReturning to the student dashboard\n");
                }
                else
                {
                    Console.WriteLine("\nInvalid selection, returning to the dashboard.\n");
                }
            }
        }

        public void ScheduleMeetingWithPS(Student student)
        {
            Console.WriteLine($"\nWould you like to schedule a meeting with your Personal Supervisor {student.StudentName}? (Yes/No)\n");
            string input = Console.ReadLine();

            if (input?.ToLower() == "yes")
            {
                Console.WriteLine("\nPlease enter the date that best suits you for the meeting in this format: DD-MM-YYYY.\n");
                string dateInput = Console.ReadLine();

                Console.WriteLine("\nPlease enter the time that also best suits you for the meeting in A 24 hour format: HH:MM\n");
                string timeInput = Console.ReadLine();

                if (DateTime.TryParse(dateInput, out DateTime date) && TimeSpan.TryParse(timeInput, out TimeSpan time))
                {
                    DateTime dateTime = date.Date + time;
                    Meetings newMeeting = new Meetings
                    {
                        MeetingDateTime = dateTime,
                        Status = MeetingStatus.Pending,
                        MeetingDetails = $"Meeting requested by {student.StudentName}"
                    };

                    student.Meetings.Add(newMeeting);
                    Console.WriteLine($"Meeting requested for {dateTime}. Awaiting Supervisor approval.\n");
                }
                else
                {
                    Console.WriteLine("Invalid date format. Please try again.");
                }
            }
            else
            {
                Console.WriteLine("Meeting request has been cancelled.");
            }
        }
    }
}
