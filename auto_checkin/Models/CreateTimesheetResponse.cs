namespace auto_checkin.Models
{
    public class CreateTimesheetResponse
    {
        public string jsonrpc { get;set; }
        public string id { get;}
        public int result { get; }
        public ErrorCreateTimesheet error { get; set; }
    }
    public class ErrorCreateTimesheet
    {
        public string code { get; set; }
        public string message { get; set; }

    }
}
