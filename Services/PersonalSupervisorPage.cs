using DDDProject.Stakeholders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDDProject.Reports_Meetings;
using System.IO;

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

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SupervisorReviewReport(PS);
                        break;
                    case "2":
                        ManageIncomingMeetings(PS);
                        break;
                    case "3":
                        ScheduleMeetingForStudent(PS);
                        break;
                    case "4":
                        InitialLoginPage initialLoginPage = new InitialLoginPage();
                        initialLoginPage.LoginPage();
                        exitPersonalSupervisorPage = true;
                        break;
                    default:
                        Console.WriteLine("That is an invalid option. Please try again.");
                        break;
                }
            }
        }

        // ------------------------------------------------------
        // -------------------- REPORT REVIEW --------------------
        // ------------------------------------------------------


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
                Console.WriteLine($"{i + 1}. {PS.AssignedStudents[i].StudentName} (ID: {PS.AssignedStudents[i].StudentID})");
            }

            Console.Write("\nEnter the number of the student you would like to review: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= PS.AssignedStudents.Count)
            {
                Student chosenStudent = PS.AssignedStudents[choice - 1];

                Console.WriteLine($"\nReports for {chosenStudent.StudentName}:\n");

                if (chosenStudent.Reports.Count == 0)
                {
                    Console.WriteLine("No reports submitted yet.");
                }
                else
                {
                    for (int i = 0; i < chosenStudent.Reports.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Date: {chosenStudent.Reports[i].SubmissionDate}");
                    }

                    Console.WriteLine("\nEnter the number of the report you would like to view, or 0 to go back:");
                    string input = Console.ReadLine();

                    if (int.TryParse(input, out int reportIndex) && reportIndex > 0 && reportIndex <= chosenStudent.Reports.Count)
                    {
                        var selectedReport = chosenStudent.Reports[reportIndex - 1];
                        Console.WriteLine($"\nDate: {selectedReport.SubmissionDate}");
                        Console.WriteLine($"Contents: {selectedReport.ReportContent}");
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid choice.");
            }
        }


        private void DisplayReports(Student student)
        {
            Console.WriteLine($"\nReports for {student.StudentName}:\n");

            if (student.Reports.Count == 0)
            {
                Console.WriteLine("No reports have been submitted yet.");
                return;
            }

            foreach (var report in student.Reports)
            {
                Console.WriteLine($"Date: {report.SubmissionDate} | Content: {report.ReportContent}");
            }
        }

        // ------------------------------------------------------
        // -------------------- MEETING MANAGEMENT --------------------
        // ------------------------------------------------------

        public void ManageIncomingMeetings(PersonalSupervisor PS)
        {
            if (!File.Exists("meetings.txt"))
            {
                Console.WriteLine("No meeting requests found.");
                return;
            }

            var lines = File.ReadAllLines("meetings.txt");
            bool foundPending = false;

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 5)
                {
                    int studentId = int.Parse(parts[0]);
                    string studentName = parts[1];
                    DateTime meetingDate = DateTime.Parse(parts[2]);
                    MeetingStatus status = Enum.Parse<MeetingStatus>(parts[3]);
                    string details = parts[4];

                    if (status == MeetingStatus.Pending)
                    {
                        foundPending = true;
                        Console.WriteLine($"Meeting request from {studentName} on {meetingDate}");
                        Console.WriteLine("1. Accept");
                        Console.WriteLine("2. Reject");
                        Console.Write("Choose an option: ");
                        string choice = Console.ReadLine();

                        if (choice == "1")
                        {
                            status = MeetingStatus.Accepted;
                            Console.WriteLine($"Accepted meeting for {studentName}.");
                        }
                        else if (choice == "2")
                        {
                            status = MeetingStatus.Rejected;
                            Console.WriteLine($"Rejected meeting for {studentName}.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice, skipped.");
                        }

                        // update the line
                        var updatedLine = $"{studentId},{studentName},{meetingDate:dd-MM-yyyy HH:mm},{status},{details}";
                        line.Replace(line, updatedLine);
                    }
                }
            }

            if (!foundPending)
            {
                Console.WriteLine("No pending meeting requests.");
            }
        }



        private void UpdateMeetingStatus(Student student, Meetings meeting)
        {
            if (!File.Exists("meetings.txt")) return;

            var lines = File.ReadAllLines("meetings.txt").ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                var parts = lines[i].Split(',');
                if (parts.Length >= 5 && int.Parse(parts[0]) == student.StudentID &&
                    DateTime.Parse(parts[2]) == meeting.MeetingDateTime)
                {
                    lines[i] = $"{parts[0]},{parts[1]},{parts[2]},{meeting.Status},{meeting.MeetingDetails}";
                }
            }

            File.WriteAllLines("meetings.txt", lines);
        }

        // ------------------------------------------------------
        // -------------------- SCHEDULE MEETING --------------------
        // ------------------------------------------------------

        public void ScheduleMeetingForStudent(PersonalSupervisor PS)
        {
            Console.WriteLine("Choose a student to schedule a meeting with:");

            for (int i = 0; i < PS.AssignedStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {PS.AssignedStudents[i].StudentName}");
            }

            Console.WriteLine("Enter the number of the student you want to schedule a meeting with:");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= PS.AssignedStudents.Count)
            {
                var student = PS.AssignedStudents[choice - 1];
                Console.WriteLine($"Scheduling a meeting with {student.StudentName}.");

                Console.WriteLine("Enter date (DD-MM-YYYY, be sure to separate numbers with dashes):");
                string dateInput = Console.ReadLine();

                Console.WriteLine("Enter time (HH:MM in 24-hour format, be sure to use a colon):");
                string timeInput = Console.ReadLine();

                bool validDate = DateTime.TryParseExact(dateInput, "dd-MM-yyyy",
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime date);

                bool validTime = TimeSpan.TryParseExact(timeInput, "hh\\:mm",
                    System.Globalization.CultureInfo.InvariantCulture, out TimeSpan time);

                if (validDate && validTime)
                {
                    DateTime dateTime = date.Date + time;

                    Meetings newMeeting = new Meetings
                    {
                        MeetingDateTime = dateTime,
                        Status = MeetingStatus.Accepted,
                        MeetingDetails = $"Meeting scheduled by Supervisor {PS.PersonalSupervisorName}"
                    };

                    student.Meetings.Add(newMeeting);

                    using (StreamWriter sw = File.AppendText("meetings.txt"))
                    {
                        sw.WriteLine($"{student.StudentID},{student.StudentName},{newMeeting.MeetingDateTime:dd-MM-yyyy HH:mm},{newMeeting.Status},{newMeeting.MeetingDetails}");
                    }

                    Console.WriteLine($"Meeting scheduled for {dateTime:dd-MM-yyyy HH:mm}.");
                }
                else
                {
                    Console.WriteLine("Invalid date or time format. Please use DD-MM-YYYY and HH:MM (24-hour).");
                }
            }
        }

    }
    // ------------------------------------------------------
}

