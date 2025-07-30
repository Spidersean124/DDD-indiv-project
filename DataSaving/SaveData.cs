using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DDDProject.Stakeholders;
using DDDProject.Reports_Meetings;

namespace DDDProject.DataSaving
{
    public class StudentDataManager
    {
        private const string DataFilePath = "studentData.txt";

        public void SaveData(List<Student> allStudents)
        {
            using (StreamWriter FWriter = new StreamWriter(DataFilePath))
            {
                foreach (var student in allStudents)
                {
                    FWriter.WriteLine($"StudentID: {student.StudentID}");
                    FWriter.WriteLine($"Name: {student.StudentName}");
                    FWriter.WriteLine($"Assigned PS: {student.AssignedPS}");

                    FWriter.WriteLine("Reports:");
                    foreach (var report in student.Reports)
                    {
                        FWriter.WriteLine($"- Date:{report.SubmissionDate}; Content:{report.ReportContent}");

                    }

                    FWriter.WriteLine("Meetings:");
                    foreach (var meeting in student.Meetings)
                    {
                        FWriter.WriteLine($"- Date:{meeting.MeetingDateTime}; Status:{meeting.Status}; Details:{meeting.MeetingDetails}.");

                    }
                    FWriter.WriteLine();
                }
            }
        }

        public List<Student> LoadData(List<PersonalSupervisor> allPersonalSupervisors)
        {
            List<Student> loadedStudents = new List<Student>();

            if (File.Exists(DataFilePath))
            {
                using (StreamReader FReader = new StreamReader(DataFilePath))
                {
                    Student loadedStudent = null;
                    string line;

                    while ((line = FReader.ReadLine()) != null)
                    {
                        if (line.StartsWith("StudentID:"))
                        {
                            if (loadedStudent != null) loadedStudents.Add(loadedStudent);
                            loadedStudent = new Student
                            {
                                StudentID = int.Parse(line.Split(":")[1])
                            };
                        }
                        else if (line.StartsWith("Name:"))
                        {
                            loadedStudent.StudentName = line.Split(":")[1];
                        }
                        else if (line.StartsWith("Assigned PS:"))
                        {
                            string psName = line.Split(":")[1];
                            loadedStudent.AssignedPS = allPersonalSupervisors.Find(ps => ps.PersonalSupervisorName == psName);
                        }
                        else if (line.StartsWith("- Date:"))
                        {
                            if (line.Contains("Content:"))
                            {
                                string[] reportParts = line.Split(";");
                                DateTime date = DateTime.Parse(reportParts[0].Split(":")[1]);
                                string content = reportParts[1].Split(":")[1];
                                loadedStudent.Reports.Add(new Report { SubmissionDate = date, ReportContent = content });

                            }
                            else
                            {
                                string[] meetingParts = line.Split(";");
                                DateTime dateTime = DateTime.Parse(meetingParts[0].Split(":")[1]);
                                MeetingStatus status = MeetingStatus.Accepted;
                                string details = meetingParts[2].Split(":")[1];
                                loadedStudent.Meetings.Add(new Meetings { MeetingDateTime = dateTime, Status = status, MeetingDetails = details });

                            }
                        }
                    }

                    if (loadedStudent != null) loadedStudents.Add(loadedStudent);
                }
            }

            return loadedStudents;
        }
    }
}
