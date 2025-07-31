using DDDProject.Stakeholders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.Globalization;
using DDDProject.Reports_Meetings;
using DDDProject.DataSaving;
using System.IO;

namespace DDDProject.Services
{
    public class StudentPage
    {
        public void StudentDashboard(Student student)
        {
            // -------------------- LOAD REPORTS & MEETINGS --------------------
            LoadReports(student);
            LoadMeetings(student);
            // ----------------------------------------------------------------

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
                        exitStudentPage = true;
                        break;
                    default:
                        Console.WriteLine("That is an invalid option. Please try again.");
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

        // -------------------------------------------------------------
        // -------------------- REPORTS PERSISTENCE --------------------
        // -------------------------------------------------------------
        
        private void SaveReport(Student student, Report report)
        {
            using (StreamWriter sw = File.AppendText("reports.txt"))
            {
                sw.WriteLine($"{student.StudentID},{student.StudentName},{report.SubmissionDate},{report.ReportContent}");
            }
        }

        private void LoadReports(Student student)
        {
            if (!File.Exists("reports.txt")) return;

            foreach (var line in File.ReadAllLines("reports.txt"))
            {
                var parts = line.Split(',');
                if (parts.Length >= 4 && int.Parse(parts[0]) == student.StudentID)
                {
                    student.Reports.Add(new Report
                    {
                        SubmissionDate = DateTime.Parse(parts[2]),
                        ReportContent = parts[3]
                    });
                }
            }
        }
        // -------------------------------------------------------------

        private void SubmitWeeklyReport(Student student)
        {
            Console.WriteLine("\nPlease write up your weekly report below about your progress and your feelings.\n");
            string reportContent = Console.ReadLine();

            Report newReport = new Report
            {
                ReportContent = reportContent,
                SubmissionDate = DateTime.Now
            };

            student.Reports.Add(newReport);
            SaveReport(student, newReport); // Persist report

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


        // -------------------------------------------------------------
        // -------------------- MEETINGS PERSISTENCE --------------------
        // -------------------------------------------------------------
        

        private void SaveMeeting(Student student, Meetings meeting)
        {
            using (StreamWriter sw = File.AppendText("meetings.txt"))
            {
                sw.WriteLine($"{student.StudentID},{student.StudentName},{meeting.MeetingDateTime},{meeting.Status},{meeting.MeetingDetails}");
            }
        }

        private void LoadMeetings(Student student)
        {
            if (!File.Exists("meetings.txt")) return;

            foreach (var line in File.ReadAllLines("meetings.txt"))
            {
                var parts = line.Split(',');
                if (parts.Length >= 5 && int.Parse(parts[0]) == student.StudentID)
                {
                    student.Meetings.Add(new Meetings
                    {
                        MeetingDateTime = DateTime.Parse(parts[2]),
                        Status = Enum.Parse<MeetingStatus>(parts[3]),
                        MeetingDetails = parts[4]
                    });
                }
            }
        }
        // -------------------------------------------------------------

        public void ScheduleMeetingWithPS(Student student)
        {
            Console.WriteLine($"\nWould you like to schedule a meeting with your Personal Supervisor {student.AssignedPS.PersonalSupervisorName}? (Yes/No)\n");
            string input = Console.ReadLine();

            if (input?.ToLower() == "yes")
            {
                Console.WriteLine("\nPlease enter the date (DD-MM-YYYY, be sure to separate numbers with dashes):\n");
                string dateInput = Console.ReadLine();

                Console.WriteLine("\nPlease enter the time (HH:MM in 24-hour format, be sure to use a colon):\n");
                string timeInput = Console.ReadLine();

                DateTime dateTime;
                bool validDate = DateTime.TryParseExact(dateInput, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                bool validTime = TimeSpan.TryParseExact(timeInput, "hh\\:mm",
                    CultureInfo.InvariantCulture, out TimeSpan time);

                if (validDate && validTime)
                {
                    dateTime = date.Date + time;

                    Meetings newMeeting = new Meetings
                    {
                        MeetingDateTime = dateTime,
                        Status = MeetingStatus.Pending,
                        MeetingDetails = $"Meeting requested by {student.StudentName}"
                    };

                    student.Meetings.Add(newMeeting);
                    SaveMeeting(student, newMeeting);

                    Console.WriteLine($"Meeting requested for {dateTime:dd-MM-yyyy HH:mm}. Awaiting Supervisor approval.\n");
                }
                else
                {
                    Console.WriteLine("Invalid date or time format. Please use DD-MM-YYYY and HH:MM (24-hour).");
                }
            }
            else
            {
                Console.WriteLine("Meeting request has been cancelled.");
            }
        }
    }
}
