using Microsoft.AspNetCore.Mvc;

namespace auto_checkin.Models
{
    public class CreateTimesheetRequest
    {
        public string sessionId { get; set; }
        public int projectId { get; set; }
        public string projectName { get; set; }
        public int employeeId { get; set; }
        public string employeeName { get; set; }
        public int taskId { get; set; }
        public string description { get; set; }
        public int type { get; set; } // 0 : full day of month , 1: range day
        public string dateTimesheet { get; set; } // type = 0 => month (1,2,3) , type = 1=> range day (24/1/2024-27/1/2024)


    }
}
