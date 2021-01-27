using System;

namespace Kantan_Test
{
    public class Consumption
    {
        public DateTime date { get; set; }
        public int value { get; set; }

        public Consumption(string date, string consumption)
        {
            this.date = DateTime.Parse(date);
            this.value = Convert.ToInt32(consumption);
        }

        public string month
        {
            get
            {
                return date.ToString("MMM yyyy");
            }
        }
    }
}
