using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Models
{
    public class CreateTimesheetRequest
    {
        public string sessionId { get;set; }
        public int projectId { get; set; }
        public string projectName { get; set; }
        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public int taskId { get; set; }
        public string description { get; set; }
        public int month { get; set; }
    }

}
