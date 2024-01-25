namespace auto_checkin.Extensions
{
    public static class DateTimeExtenions
    {
        public static IEnumerable<DateTime> ListAllDates(this DateTime lhs, DateTime futureDate)
        {
            List<DateTime> dateRange = new List<DateTime>();
            TimeSpan difference = futureDate - lhs;
            for (int i = 0; i <= difference.Days; i++)
            {
                dateRange.Add(lhs.AddDays(i));
            }
            return dateRange;
        }
    }
}
