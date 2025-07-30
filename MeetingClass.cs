using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDProject.Reports_Meetings
{
    public class Meetings
    {
        public DateTime MeetingDateTime { get; set; }
        public MeetingStatus Status { get; set; }
        public string MeetingDetails { get; set; }
    }

    public enum MeetingStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public class Report
    {
        public string ReportContent { get; set; }
        public DateTime SubmissionDate { get; set; }
    }
}
